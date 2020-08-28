Option Strict On
Option Explicit On

Public Class Class2
    Private items As System.Collections.Generic.List(Of Integer)

    Public Sub Main1()
        items = Nothing
        SimpleCode1()
        System.Console.ReadLine()
    End Sub

    Private Sub SimpleCode1()
        If items?.Count > 0 AndAlso AlwaysTrue1() Then
            WriteLog1(String.Empty)
        Else
            WriteLog1(String.Empty)
        End If
    End Sub

    Private Function AlwaysTrue1() As Boolean
        WriteLog1(String.Empty)
        Return True
    End Function

    Sub WriteLog1(ByVal message As String)
        System.Diagnostics.Debug.WriteLine(message)
        System.Console.WriteLine(message)
    End Sub
End Class