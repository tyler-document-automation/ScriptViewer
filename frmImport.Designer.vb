<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImport))
        rdoNonCloud = New RadioButton()
        rdoCloud = New RadioButton()
        txtFolderPath = New TextBox()
        Label1 = New Label()
        btnBrowse = New Button()
        btnImport = New Button()
        Label2 = New Label()
        txtClient = New TextBox()
        txtOutput = New TextBox()
        SuspendLayout()
        ' 
        ' rdoNonCloud
        ' 
        rdoNonCloud.Appearance = Appearance.Button
        rdoNonCloud.AutoSize = True
        rdoNonCloud.Font = New Font("Segoe UI", 11.25F)
        rdoNonCloud.Location = New Point(12, 26)
        rdoNonCloud.Name = "rdoNonCloud"
        rdoNonCloud.Size = New Size(92, 30)
        rdoNonCloud.TabIndex = 5
        rdoNonCloud.TabStop = True
        rdoNonCloud.Text = "Non-Cloud"
        rdoNonCloud.UseVisualStyleBackColor = True
        ' 
        ' rdoCloud
        ' 
        rdoCloud.Appearance = Appearance.Button
        rdoCloud.AutoSize = True
        rdoCloud.Font = New Font("Segoe UI", 11.25F)
        rdoCloud.Location = New Point(121, 26)
        rdoCloud.Name = "rdoCloud"
        rdoCloud.Size = New Size(58, 30)
        rdoCloud.TabIndex = 6
        rdoCloud.TabStop = True
        rdoCloud.Text = "Cloud"
        rdoCloud.UseVisualStyleBackColor = True
        ' 
        ' txtFolderPath
        ' 
        txtFolderPath.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtFolderPath.Location = New Point(103, 141)
        txtFolderPath.Name = "txtFolderPath"
        txtFolderPath.Size = New Size(365, 27)
        txtFolderPath.TabIndex = 7
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(11, 144)
        Label1.Name = "Label1"
        Label1.Size = New Size(86, 20)
        Label1.TabIndex = 8
        Label1.Text = "Folder Path:"
        ' 
        ' btnBrowse
        ' 
        btnBrowse.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnBrowse.Location = New Point(485, 141)
        btnBrowse.Name = "btnBrowse"
        btnBrowse.Size = New Size(75, 27)
        btnBrowse.TabIndex = 9
        btnBrowse.Text = "Browse"
        btnBrowse.UseVisualStyleBackColor = True
        ' 
        ' btnImport
        ' 
        btnImport.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnImport.Location = New Point(581, 141)
        btnImport.Name = "btnImport"
        btnImport.Size = New Size(75, 27)
        btnImport.TabIndex = 10
        btnImport.Text = "Import"
        btnImport.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(11, 108)
        Label2.Name = "Label2"
        Label2.Size = New Size(50, 20)
        Label2.TabIndex = 12
        Label2.Text = "Client:"
        ' 
        ' txtClient
        ' 
        txtClient.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtClient.Location = New Point(103, 105)
        txtClient.Name = "txtClient"
        txtClient.Size = New Size(365, 27)
        txtClient.TabIndex = 11
        ' 
        ' txtOutput
        ' 
        txtOutput.Location = New Point(103, 194)
        txtOutput.Multiline = True
        txtOutput.Name = "txtOutput"
        txtOutput.ReadOnly = True
        txtOutput.ScrollBars = ScrollBars.Both
        txtOutput.Size = New Size(553, 221)
        txtOutput.TabIndex = 13
        txtOutput.WordWrap = False
        ' 
        ' frmImport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(800, 450)
        Controls.Add(txtOutput)
        Controls.Add(Label2)
        Controls.Add(txtClient)
        Controls.Add(btnImport)
        Controls.Add(btnBrowse)
        Controls.Add(Label1)
        Controls.Add(txtFolderPath)
        Controls.Add(rdoCloud)
        Controls.Add(rdoNonCloud)
        FormBorderStyle = FormBorderStyle.Fixed3D
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmImport"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Import Scripts"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents rdoNonCloud As RadioButton
    Friend WithEvents rdoCloud As RadioButton
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnImport As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtClient As TextBox
    Friend WithEvents txtOutput As TextBox
End Class
