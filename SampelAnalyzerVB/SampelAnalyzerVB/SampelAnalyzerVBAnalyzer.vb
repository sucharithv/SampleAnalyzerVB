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
        context.RegisterSyntaxNodeAction(AddressOf AnalyzeNode, SyntaxKind.IfStatement)
    End Sub

    Private Sub AnalyzeNode(context As SyntaxNodeAnalysisContext)
        Dim diagnostics = context.SemanticModel.GetDiagnostics()
        If diagnostics.Length > 0 Then Return

        Dim ifExp = DirectCast(context.Node, IfStatementSyntax)
        ProcessNodes(context, ifExp.Condition)
    End Sub

    Private Sub ProcessNodes(context As SyntaxNodeAnalysisContext, node As SyntaxNode)
        If TypeOf node Is ConditionalAccessExpressionSyntax OrElse
            TypeOf node Is InvocationExpressionSyntax OrElse
            TypeOf node Is MemberAccessExpressionSyntax Then
            Dim typeInfo = context.SemanticModel.GetTypeInfo(node)

            If IsNullabelType(typeInfo.Type) Then
                Dim diag = Diagnostic.Create(Rule, node.GetLocation(), node.GetText().ToString().TrimEnd())
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
