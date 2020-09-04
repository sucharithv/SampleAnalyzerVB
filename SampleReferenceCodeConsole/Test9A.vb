Option Strict On
Option Explicit On

Public Class Test9A
    Sub EvaluateNullableBool()
        Dim newNullableBool As New Boolean?
        WriteLog("Evaluating newNullableBool.HasValue AndAlso newNullableBool.Value = True OrElse newNullableBool.GetValueOrDefault = True...")
        If newNullableBool.HasValue AndAlso newNullableBool = True OrElse newNullableBool = False Then
            WriteLog("newNullableBool.HasValue AndAlso newNullableBool.Value = True OrElse newNullableBool.GetValueOrDefault = True evaluated to TRUE")
        Else
            WriteLog("newNullableBool.HasValue AndAlso newNullableBool.Value = True OrElse newNullableBool.GetValueOrDefault = True evaluated to FALSE")
        End If
    End Sub

    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
