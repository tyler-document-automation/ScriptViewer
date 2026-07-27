<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCloudSites
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
        tlpCloudSites = New TableLayoutPanel()
        pnlSites = New Panel()
        pnlSites.SuspendLayout()
        SuspendLayout()
        ' 
        ' tlpCloudSites
        ' 
        tlpCloudSites.AutoScroll = True
        tlpCloudSites.AutoSize = True
        tlpCloudSites.AutoSizeMode = AutoSizeMode.GrowAndShrink
        tlpCloudSites.ColumnCount = 5
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        tlpCloudSites.Dock = DockStyle.Top
        tlpCloudSites.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tlpCloudSites.Location = New Point(0, 0)
        tlpCloudSites.Name = "tlpCloudSites"
        tlpCloudSites.RowCount = 2
        tlpCloudSites.RowStyles.Add(New RowStyle())
        tlpCloudSites.RowStyles.Add(New RowStyle())
        tlpCloudSites.Size = New Size(547, 0)
        tlpCloudSites.TabIndex = 0
        ' 
        ' pnlSites
        ' 
        pnlSites.AutoScroll = True
        pnlSites.BackColor = SystemColors.AppWorkspace
        pnlSites.Controls.Add(tlpCloudSites)
        pnlSites.Dock = DockStyle.Fill
        pnlSites.Location = New Point(0, 0)
        pnlSites.Name = "pnlSites"
        pnlSites.Size = New Size(547, 450)
        pnlSites.TabIndex = 1
        ' 
        ' frmCloudSites
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(547, 450)
        Controls.Add(pnlSites)
        FormBorderStyle = FormBorderStyle.Fixed3D
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmCloudSites"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Cloud Sites"
        pnlSites.ResumeLayout(False)
        pnlSites.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents tlpCloudSites As TableLayoutPanel
    Friend WithEvents pnlSites As Panel
End Class
