Option Strict On
Option Explicit On

Public Class Test12P
    Sub EvaluatedHasValueFirst()
        Dim budgetEligibilityBO As BudgetEligibilityBO = Nothing
        WriteLog("budgetEligibilityBO.ProcessCode.HasValue AndAlso budgetEligibilityBO.ProcessCode <> ProcessCodes.NonContinuousEligibility...")
        If (budgetEligibilityBO.ProcessCode.HasValue AndAlso
            budgetEligibilityBO.ProcessCode <> ProcessCodes.NonContinuousEligibility) Then
            WriteLog("evaluated to TRUE")
        Else
            WriteLog("evaluated to FALSE")
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function

    Private Function AlwaysFalse() As Boolean
        Return False
    End Function

    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub

    Private Class BudgetEligibilityBO

        Public Property ProcessCode As System.Nullable(Of ProcessCodes)

    End Class

    Protected Enum ProcessCodes
        NonContinuousEligibility = 1
        RecurringBenefit = 2
    End Enum
End Class