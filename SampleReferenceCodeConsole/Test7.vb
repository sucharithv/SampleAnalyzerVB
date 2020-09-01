Option Strict On
Option Explicit On
Public Class Test7
    Sub EvaluateNullableBool()
        Dim newNullableBool As New Boolean?
        WriteLog("Evaluating If newNullableBool IsNot Nothing...")
        If newNullableBool IsNot Nothing Then
            WriteLog("newNullableBool IsNot Nothing evaluated to TRUE")
        Else
            WriteLog("newNullableBool IsNot Nothing evaluated to FALSE")
        End If
    End Sub
    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
