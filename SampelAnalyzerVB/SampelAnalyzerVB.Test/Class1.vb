Option Strict On
Option Explicit On

Public Class Class1
    Private items As List(Of Integer)

    Public Sub Main()
        items = Nothing
        SimpleCode()
        Console.ReadLine()
    End Sub

    Private Sub SimpleCode()
        If items?.Count > 0 AndAlso AlwaysTrue() Then
            WriteLog(String.Empty)
        Else
            WriteLog(String.Empty)
        End If
    End Sub

    Private Function AlwaysTrue() As Nullable(Of Boolean)
        WriteLog(String.Empty)
        Return True
    End Function

    Sub WriteLog(ByVal message As String)
        Debug.WriteLine(message)
        Console.WriteLine(message)
    End Sub
End Class