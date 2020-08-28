Imports SampelAnalyzerVB
Imports SampelAnalyzerVB.Test.TestHelper
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace SampelAnalyzerVB.Test
    <TestClass>
    Public Class UnitTest
        Inherits CodeFixVerifier

        'No diagnostics expected to show up
        <TestMethod>
        Public Sub TestMethod1()
            Dim test = ""
            VerifyBasicDiagnostic(test)
        End Sub

        'Diagnostic And CodeFix both triggered And checked for
        <TestMethod>
        Public Sub TestMethod2()

            Dim test = "
Option Strict On
Option Explicit On

Public Class Class1
    Private items As List(Of Integer)

    Public Sub Main()
        items = Nothing
        SimpleCode()
        Console.ReadLine()
    End Sub

    Private Sub SimpleCode()
        If items?.Count > 0 AndAlso AlwaysTrue() Then
            WriteLog(String.Empty)
        Else
            WriteLog(String.Empty)
        End If
    End Sub

    Private Function AlwaysTrue() As Nullable(Of Boolean)
        WriteLog(String.Empty)
        Return True
    End Function

    Sub WriteLog(ByVal message As String)
        Debug.WriteLine(message)
        Console.WriteLine(message)
    End Sub
End Class
"
            Dim expected = New DiagnosticResult With {.Id = "SampelAnalyzerVB",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "Module1"),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 13, 12)
                    }
            }

            VerifyBasicDiagnostic(test, expected)

            '            Dim fixtest = "
            'Module MODULE1

            '    Sub Main()

            '    End Sub

            'End Module"
            '            VerifyBasicFix(test, fixtest)
        End Sub

        Protected Overrides Function GetBasicCodeFixProvider() As CodeFixProvider
            Return New SampelAnalyzerVBCodeFixProvider()
        End Function

        Protected Overrides Function GetBasicDiagnosticAnalyzer() As DiagnosticAnalyzer
            Return New SampelAnalyzerVBAnalyzer()
        End Function

    End Class
End Namespace
