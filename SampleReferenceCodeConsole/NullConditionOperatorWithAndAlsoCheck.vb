Option Strict On
Option Explicit On
Imports System.Collections.Generic

Public Class NullConditionOperatorWithAndAlsoCheck
    Private items As List(Of Integer) = Nothing

    Public Sub SimpleCode()
        If items?.Count > 0 AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function
End Class
'Basic Scenario - items?.Count > 0