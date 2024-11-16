<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadingForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.progressbar1 = New Guna.UI2.WinForms.Guna2WinProgressIndicator()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'progressbar1
        '
        Me.progressbar1.AutoStart = True
        Me.progressbar1.BackColor = System.Drawing.Color.Transparent
        Me.progressbar1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.progressbar1.Location = New System.Drawing.Point(0, 0)
        Me.progressbar1.Name = "progressbar1"
        Me.progressbar1.Size = New System.Drawing.Size(130, 130)
        Me.progressbar1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "SVP Patiente ..."
        '
        'LoadingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(130, 130)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.progressbar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LoadingForm"
        Me.Opacity = 0.75R
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LoadingForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents progressbar1 As Guna.UI2.WinForms.Guna2WinProgressIndicator
    Friend WithEvents Label1 As Label
End Class
