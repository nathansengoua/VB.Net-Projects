Imports System

Module Program
    Sub Main()
        Dim manager = New TaskManager()

        While True
            Console.WriteLine("Enter command (add, update, delete, mark, list, exit):")
            Dim command = Console.ReadLine()

            Select Case command.ToLower()
                Case "add"
                    Console.WriteLine("Enter task description:")
                    Dim description = Console.ReadLine()
                    manager.AddTask(description)

                Case "update"
                    Console.WriteLine("Enter task ID:")
                    Dim id = Integer.Parse(Console.ReadLine())
                    Console.WriteLine("Enter new description:")
                    Dim newDescription = Console.ReadLine()
                    manager.UpdateTask(id, newDescription)

                Case "delete"
                    Console.WriteLine("Enter task ID:")
                    Dim id = Integer.Parse(Console.ReadLine())
                    manager.DeleteTask(id)

                Case "mark"
                    Console.WriteLine("Enter task ID:")
                    Dim id = Integer.Parse(Console.ReadLine())
                    Console.WriteLine("Enter status (todo, in-progress, done):")
                    Dim status = Console.ReadLine()
                    manager.MarkTask(id, status)

                Case "list"
                    Console.WriteLine("Enter status filter (all, todo, in-progress, done):")
                    Dim statusFilter = Console.ReadLine()
                    manager.ListTasks(statusFilter)

                Case "exit"
                    Exit While

                Case Else
                    Console.WriteLine("Unknown command")
            End Select
        End While
    End Sub
End Module
