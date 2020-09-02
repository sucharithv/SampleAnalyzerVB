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

    Private Shared Rule As New DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)
    Private Shared AndAlsoRule As New DiagnosticDescriptor(AndAlsoDiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)


    Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
        Get
            Return ImmutableArray.Create(Rule, AndAlsoRule)
        End Get
    End Property

    Public Overrides Sub Initialize(context As AnalysisContext)
        ' TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        context.RegisterSyntaxNodeAction(AddressOf AnalyzeNode, SyntaxKind.IfStatement)
    End Sub

    Private Sub AnalyzeNode(context As SyntaxNodeAnalysisContext)
        Dim ifExp = DirectCast(context.Node, IfStatementSyntax)

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
            ProcessNodes(context, ifExp.Condition, False)
        End If
    End Sub

    Private Shared Function IsShortCircuitNode(node As SyntaxNode) As Boolean
        Return node.Kind = SyntaxKind.AndAlsoExpression OrElse node.Kind = SyntaxKind.OrElseExpression
    End Function

    Private Sub ProcessShortCircuitingExpression(context As SyntaxNodeAnalysisContext, node As SyntaxNode)
        For Each childNode In node.ChildNodes()
            If Not IsShortCircuitNode(childNode) Then
                If IsNullableExpression(context, childNode) Then
                    Dim diag = Diagnostic.Create(AndAlsoRule, childNode.GetLocation(), childNode.GetText().ToString().TrimEnd())
                    context.ReportDiagnostic(diag)
                End If
            End If
        Next
    End Sub

    Private Sub ProcessNodes(context As SyntaxNodeAnalysisContext, node As SyntaxNode, ByVal containsAndAlso As Boolean)
        Dim shouldProcessChildren As Boolean = False
        ' Process children if the node is a short circuit expression
        If IsShortCircuitNode(node) Then
            Dim shortCircuitNode = DirectCast(node, BinaryExpressionSyntax)
            ProcessNodes(context, shortCircuitNode.Right, containsAndAlso)
            ProcessNodes(context, shortCircuitNode.Left, True)
        Else
            ' Process children if type information is not available for the current node
            Dim typeInfo = context.SemanticModel.GetTypeInfo(node)
            If typeInfo.Type Is Nothing Then
                shouldProcessChildren = True
            Else
                ' Check if the resulting type is nullable and report diagnostics accordingly
                If IsNullabelType(typeInfo.Type) Then
                    Dim diag = If(containsAndAlso,
                                        Diagnostic.Create(AndAlsoRule, node.GetLocation(), node.GetText().ToString().TrimEnd()),
                                        Diagnostic.Create(Rule, node.GetLocation(), node.GetText().ToString().TrimEnd()))
                    context.ReportDiagnostic(diag)
                End If
            End If
        End If

        If shouldProcessChildren Then
            Dim childNodeCount As Long = 0
            For Each childNode In node.ChildNodes()
                childNodeCount += 1
                ProcessNodes(context, childNode, containsAndAlso)
            Next

            ' TODO: when can we neither have type informtion or children?
            If childNodeCount = 0 Then
                Throw New NotSupportedException($"Unsupported expression '{node.GetText()}' encountered withing if statement '{context.Node}'.")
            End If
        End If
    End Sub

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
