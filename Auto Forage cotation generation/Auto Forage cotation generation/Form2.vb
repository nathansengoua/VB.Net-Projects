Imports System.ComponentModel
Imports System.Globalization
Imports System.Threading

Public Class Form2
    Private project_id As Long
    Private db As New DbManagement()

    ' Function to update the UI with the global language
    Private Sub ApplyLanguage()
        Dim cultureInfo As CultureInfo = New CultureInfo(GlobalSettings.selectedlanguage)
        Thread.CurrentThread.CurrentUICulture = cultureInfo

        ' Update UI elements with the selected language
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(Form2))
        For Each control As Control In Me.Controls
            resources.ApplyResources(control, control.Name, cultureInfo)
        Next
    End Sub

    ' Form Load event
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyLanguage() ' Apply the selected language when the form loads
    End Sub


    Private Sub Guna2GradientButton3_Click_1(sender As Object, e As EventArgs) Handles saveButton.Click
        ' Declare and assign the values from text boxes
        Dim name, location, contact, Description As String
        Dim DepthEstimation, Region As Integer
        name = nameTextBox.Text
        location = locationTextBox.Text
        contact = telTextBox.Text
        Description = Descriptiontextbox.Text
        Region = (regiontextbox.SelectedIndex) + 1
        DepthEstimation = Integer.Parse(Depthtextbox.Text)

        ' Check if the fields are not empty before proceeding
        If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(location) OrElse String.IsNullOrWhiteSpace(Description) OrElse String.IsNullOrWhiteSpace(contact) OrElse String.IsNullOrWhiteSpace(DepthEstimation) Then
            MessageBox.Show("Please fill in all fields before saving.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Try inserting client information into the database
        Try
            If db.InsertClientInfo(name, location, contact) Then
                project_id = db.SaveProject(Description, Region)
                ' Successfully inserted, close this form and show main1 form
                MessageBox.Show("Success" & Region)
                ' Create an instance of main1 and pass the project_id via the constructor
                Dim mainForm As New main1(project_id)

                ' Show the main1 form
                mainForm.Show()

                Me.Close()
                Form1.Close()

            Else
                ' Handle insertion failure
                MessageBox.Show("An error occurred while saving the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            ' Handle any unexpected exceptions
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cancelbtn_Click(sender As Object, e As EventArgs) Handles cancelbtn.Click
        Me.Close()

    End Sub

    Private Sub Guna2Shapes6_Click(sender As Object, e As EventArgs) Handles Guna2Shapes6.Click

    End Sub

    Private Sub Guna2Shapes4_Click(sender As Object, e As EventArgs) Handles Guna2Shapes4.Click

    End Sub

    Private Sub Guna2Shapes1_Click(sender As Object, e As EventArgs) Handles Guna2Shapes1.Click

    End Sub
End Class