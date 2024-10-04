Imports System.ComponentModel
Imports System.Globalization
Imports System.Threading

Public Class Form1

    ' Function to update the UI with the selected language resources
    Private Sub UpdateLanguage(lang As String)
        ' Set the current culture based on ComboBox selection
        Dim cultureInfo As CultureInfo = New CultureInfo(lang)
        Thread.CurrentThread.CurrentUICulture = cultureInfo

        ' Update the UI elements with the new culture
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(Form1))

        For Each control As Control In Me.Controls
            resources.ApplyResources(control, control.Name, cultureInfo)
        Next

        ' Update form title
        Me.Text = resources.GetString("$this.Text", cultureInfo)

        ' Update global selected language
        GlobalSettings.selectedlanguage = lang

        ' Force the form to repaint to apply changes immediately
        Me.Invalidate()
        Me.Update()
    End Sub

    ' ComboBox Selection Changed Event for Language Selection
    Private Sub Guna2ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2ComboBox1.SelectedIndexChanged
        Select Case Guna2ComboBox1.SelectedIndex
            Case 0 ' English
                UpdateLanguage("en")
            Case 1 ' French
                UpdateLanguage("fr")
            Case 2 ' German
                UpdateLanguage("de")
        End Select
    End Sub

    ' Form Load Event to set initial language
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Check for the saved language and set the ComboBox selection accordingly
        Select Case GlobalSettings.selectedlanguage
            Case "en"
                Guna2ComboBox1.SelectedIndex = 0
            Case "fr"
                Guna2ComboBox1.SelectedIndex = 1
            Case "de"
                Guna2ComboBox1.SelectedIndex = 2
            Case Else
                Guna2ComboBox1.SelectedIndex = 0 ' Default to English
        End Select

    End Sub

    ' Button Click Event to open Form2
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Form2.ShowDialog()
    End Sub

End Class
