Imports System.Data.SQLite
Imports System.IO
Imports System.Web.UI.WebControls

Public Class main1

    Private db As New DbManagement
    Private quotationGen As QuotationGeneration
    Private projectID As Integer

    ' Constructor that accepts projectID and initializes quotationGen
    Public Sub New(Optional ByVal passedProjectID As Integer = 0)
        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize quotationGen
        quotationGen = New QuotationGeneration()

        ' Store the passed projectID in the form's variable
        projectID = passedProjectID
    End Sub

    Private Sub main1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim projectData As DataTable = db.GetProjectById(projectID)

            If projectData.Rows.Count > 0 Then
                Dim row As DataRow = projectData.Rows(0)
                descriptionTextBox.Text = row("Description").ToString()
                DateTextBox.Text = row("QuotationDate").ToString()
                RefTextBox.Text = row("reference_code").ToString()
                clientnameTextBox.Text = row("name").ToString()
                clientaddressTextBox.Text = row("address").ToString()
                clientphonetextbox.Text = row("phone").ToString()
            Else
                MessageBox.Show("No project found with the specified ID.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading project details: " & ex.Message)
        End Try
    End Sub

    Private Sub SaveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem1.Click
        Try
            ' Extract value from text fields
            Dim name As String = clientnameTextBox.Text.ToString()
            Dim address As String = clientaddressTextBox.Text.ToString()
            Dim tel As String = clientphonetextbox.Text.ToString()
            Dim dates As String = DateTextBox.Text.ToString()
            Dim ref As String = RefTextBox.Text.ToString()
            Dim descrip As String = descriptionTextBox.Text.ToString()
            Dim loc As String = projectlocation.Text.ToString()

            ' Replace client and project details in the document
            quotationGen.ReplaceClientAndProjectDetails(dates, ref, name, address, tel, descrip, "50", loc)

            Using saveFileDialog As New SaveFileDialog()
                saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf"
                saveFileDialog.Title = "Save PDF File"
                If saveFileDialog.ShowDialog() = DialogResult.OK Then
                    quotationGen.SaveDocumentAsPdf(saveFileDialog.FileName)
                End If
            End Using

            MessageBox.Show("Quotation PDF generated successfully.")
            LoadPdfDocument(ref)
        Catch ex As Exception
            MessageBox.Show("Error generating PDF: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPdfDocument(quotationReference As String)
        ' Construct the output PDF path using the quotationReference
        Dim outputPdfPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Auto Forage Quotation App", quotationReference & ".pdf")

        WebBrowser1.Navigate(outputPdfPath)
    End Sub
End Class
