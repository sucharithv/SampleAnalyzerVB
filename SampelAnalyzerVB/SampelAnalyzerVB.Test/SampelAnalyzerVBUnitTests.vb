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

        Private Const AndAlsoRuleId As String = "DHS9998"
        Private Const RuleId As String = "DHS9999"
        Private Const RuleMessage As String = "Null conditional issue '{0}' may result in Nothing"

        'No diagnostics expected to show up
        <TestMethod>
        Public Sub TestEmptyCodeSegmentShouldPass()
            Dim test = ""
            VerifyBasicDiagnostic(test)
        End Sub

        'Basic Scenario - items?.Count > 0
        <TestMethod>
        Public Sub TestNullConditionOperatorWithAndAlso()
            PerformCodeSegmentTest("NullConditionOperatorWithAndAlso", AndAlsoRuleId, "items?.Count", 9, 12)

        End Sub


        Private Shared Function GetTestCodeSegment(ByVal fileName As String) As String
            Return IO.File.ReadAllText($"..\..\..\..\..\SampleReferenceCodeConsole\{fileName}.vb")
        End Function

        <TestMethod>
        Public Sub TestMethod2Fixed()
            VerifyBasicDiagnostic(GetTestCodeSegment("Test2Fixed"))
        End Sub

        ' HCSIS Scenario 1 - items?.Exists(Function(x) x.id = 1)
        <TestMethod>
        Public Sub TestMethod3()
            PerformCodeSegmentTest("Test3", AndAlsoRuleId, "items?.Exists(Function(x) x > 10)", 8, 12)
        End Sub

        ' HCSIS Scenario 2 - selectedSegment?.EffectiveBeginDate.HasValue
        <TestMethod>
        Public Sub TestMethod4()
            PerformCodeSegmentTest("Test4", AndAlsoRuleId, "record?.EffectiveBeginDate.HasValue", 9, 12)
        End Sub

        <TestMethod>
        Public Sub TestMethod5()
            PerformCodeSegmentTest("Test5", RuleId, "newNullableBool", 7, 12)
        End Sub


        <DataTestMethod()>
        <DataRow("Test5", RuleId, "newNullableBool", 7, 12, DisplayName:="DataTestMethod5")>
        Public Sub PerformCodeSegmentTests(ByVal codeFileName As String,
                                           ByVal ruleToTest As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            PerformCodeSegmentTest(codeFileName, ruleToTest, message, lineNumber, columnNumber)
        End Sub
        Private Sub PerformCodeSegmentTest(ByVal codeFileName As String,
                                           ByVal ruleToTest As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            Dim test = GetTestCodeSegment(codeFileName)
            Dim expected = New DiagnosticResult With {.Id = ruleToTest,
                .Message = String.Format(RuleMessage, message),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", lineNumber, columnNumber)
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
