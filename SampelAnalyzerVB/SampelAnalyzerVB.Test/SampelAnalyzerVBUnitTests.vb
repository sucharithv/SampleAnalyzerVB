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
    Private items As System.Collections.Generic.List(Of Integer)


    Private Sub SimpleCode1()
        If items?.Count > 0 AndAlso AlwaysTrue1() Then
            WriteLog1(String.Empty)
        Else
            WriteLog1(String.Empty)
        End If
    End Sub

    Private Function AlwaysTrue1() As Boolean
        WriteLog1(String.Empty)
        Return True
    End Function

    Sub WriteLog1(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
"
            Dim expected = New DiagnosticResult With {.Id = "SampelAnalyzerVB",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "items?.Count "),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 10, 12)
                    }
            }

            VerifyBasicDiagnostic(test, expected)

        End Sub

        Protected Overrides Function GetBasicCodeFixProvider() As CodeFixProvider
            Return New SampelAnalyzerVBCodeFixProvider()
        End Function

        Protected Overrides Function GetBasicDiagnosticAnalyzer() As DiagnosticAnalyzer
            Return New SampelAnalyzerVBAnalyzer()
        End Function

    End Class
End Namespace
