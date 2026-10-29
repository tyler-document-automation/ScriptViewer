<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCompare
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
        cmbVersionLeft = New ComboBox()
        cmbVersionRight = New ComboBox()
        fctbLeft = New CompareTextBox()
        fctbRight = New CompareTextBox()
        CType(fctbLeft, ComponentModel.ISupportInitialize).BeginInit()
        CType(fctbRight, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' cmbVersionLeft
        ' 
        cmbVersionLeft.DropDownStyle = ComboBoxStyle.DropDownList
        cmbVersionLeft.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbVersionLeft.FormattingEnabled = True
        cmbVersionLeft.ItemHeight = 20
        cmbVersionLeft.Location = New Point(77, 34)
        cmbVersionLeft.Name = "cmbVersionLeft"
        cmbVersionLeft.Size = New Size(226, 28)
        cmbVersionLeft.TabIndex = 26
        ' 
        ' cmbVersionRight
        ' 
        cmbVersionRight.DropDownStyle = ComboBoxStyle.DropDownList
        cmbVersionRight.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbVersionRight.FormattingEnabled = True
        cmbVersionRight.ItemHeight = 20
        cmbVersionRight.Location = New Point(432, 34)
        cmbVersionRight.Name = "cmbVersionRight"
        cmbVersionRight.Size = New Size(226, 28)
        cmbVersionRight.TabIndex = 27
        ' 
        ' fctbLeft
        ' 
        fctbLeft.BackColor = Color.LightGray
        fctbLeft.CharHeight = 18
        fctbLeft.CharWidth = 10
        fctbLeft.Font = New Font("Courier New", 12.0F, FontStyle.Bold)
        fctbLeft.Location = New Point(24, 96)
        fctbLeft.Name = "fctbLeft"
        fctbLeft.ReadOnly = True
        fctbLeft.Size = New Size(354, 342)
        fctbLeft.TabIndex = 28
        fctbLeft.Visible = False
        fctbLeft.WordWrap = False
        ' 
        ' fctbRight
        ' 
        fctbRight.BackColor = Color.LightGray
        fctbRight.CharHeight = 18
        fctbRight.CharWidth = 10
        fctbRight.Font = New Font("Courier New", 12.0F, FontStyle.Bold)
        fctbRight.Location = New Point(411, 96)
        fctbRight.Name = "fctbRight"
        fctbRight.ReadOnly = True
        fctbRight.Size = New Size(354, 342)
        fctbRight.TabIndex = 29
        fctbRight.Visible = False
        fctbRight.WordWrap = False
        ' 
        ' frmCompare
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(fctbRight)
        Controls.Add(fctbLeft)
        Controls.Add(cmbVersionRight)
        Controls.Add(cmbVersionLeft)
        MaximizeBox = False
        Name = "frmCompare"
        Text = "frmCompare"
        WindowState = FormWindowState.Maximized
        CType(fctbLeft, ComponentModel.ISupportInitialize).EndInit()
        CType(fctbRight, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents cmbVersionLeft As ComboBox
    Friend WithEvents cmbVersionRight As ComboBox
    Friend WithEvents fctbLeft As CompareTextBox
    Friend WithEvents fctbRight As CompareTextBox
End Class
