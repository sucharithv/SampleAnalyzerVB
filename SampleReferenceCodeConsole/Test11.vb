Option Strict On
Option Explicit On

Public Class Test11
    Sub EvaluateHasValue()
        Dim fooInstance As Foo = Nothing
        WriteLog("fooInstance.Status <> CaseStatus.Closed AndAlso fooInstance.Status <> CaseStatus.Rejected...")
        If fooInstance.Status <> CaseStatus.Closed AndAlso fooInstance.Status <> CaseStatus.Rejected Then
            WriteLog("fooInstance.Status <> CaseStatus.Closed AndAlso fooInstance.Status <> CaseStatus.Rejected evaluated to TRUE")
        Else
            WriteLog("fooInstance.Status <> CaseStatus.Closed AndAlso fooInstance.Status <> CaseStatus.Rejected evaluated to FALSE")
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

    Private Class Foo

        Public Property Status As System.Nullable(Of CaseStatus)

    End Class

    Protected Enum CaseStatus As Byte
        InProgress = 1
        Open = 2
        Rejected = 3
        Closed = 4
    End Enum
End Class
