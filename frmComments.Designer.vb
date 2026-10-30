<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmComments
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmComments))
        dgvComments = New DataGridView()
        CType(dgvComments, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvComments
        ' 
        dgvComments.AllowUserToAddRows = False
        dgvComments.AllowUserToDeleteRows = False
        dgvComments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvComments.Dock = DockStyle.Fill
        dgvComments.Location = New Point(0, 0)
        dgvComments.MultiSelect = False
        dgvComments.Name = "dgvComments"
        dgvComments.ReadOnly = True
        dgvComments.RowHeadersVisible = False
        dgvComments.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvComments.Size = New Size(800, 450)
        dgvComments.TabIndex = 0
        ' 
        ' frmComments
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(dgvComments)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        MinimumSize = New Size(500, 300)
        Name = "frmComments"
        StartPosition = FormStartPosition.CenterParent
        Text = "Version Comments"
        CType(dgvComments, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvComments As DataGridView
    Friend WithEvents colVersionNumber As DataGridViewTextBoxColumn
    Friend WithEvents colDescription As DataGridViewTextBoxColumn
End Class
