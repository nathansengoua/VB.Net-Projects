
Imports System.Text.RegularExpressions
Imports Spire.DocViewer.Forms

Public Class main1

    Private db As DbManagement
    Private quotationGen As QuotationGeneration
    Private projectID As Integer
    Private totalequipment As Double
    Private totaltotal As Double
    Private docviewer2 As New DocDocumentViewer
    Dim mrd, dthd, ard, td As String
    Dim pvc1, pvc2, pvc3, pvc4 As String
    Dim ac1, ac2, ac3, ac4 As String
    Dim pvccep1, pvccep2, pvccep3, pvccep4 As String
    Dim wel, seal, clay, beto As String
    Dim pump1, pump2, pump3, pump4 As String

    Private Sub tubageprovisoire_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tubageprovisoire.SelectedIndexChanged
        xticsppo.SelectedIndex = -1
    End Sub
    Private Sub xtics_SelectedIndexChanged(sender As Object, e As EventArgs) Handles xticsppo.SelectedIndexChanged
        Select Case xticsppo.SelectedIndex
            Case 0
                If tubageprovisoire.SelectedIndex = 0 Then
                    ppo.Text = pvc1
                ElseIf tubageprovisoire.SelectedIndex = 1 Then
                    ppo.Text = ac1
                End If
            Case 1
                If tubageprovisoire.SelectedIndex = 0 Then
                    ppo.Text = pvc2
                ElseIf tubageprovisoire.SelectedIndex = 1 Then
                    ppo.Text = ac2
                End If
            Case 2
                If tubageprovisoire.SelectedIndex = 0 Then
                    ppo.Text = pvc3
                ElseIf tubageprovisoire.SelectedIndex = 1 Then
                    ppo.Text = ac3
                End If
            Case 3
                If tubageprovisoire.SelectedIndex = 0 Then
                    ppo.Text = pvc4
                ElseIf tubageprovisoire.SelectedIndex = 1 Then
                    ppo.Text = ac4
                End If
        End Select
    End Sub
    Private Sub tubagepermanent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tubagepermanent.SelectedIndexChanged
        xticsppe.SelectedIndex = -1
    End Sub

    Private Sub xticsppe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles xticsppe.SelectedIndexChanged
        Select Case xticsppe.SelectedIndex
            Case 0
                If tubagepermanent.SelectedIndex = 0 Then
                    ppe.Text = pvc1
                ElseIf tubagepermanent.SelectedIndex = 1 Then
                    ppe.Text = ac1
                End If
            Case 1
                If tubagepermanent.SelectedIndex = 0 Then
                    ppe.Text = pvc2
                ElseIf tubagepermanent.SelectedIndex = 1 Then
                    ppe.Text = ac2
                End If
            Case 2
                If tubagepermanent.SelectedIndex = 0 Then
                    ppe.Text = pvc3
                ElseIf tubagepermanent.SelectedIndex = 1 Then
                    ppe.Text = ac3
                End If
            Case 3
                If tubagepermanent.SelectedIndex = 0 Then
                    ppe.Text = pvc4
                ElseIf tubagepermanent.SelectedIndex = 1 Then
                    ppe.Text = ac4
                End If
        End Select
    End Sub

    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles typedepompe.SelectedIndexChanged
        Select Case typedepompe.SelectedIndex
            Case 0
                a24a27.Text = pump1
            Case 1
                a24a27.Text = pump2
            Case 2
                a24a27.Text = pump3
            Case 3
                a24a27.Text = pump4

        End Select
    End Sub

    Private Sub buchonetanche_SelectedIndexChanged(sender As Object, e As EventArgs) Handles buchonetanche.SelectedIndexChanged
        Select Case buchonetanche.SelectedIndex
            Case 0
                a22a23.Text = clay
            Case 1
                a22a23.Text = beto
        End Select
    End Sub

    Private Sub pvccrepine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles pvccrepine.SelectedIndexChanged

        Select Case pvccrepine.SelectedIndex
            Case 0
                unitepvccrepine.Text = pvccep1
            Case 1
                unitepvccrepine.Text = pvccep2
            Case 2
                unitepvccrepine.Text = pvccep3
            Case 3
                unitepvccrepine.Text = pvccep4

        End Select
    End Sub



    Private Sub methodunion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles methodunion.SelectedIndexChanged
        Select Case methodunion.SelectedIndex
            Case 0
                a19a20.Text = wel
            Case 1
                a19a20.Text = seal
        End Select
    End Sub

    ' Constructor that accepts projectID and initializes quotationGen
    Public Sub New(Optional ByVal passedProjectID As Integer = 0, Optional ByVal lieux As String = "")

        ' This call is required by the designer.

        InitializeComponent()


        ' Initialize quotationGen

        quotationGen = New QuotationGeneration()
        db = New DbManagement()

        Try

            ' Load the Word document
            quotationGen.PreviewDocument(docviewer2)

            ' Set properties (Dock to fill the panel)
            docviewer2.Dock = DockStyle.Fill

            ' Add the DocViewer to the previewer panel
            preview.Controls.Add(docviewer2)
            Me.docviewer2.ZoomMode = ZoomMode.FitWidth
            Me.docviewer2.ZoomTo(81)
            Me.docviewer2.ZoomMode = ZoomMode.FitPage
        Catch ex As Exception
            MessageBox.Show($"An error occurred while loading the document: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Store the passed projectID in the form's variable
        projectID = passedProjectID
        projectlocation.Text = lieux

    End Sub

    Private Sub main1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadclient_projectdetails()
        loadunitprice()
    End Sub
    Private Sub loadclient_projectdetails()
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
                depthEtextbox.Text = row("depthE").ToString()

                ' Extract value from text fields
                Dim name As String = clientnameTextBox.Text.ToString()
                Dim address As String = clientaddressTextBox.Text.ToString()
                Dim tel As String = clientphonetextbox.Text.ToString()
                Dim dates As String = DateTextBox.Text.ToString()
                Dim ref As String = RefTextBox.Text.ToString()
                Dim descrip As String = descriptionTextBox.Text.ToString()
                Dim loc As String = projectlocation.Text.ToString()
                Dim depth As String = depthEtextbox.Text.ToString()

                ' Replace client and project details in the document
                quotationGen.ReplaceClientAndProjectDetails(dates, ref, name, address, tel, descrip, depth, loc)
                RefreshPreviewDoc()



            Else
                MessageBox.Show("No project found with the specified ID.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading project details: " & ex.Message)
        End Try
    End Sub

    Private Sub loadunitprice()
        Try
            Dim oldprice As DataTable = db.GetOldUnitPrices()

            If oldprice.Rows.Count > 0 Then
                Dim unmatchedList As New List(Of String)() ' List to keep track of unmatched designations

                For Each row As DataRow In oldprice.Rows
                    Dim designation As String = row("Designation").ToString().Trim().ToLower() ' Normalize to lowercase
                    Dim ancienPrix As String = row("Ancien Prix Unitaire").ToString().Trim()

                    ' Check if the price is empty or null
                    If String.IsNullOrEmpty(ancienPrix) Then
                        ancienPrix = "N/A" ' Fallback if the price is missing
                    End If

                    ' Check which designation to populate the corresponding TextBox
                    Dim matched As Boolean = False ' Flag to check if any match was found

                    If designation.Contains("equipment transportation") Then
                        a1.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("site preparation") Then
                        a2.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("mud rotary drilling") Then
                        mrd = ancienPrix
                        matched = True
                    ElseIf designation.Contains("down the hole drilling") Then
                        dthd = ancienPrix
                        matched = True
                    ElseIf designation.Contains("air rotary drilling") Then
                        ard = ancienPrix
                        matched = True
                    ElseIf designation.Contains("tricone drilling") Then
                        td = ancienPrix
                        matched = True
                    ElseIf designation.Contains("111") Then
                        pvc1 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("122") Then
                        pvc2 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("135") Then
                        pvc3 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("146") Then
                        pvc4 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("319") Then
                        ac1 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3210") Then
                        ac2 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3311") Then
                        ac3 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3412") Then
                        ac4 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("217") Then
                        pvccep1 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("228") Then
                        pvccep2 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("233") Then
                        pvccep3 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("244") Then
                        pvccep4 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("welding") Then
                        wel = ancienPrix
                        matched = True
                    ElseIf designation.Contains("sealing") Then
                        seal = ancienPrix
                        matched = True
                    ElseIf designation.Contains("gravel pack") Then
                        a21.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("plugs clay") Then
                        clay = ancienPrix
                        matched = True
                    ElseIf designation.Contains("plugs bentonite") Then
                        beto = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pump submersible") Then
                        pump1 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe de surface") Then
                        pump2 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe solaire") Then
                        pump3 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe a energie humain") Then
                        pump4 = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pump installation") Then
                        a28.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("test pumping") Then
                        a29.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("water flow test") Then
                        a30.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("well development with air lift") Then
                        a31.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("water tower") Then
                        a32.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("tower installation") Then
                        a33.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("water analysis") Then
                        a34.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("water treatment") Then
                        a35.Text = ancienPrix
                        matched = True
                    End If

                    ' If no match was found, log the unmatched designation from the DataTable
                    If Not matched Then
                        unmatchedList.Add(designation & " (" & ancienPrix & ")")
                    End If
                Next

                ' Display unmatched designations from the DataTable
                If unmatchedList.Count > 0 Then
                    Dim unmatchedMessage As String = "Unmatched Designations from DataTable:" & Environment.NewLine & String.Join(Environment.NewLine, unmatchedList)
                    MessageBox.Show(unmatchedMessage, "Unmatched Designations", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while loading project details: " & ex.Message)
        End Try
    End Sub
    Private Sub SaveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem1.Click
        quotationGen.SaveCurrentDocument()
    End Sub

    Private Sub RefreshPreviewDoc()
        quotationGen.PreviewDocument(docviewer2)
        Me.docviewer2.ZoomMode = ZoomMode.FitWidth
        Me.docviewer2.ZoomTo(81)
        Me.docviewer2.ZoomMode = ZoomMode.FitPage
    End Sub

    Private Sub EXPORTERToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EXPORTERToolStripMenuItem.Click
        Try

            Using saveFileDialog1 As New SaveFileDialog()
                saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf"
                saveFileDialog1.Title = "Save Quotation as PDF"
                If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                    ' Export the document as a PDF
                    quotationGen.SaveDocumentAsPdf(saveFileDialog1.FileName)
                    MessageBox.Show("Document saved successfully!")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            ' Code to execute when the CheckBox is checked
            tpte.Text = a1.Text
            quotationGen.UpdateTableRow(2, "1", "Transport des équipements", "-", "Unité", a1.Text, tpte.Text)


            a1.Enabled = False
            tpte.Enabled = False
            RefreshPreviewDoc()
        Else
            a1.Enabled = True

        End If
        totals()
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a2.Text, "\d+").Value)
            tppt.Text = unitprice & " FCFA"
            quotationGen.UpdateTableRow(3, "2", "Préparation du terrain", "-", "Unité", unitprice & " FCFA", unitprice & " FCFA")
            a2.Enabled = False
            tppt.Enabled = False
            RefreshPreviewDoc()
        Else
            a2.Enabled = True

        End If
        totals()
    End Sub

    Private Sub ActualiserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActualiserToolStripMenuItem.Click
        loadclient_projectdetails()
        loadunitprice()
        Me.docviewer2.ZoomMode = ZoomMode.FitWidth
        Me.docviewer2.ZoomTo(81)
        Me.docviewer2.ZoomMode = ZoomMode.FitPage

    End Sub

    Private Sub DocViewer2_DocumentOpened(sender As Object, args As EventArgs)
        Me.docviewer2.ZoomMode = ZoomMode.FitWidth
        Me.docviewer2.ZoomTo(81)
        Me.docviewer2.ZoomMode = ZoomMode.FitPage

    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            If typeforage.SelectedIndex < 0 Then
                MessageBox.Show("selectioner un type de forage", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox3.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(a3a6.Text, "\d+").Value)

                Dim desg As String
                desg = typeforage.SelectedItem.ToString
                'MessageBox.Show(desg)
                ptdeforeuse.Text = (Integer.Parse(qtetypedeforage.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(5, "3", desg, qtetypedeforage.Text, "Unité", unitprice & " FCFA", ptdeforeuse.Text)
                typeforage.Enabled = False
                qtetypedeforage.Enabled = False
                a3a6.Enabled = False
                RefreshPreviewDoc()
            End If

        Else
            typeforage.Enabled = True
            qtetypedeforage.Enabled = True
            a3a6.Enabled = True

        End If
        totals()
    End Sub

    Private Sub labourtb_TextChanged(sender As Object, e As EventArgs) Handles labourtb.TextChanged
        Dim numericValue As Double = 0

        ' Attempt to extract a numeric value using Regex
        If Regex.IsMatch(labourtb.Text, "\d+") Then
            numericValue = Math.Max(0, Double.Parse(Regex.Match(labourtb.Text, "\d+").Value))
        End If

        ' Update the textbox content with the formatted value
        labourtb.Text = numericValue & " FCFA"
        totals()

        ' Move the caret to the end to avoid user experience issues
        'labourtb.SelectionStart = labourtb.Text.Length
    End Sub



    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            If tubageprovisoire.SelectedIndex < 0 And xticsppo.SelectedIndex < 0 Then
                MessageBox.Show("selectioner un type de tubage et c'est caracteristic", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox4.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(ppo.Text, "\d+").Value)

                Dim pipe, pipex As String
                pipe = tubageprovisoire.SelectedItem.ToString()
                pipex = xticsppo.SelectedItem.ToString
                'MessageBox.Show(desg)
                tpps.Text = (Integer.Parse(qteps.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(6, "4", "Tubage provisoire " & pipe & " " & pipex, qteps.Text, "Metre", unitprice & " FCFA", tpps.Text)
                tubageprovisoire.Enabled = False
                xticsppo.Enabled = False
                qteps.Enabled = False
                ppo.Enabled = False

                RefreshPreviewDoc()
            End If
        Else
            tubageprovisoire.Enabled = True
            xticsppo.Enabled = True
            qteps.Enabled = True
            ppo.Enabled = True

        End If
        totals()
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked Then
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a21.Text, "\d+").Value)

            ptgf.Text = (Integer.Parse(qtegf.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(7, "5", "Gravure filtrant", qtegf.Text, "Metre", unitprice & " FCFA", ptgf.Text)

            qtegf.Enabled = False
            a21.Enabled = False

            RefreshPreviewDoc()
        Else
            qtegf.Enabled = True
            a21.Enabled = True

        End If
        totals()
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked Then
            If tubagepermanent.SelectedIndex < 0 And xticsppe.SelectedIndex < 0 Then
                MessageBox.Show("selectioner un type de tubage et c'est caracteristic", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox6.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(ppe.Text, "\d+").Value)

                Dim pipe, pipex As String
                pipe = tubagepermanent.SelectedItem.ToString()
                pipex = xticsppe.SelectedItem.ToString
                'MessageBox.Show(desg)
                tpppe.Text = (Integer.Parse(qteppe.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(9, "6", "Tubage permanent " & pipe & " " & pipex, qteppe.Text, "Metre", unitprice & " FCFA", tpppe.Text)
                tubagepermanent.Enabled = False
                xticsppe.Enabled = False
                qteppe.Enabled = False
                ppe.Enabled = False

                RefreshPreviewDoc()
            End If
        Else
            tubagepermanent.Enabled = True
            xticsppe.Enabled = True
            qteppe.Enabled = True
            ppe.Enabled = True

        End If
        totals()
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked Then
            If methodunion.SelectedIndex < 0 Then
                MessageBox.Show("selectioner une type methode d'union", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox7.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(a19a20.Text, "\d+").Value)

                Dim desg As String
                desg = methodunion.SelectedItem.ToString
                'MessageBox.Show(desg)
                tpmd.Text = (Integer.Parse(qtemd.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(10, "7", desg, qtemd.Text, "Unité", unitprice & " FCFA", tpmd.Text)
                methodunion.Enabled = False
                qtemd.Enabled = False
                a19a20.Enabled = False
                RefreshPreviewDoc()
            End If

        Else
            methodunion.Enabled = True
            qtemd.Enabled = True
            a19a20.Enabled = True
        End If
        totals()
    End Sub
    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked Then
            If buchonetanche.SelectedIndex < 0 Then
                MessageBox.Show("selectioner le type de buchonetanche", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox8.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(a22a23.Text, "\d+").Value)

                Dim desg As String
                desg = buchonetanche.SelectedItem.ToString
                'MessageBox.Show(desg)
                tpbe.Text = (Integer.Parse(qtebe.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(11, "8", "Bouchons étanches " & desg, qtebe.Text, "Unité", unitprice & " FCFA", tpbe.Text)
                buchonetanche.Enabled = False
                qtebe.Enabled = False
                a22a23.Enabled = False
                RefreshPreviewDoc()
            End If

        Else
            buchonetanche.Enabled = True
            qtebe.Enabled = True
            a22a23.Enabled = True
        End If
        totals()
    End Sub
    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked Then
            If pvccrepine.SelectedIndex < 0 Then
                MessageBox.Show("selectioner les caracteristic du tubage pvc crepine", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox9.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(unitepvccrepine.Text, "\d+").Value)

                Dim desg As String
                desg = pvccrepine.SelectedItem.ToString
                'MessageBox.Show(desg)
                tppvcc.Text = (Integer.Parse(qtepvcc.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(12, "9", "PVC crépine" & desg, qtepvcc.Text, "Unité", unitprice & " FCFA", tppvcc.Text)
                pvccrepine.Enabled = False
                qtepvcc.Enabled = False
                unitepvccrepine.Enabled = False
                RefreshPreviewDoc()
            End If

        Else
            pvccrepine.Enabled = True
            qtepvcc.Enabled = True
            unitepvccrepine.Enabled = True
        End If
        totals()
    End Sub
    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked Then
            If typedepompe.SelectedIndex < 0 Then
                MessageBox.Show("selectioner le type de Pompe", "Avertisement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                CheckBox10.Checked = False
            Else
                ' Code to execute when the CheckBox is checked
                Dim unitprice As Integer
                unitprice = Integer.Parse(Regex.Match(a24a27.Text, "\d+").Value)

                Dim desg As String
                desg = typedepompe.SelectedItem.ToString
                'MessageBox.Show(desg)
                tpompe.Text = (Integer.Parse(qtep.Text) * unitprice) & " FCFA"
                quotationGen.UpdateTableRow(14, "10", desg, qtep.Text, "Unité", unitprice & " FCFA", tpompe.Text)
                typedepompe.Enabled = False
                qtep.Enabled = False
                a24a27.Enabled = False
                RefreshPreviewDoc()
            End If

        Else
            typedepompe.Enabled = True
            qtep.Enabled = True
            a24a27.Enabled = True
        End If
        totals()
    End Sub
    Private Sub CheckBox11_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a28.Text, "\d+").Value)
            tipompe.Text = unitprice & " FCFA"
            quotationGen.UpdateTableRow(15, "11", "Installation de la pompe", "-", "Heure", unitprice & " FCFA", unitprice & " FCFA")
            a2.Enabled = False
            tppt.Enabled = False
            RefreshPreviewDoc()
        Else
            a2.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox12_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        If CheckBox12.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a29.Text, "\d+").Value)
            tpompeessai.Text = (Integer.Parse(qtepe.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(17, "12", "Pompage d'essai", qtepe.Text, "Jour", unitprice & " FCFA", tpompeessai.Text)
            a29.Enabled = False
            qtepe.Enabled = False
            tpompeessai.Enabled = False
            RefreshPreviewDoc()
        Else
            a29.Enabled = True
            qtepe.Enabled = True
            tpompeessai.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged
        If CheckBox13.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a30.Text, "\d+").Value)
            ttesteau.Text = (Integer.Parse(qtetde.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(18, "13", "Test de débit d'eau", qtetde.Text, "Jour", unitprice & " FCFA", ttesteau.Text)
            a30.Enabled = False
            qtetde.Enabled = False

            RefreshPreviewDoc()
        Else
            a30.Enabled = True
            qtetde.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        If CheckBox14.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a31.Text, "\d+").Value)
            tairlift.Text = (Integer.Parse(qteal.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(19, "14", "Développement du puits à l'air lift", qteal.Text, "Unité", unitprice & " FCFA", tairlift.Text)
            a31.Enabled = False
            qteal.Enabled = False

            RefreshPreviewDoc()
        Else
            a31.Enabled = True
            qteal.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox15_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox15.CheckedChanged
        If CheckBox15.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a32.Text, "\d+").Value)
            a32t.Text = (Integer.Parse(qteal.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(21, "15", "Installation de château d'eau", "-", "Unité", unitprice & " FCFA", a32t.Text)
            a32.Enabled = False


            RefreshPreviewDoc()
        Else
            a32.Enabled = True
            qteal.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox16_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox16.CheckedChanged
        If CheckBox16.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a33.Text, "\d+").Value)
            a33t.Text = (Integer.Parse(watertowerno.Text) * unitprice) & " FCFA"
            quotationGen.UpdateTableRow(22, "16", "Château d'eau", watertowerno.Text, "Littre", unitprice & " FCFA", a33t.Text)
            a33.Enabled = False
            watertowerno.Enabled = False

            RefreshPreviewDoc()
        Else
            a31.Enabled = True
            qteal.Enabled = True
            tairlift.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox17_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox17.CheckedChanged
        If CheckBox16.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a34.Text, "\d+").Value)
            a34t.Text = unitprice & " FCFA"
            quotationGen.UpdateTableRow(24, "17", "Analyse de l'eau", "-", "Unité", unitprice & " FCFA", a34t.Text)
            a34.Enabled = False

            RefreshPreviewDoc()
        Else
            a34.Enabled = True

        End If
        totals()
    End Sub
    Private Sub CheckBox18_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox18.CheckedChanged
        If CheckBox16.Checked Then
            ' Code to execute when the CheckBox is checked
            Dim unitprice As Integer
            unitprice = Integer.Parse(Regex.Match(a35.Text, "\d+").Value)
            a35t.Text = unitprice & " FCFA"
            quotationGen.UpdateTableRow(25, "18", "Traitement de l'eau", "-", "Unité", unitprice & " FCFA", a35t.Text)
            a35.Enabled = False

            RefreshPreviewDoc()
        Else
            a35.Enabled = True

        End If
        totals()
    End Sub

    Private Sub ChangerLesPrixUnitaireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangerLesPrixUnitaireToolStripMenuItem.Click
        PRIX_UNITAIRE.ShowDialog()
    End Sub

    Private Sub typeforage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles typeforage.SelectedIndexChanged
        Select Case typeforage.SelectedIndex
            Case 0
                a3a6.Text = mrd
            Case 1
                a3a6.Text = dthd
            Case 2
                a3a6.Text = ard
            Case 3
                a3a6.Text = td
        End Select
    End Sub

    Private Sub totals()
        Try
            Dim total As Double = 0

            Dim textBoxes() As String = {
            If(tpte?.Text, ""), If(tppt?.Text, ""), If(ptdeforeuse?.Text, ""), If(tpps?.Text, ""), If(ptgf?.Text, ""),
            If(tpppe?.Text, ""), If(tpmd?.Text, ""), If(tpbe?.Text, ""), If(tppvcc?.Text, ""), If(tpompe?.Text, ""),
            If(tipompe?.Text, ""), If(tpompeessai?.Text, ""), If(ttesteau?.Text, ""), If(tairlift?.Text, ""),
            If(a32t?.Text, ""), If(a33t?.Text, ""), If(a34t?.Text, ""), If(a35t?.Text, "")
        }

            For Each value As String In textBoxes
                Dim numericValue As Double = 0
                If Not String.IsNullOrWhiteSpace(value) AndAlso Regex.IsMatch(value, "\d+") Then
                    Double.TryParse(Regex.Match(value, "\d+").Value, numericValue)
                End If
                total += numericValue
            Next

            If totalequip Is Nothing Then
                Throw New Exception("totalequip control is not initialized.")
            End If
            totalequip.Text = total & " FCFA"

            If quotationGen Is Nothing Then
                Throw New Exception("quotationGen is not initialized.")
            End If

            quotationGen.UpdateTotalsTable(1, 1, total & " FCFA")
            quotationGen.UpdateTotalsTable(2, 1, labourtb?.Text)

            If labourtb Is Nothing OrElse String.IsNullOrWhiteSpace(labourtb.Text) Then
                Throw New Exception("labourtb is empty or not initialized.")
            End If

            Dim labourValue As Double
            Double.TryParse(Regex.Match(labourtb.Text, "\d+").Value, labourValue)
            totaltotal = total + labourValue

            quotationGen.UpdateTotalsTable(3, 1, totaltotal & " FCFA")

        Catch ex As Exception

        End Try
    End Sub


    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        quotationGen.InsertTotalInWords(totaltotal)
        RefreshPreviewDoc()
    End Sub



End Class
