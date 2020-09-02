Option Strict On
Option Explicit On

Public Class TestXXX
    Sub EvaluateNullableBool()
        Dim newNullableBool As New Boolean?
        WriteLog("Evaluating If Not newNullableBool Is Nothing...")
        If Nothing AndAlso True Then
            WriteLog("Not newNullableBool Is Nothing evaluated to TRUE")
        Else
            WriteLog("Not newNullableBool Is Nothing evaluated to FALSE")
        End If
    End Sub

    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub
End Class
