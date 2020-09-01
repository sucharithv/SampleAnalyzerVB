Option Strict On
Option Explicit On
Public Class Test5
    Sub EvaluateNunnableBool()
        Dim newNullableBool As New Boolean?
        WriteLog("Evaluating If newNullableBool ...")
        If newNullableBool Then
            WriteLog("newNullableBool evaluated to TRUE")
        Else
            WriteLog("newNullableBool  evaluated to FALSE")
        End If

    End Sub
    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
