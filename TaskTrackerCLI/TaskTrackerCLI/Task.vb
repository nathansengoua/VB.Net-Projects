Imports System.Text.Json
Public Class Task
    Private Id As Integer
    Private Description As String
    Private Status As String ' "todo", "in-progress", "done"
    Private CreatedAt As String
    Private UpdatedAt As String

    Public Property Id1 As Integer
        Get
            Return Id
        End Get
        Set(value As Integer)
            Id = value
        End Set
    End Property

    Public Property Description1 As String
        Get
            Return Description
        End Get
        Set(value As String)
            Description = value
        End Set
    End Property

    Public Property Status1 As String
        Get
            Return Status
        End Get
        Set(value As String)
            Status = value
        End Set
    End Property

    Public Property CreatedAt1 As String
        Get
            Return CreatedAt
        End Get
        Set(value As String)
            CreatedAt = value
        End Set
    End Property

    Public Property UpdatedAt1 As String
        Get
            Return UpdatedAt
        End Get
        Set(value As String)
            UpdatedAt = value
        End Set
    End Property

    Public Sub New(id As Integer, description As String)
        Me.Id = id
        Me.Description = description
        Me.Status = "todo"
        Me.CreatedAt = GetCurrentTime()
        Me.UpdatedAt = Me.CreatedAt

    End Sub

    Public Function getter() As String

    End Function



    Private Function GetCurrentTime() As String
        Return DateTime.Now.ToString("G")
    End Function

    Public Function ToJson() As String
        Return JsonSerializer.Serialize(Me)
    End Function

    Public Shared Function FromJson(json As String) As Task
        Return JsonSerializer.Deserialize(Of Task)(json)
    End Function
End Class
