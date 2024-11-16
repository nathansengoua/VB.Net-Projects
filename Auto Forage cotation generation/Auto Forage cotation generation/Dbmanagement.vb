Imports System.Data.SQLite
Imports System.IO

Public Class DbManagement
    Private clientID As Integer

    ' Flag to control whether the app is in development or deployment mode
    Private isDeployment As Boolean = True ' Set to True for deployment, False for development

    ' Paths for development and deployment
    Private appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App")
    Private dbFileName As String = "ForageQuotationDB.db"
    Private developmentDbFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "ForageQuotationDB.db")
    Private dbFilePath As String

    ' Connection string
    Private connectionString As String

    Public Sub New()
        Try
            ' Set the database file path depending on development or deployment
            If isDeployment Then
                dbFilePath = Path.Combine(appDataPath, dbFileName)
            Else
                dbFilePath = developmentDbFilePath
            End If

            ' Ensure the database exists in the appropriate location
            EnsureDatabaseExists()

            ' Set up the connection string
            connectionString = "Data Source=" & dbFilePath & ";Version=3;"
        Catch ex As Exception
            ' Handle any exceptions related to ensuring the database
            MessageBox.Show("An error occurred while initializing the database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Method to ensure the database is copied to AppData for deployment, or used directly in development
    Private Sub EnsureDatabaseExists()
        Try
            If isDeployment Then
                ' Ensure the directory in AppData exists for deployment
                If Not Directory.Exists(appDataPath) Then
                    Directory.CreateDirectory(appDataPath)
                End If

                ' Check if the database file already exists in AppData for deployment
                If Not File.Exists(dbFilePath) Then
                    ' Define the source path of the database file (from the installed folder during deployment)
                    Dim sourcePath As String = developmentDbFilePath ' Use development path as source for copying

                    ' Check if the source database exists before copying
                    If File.Exists(sourcePath) Then
                        ' Copy the database from the development directory to the AppData folder for deployment
                        File.Copy(sourcePath, dbFilePath)
                    Else
                        Throw New FileNotFoundException("Database file not found in the source path: " & sourcePath)
                    End If
                End If
            Else
                ' In development mode, ensure the database exists in the development folder
                If Not File.Exists(developmentDbFilePath) Then
                    Throw New FileNotFoundException("Database file not found in the development folder: " & developmentDbFilePath)
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

    Public Function SaveProject(Description As String, Region As Integer, depth As String) As Long
        Dim projectId As Long
        Try
            Using conn As SQLiteConnection = GetConnection()
                conn.Open()

                ' Insert into the Projects table without the reference_code
                Dim query As String = "INSERT INTO Projects (client_id, quotation_date, Description, Region_id, depthE) VALUES (@clientID, @quotationDate, @description, @region, @depth)"
                Dim cmd As New SQLiteCommand(query, conn)
                cmd.Parameters.AddWithValue("@clientID", clientID)
                cmd.Parameters.AddWithValue("@quotationDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@description", Description)
                cmd.Parameters.AddWithValue("@region", Region)
                cmd.Parameters.AddWithValue("@depth", depth)
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
                    p.depthE,
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

    Public Function GetOldUnitPrices() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SQLiteConnection = GetConnection()

                conn.Open()

                ' Query to get old unit prices from relevant tables
                Dim query As String = "
    SELECT 
        name AS Designation,
        CAST(unit_cost AS INTEGER) || ' FCFA' AS 'Ancien Prix Unitaire' 
    FROM
        DrillingMethods

    UNION ALL

    SELECT 
        cs.casing_id || cs.length_id||cs.specification_id AS Designation, 
        CAST(cs.unit_cost AS INTEGER) || ' FCFA' AS 'Ancien Prix Unitaire' 
    FROM
        Casing_specification cs

    UNION ALL

    SELECT 
        category_name AS Designation,
        CAST(unit_cost AS INTEGER) || ' FCFA'  AS 'Ancien Prix Unitaire' 
    FROM
        category

    UNION ALL

    SELECT 
        p.name AS Designation,
        CAST(p.unit_cost AS INTEGER) || ' FCFA'  AS 'Ancien Prix Unitaire' 
    FROM
        pump p;"



                ' Execute query and fill DataTable
                Using command As New SQLiteCommand(query, conn)
                    Using adapter As New SQLiteDataAdapter(command)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving old unit prices: " & ex.Message)

        End Try

        ' Return the filled DataTable
        Return dt
    End Function

    ' Method to insert new unit prices (Nouveau Prix Unitaire)
    Public Sub InsertNewUnitPrice(ByVal designation As String, ByVal newPrice As Double)
        Using conn As SQLiteConnection = GetConnection()
            Try
                conn.Open()

                ' General query to update category, drilling methods, or casing specification based on designation
                Dim updateQuery As String = ""

                ' Determine which table to update based on designation
                If designation.Contains("PVC") OrElse designation.Contains("Steel") Then
                    ' Update casing specification prices
                    updateQuery = "UPDATE Casing_specification SET unit_cost = @newUnitCost " &
                                  "WHERE specification_id IN (SELECT cs.specification_id " &
                                                             "FROM Casing_specification cs " &
                                                             "JOIN Casing c ON c.casing_id = cs.casing_id " &
                                                             "JOIN CasingLengths cl ON cl.length_id = cs.length_id " &
                                                             "WHERE c.casing_name || ' (' || cl.length || 'm, ' || cl.diameter || 'mm)' = @designation);"
                ElseIf designation = "Tripod Drilling" OrElse designation = "Rotary Drilling" Then
                    ' Update drilling method prices
                    updateQuery = "UPDATE DrillingMethods SET unit_cost = @newUnitCost WHERE name = @designation;"
                Else
                    ' Update category prices
                    updateQuery = "UPDATE category SET unit_cost = @newUnitCost WHERE category_name = @designation;"
                End If

                ' Execute the update query
                Dim cmd As New SQLiteCommand(updateQuery, conn)
                cmd.Parameters.AddWithValue("@newUnitCost", newPrice)
                cmd.Parameters.AddWithValue("@designation", designation)
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                MessageBox.Show("Error updating unit price: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub
End Class
