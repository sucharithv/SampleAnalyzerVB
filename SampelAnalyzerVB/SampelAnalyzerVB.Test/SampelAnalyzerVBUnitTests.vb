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




        <DataTestMethod()>
        <DataRow("NullConditionOperatorUsingGetDefaultWithAndAlsoCheck", DisplayName:="NoIssuesNullConditionOperatorUsingGetDefaultWithAndAlsoCheck")>
        Public Sub PerformTestsThatShouldNotFindIssues(ByVal codeFileName As String)
            VerifyBasicDiagnostic(GetTestCodeSegment(codeFileName))
        End Sub

        <DataTestMethod()>
        <DataRow("NullConditionOperatorWithAndAlsoCheck", AndAlsoRuleId, "items?.Count", 9, 12, DisplayName:="ShouldFindNullConditionOperatorWithAndAlsoCheck")>   'Basic Scenario - items?.Count > 0
        <DataRow("NullableWithExistsWithAndAlsoCheck", AndAlsoRuleId, "items?.Exists(Function(x) x > 10)", 7, 12, DisplayName:="ShouldFindNullableWithExistsWithAndAlsoCheck")> ' HCSIS Scenario 1 - items?.Exists(Function(x) x.id = 1)
        <DataRow("NullableWithHasValueCheckWithAndAlsoCheck", AndAlsoRuleId, "record?.EffectiveBeginDate.HasValue", 9, 12, DisplayName:="ShouldFindNullableWithHasValueCheckWithAndAlsoCheck")> ' HCSIS Scenario 2 - selectedSegment?.EffectiveBeginDate.HasValue
        <DataRow("EvaluateNullableBool", RuleId, "newNullableBool", 7, 12, DisplayName:="ShouldFindNullableBool")>
        Public Sub PerformTestsThatShouldFindIssues(ByVal codeFileName As String,
                                           ByVal ruleToTest As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            PerformCodeSegmentTest(codeFileName, ruleToTest, message, lineNumber, columnNumber)
        End Sub


        Protected Overrides Function GetBasicCodeFixProvider() As CodeFixProvider
            Return New SampelAnalyzerVBCodeFixProvider()
        End Function

        Protected Overrides Function GetBasicDiagnosticAnalyzer() As DiagnosticAnalyzer
            Return New SampelAnalyzerVBAnalyzer()
        End Function


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


        Private Shared Function GetTestCodeSegment(ByVal fileName As String) As String
            Return IO.File.ReadAllText($"..\..\..\..\..\SampleReferenceCodeConsole\{fileName}.vb")
        End Function
    End Class
End Namespace
