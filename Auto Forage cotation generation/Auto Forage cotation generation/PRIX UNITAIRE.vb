Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports Guna.UI2.WinForms
Imports MigraDoc.Rendering

Public Class PRIX_UNITAIRE
    Private db As New DbManagement
    Private Sub PRIX_UNITAIRE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadoldunitprice()
    End Sub
    Private Sub loadoldunitprice()
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
                        A1.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("site preparation") Then
                        A2.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("mud rotary drilling") Then
                        A3.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("down the hole drilling") Then
                        A4.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("air rotary drilling") Then
                        A5.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("tricone drilling") Then
                        A6.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("111") Then
                        A7.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("122") Then
                        A8.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("135") Then
                        A9.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("146") Then
                        a10.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("319") Then
                        a11.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3210") Then
                        a12.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3311") Then
                        a13.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("3412") Then
                        a14.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("217") Then
                        a15.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("228") Then
                        a16.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("233") Then
                        a17.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("244") Then
                        a18.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("welding") Then
                        a19.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("sealing") Then
                        a20.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("gravel pack") Then
                        a21.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("plugs clay") Then
                        a22.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("plugs bentonite") Then
                        a23.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pump submersible") Then
                        a24.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe de surface") Then
                        a25.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe solaire") Then
                        a26.Text = ancienPrix
                        matched = True
                    ElseIf designation.Contains("pompe a energie humain") Then
                        a27.Text = ancienPrix
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




    Private Sub Guna2TileButton2_Click(sender As Object, e As EventArgs) Handles Guna2TileButton2.Click
        Me.Dispose()
    End Sub

    ' Button s1: Updating "equipment transportation" in equipment
    Private Sub s1_Click(sender As Object, e As EventArgs) Handles s1.Click
        UpdateUnitCost("category", "category_name", "Equipment transportation", n1.Text)
    End Sub

    ' Button s2: Updating "site preparation" in equipment
    Private Sub s2_Click(sender As Object, e As EventArgs) Handles s2.Click
        UpdateUnitCost("category", "category_name", "Site preparation", n2.Text)
    End Sub

    ' Button s3: Updating "mud rotary drilling" in equipment
    Private Sub s3_Click(sender As Object, e As EventArgs) Handles s3.Click
        UpdateUnitCost("DrillingMethods", "name", "Mud Rotary Drilling", n3.Text)
    End Sub

    ' Button s4: Updating "down the hole drilling" in equipment
    Private Sub s4_Click(sender As Object, e As EventArgs) Handles s4.Click
        UpdateUnitCost("DrillingMethods", "name", "Down the Hole Drilling", n4.Text)
    End Sub

    ' Button s5: Updating "air rotary drilling" in equipment
    Private Sub s5_Click(sender As Object, e As EventArgs) Handles s5.Click
        UpdateUnitCost("DrillingMethods", "name", "Air Rotary Drilling", n5.Text)
    End Sub

    ' Button s6: Updating "tricone drilling" in equipment
    Private Sub s6_Click(sender As Object, e As EventArgs) Handles s6.Click
        UpdateUnitCost("DrillingMethods", "name", "Tricone Drilling", n6.Text)
    End Sub

    ' Button s7: Updating "111" in equipment
    Private Sub s7_Click(sender As Object, e As EventArgs) Handles s7.Click
        UpdateUnitCost("Casing_specification", "specification_id", "1", n7.Text)
    End Sub

    ' Button s8: Updating "122" in equipment
    Private Sub s8_Click(sender As Object, e As EventArgs) Handles s8.Click
        UpdateUnitCost("Casing_specification", "specification_id", "2", n8.Text)
    End Sub

    ' Button s9: Updating "135" in equipment
    Private Sub s9_Click(sender As Object, e As EventArgs) Handles s9.Click
        UpdateUnitCost("Casing_specification", "specification_id", "5", n9.Text)
    End Sub

    ' Button s10: Updating "146" in equipment
    Private Sub s10_Click(sender As Object, e As EventArgs) Handles s10.Click
        UpdateUnitCost("Casing_specification", "specification_id", "6", n10.Text)
    End Sub

    ' Button s11: Updating "319" in equipment
    Private Sub s11_Click(sender As Object, e As EventArgs) Handles s11.Click
        UpdateUnitCost("Casing_specification", "specification_id", "9", n11.Text)
    End Sub

    ' Button s12: Updating "3210" in equipment
    Private Sub s12_Click(sender As Object, e As EventArgs) Handles s12.Click
        UpdateUnitCost("Casing_specification", "specification_id", "10", n12.Text)
    End Sub

    ' Button s13: Updating "3311" in equipment
    Private Sub s13_Click(sender As Object, e As EventArgs) Handles s13.Click
        UpdateUnitCost("Casing_specification", "specification_id", "11", n13.Text)
    End Sub

    ' Button s14: Updating "3412" in equipment
    Private Sub s14_Click(sender As Object, e As EventArgs) Handles s14.Click
        UpdateUnitCost("Casing_specification", "specification_id", "12", n14.Text)
    End Sub

    ' Button s15: Updating "217" in equipment
    Private Sub s15_Click(sender As Object, e As EventArgs) Handles s15.Click
        UpdateUnitCost("Casing_specification", "specification_id", "7", n15.Text)
    End Sub

    ' Button s16: Updating "228" in equipment
    Private Sub s16_Click(sender As Object, e As EventArgs) Handles s16.Click
        UpdateUnitCost("Casing_specification", "specification_id", "8", n16.Text)
    End Sub

    ' Button s17: Updating "233" in equipment
    Private Sub s17_Click(sender As Object, e As EventArgs) Handles s17.Click
        UpdateUnitCost("Casing_specification", "specification_id", "3", n17.Text)
    End Sub

    ' Button s18: Updating "244" in equipment
    Private Sub s18_Click(sender As Object, e As EventArgs) Handles s18.Click
        UpdateUnitCost("Casing_specification", "specification_id", "4", n18.Text)
    End Sub

    ' Button s19: Updating "welding" in equipment
    Private Sub s19_Click(sender As Object, e As EventArgs) Handles s19.Click
        UpdateUnitCost("category ", "category_name", "Welding", n19.Text)
    End Sub

    ' Button s20: Updating "sealing" in equipment
    Private Sub s20_Click(sender As Object, e As EventArgs) Handles s20.Click
        UpdateUnitCost("category", "category_name", "Sealing", n20.Text)
    End Sub

    ' Button s21: Updating "gravel pack" in equipment
    Private Sub s21_Click(sender As Object, e As EventArgs) Handles s21.Click
        UpdateUnitCost("category", "category_name", "Gravel pack", n21.Text)
    End Sub

    ' Button s22: Updating "plugs clay" in equipment
    Private Sub s22_Click(sender As Object, e As EventArgs) Handles s22.Click
        UpdateUnitCost("category", "category_name", "plugs clay", n22.Text)
    End Sub

    ' Button s23: Updating "plugs bentonite" in equipment
    Private Sub s23_Click(sender As Object, e As EventArgs) Handles s23.Click
        UpdateUnitCost("category", "category_name", "plugs bentonite", n23.Text)
    End Sub

    ' Button s24: Updating "pump submersible" in pump
    Private Sub s24_Click(sender As Object, e As EventArgs) Handles s24.Click
        UpdateUnitCost("pump", "name", "pump submersible", n24.Text)
    End Sub

    ' Button s25: Updating "pompe de surface" in pump
    Private Sub s25_Click(sender As Object, e As EventArgs) Handles s25.Click
        UpdateUnitCost("pump", "name", "pompe de surface", n25.Text)
    End Sub

    ' Button s26: Updating "pompe solaire" in pump
    Private Sub s26_Click(sender As Object, e As EventArgs) Handles s26.Click
        UpdateUnitCost("pump", "name", "pompe solaire", n26.Text)
    End Sub

    ' Button s27: Updating "pompe a energie humain" in pump
    Private Sub s27_Click(sender As Object, e As EventArgs) Handles s27.Click
        UpdateUnitCost("pump", "name", "pompe a energie humain", n27.Text)
    End Sub

    ' Button s28: Updating "pump installation" in pump
    Private Sub s28_Click(sender As Object, e As EventArgs) Handles s28.Click
        UpdateUnitCost("category", "category_name", "Pump installation", n27.Text)
    End Sub

    ' Button s29: Updating "test pumping" in pump
    Private Sub s29_Click(sender As Object, e As EventArgs) Handles s29.Click
        UpdateUnitCost("category", "category_name", "Test pumping", n28.Text)
    End Sub

    ' Button s30: Updating "water flow test" in pump
    Private Sub s30_Click(sender As Object, e As EventArgs) Handles s30.Click
        UpdateUnitCost("category", "category_name", "Water flow test", n30.Text)
    End Sub

    ' Button s31: Updating "well development with air lift" in pump
    Private Sub s31_Click(sender As Object, e As EventArgs) Handles s31.Click
        UpdateUnitCost("category", "category_name", "Well development with air lift", n32.Text)
    End Sub

    ' Button s32: Updating "water tower" in pump
    Private Sub s32_Click(sender As Object, e As EventArgs) Handles s32.Click
        UpdateUnitCost("category", "category_name", "Water tower", n33.Text)
    End Sub

    ' Button s33: Updating "tower installation" in pump
    Private Sub s33_Click(sender As Object, e As EventArgs) Handles s33.Click
        UpdateUnitCost("category", "category_name", "tower installation", n33.Text)
    End Sub

    ' Button s34: Updating "water analysis" in pump
    Private Sub s34_Click(sender As Object, e As EventArgs) Handles s34.Click
        UpdateUnitCost("category", "category_name", "Water analysis", n34.Text)
    End Sub

    ' Button s35: Updating "water treatment" in pump
    Private Sub s35_Click(sender As Object, e As EventArgs) Handles s35.Click
        UpdateUnitCost("category", "category_name", "Water treatment", n35.Text)
    End Sub



    Private Sub ActualiserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActualiserToolStripMenuItem.Click
        loadoldunitprice()
        ClearTextBoxes()
    End Sub
    Private Sub ClearTextBoxes()
        For i As Integer = 1 To 35
            Dim textBoxName As String = "n" & i.ToString() ' Create the text box name (e.g., s1, s2, ..., s35)
            Dim Guna2TextBox As Guna2TextBox = Me.Controls.Find(textBoxName, True).FirstOrDefault() ' Find the text box

            If Guna2TextBox IsNot Nothing Then
                Guna2TextBox.Text = String.Empty ' Clear the text box
            End If
        Next
    End Sub

    ' Method for updating unit cost in DrillingMethods, category, or pump
    Private Sub UpdateUnitCost(tableName As String, columnName As String, designation As String, newCost As String)
        Dim unitCost As Decimal
        If Decimal.TryParse(newCost, unitCost) Then
            Try
                Using conn As SQLiteConnection = db.GetConnection()
                    conn.Open()
                    Dim query As String = "UPDATE " & tableName & " SET unit_cost = @newCost WHERE " & columnName & " = @designation"
                    Using cmd As New SQLiteCommand(query, conn)
                        cmd.Parameters.AddWithValue("@newCost", newCost)
                        cmd.Parameters.AddWithValue("@designation", designation)

                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                        If rowsAffected > 0 Then
                            MsgBox("Modification réussie")
                        Else
                            MsgBox("une erreur s'est produite.")
                        End If
                    End Using
                End Using
                MessageBox.Show("Price updated successfully!")
            Catch ex As Exception
                MessageBox.Show("Error updating price: " & ex.Message)
            End Try
        Else
            MessageBox.Show("Veuillez entrer un Prix valide sans lettre.")
        End If
    End Sub
End Class