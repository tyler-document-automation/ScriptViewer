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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCompare))
        cmbVersionLeft = New ComboBox()
        cmbVersionRight = New ComboBox()
        lblCompareClient = New Label()
        lblCompareLibrary = New Label()
        lblCompareItem = New Label()
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
        cmbVersionLeft.Location = New Point(24, 82)
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
        cmbVersionRight.Location = New Point(411, 82)
        cmbVersionRight.Name = "cmbVersionRight"
        cmbVersionRight.Size = New Size(226, 28)
        cmbVersionRight.TabIndex = 27
        ' 
        ' lblCompareClient
        ' 
        lblCompareClient.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblCompareClient.AutoEllipsis = True
        lblCompareClient.Font = New Font("Segoe UI", 11.25F)
        lblCompareClient.Location = New Point(24, 8)
        lblCompareClient.Name = "lblCompareClient"
        lblCompareClient.Size = New Size(741, 28)
        lblCompareClient.TabIndex = 30
        lblCompareClient.Text = "Client:"
        ' 
        ' lblCompareLibrary
        ' 
        lblCompareLibrary.AutoEllipsis = True
        lblCompareLibrary.Font = New Font("Segoe UI", 11.25F)
        lblCompareLibrary.Location = New Point(24, 36)
        lblCompareLibrary.Name = "lblCompareLibrary"
        lblCompareLibrary.Size = New Size(260, 28)
        lblCompareLibrary.TabIndex = 31
        lblCompareLibrary.Text = "Library:"
        ' 
        ' lblCompareItem
        ' 
        lblCompareItem.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblCompareItem.AutoEllipsis = True
        lblCompareItem.Font = New Font("Segoe UI", 11.25F)
        lblCompareItem.Location = New Point(354, 36)
        lblCompareItem.Name = "lblCompareItem"
        lblCompareItem.Size = New Size(354, 28)
        lblCompareItem.TabIndex = 32
        lblCompareItem.Text = "Event/Function:"
        ' 
        ' fctbLeft
        ' 
        fctbLeft.AutoCompleteBracketsList = New Char() {"("c, ")"c, "{"c, "}"c, "["c, "]"c, """"c, """"c, "'"c, "'"c}
        fctbLeft.AutoIndentCharsPatterns = "^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);" & vbCrLf & "^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);"
        fctbLeft.AutoScrollMinSize = New Size(31, 18)
        fctbLeft.BackBrush = Nothing
        fctbLeft.BackColor = Color.LightGray
        fctbLeft.BorderStyle = BorderStyle.FixedSingle
        fctbLeft.CharHeight = 18
        fctbLeft.CharWidth = 10
        fctbLeft.DisabledColor = Color.FromArgb(CByte(100), CByte(180), CByte(180), CByte(180))
        fctbLeft.Font = New Font("Courier New", 12F, FontStyle.Bold)
        fctbLeft.Hotkeys = resources.GetString("fctbLeft.Hotkeys")
        fctbLeft.IsReplaceMode = False
        fctbLeft.Location = New Point(24, 96)
        fctbLeft.Name = "fctbLeft"
        fctbLeft.Paddings = New Padding(0)
        fctbLeft.ReadOnly = True
        fctbLeft.SelectionColor = Color.FromArgb(CByte(60), CByte(0), CByte(0), CByte(255))
        fctbLeft.ServiceColors = CType(resources.GetObject("fctbLeft.ServiceColors"), FastColoredTextBoxNS.ServiceColors)
        fctbLeft.Size = New Size(354, 342)
        fctbLeft.TabIndex = 28
        fctbLeft.Visible = False
        fctbLeft.Zoom = 100
        ' 
        ' fctbRight
        ' 
        fctbRight.AutoCompleteBracketsList = New Char() {"("c, ")"c, "{"c, "}"c, "["c, "]"c, """"c, """"c, "'"c, "'"c}
        fctbRight.AutoIndentCharsPatterns = "^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);" & vbCrLf & "^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);"
        fctbRight.AutoScrollMinSize = New Size(31, 18)
        fctbRight.BackBrush = Nothing
        fctbRight.BackColor = Color.LightGray
        fctbRight.BorderStyle = BorderStyle.FixedSingle
        fctbRight.CharHeight = 18
        fctbRight.CharWidth = 10
        fctbRight.DisabledColor = Color.FromArgb(CByte(100), CByte(180), CByte(180), CByte(180))
        fctbRight.Font = New Font("Courier New", 12F, FontStyle.Bold)
        fctbRight.Hotkeys = resources.GetString("fctbRight.Hotkeys")
        fctbRight.IsReplaceMode = False
        fctbRight.Location = New Point(411, 96)
        fctbRight.Name = "fctbRight"
        fctbRight.Paddings = New Padding(0)
        fctbRight.ReadOnly = True
        fctbRight.SelectionColor = Color.FromArgb(CByte(60), CByte(0), CByte(0), CByte(255))
        fctbRight.ServiceColors = CType(resources.GetObject("fctbRight.ServiceColors"), FastColoredTextBoxNS.ServiceColors)
        fctbRight.Size = New Size(354, 342)
        fctbRight.TabIndex = 29
        fctbRight.Visible = False
        fctbRight.Zoom = 100
        ' 
        ' frmCompare
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(fctbRight)
        Controls.Add(fctbLeft)
        Controls.Add(lblCompareItem)
        Controls.Add(lblCompareLibrary)
        Controls.Add(lblCompareClient)
        Controls.Add(cmbVersionRight)
        Controls.Add(cmbVersionLeft)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "frmCompare"
        Text = "Version Compare"
        WindowState = FormWindowState.Maximized
        CType(fctbLeft, ComponentModel.ISupportInitialize).EndInit()
        CType(fctbRight, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents cmbVersionLeft As ComboBox
    Friend WithEvents cmbVersionRight As ComboBox
    Friend WithEvents lblCompareClient As Label
    Friend WithEvents lblCompareLibrary As Label
    Friend WithEvents lblCompareItem As Label
    Friend WithEvents fctbLeft As CompareTextBox
    Friend WithEvents fctbRight As CompareTextBox
End Class
