Option Strict On
Option Explicit On
Public Class Test6
    Sub EvaluateNullableBool()
        Dim newNullableBool As New Boolean?
        WriteLog("Evaluating If newNullableBool Is Nothing...")
        If newNullableBool Is Nothing Then
            WriteLog("newNullableBool Is Nothing evaluated to TRUE")
        Else
            WriteLog("newNullableBool Is Nothing evaluated to FALSE")
        End If
    End Sub
    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
