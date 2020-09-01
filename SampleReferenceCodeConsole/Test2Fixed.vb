Option Strict On
Option Explicit On

Public Class Test2Fixed
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        If (items?.Count).GetValueOrDefault > 0 AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function
End Class