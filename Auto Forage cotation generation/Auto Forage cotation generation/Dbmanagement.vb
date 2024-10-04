Imports System.Data.SQLite
Imports System.IO

Public Class DbManagement
    Private clientID As Integer
    ' Path to the SQLite database in AppData
    Private appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App")
    Private dbFileName As String = "ForageQuotationDB.db"
    Private dbFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "ForageQuotationDB.db")
    'Path.Combine(appDataPath, dbFileName)
    Private connectionString As String

    Public Sub New()
        Try
            ' Ensure the database exists in AppData
            EnsureDatabaseExists()

            ' Set up the connection string
            connectionString = "Data Source=" & dbFilePath & ";Version=3;"
        Catch ex As Exception
            ' Handle any exceptions related to ensuring the database
            MessageBox.Show("An error occurred while initializing the database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Method to ensure the database is copied to AppData if it doesn't exist
    Private Sub EnsureDatabaseExists()
        Try
            ' Ensure the directory in AppData exists
            If Not Directory.Exists(appDataPath) Then
                Directory.CreateDirectory(appDataPath)
            End If

            ' Check if the database file already exists in AppData
            If Not File.Exists(dbFilePath) Then
                ' Define the relative source path of the database file (assuming it's in the project folder)
                Dim sourcePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "ForageQuotationDB.db")

                ' Check if the source database exists before copying
                If File.Exists(sourcePath) Then
                    ' Copy the database from the source directory to the AppData folder
                    File.Copy(sourcePath, dbFilePath)
                Else
                    Throw New FileNotFoundException("Database file not found in the source path: " & sourcePath)
                End If
            End If
        Catch ex As DirectoryNotFoundException
            MessageBox.Show("An error occurred while accessing the directory: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As FileNotFoundException
            MessageBox.Show("An error occurred while copying the database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Method to establish a connection to the SQLite database
    Public Function GetConnection() As SQLiteConnection
        Try
            Return New SQLiteConnection(connectionString)
        Catch ex As Exception
            MessageBox.Show("Failed to establish a database connection: " & ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    ' Method to insert client information into the database
    Public Function InsertClientInfo(name As String, location As String, tel As String) As Boolean
        Try
            ' Open a connection to the database
            Using conn As SQLiteConnection = GetConnection()
                If conn IsNot Nothing Then
                    conn.Open()

                    ' Insert query
                    Dim query As String = "INSERT INTO Clients (name, address, phone) VALUES (@n, @l, @t)"

                    Using cmd As New SQLiteCommand(query, conn)
                        ' Add parameters to the command
                        cmd.Parameters.AddWithValue("@n", name)
                        cmd.Parameters.AddWithValue("@l", location)
                        cmd.Parameters.AddWithValue("@t", tel)

                        ' Execute the insert command
                        cmd.ExecuteNonQuery()
                        clientID = conn.LastInsertRowId
                        conn.Close()
                    End Using
                Else
                    Return False
                End If
            End Using

            ' Return true if the insert was successful
            Return True
        Catch ex As SQLiteException
            MessageBox.Show("SQLite error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Function SaveProject(Description As String, Region As Integer) As Long
        Dim projectId As Long
        Try
            Using conn As SQLiteConnection = GetConnection()
                conn.Open()

                ' Insert into the Projects table without the reference_code
                Dim query As String = "INSERT INTO Projects (client_id, quotation_date, Description, Region_id) VALUES (@clientID, @quotationDate, @description, @region)"
                Dim cmd As New SQLiteCommand(query, conn)
                cmd.Parameters.AddWithValue("@clientID", clientID)
                cmd.Parameters.AddWithValue("@quotationDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@description", Description)
                cmd.Parameters.AddWithValue("@region", Region)
                cmd.ExecuteNonQuery()

                ' Get the last inserted project_id
                projectId = conn.LastInsertRowId

                ' Generate the reference based on the project ID
                Dim quotationReference As String = $"QUO-{DateTime.Now.ToString("yyyyMMdd")}-{projectId.ToString("D5")}"

                ' Update the Projects table with the generated reference
                Dim updateQuery As String = "UPDATE Projects SET reference_code = @referenceCode WHERE project_id = @projectID"
                Dim updateCmd As New SQLiteCommand(updateQuery, conn)
                updateCmd.Parameters.AddWithValue("@referenceCode", quotationReference)
                updateCmd.Parameters.AddWithValue("@projectID", projectId)

                updateCmd.ExecuteNonQuery()
                conn.Close()

                ' Return true if the insert was successful

            End Using
            ' Return true if the insert was successful
        Catch ex As SQLiteException
            MessageBox.Show("SQLite error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return projectId
    End Function

    Public Function GetProjectById(projectId As Integer) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SQLiteConnection = GetConnection()
                conn.Open()

                Dim query As String = "
                SELECT 
                    P.Description, 
                    strftime('%Y-%m-%d', P.quotation_date) AS QuotationDate, 
                    P.reference_code, 
                    C.name, 
                    C.address,
                    C.phone
                FROM 
                    Projects P 
                JOIN 
                    Clients C ON P.client_id = C.client_id 
                WHERE 
                    P.project_id = @project_id"

                Using command As New SQLiteCommand(query, conn)
                    command.Parameters.AddWithValue("@project_id", projectId)

                    Using adapter As New SQLiteDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As SQLiteException
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        Return dt
    End Function

End Class
