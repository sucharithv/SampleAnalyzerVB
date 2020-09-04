Imports System
Imports System.Collections.Generic
Imports System.Collections.Immutable
Imports System.Linq
Imports System.Threading
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis.Diagnostics

<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public Class SampelAnalyzerVBAnalyzer
    Inherits DiagnosticAnalyzer

    Public Const DiagnosticId = "DHS9999"
    Public Const AndAlsoDiagnosticId = "DHS9998"

    ' You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
    Private Shared ReadOnly Title As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerTitle), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly MessageFormat As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerMessageFormat), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly Description As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerDescription), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Const Category = "Naming"

    Private Shared NullableRule As New DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)
    Private Shared ShortCircuitRule As New DiagnosticDescriptor(AndAlsoDiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)


    Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
        Get
            Return ImmutableArray.Create(NullableRule, ShortCircuitRule)
        End Get
    End Property

    Public Overrides Sub Initialize(context As AnalysisContext)
        ' TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        context.RegisterSyntaxNodeAction(AddressOf AnalyzeNode, SyntaxKind.IfStatement)
    End Sub

    Private Sub AnalyzeNode(context As SyntaxNodeAnalysisContext)
        Dim ifExp = DirectCast(context.Node, IfStatementSyntax)
        Debug.WriteLine($"AnalyzeNode - ifExp: {ifExp.Condition}")

        ' Skip analysis if there are errors in the source code as type information is not available
        ' when there are errors in the file
        Dim diagnosticIssues = context.SemanticModel.GetDiagnostics()
        If diagnosticIssues.Any(Function(d) d.Severity = DiagnosticSeverity.Error) Then
            Debug.WriteLine("Diagnostistics errors found in source:")
            For Each issue In diagnosticIssues
                Debug.WriteLine($"{issue}")
            Next
            Throw New NotSupportedException("Diagnostistics errors found in provided source. Check debug logs for details.")
        End If

        Dim analyzeVs2019Issue As Boolean = False
        If analyzeVs2019Issue Then
            ' If the expression contains a short curcuit operator evaluate each of the expressions
            ' to see any any of them result in Nullable type
            Dim ShortCircuitingExpressions = ifExp.DescendantNodes().Where(Function(node) IsShortCircuitNode(node))
            For Each node In ShortCircuitingExpressions
                ProcessShortCircuitingExpression(context, node)
            Next
        Else
            Dim state As New HashSet(Of String)
            ' determine if the right expression has a conditional access expression
            Dim containsConditionalAccess = ifExp.DescendantNodes.Any(Function(childNode) childNode.Kind = SyntaxKind.ConditionalAccessExpression)
            ' NOTE: for the right most expression processingAndAlso should be false... this is required as we dont want to report short
            ' circuit issues on the right most expression
            Dim processingAndAlso = False
            ProcessNode(context, ifExp.Condition, processingAndAlso, containsConditionalAccess, state)
        End If
    End Sub

    Private Shared Function IsShortCircuitNode(node As SyntaxNode) As Boolean
        Return node.Kind = SyntaxKind.AndAlsoExpression OrElse node.Kind = SyntaxKind.OrElseExpression
    End Function

    Private Sub ProcessShortCircuitingExpression(context As SyntaxNodeAnalysisContext, node As SyntaxNode)
        For Each childNode In node.ChildNodes()
            If Not IsShortCircuitNode(childNode) Then
                If IsNullableExpression(context, childNode) Then
                    Dim diag = Diagnostic.Create(ShortCircuitRule, childNode.GetLocation(), childNode.GetText().ToString().Trim())
                    context.ReportDiagnostic(diag)
                End If
            End If
        Next
    End Sub

    Private Sub ProcessNode(context As SyntaxNodeAnalysisContext, node As SyntaxNode, processingAndAlso As Boolean, containsConditionalAccess As Boolean, state As HashSet(Of String))
        Dim shouldProcessChildren As Boolean = False
        ' Process children if the node is a short circuit expression
        If IsShortCircuitNode(node) Then
            Dim shortCircuitNode = DirectCast(node, BinaryExpressionSyntax)

            ' process right expression
            ProcessNode(context, shortCircuitNode.Right, processingAndAlso, containsConditionalAccess, state)

            ' process left expression
            processingAndAlso = True
            ProcessNode(context, shortCircuitNode.Left, processingAndAlso, containsConditionalAccess, state)
        ElseIf node.Kind = SyntaxKind.ParenthesizedExpression Then
            ProcessChildNodes(context, node, processingAndAlso, containsConditionalAccess, state)
        Else
            Dim leafNodeProcessed = TryProcessLeafNode(context, node, processingAndAlso, containsConditionalAccess, state)
            If Not leafNodeProcessed Then
                ProcessChildNodes(context, node, processingAndAlso, containsConditionalAccess, state)
            End If
        End If

    End Sub

    Private Sub ProcessChildNodes(context As SyntaxNodeAnalysisContext, node As SyntaxNode, processingAndAlso As Boolean, containsConditionalAccess As Boolean, state As HashSet(Of String))
        Dim childNodeCount As Long = 0
        For Each childNode In node.ChildNodes()
            childNodeCount += 1
            ProcessNode(context, childNode, processingAndAlso, containsConditionalAccess, state)
        Next

        ' TODO: when can we neither have type informtion or children?
        ' NothingLiteralExpression
        If childNodeCount = 0 Then
            Throw New NotSupportedException($"Unsupported expression '{node.GetText()}' encountered withing if statement '{context.Node}'.")
        End If
    End Sub

    Function TryProcessLeafNode(context As SyntaxNodeAnalysisContext, node As SyntaxNode, processingAndAlso As Boolean, containsConditionalAccess As Boolean, state As HashSet(Of String)) As Boolean
        Debug.WriteLine("")
        Debug.WriteLine($"TryProcessLeafNode - node: {node}")
        Dim typeInfo = context.SemanticModel.GetTypeInfo(node)
        Debug.WriteLine($"type info: {typeInfo.Type}")
        If typeInfo.Type Is Nothing Then
            Return False
        Else
            ' Check if the resulting type is nullable and report diagnostics accordingly
            If IsNullabelType(typeInfo.Type) Then
                Dim reportShortCircuitRule = processingAndAlso AndAlso containsConditionalAccess
                Dim isBinaryExp = TypeOf node Is BinaryExpressionSyntax

                If (processingAndAlso OrElse IsShortCircuitNode(node.Parent)) AndAlso isBinaryExp Then
                    Dim binaryExp = DirectCast(node, BinaryExpressionSyntax)
                    Debug.WriteLine($"-->Processing left part of binary expression: {binaryExp.Left}")
                    Dim hasValueFound = FindHasValue(context, binaryExp.Left, state)
                    If Not hasValueFound Then
                        Debug.WriteLine($"-->Processing right part of binary expression: {binaryExp.Right}")
                        hasValueFound = FindHasValue(context, binaryExp.Right, state)
                    End If
                    If hasValueFound Then Return True
                End If

                If reportShortCircuitRule Then
                    If isBinaryExp Then
                        Debug.WriteLine("==>Processing binary expression")
                        Dim binaryExp = DirectCast(node, BinaryExpressionSyntax)
                        Dim nodeText = binaryExp.Left.GetText.ToString.Trim
                        If state.Contains(nodeText) Then
                            reportShortCircuitRule = False
                            Debug.WriteLine($"Found .HasValue call in state")
                        End If
                    Else
                        Debug.WriteLine("==>Processing uniary expression")
                        Dim nodeText = node.GetText.ToString.Trim
                        If state.Contains(nodeText) Then
                            reportShortCircuitRule = False
                            Debug.WriteLine($"Found .HasValue call in state")
                        End If
                    End If
                End If

                Dim diag = If(reportShortCircuitRule,
                                    Diagnostic.Create(ShortCircuitRule, node.GetLocation(), node.GetText().ToString().TrimEnd()),
                                    Diagnostic.Create(NullableRule, node.GetLocation(), node.GetText().ToString().TrimEnd()))
                context.ReportDiagnostic(diag)
            End If
        End If
        Return True
    End Function

    Private Function FindHasValue(context As SyntaxNodeAnalysisContext, node As SyntaxNode, state As HashSet(Of String)) As Boolean
        If TypeOf node IsNot LiteralExpressionSyntax Then
            Dim expText = node.GetText.ToString
            Dim ifText = context.Node.GetText.ToString
            Dim searchText = expText.Trim + ".HasValue"
            Dim isFound = ifText.Contains(searchText)
            ' TODO: HasValue has to be before the current expression
            If isFound Then
                state.Add(searchText)
                Debug.WriteLine("Not adding diagnostics. Found call to HasValue for current expression.")
                Return True
            Else
                Debug.WriteLine($"Call to HasValue was not found. expText: {searchText}, ifText: {ifText}")
            End If
        End If
        Return False
    End Function

    Private Function IsNullableExpression(context As SyntaxNodeAnalysisContext, node As SyntaxNode) As Boolean
        Dim typeInfo = context.SemanticModel.GetTypeInfo(node)
        Return IsNullabelType(typeInfo.Type)
    End Function

    Private Function IsNullabelType(typeInfo As ITypeSymbol) As Boolean
        If typeInfo Is Nothing Then Return False
        If TypeOf (typeInfo) Is IErrorTypeSymbol Then Return False

        If typeInfo.Name = "Nullable" Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
