'Imports Microsoft.Identity.Extensions
Imports FastColoredTextBoxNS

Imports System.Windows.Forms.VisualStyles.VisualStyleElement


'Imports Microsoft.Data.SqlClient

'Public Class GlobalVariables
'    Public Shared EventLibrary As String
'End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    'Inherits MaterialSkin.Controls.MaterialForm
    Inherits System.Windows.Forms.Form


    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        dgvEventLibraries = New DataGridView()
        btnEventLibraries = New System.Windows.Forms.Button()
        btnCustomLibraries = New System.Windows.Forms.Button()
        dgvEventLibraryEvents = New DataGridView()
        lblLibraryType = New Label()
        SqlCommand1 = New Microsoft.Data.SqlClient.SqlCommand()
        lblEventFunction = New Label()
        lblScriptVersion = New Label()
        MenuStrip1 = New MenuStrip()
        ReportsToolStripMenuItem = New ToolStripMenuItem()
        tsVersionByClient = New ToolStripMenuItem()
        tsMenu = New ToolStripMenuItem()
        tsAbout = New ToolStripMenuItem()
        lblSearch = New Label()
        txtSearch = New System.Windows.Forms.TextBox()
        btnSearch = New System.Windows.Forms.Button()
        btnUndo = New System.Windows.Forms.Button()
        btnAPI = New System.Windows.Forms.Button()
        fctbScript = New FastColoredTextBox()
        Panel1 = New Panel()
        btnCloudSites = New System.Windows.Forms.Button()
        rdoCustom = New RadioButton()
        rdoEvent = New RadioButton()
        cmbClients = New System.Windows.Forms.ComboBox()
        CType(dgvEventLibraries, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvEventLibraryEvents, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        CType(fctbScript, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvEventLibraries
        ' 
        dgvEventLibraries.AllowUserToAddRows = False
        dgvEventLibraries.AllowUserToDeleteRows = False
        dgvEventLibraries.AllowUserToResizeColumns = False
        dgvEventLibraries.AllowUserToResizeRows = False
        dgvEventLibraries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEventLibraries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvEventLibraries.ColumnHeadersVisible = False
        dgvEventLibraries.Location = New Point(11, 158)
        dgvEventLibraries.MultiSelect = False
        dgvEventLibraries.Name = "dgvEventLibraries"
        dgvEventLibraries.ReadOnly = True
        dgvEventLibraries.RowHeadersVisible = False
        dgvEventLibraries.RowHeadersWidth = 100
        dgvEventLibraries.Size = New Size(240, 303)
        dgvEventLibraries.TabIndex = 1
        dgvEventLibraries.Visible = False
        ' 
        ' btnEventLibraries
        ' 
        btnEventLibraries.Enabled = False
        btnEventLibraries.FlatStyle = FlatStyle.System
        btnEventLibraries.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnEventLibraries.Location = New Point(15, 42)
        btnEventLibraries.Name = "btnEventLibraries"
        btnEventLibraries.Size = New Size(147, 36)
        btnEventLibraries.TabIndex = 2
        btnEventLibraries.Text = "Event Libraries"
        btnEventLibraries.UseVisualStyleBackColor = True
        ' 
        ' btnCustomLibraries
        ' 
        btnCustomLibraries.Enabled = False
        btnCustomLibraries.FlatStyle = FlatStyle.System
        btnCustomLibraries.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnCustomLibraries.Location = New Point(168, 42)
        btnCustomLibraries.Name = "btnCustomLibraries"
        btnCustomLibraries.Size = New Size(133, 36)
        btnCustomLibraries.TabIndex = 3
        btnCustomLibraries.Text = "Custom Libraries"
        btnCustomLibraries.UseVisualStyleBackColor = True
        ' 
        ' dgvEventLibraryEvents
        ' 
        dgvEventLibraryEvents.AllowUserToAddRows = False
        dgvEventLibraryEvents.AllowUserToDeleteRows = False
        dgvEventLibraryEvents.AllowUserToResizeRows = False
        dgvEventLibraryEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEventLibraryEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvEventLibraryEvents.ColumnHeadersVisible = False
        dgvEventLibraryEvents.Location = New Point(12, 501)
        dgvEventLibraryEvents.MultiSelect = False
        dgvEventLibraryEvents.Name = "dgvEventLibraryEvents"
        dgvEventLibraryEvents.ReadOnly = True
        dgvEventLibraryEvents.RowHeadersVisible = False
        dgvEventLibraryEvents.RowHeadersWidth = 100
        dgvEventLibraryEvents.Size = New Size(240, 358)
        dgvEventLibraryEvents.TabIndex = 4
        dgvEventLibraryEvents.Visible = False
        ' 
        ' lblLibraryType
        ' 
        lblLibraryType.AutoSize = True
        lblLibraryType.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblLibraryType.Location = New Point(12, 135)
        lblLibraryType.Name = "lblLibraryType"
        lblLibraryType.Size = New Size(102, 20)
        lblLibraryType.TabIndex = 8
        lblLibraryType.Text = "lblLibraryType"
        lblLibraryType.Visible = False
        ' 
        ' SqlCommand1
        ' 
        SqlCommand1.CommandTimeout = 30
        SqlCommand1.EnableOptimizedParameterBinding = False
        ' 
        ' lblEventFunction
        ' 
        lblEventFunction.AutoSize = True
        lblEventFunction.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblEventFunction.Location = New Point(11, 478)
        lblEventFunction.Name = "lblEventFunction"
        lblEventFunction.Size = New Size(51, 20)
        lblEventFunction.TabIndex = 9
        lblEventFunction.Text = "Events"
        lblEventFunction.Visible = False
        ' 
        ' lblScriptVersion
        ' 
        lblScriptVersion.AutoSize = True
        lblScriptVersion.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblScriptVersion.Location = New Point(332, 11)
        lblScriptVersion.Name = "lblScriptVersion"
        lblScriptVersion.Size = New Size(133, 20)
        lblScriptVersion.TabIndex = 10
        lblScriptVersion.Text = "ClientScriptVersion"
        lblScriptVersion.Visible = False
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {ReportsToolStripMenuItem, tsMenu})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1479, 24)
        MenuStrip1.TabIndex = 13
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ReportsToolStripMenuItem
        ' 
        ReportsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {tsVersionByClient})
        ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        ReportsToolStripMenuItem.Size = New Size(59, 20)
        ReportsToolStripMenuItem.Text = "Reports"
        ' 
        ' tsVersionByClient
        ' 
        tsVersionByClient.Name = "tsVersionByClient"
        tsVersionByClient.Size = New Size(162, 22)
        tsVersionByClient.Text = "Version by Client"
        ' 
        ' tsMenu
        ' 
        tsMenu.DropDownItems.AddRange(New ToolStripItem() {tsAbout})
        tsMenu.Name = "tsMenu"
        tsMenu.Size = New Size(44, 20)
        tsMenu.Text = "Help"
        ' 
        ' tsAbout
        ' 
        tsAbout.Name = "tsAbout"
        tsAbout.Size = New Size(116, 22)
        tsAbout.Text = "About..."
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(734, 11)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(56, 20)
        lblSearch.TabIndex = 15
        lblSearch.Text = "Search:"
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtSearch.Location = New Point(796, 9)
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(241, 27)
        txtSearch.TabIndex = 16
        ' 
        ' btnSearch
        ' 
        btnSearch.FlatStyle = FlatStyle.System
        btnSearch.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(806, 39)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(53, 27)
        btnSearch.TabIndex = 17
        btnSearch.Text = "Go"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' btnUndo
        ' 
        btnUndo.FlatStyle = FlatStyle.System
        btnUndo.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnUndo.Location = New Point(865, 39)
        btnUndo.Name = "btnUndo"
        btnUndo.Size = New Size(53, 27)
        btnUndo.TabIndex = 18
        btnUndo.Text = "Undo"
        btnUndo.UseVisualStyleBackColor = True
        ' 
        ' btnAPI
        ' 
        btnAPI.Location = New Point(1366, 34)
        btnAPI.Name = "btnAPI"
        btnAPI.Size = New Size(75, 23)
        btnAPI.TabIndex = 19
        btnAPI.Text = "API Test"
        btnAPI.UseVisualStyleBackColor = True
        btnAPI.Visible = False
        ' 
        ' fctbScript
        ' 
        fctbScript.AutoCompleteBracketsList = New Char() {"("c, ")"c, "{"c, "}"c, "["c, "]"c, """"c, """"c, "'"c, "'"c}
        fctbScript.AutoIndentCharsPatterns = "^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);" & vbCrLf & "^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);"
        fctbScript.AutoScrollMinSize = New Size(31, 18)
        fctbScript.AutoSize = True
        fctbScript.BackBrush = Nothing
        fctbScript.BackColor = Color.LightGray
        fctbScript.CharHeight = 18
        fctbScript.CharWidth = 10
        fctbScript.DisabledColor = Color.FromArgb(CByte(100), CByte(180), CByte(180), CByte(180))
        fctbScript.Font = New Font("Courier New", 12F, FontStyle.Bold)
        fctbScript.ForeColor = SystemColors.ActiveCaptionText
        fctbScript.Hotkeys = resources.GetString("fctbScript.Hotkeys")
        fctbScript.IsReplaceMode = False
        fctbScript.Location = New Point(277, 117)
        fctbScript.Name = "fctbScript"
        fctbScript.Paddings = New Padding(0)
        fctbScript.SelectionColor = Color.FromArgb(CByte(60), CByte(0), CByte(0), CByte(255))
        fctbScript.ServiceColors = CType(resources.GetObject("fctbScript.ServiceColors"), ServiceColors)
        fctbScript.Size = New Size(1190, 742)
        fctbScript.TabIndex = 6
        fctbScript.Visible = False
        fctbScript.Zoom = 100
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnCloudSites)
        Panel1.Controls.Add(rdoCustom)
        Panel1.Controls.Add(rdoEvent)
        Panel1.Controls.Add(cmbClients)
        Panel1.Controls.Add(btnEventLibraries)
        Panel1.Controls.Add(btnCustomLibraries)
        Panel1.Controls.Add(lblScriptVersion)
        Panel1.Controls.Add(btnUndo)
        Panel1.Controls.Add(btnSearch)
        Panel1.Controls.Add(lblSearch)
        Panel1.Controls.Add(txtSearch)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 24)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1479, 87)
        Panel1.TabIndex = 21
        ' 
        ' btnCloudSites
        ' 
        btnCloudSites.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnCloudSites.Location = New Point(1217, 11)
        btnCloudSites.Name = "btnCloudSites"
        btnCloudSites.Size = New Size(119, 37)
        btnCloudSites.TabIndex = 24
        btnCloudSites.Text = "Cloud Sites"
        btnCloudSites.UseVisualStyleBackColor = True
        ' 
        ' rdoCustom
        ' 
        rdoCustom.Appearance = Appearance.Button
        rdoCustom.AutoSize = True
        rdoCustom.Location = New Point(514, 53)
        rdoCustom.Name = "rdoCustom"
        rdoCustom.Size = New Size(106, 25)
        rdoCustom.TabIndex = 23
        rdoCustom.TabStop = True
        rdoCustom.Text = "Custom Libraries"
        rdoCustom.UseVisualStyleBackColor = True
        rdoCustom.Visible = False
        ' 
        ' rdoEvent
        ' 
        rdoEvent.Appearance = Appearance.Button
        rdoEvent.AutoSize = True
        rdoEvent.Location = New Point(402, 52)
        rdoEvent.Name = "rdoEvent"
        rdoEvent.Size = New Size(93, 25)
        rdoEvent.TabIndex = 22
        rdoEvent.TabStop = True
        rdoEvent.Text = "Event Libraries"
        rdoEvent.UseVisualStyleBackColor = True
        rdoEvent.Visible = False
        ' 
        ' cmbClients
        ' 
        cmbClients.DropDownStyle = ComboBoxStyle.DropDownList
        cmbClients.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbClients.FormattingEnabled = True
        cmbClients.ItemHeight = 20
        cmbClients.Location = New Point(16, 9)
        cmbClients.Name = "cmbClients"
        cmbClients.Size = New Size(298, 28)
        cmbClients.TabIndex = 21
        ' 
        ' frmMain
        ' 
        AcceptButton = btnSearch
        AutoScaleDimensions = New SizeF(96F, 96F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(1479, 872)
        Controls.Add(Panel1)
        Controls.Add(fctbScript)
        Controls.Add(btnAPI)
        Controls.Add(dgvEventLibraryEvents)
        Controls.Add(lblEventFunction)
        Controls.Add(lblLibraryType)
        Controls.Add(dgvEventLibraries)
        Controls.Add(MenuStrip1)
        Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        MaximizeBox = False
        MaximumSize = New Size(1495, 911)
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Script Viewer"
        CType(dgvEventLibraries, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvEventLibraryEvents, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(fctbScript, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
        ' Me.Text = "Form1"
    End Sub
    Friend WithEvents lblLibraryType As Label
    Friend WithEvents SqlCommand1 As Microsoft.Data.SqlClient.SqlCommand
    Friend WithEvents lblEventFunction As Label
    Friend WithEvents lblScriptVersion As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents tsMenu As ToolStripMenuItem
    Friend WithEvents ReportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsVersionByClient As ToolStripMenuItem
    Friend WithEvents tsAbout As ToolStripMenuItem
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnUndo As System.Windows.Forms.Button
    Friend WithEvents btnAPI As System.Windows.Forms.Button
    Friend WithEvents fctbScript As FastColoredTextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cmbClients As System.Windows.Forms.ComboBox
    Friend WithEvents dgvEventLibraries As DataGridView
    Friend WithEvents btnEventLibraries As System.Windows.Forms.Button
    Friend WithEvents btnCustomLibraries As System.Windows.Forms.Button
    Friend WithEvents dgvEventLibraryEvents As DataGridView
    Friend WithEvents rdoCustom As RadioButton
    Friend WithEvents rdoEvent As RadioButton
    Friend WithEvents btnCloudSites As System.Windows.Forms.Button


End Class
