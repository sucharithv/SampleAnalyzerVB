Option Strict On
Option Explicit On

Public Class Test4
    Private items As System.Collections.Generic.List(Of Integer) = Nothing

    Public Sub SimpleCode()
        Dim record As Foo = Nothing
        If record?.EffectiveBeginDate.HasValue AndAlso AlwaysTrue() Then
        End If
    End Sub

    Private Function AlwaysTrue() As Boolean
        Return True
    End Function

    Private Class Foo
        Private _effectiveBeginDate As System.Nullable(Of Date)
        Public Property EffectiveBeginDate() As System.Nullable(Of Date)
            Get
                Return _effectiveBeginDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _effectiveBeginDate = value
            End Set
        End Property
    End Class
End Class
