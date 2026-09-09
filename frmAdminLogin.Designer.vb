<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdminLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdminLogin))
        txtPassword = New TextBox()
        btnOK = New Button()
        Label1 = New Label()
        SuspendLayout()
        ' 
        ' txtPassword
        ' 
        txtPassword.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtPassword.Location = New Point(53, 37)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(200, 27)
        txtPassword.TabIndex = 0
        ' 
        ' btnOK
        ' 
        btnOK.Location = New Point(259, 37)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(75, 27)
        btnOK.TabIndex = 1
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(53, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(126, 15)
        Label1.TabIndex = 2
        Label1.Text = "Please enter password:"
        ' 
        ' frmAdminLogin
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(401, 88)
        Controls.Add(Label1)
        Controls.Add(btnOK)
        Controls.Add(txtPassword)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmAdminLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Admin Login"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtPassword As TextBox
    Friend WithEvents btnOK As Button
    Friend WithEvents Label1 As Label
End Class
