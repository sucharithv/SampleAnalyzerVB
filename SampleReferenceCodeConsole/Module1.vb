Option Strict On
Option Explicit On

Module Module1
    Private Const BaselineActionCode As String = "baseline"
    Private Const UpdatedActionCode As String = "updated"

    Sub Main()
        Dim budgetGroupObject As New BudgetGroupFsBO With
        {
         .BudgetGroupIndividualRollup = New List(Of BudgetGroupClientRollup) From
         {
          {New BudgetGroupClientRollup With {.ActionCode = BaselineActionCode, .IndividualIncomeExpenseResources = Nothing}},
          {New BudgetGroupClientRollup With {.ActionCode = UpdatedActionCode, .IndividualIncomeExpenseResources = Nothing}},
          {New BudgetGroupClientRollup With {.ActionCode = BaselineActionCode,
                                             .IndividualIncomeExpenseResources = New List(Of IndividualIncomeExpenseResource) From
                                             {New IndividualIncomeExpenseResource With {.ActionCode = UpdatedActionCode}}
                                            }
          }
         }
        }

        WriteLog("Executing VB Console")
        EvaluateConditions()
        'SimpleCode()
        ' CopyBaselineIndividualIncomeExpenseResources(budgetGroupObject)
        Console.ReadLine()
    End Sub


    Private Sub EvaluateConditions()
        Dim newNullableBool As New Boolean?
        Dim nullableBoolNotInitialized As Boolean?
        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool ...")
        If newNullableBool Then
            WriteLog("newNullableBool evaluated to TRUE")
        Else
            WriteLog("newNullableBool  evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool = False ...")
        If newNullableBool = False Then
            WriteLog("newNullableBool = False evaluated to TRUE")
        Else
            WriteLog("newNullableBool = False  evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool = True ...")
        If newNullableBool = True Then
            WriteLog("newNullableBool = True evaluated to TRUE")
        Else
            WriteLog("newNullableBool = True  evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If Not newNullableBool ...")
        If Not newNullableBool Then
            WriteLog("Not newNullableBool evaluated to TRUE")
        Else
            WriteLog("Not newNullableBool  evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If Not  newNullableBool = False ...")
        If Not newNullableBool = False Then
            WriteLog("Not newNullableBool = False evaluated to TRUE")
        Else
            WriteLog("Not newNullableBool = False  evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If Not newNullableBool = True ...")
        If Not newNullableBool = True Then
            WriteLog("Not newNullableBool = True evaluated to TRUE")
        Else
            WriteLog("Not newNullableBool = True  evaluated to FALSE")
        End If
        WriteSeparator()


        Dim nullableInteger As Int16?

        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If nullableInteger = 0 ...")
        If nullableInteger = 0 Then
            WriteLog("nullableInteger = 0 evaluated to TRUE")
        Else
            WriteLog("nullableInteger = 0  evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If nullableInteger > 0 ...")
        If nullableInteger > 0 Then
            WriteLog("nullableInteger > 0 evaluated to TRUE")
        Else
            WriteLog("nullableInteger > 0  evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If nullableInteger <> 0 ...")
        If nullableInteger <> 0 Then
            WriteLog("nullableInteger <> 0 evaluated to TRUE")
        Else
            WriteLog("nullableInteger <> 0  evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If nullableInteger < 0 ...")
        If nullableInteger < 0 Then
            WriteLog("nullableInteger < 0 evaluated to TRUE")
        Else
            WriteLog("nullableInteger < 0  evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool AndAlso AlwaysTrue() ...")
        If newNullableBool AndAlso AlwaysTrue() Then
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()


        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool AndAlso AlwaysTrue() ...")
        If newNullableBool AndAlso AlwaysTrue() Then
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("Evaluating: If nullableBoolNotInitialized AndAlso AlwaysTrue() ...")
        If nullableBoolNotInitialized AndAlso AlwaysTrue() Then
            WriteLog("nullableBoolNotInitialized AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("nullableBoolNotInitialized AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()
    End Sub
    Private Sub SimpleCode()
        WriteLog("Entered SimpleCode")

        Dim newNullableBool As New Boolean?
        Dim nullableBoolNotInitialized As Boolean?
        WriteLog("")
        WriteLog("-----------------------------")
        WriteLog("Evaluating If newNullableBool AndAlso AlwaysTrue() ...")
        If newNullableBool AndAlso AlwaysTrue() Then
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("newNullableBool AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("Evaluating: If nullableBoolNotInitialized AndAlso AlwaysTrue() ...")
        If nullableBoolNotInitialized AndAlso AlwaysTrue() Then
            WriteLog("nullableBoolNotInitialized AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("nullableBoolNotInitialized AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()

        WriteLog("Evaluating: If Nothing Andalso AlwaysTrue() ...")
        If Nothing AndAlso AlwaysTrue() Then
            WriteLog("Nothing AndAlso AlwaysTrue() evaluated to TRUE")
        Else
            WriteLog("Nothing AndAlso AlwaysTrue() evaluated to FALSE")
        End If
        WriteSeparator()

        Dim items As List(Of Integer) = Nothing

        WriteLog("Evaluating: If (items?.Count).GetValueOrDefault > 0 AndAlso AlwaysTrue() ...")
        If (items?.Count).GetValueOrDefault > 0 AndAlso AlwaysTrue() Then
            WriteLog("Entered TRUE part of ""If items?.Count > 0 AndAlso AlwaysTrue()"" condition.")
        Else
            WriteLog("Entered ELSE part of  ""If items?.Count > 0 AndAlso AlwaysTrue()"" condition.")
        End If
        WriteSeparator()


        WriteLog("Evaluating: If items?.Count > 0 AndAlso AlwaysTrue() ...")
        If items?.Count > 0 AndAlso AlwaysTrue() Then
            WriteLog("Entered TRUE part of ""If items?.Count > 0 AndAlso AlwaysTrue()"" condition.")
        Else
            WriteLog("Entered ELSE part of  ""If items?.Count > 0 AndAlso AlwaysTrue()"" condition.")
        End If

        WriteSeparator()

        WriteLog("Evaluating: If items?.Count > 0 ...")
        If items?.Count > 0 Then
            WriteLog("Entered TRUE part of  ""If items?.Count > 0 "" condition.")
        Else
            WriteLog("Entered ELSE part of  ""If items?.Count > 0 "" condition.")
        End If
        WriteLog("=============================")
        WriteLog("")

        WriteLog("Exiting SimpleCode")
    End Sub

    Private Sub WriteSeparator()
        WriteLog("=============================")
        WriteLog("")
        WriteLog("-----------------------------")
    End Sub

    Private Function AlwaysTrue() As Boolean
        WriteLog("Executing ""AlwaysTrue"" method. ***THIS SHOULD NOT EXECUTE***")
        Return True
    End Function

    Private Sub CopyBaselineIndividualIncomeExpenseResources(ByVal budgetGroupObject As BudgetGroupFsBO)
        'Check if a updated roll up exists
        If budgetGroupObject?.BudgetGroupIndividualRollup?.Count > 0 AndAlso
            budgetGroupObject.BudgetGroupIndividualRollup.Exists(Function(rollup) rollup.ActionCode = UpdatedActionCode) Then
            'Loop roll-ups
            For Each authorizedRollup As BudgetGroupClientRollup In budgetGroupObject?.BudgetGroupIndividualRollup
                'Check if non-baseline indivIncomeExpense record exists under baseline roll up
                If authorizedRollup.ActionCode = BaselineActionCode AndAlso authorizedRollup?.IndividualIncomeExpenseResources?.Count > 0 AndAlso
                    authorizedRollup.IndividualIncomeExpenseResources.Exists(Function(incomeExpense) incomeExpense.ActionCode <> BaselineActionCode) Then
                    WriteLog("Inside if")
                End If
            Next
        End If
    End Sub

    Sub WriteLog(ByVal message As String)
        Debug.WriteLine(message)
        Console.WriteLine(message)
    End Sub
End Module




Public Class BudgetGroupFsBO
    Public Property BudgetGroupIndividualRollup() As List(Of BudgetGroupClientRollup)

End Class

Public Class BudgetGroupClientRollup
    Public Property ActionCode As String
    Public Property IndividualIncomeExpenseResources As List(Of IndividualIncomeExpenseResource)
End Class

Public Class IndividualIncomeExpenseResource
    Public Property ActionCode As String
End Class
