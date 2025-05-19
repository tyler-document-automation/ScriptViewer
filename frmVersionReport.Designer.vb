<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVersionReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVersionReport))
        dgvVersionReport = New DataGridView()
        CType(dgvVersionReport, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvVersionReport
        ' 
        dgvVersionReport.AllowUserToAddRows = False
        dgvVersionReport.AllowUserToDeleteRows = False
        dgvVersionReport.AllowUserToResizeColumns = False
        dgvVersionReport.AllowUserToResizeRows = False
        dgvVersionReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvVersionReport.Location = New Point(13, 12)
        dgvVersionReport.Name = "dgvVersionReport"
        dgvVersionReport.Size = New Size(400, 678)
        dgvVersionReport.TabIndex = 0
        ' 
        ' frmVersionReport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(425, 704)
        Controls.Add(dgvVersionReport)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmVersionReport"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Version Report"
        CType(dgvVersionReport, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvVersionReport As DataGridView
End Class
