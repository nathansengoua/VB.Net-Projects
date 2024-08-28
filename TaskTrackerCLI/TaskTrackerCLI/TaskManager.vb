Imports System.IO
Imports System.Text.Json

Public Class TaskManager
    Private Const FilePath As String = "tasks.json"
    Private Tasks As List(Of Task)

    Public Sub New()
        Tasks = New List(Of Task)()
        LoadTasks()
    End Sub

    Private Sub LoadTasks()
        If File.Exists(FilePath) Then
            Dim json = File.ReadAllText(FilePath)
            Tasks = JsonSerializer.Deserialize(Of List(Of Task))(json)
        End If
    End Sub

    Private Sub SaveTasks()
        Dim json = JsonSerializer.Serialize(Tasks)
        File.WriteAllText(FilePath, json)
    End Sub

    Public Sub AddTask(description As String)
        Dim id = If(Tasks.Count > 0, Tasks.Last().Id1 + 1, 1)
        Dim task = New Task(id, description)
        Tasks.Add(task)
        SaveTasks()
        Console.WriteLine($"Task added successfully (ID: {id})")
    End Sub

    Public Sub UpdateTask(id As Integer, newDescription As String)
        Dim task = Tasks.FirstOrDefault(Function(t) t.Id1 = id)
        If task IsNot Nothing Then
            task.Description1 = newDescription
            task.UpdatedAt1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            SaveTasks()
            Console.WriteLine($"Task updated successfully (ID: {id})")
        Else
            Console.WriteLine($"Task not found (ID: {id})")
        End If
    End Sub

    Public Sub DeleteTask(id As Integer)
        Tasks.RemoveAll(Function(t) t.Id1 = id)
        SaveTasks()
        Console.WriteLine($"Task deleted successfully (ID: {id})")
    End Sub

    Public Sub MarkTask(id As Integer, status As String)
        Dim task = Tasks.FirstOrDefault(Function(t) t.Id1 = id)
        If task IsNot Nothing Then
            task.Status1 = status
            task.UpdatedAt1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            SaveTasks()
            Console.WriteLine($"Task marked as {status} (ID: {id})")
        Else
            Console.WriteLine($"Task not found (ID: {id})")
        End If
    End Sub

    Public Sub ListTasks(Optional statusFilter As String = "all")
        For Each task In Tasks
            If statusFilter = "all" OrElse task.Status1 = statusFilter Then
                Console.WriteLine(task.ToJson())
            End If
        Next
    End Sub
End Class
