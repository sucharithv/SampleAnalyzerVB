﻿Imports System
Imports System.Collections.Generic
Imports System.Collections.Immutable
Imports System.Composition
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.CodeActions
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis.Rename
Imports Microsoft.CodeAnalysis.Text

<ExportCodeFixProvider(LanguageNames.VisualBasic, Name:=NameOf(SampelAnalyzerVBCodeFixProvider)), [Shared]>
Public Class SampelAnalyzerVBCodeFixProvider
    Inherits CodeFixProvider

    Private Const title As String = "Make uppercase"

    Public NotOverridable Overrides ReadOnly Property FixableDiagnosticIds As ImmutableArray(Of String)
        Get
            Return ImmutableArray.Create(SampelAnalyzerVBAnalyzer.DiagnosticId)
        End Get
    End Property

    Public NotOverridable Overrides Function GetFixAllProvider() As FixAllProvider
        ' See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        Return WellKnownFixAllProviders.BatchFixer
    End Function

    Public NotOverridable Overrides Async Function RegisterCodeFixesAsync(context As CodeFixContext) As Task
        Dim root = Await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(False)

        ' TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest

        Dim diagnostic = context.Diagnostics.First()
        Dim diagnosticSpan = diagnostic.Location.SourceSpan

        ' Find the type statement identified by the diagnostic.
        Dim declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType(Of TypeStatementSyntax)().First()

        ' Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title:=title,
                createChangedSolution:=Function(c) MakeUppercaseAsync(context.Document, declaration, c),
                equivalenceKey:=title),
            diagnostic)
    End Function

    Private Async Function MakeUppercaseAsync(document As Document, typeStmt As TypeStatementSyntax, cancellationToken As CancellationToken) As Task(Of Solution)
        ' Compute new uppercase name.
        Dim identifierToken = typeStmt.Identifier
        Dim newName = identifierToken.Text.ToUpperInvariant()

        ' Get the symbol representing the type to be renamed.
        Dim semanticModel = Await document.GetSemanticModelAsync(cancellationToken)
        Dim typeSymbol = semanticModel.GetDeclaredSymbol(typeStmt, cancellationToken)

        ' Produce a new solution that has all references to that type renamed, including the declaration.
        Dim originalSolution = document.Project.Solution
        Dim optionSet = originalSolution.Workspace.Options
        Dim newSolution = Await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(False)

        ' Return the new solution with the now-uppercase type name.
        Return newSolution
    End Function
End Class
