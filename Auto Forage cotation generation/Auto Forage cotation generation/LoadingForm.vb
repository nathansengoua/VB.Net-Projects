Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class LoadingForm
    Private Sub LoadingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar1.Style = ProgressBarStyle.Marquee ' Continuous animation
    End Sub
End Class