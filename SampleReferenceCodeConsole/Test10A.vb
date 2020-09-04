Option Strict On
Option Explicit On

Public Class Test10A
    Sub EvaluateHasValueWithNullConditionEqualsTrue()
        Dim fooInstance As Foo = Nothing
        WriteLog("fooInstance?.BeginDate.HasValue AndAlso fooInstance.BeginDate > System.DateTime.MinValue...")
        If fooInstance?.BeginDate.HasValue = True AndAlso fooInstance.BeginDate > System.DateTime.MinValue Then
            WriteLog("fooInstance?.BeginDate.HasValue AndAlso fooInstance.BeginDate > System.DateTime.MinValue evaluated to TRUE")
        Else
            WriteLog("fooInstance?.BeginDate.HasValue AndAlso fooInstance.BeginDate > System.DateTime.MinValue evaluated to FALSE")
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

        Public Property BeginDate() As System.Nullable(Of Date)

    End Class
End Class
