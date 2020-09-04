﻿Imports SampelAnalyzerVB
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
        <DataRow("Test11", "fooInstance.Status <> CaseStatus.Closed", 8, 12, DisplayName:="ECIS Scenario 3")>
        Public Sub ShortCircuitingIfShouldFail9999(ByVal codeFileName As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            PerformFailingTest(codeFileName, RuleId, message, lineNumber, columnNumber)
        End Sub

        <DataTestMethod()>
        <DataRow("NullConditionOperatorWithAndAlso", "items?.Count > 0", 9, 12, DisplayName:="ECIS Scenario 1")>
        <DataRow("Test3", "items?.Exists(Function(x) x > 10)", 8, 12, DisplayName:="HCSIS Scenario 1")>
        <DataRow("Test4", "record?.EffectiveBeginDate.HasValue", 9, 12, DisplayName:="HCSIS Scenario 2")>
        <DataRow("Test10", "fooInstance?.BeginDate.HasValue", 8, 12, DisplayName:="ECIS Scenario 2")>
        Public Sub ShortCircuitingIfShouldFail9998(ByVal codeFileName As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            PerformFailingTest(codeFileName, AndAlsoRuleId, message, lineNumber, columnNumber)
        End Sub

        <DataTestMethod()>
        Public Sub PerformFailingTests(ByVal codeFileName As String,
                                       ByVal firstRuleToTest As String,
                                       ByVal firstMessage As String,
                                       ByVal firstLineNumber As Int32,
                                       ByVal firstColumnNumber As Int32,
                                       ByVal secondRuleToTest As String,
                                       ByVal secondMessage As String,
                                       ByVal secondLineNumber As Int32,
                                       ByVal secondColumnNumber As Int32)
            PerformFailingTest(codeFileName,
                               firstRuleToTest, firstMessage, firstLineNumber, firstColumnNumber,
                               secondRuleToTest, secondMessage, secondLineNumber, secondColumnNumber)

        End Sub

        <DataTestMethod()>
        <DataRow("Test5", "newNullableBool", 7, 12, DisplayName:="Boolean?")>
        Public Sub BasicIfShouldFail(ByVal codeFileName As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            PerformFailingTest(codeFileName, RuleId, message, lineNumber, columnNumber)
        End Sub

        <DataTestMethod()>
        <DataRow("Test6", DisplayName:="Is Nothing")>
        <DataRow("Test7", DisplayName:="IsNot Nothing")>
        <DataRow("Test8", DisplayName:="Not Is Nothing")>
        Public Sub BasicIfShouldPass(ByVal codeFileName As String)
            PerformPassingTest(codeFileName)
        End Sub

        <DataTestMethod()>
        <DataRow("Test2Fixed", DisplayName:="?. Wrapped in GetValueOrDefault")>
        <DataRow("IsNothingCheck", DisplayName:="Nothing check on a nullable type")>
        <DataRow("Test9", DisplayName:="Proper use of HasValue and Value")>
        Public Sub ShortCircuitingIfShouldPass(ByVal codeFileName As String)
            PerformPassingTest(codeFileName)
        End Sub

        <Ignore>
        <TestMethod>
        Public Sub DebugTest()
            PerformPassingTest("Template")
        End Sub


        Private Shared Function GetTestCodeSegment(ByVal fileName As String) As String
            Return IO.File.ReadAllText($"..\..\..\..\..\SampleReferenceCodeConsole\{fileName}.vb")
        End Function

        Private Sub PerformPassingTest(ByVal codeFileName As String)
            Dim testCode = GetTestCodeSegment(codeFileName)
            VerifyBasicDiagnostic(testCode)
        End Sub

        Private Sub PerformFailingTest(ByVal codeFileName As String,
                                           ByVal ruleToTest As String,
                                           ByVal message As String,
                                           ByVal lineNumber As Int32,
                                           ByVal columnNumber As Int32)
            Dim testCode = GetTestCodeSegment(codeFileName)
            Dim expected = New DiagnosticResult With {.Id = ruleToTest,
                .Message = String.Format(RuleMessage, message),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", lineNumber, columnNumber)
                    }
            }

            VerifyBasicDiagnostic(testCode, expected)
        End Sub

        Private Sub PerformFailingTest(ByVal codeFileName As String,
                                       ByVal firstRuleToTest As String,
                                       ByVal firstMessage As String,
                                       ByVal firstLineNumber As Int32,
                                       ByVal firstColumnNumber As Int32,
                                       ByVal secondRuleToTest As String,
                                       ByVal secondMessage As String,
                                       ByVal secondLineNumber As Int32,
                                       ByVal secondColumnNumber As Int32)
            Dim testCode = GetTestCodeSegment(codeFileName)
            Dim firstExpected = New DiagnosticResult With {.Id = firstRuleToTest,
                .Message = String.Format(RuleMessage, firstMessage),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", firstLineNumber, firstColumnNumber)
                    }
            }
            Dim secondExpected = New DiagnosticResult With {.Id = secondRuleToTest,
                .Message = String.Format(RuleMessage, secondMessage),
                .Severity = DiagnosticSeverity.Warning,
                .Locations = New DiagnosticResultLocation() {
                        New DiagnosticResultLocation("Test0.vb", secondLineNumber, secondColumnNumber)
                    }
            }

            VerifyBasicDiagnostic(testCode, firstExpected, secondExpected)
        End Sub

        Protected Overrides Function GetBasicCodeFixProvider() As CodeFixProvider
            Return New SampelAnalyzerVBCodeFixProvider()
        End Function

        Protected Overrides Function GetBasicDiagnosticAnalyzer() As DiagnosticAnalyzer
            Return New SampelAnalyzerVBAnalyzer()
        End Function
    End Class
End Namespace
