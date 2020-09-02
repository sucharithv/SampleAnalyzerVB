Option Strict On
Option Explicit On

Public Class IsNothingCheck
    Sub Test()
        Dim address As AddressBO = Nothing
        WriteLog("Evaluating address IsNot Nothing AndAlso address.GISValidationDate Is Nothing...")
        If address IsNot Nothing AndAlso address.GISValidationDate Is Nothing Then
            WriteLog("address IsNot Nothing AndAlso address.GISValidationDate Is Nothing evaluated to TRUE")
        Else
            WriteLog("address IsNot Nothing AndAlso address.GISValidationDate Is Nothing evaluated to FALSE")
        End If
    End Sub

    Sub WriteLog(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
    End Sub

    Private Class AddressBO
        Public Property GISValidationDate As Date?
    End Class
End Class
