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

    Public Const DiagnosticId = "SampelAnalyzerVB"

    ' You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
    Private Shared ReadOnly Title As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerTitle), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly MessageFormat As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerMessageFormat), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Shared ReadOnly Description As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerDescription), My.Resources.ResourceManager, GetType(My.Resources.Resources))
    Private Const Category = "Naming"

    Private Shared Rule As New DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)

    Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
        Get
            Return ImmutableArray.Create(Rule)
        End Get
    End Property

    Public Overrides Sub Initialize(context As AnalysisContext)
        ' TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
        ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        'context.RegisterSymbolAction(AddressOf AnalyzeSymbol, SymbolKind.NamedType)
        context.RegisterSyntaxNodeAction(AddressOf AnalyzeNode, SyntaxKind.IfStatement)
    End Sub

    Private Sub AnalyzeNode(context As SyntaxNodeAnalysisContext)
        Dim ifExp = DirectCast(context.Node, IfStatementSyntax)
        ProcessNodes(context, ifExp.Condition)
        'ProcessExpression(context, ifExp.Condition)
    End Sub

    Private Sub ProcessNodes(context As SyntaxNodeAnalysisContext, node As SyntaxNode)
        If TypeOf node Is ConditionalAccessExpressionSyntax OrElse TypeOf node Is InvocationExpressionSyntax Then
            Dim typeInfo = context.SemanticModel.GetTypeInfo(node)
            If IsNullabelType(typeInfo.Type) Then
                Dim diag = Diagnostic.Create(Rule, node.GetLocation(), node.GetText())
                context.ReportDiagnostic(diag)
            End If
        Else
            For Each childNode In node.ChildNodes()
                If TypeOf childNode IsNot LiteralExpressionSyntax Then
                    ProcessNodes(context, childNode)
                End If
            Next
        End If
    End Sub


    'Private Sub ProcessExpression(context As SyntaxNodeAnalysisContext, exp As ExpressionSyntax)
    '    Select Case exp.Kind
    '        Case SyntaxKind.AndAlsoExpression,
    '             SyntaxKind.AndExpression,
    '             SyntaxKind.OrElseExpression,
    '             SyntaxKind.OrExpression
    '            Dim binExp = DirectCast(exp, BinaryExpressionSyntax)
    '            ProcessBinaryExpression(context, binExp)
    '        Case SyntaxKind.InvocationExpression
    '            Dim invocationExp = DirectCast(exp, InvocationExpressionSyntax)
    '            ProcessInvocationExpression(context, invocationExp)
    '        Case SyntaxKind.GreaterThanExpression

    '        Case SyntaxKind.CharacterLiteralExpression, SyntaxKind.ConditionalAccessExpression, SyntaxKind.CTypeExpression, SyntaxKind.DateLiteralExpression, SyntaxKind.DictionaryAccessExpression, SyntaxKind.DirectCastExpression, 

    '        Case Else
    '            Console.WriteLine("unhandled kind encountered : " + exp.Kind)
    '    End Select
    'End Sub

    'Private Sub ProcessBinaryExpression(context As SyntaxNodeAnalysisContext, binaryExp As BinaryExpressionSyntax)
    '    ProcessExpression(binaryExp.Left)
    '    ProcessExpression(binaryExp.Right)
    'End Sub

    'Private Sub ProcessInvocationExpression(context As SyntaxNodeAnalysisContext, invocationExp As InvocationExpressionSyntax)
    '    Dim type = context.SemanticModel.GetTypeInfo(invocationExp).Type
    '    If IsNullabelType(type) Then
    '        Dim diag = Diagnostic.Create(Rule, invocationExp.GetLocation(), invocationExp.GetText())
    '        context.ReportDiagnostic(diag)
    '    End If
    'End Sub

    Private Function IsNullabelType(typeInfo As ITypeSymbol) As Boolean
        If typeInfo Is Nothing Then Return False
        If TypeOf (typeInfo) Is IErrorTypeSymbol Then Return False

        If typeInfo.SpecialType = SpecialType.System_Nullable_T Then
            Return True
        Else
            Return False
            End If
    End Function

    'Private Sub AnalyzeSymbol(context As SymbolAnalysisContext)
    '    ' TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find

    '    Dim namedTypeSymbol = CType(context.Symbol, INamedTypeSymbol)

    '    ' Find just those named type symbols with names containing lowercase letters.
    '    If namedTypeSymbol.Name.ToCharArray.Any(AddressOf Char.IsLower) Then
    '        ' For all such symbols, produce a diagnostic.
    '        Dim diag = Diagnostic.Create(Rule, namedTypeSymbol.Locations(0), namedTypeSymbol.Name)

    '        context.ReportDiagnostic(diag)
    '    End If
    'End Sub
End Class
