Imports System
Imports System.Net.Http
Imports System.Threading.Tasks

Module GitHubActivityCLI
    Sub Main()
        Console.Write("Enter GitHub username: ")
        Dim username As String = Console.ReadLine()

        Dim task As Task = FetchAndDisplayActivity(username)
        task.Wait()
    End Sub

    Async Function FetchAndDisplayActivity(username As String) As Task
        Dim url As String = $"https://api.github.com/users/{username}/events"
        Dim client As New HttpClient()
        client.DefaultRequestHeaders.Add("User-Agent", "GitHubActivityCLI")

        Try
            Dim response As HttpResponseMessage = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim content As String = Await response.Content.ReadAsStringAsync()
                ParseAndDisplayActivity(content)
            ElseIf response.StatusCode = Net.HttpStatusCode.NotFound Then
                Console.WriteLine("User not found. Please check the username and try again.")
            Else
                Console.WriteLine("Failed to fetch data. HTTP response code: " & response.StatusCode)
            End If
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Function

    Sub ParseAndDisplayActivity(jsonResponse As String)
        Dim events As String() = jsonResponse.Split(New String() {"},{"}, StringSplitOptions.None)
        For Each e As String In events
            If e.Contains("""type"":""PushEvent""") Then
                Dim repoName As String = ExtractRepoName(e)
                Console.WriteLine("Pushed commits to " & repoName)
            ElseIf e.Contains("""type"":""IssuesEvent""") Then
                Dim action As String = ExtractAction(e)
                Dim repoName As String = ExtractRepoName(e)
                Console.WriteLine(action & " an issue in " & repoName)
            ElseIf e.Contains("""type"":""WatchEvent""") Then
                Dim repoName As String = ExtractRepoName(e)
                Console.WriteLine("Starred " & repoName)
            End If
        Next
    End Sub

    Function ExtractRepoName(e As String) As String
        Dim start As Integer = e.IndexOf("""name"":""") + 8
        Dim ends As Integer = e.IndexOf("""", start)
        Return If(start >= 0 AndAlso ends > start, e.Substring(start, ends - start), "unknown repository")
    End Function

    Function ExtractAction(e As String) As String
        Dim start As Integer = e.IndexOf("""action"":""") + 10
        Dim ends As Integer = e.IndexOf("""", start)
        Return If(start >= 0 AndAlso ends > start, e.Substring(start, ends - start), "Performed")
    End Function
End Module
