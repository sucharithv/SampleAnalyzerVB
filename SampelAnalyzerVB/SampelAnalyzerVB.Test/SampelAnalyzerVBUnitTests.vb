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

        'Basic Scenario - items?.Count > 0
        <TestMethod>
        Public Sub TestMethod2()

            Dim test = "
Option Strict On
Option Explicit On

Public Class Class1
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        If items?.Count > 0 AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function
End Class
"
            Dim expected = New DiagnosticResult With {.Id = "SampelAnalyzerVB",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "items?.Count"),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 9, 12)
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

        <TestMethod>
        Public Sub TestMethod2Fixed()

            Dim test = "
Option Strict On
Option Explicit On

Public Class Class1
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        If (items?.Count).GetValueOrDefault > 0 AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function
End Class
"

            VerifyBasicDiagnostic(test)
        End Sub

        ' HCSIS Scenario 1 - items?.Exists(Function(x) x.id = 1)
        <TestMethod>
        Public Sub TestMethod3()

            Dim test = "
Option Strict On
Option Explicit On

Public Class Class1
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        If items?.Exists(Function(x) x > 10) AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function
End Class
"
            Dim expected = New DiagnosticResult With {.Id = "SampelAnalyzerVB",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "items?.Exists(Function(x) x > 10)"),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 9, 12)
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

        ' HCSIS Scenario 2 - selectedSegment?.EffectiveBeginDate.HasValue
        <TestMethod>
        Public Sub TestMethod4()

            Dim test = "
Option Strict On
Option Explicit On

Public Class Class1
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        Dim record As Foo = Nothing
        If record?.EffectiveBeginDate.HasValue AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function

    Private Class Foo
        Private _effectiveBeginDate As System.Nullable(Of Date)
        Public Property EffectiveBeginDate() As System.Nullable(Of Date)
            Get
                Return _effectiveBeginDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _effectiveBeginDate = value
            End Set
        End Property
    End Class
End Class
"
            Dim expected = New DiagnosticResult With {.Id = "SampelAnalyzerVB",
                .Message = String.Format("Type name '{0}' contains lowercase letters", "record?.EffectiveBeginDate.HasValue"),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", 10, 12)
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
