Public Class Form2
    Private project_id As Long
    Private db As New DbManagement()

    Private Async Sub Guna2GradientButton3_Click_1(sender As Object, e As EventArgs) Handles saveButton.Click
        ' Declare and assign the values from text boxes
        Dim name As String = nameTextBox.Text
        Dim location As String = locationTextBox.Text
        Dim contact As String = telTextBox.Text
        Dim Description As String = Descriptiontextbox.Text
        Dim DepthEstimation As Integer = Integer.Parse(Depthtextbox.Text)
        Dim Region As Integer = (regiontextbox.SelectedIndex) + 1

        ' Check if the fields are not empty before proceeding
        If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(location) OrElse
           String.IsNullOrWhiteSpace(Description) OrElse String.IsNullOrWhiteSpace(contact) OrElse
           DepthEstimation <= 0 Then ' Adjusted for integer check
            MessageBox.Show("S.V.P remplisser toute les caase avant de valider.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Create and show the loading form (non-blocking)
        Dim loadingForm As New LoadingForm()
        loadingForm.Show() ' Show loading form immediately

        ' Ensure the loading form stays responsive by running background tasks
        Await Task.Run(Async Function()
                           Try
                               ' Run database insertion and project saving on a background thread
                               Dim insertionSuccess As Boolean = Await Task.Run(Function() db.InsertClientInfo(name, location, contact))
                               If insertionSuccess Then
                                   Dim projectId As Integer = Await Task.Run(Function() db.SaveProject(Description, Region, DepthEstimation))

                                   ' Once done, close loading form and show the main form (on the UI thread)
                                   Me.Invoke(Sub()
                                                 Dim lieu As String = regiontextbox.SelectedItem.ToString & " - " & lieutbox.Text
                                                 Dim mainForm As New main1(projectId, lieu)  ' Create the main form with project ID
                                                 mainForm.WindowState = FormWindowState.Maximized ' Maximize the main form
                                                 mainForm.Show() ' Show main form
                                                 loadingForm.Close() ' Close loading form
                                                 Me.Close() ' Optionally close Form2
                                                 Form1.Close() ' Close Form1 if necessary
                                             End Sub)
                               Else
                                   ' Handle insertion failure (on the UI thread)
                                   Me.Invoke(Sub()
                                                 loadingForm.Close() ' Close loading form
                                                 MessageBox.Show("An error occurred while saving the data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                             End Sub)
                               End If
                           Catch ex As Exception
                               ' Handle any unexpected exceptions and close loadingForm (on the UI thread)
                               Me.Invoke(Sub()
                                             loadingForm.Close() ' Close loading form
                                             MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End Sub)
                           End Try
                       End Function)
    End Sub

    Private Sub cancelbtn_Click(sender As Object, e As EventArgs) Handles cancelbtn.Click
        Me.Close()
    End Sub

    Private Sub regiontextbox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles regiontextbox.SelectedIndexChanged
        ville.Text = regiontextbox.SelectedItem.ToString & " - "

    End Sub

    Private Sub generbtn_Click(sender As Object, e As EventArgs) Handles generbtn.Click
        Descriptiontextbox.Text = "Realisation D'un Forage de " & Depthtextbox.Text & "Metre de Profondeur Dans la ville de " & regiontextbox.SelectedItem.ToString & " - " & lieutbox.Text
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ville.Text = regiontextbox.SelectedItem.ToString & " - "
    End Sub
End Class