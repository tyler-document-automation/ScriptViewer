Imports Microsoft.Identity.Extensions
Imports FastColoredTextBoxNS

Imports System.Windows.Forms.VisualStyles.VisualStyleElement


'Imports Microsoft.Data.SqlClient

'Public Class GlobalVariables
'    Public Shared EventLibrary As String
'End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
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
        cmbClients = New System.Windows.Forms.ComboBox()
        dgvEventLibraries = New DataGridView()
        btnEventLibraries = New System.Windows.Forms.Button()
        btnCustomLibraries = New System.Windows.Forms.Button()
        dgvEventLibraryEvents = New DataGridView()
        fctbScript = New FastColoredTextBox()
        lblClient = New Label()
        lblLibraryType = New Label()
        SqlCommand1 = New Microsoft.Data.SqlClient.SqlCommand()
        lblEventFunction = New Label()
        lblScriptVersion = New Label()
        btnVersionReport = New System.Windows.Forms.Button()
        lblVersion = New Label()
        MenuStrip1 = New MenuStrip()
        tsMenu = New ToolStripMenuItem()
        LinksToolStripMenuItem = New ToolStripMenuItem()
        ImportDataToolStripMenuItem = New ToolStripMenuItem()
        CType(dgvEventLibraries, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvEventLibraryEvents, ComponentModel.ISupportInitialize).BeginInit()
        CType(fctbScript, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' cmbClients
        ' 
        cmbClients.DropDownStyle = ComboBoxStyle.DropDownList
        cmbClients.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbClients.FormattingEnabled = True
        cmbClients.Location = New Point(88, 38)
        cmbClients.Name = "cmbClients"
        cmbClients.Size = New Size(291, 28)
        cmbClients.Sorted = True
        cmbClients.TabIndex = 0
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
        dgvEventLibraries.Location = New Point(12, 103)
        dgvEventLibraries.MultiSelect = False
        dgvEventLibraries.Name = "dgvEventLibraries"
        dgvEventLibraries.ReadOnly = True
        dgvEventLibraries.RowHeadersVisible = False
        dgvEventLibraries.RowHeadersWidth = 100
        dgvEventLibraries.Size = New Size(240, 282)
        dgvEventLibraries.TabIndex = 1
        dgvEventLibraries.Visible = False
        ' 
        ' btnEventLibraries
        ' 
        btnEventLibraries.Enabled = False
        btnEventLibraries.FlatStyle = FlatStyle.System
        btnEventLibraries.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnEventLibraries.Location = New Point(876, 33)
        btnEventLibraries.Name = "btnEventLibraries"
        btnEventLibraries.Size = New Size(120, 36)
        btnEventLibraries.TabIndex = 2
        btnEventLibraries.Text = "Event Libraries"
        btnEventLibraries.UseVisualStyleBackColor = True
        ' 
        ' btnCustomLibraries
        ' 
        btnCustomLibraries.Enabled = False
        btnCustomLibraries.FlatStyle = FlatStyle.System
        btnCustomLibraries.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnCustomLibraries.Location = New Point(1018, 33)
        btnCustomLibraries.Name = "btnCustomLibraries"
        btnCustomLibraries.Size = New Size(159, 36)
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
        dgvEventLibraryEvents.Location = New Point(12, 427)
        dgvEventLibraryEvents.MultiSelect = False
        dgvEventLibraryEvents.Name = "dgvEventLibraryEvents"
        dgvEventLibraryEvents.ReadOnly = True
        dgvEventLibraryEvents.RowHeadersVisible = False
        dgvEventLibraryEvents.RowHeadersWidth = 100
        dgvEventLibraryEvents.Size = New Size(240, 347)
        dgvEventLibraryEvents.TabIndex = 4
        dgvEventLibraryEvents.Visible = False
        ' 
        ' fctbScript
        ' 
        fctbScript.AutoCompleteBracketsList = New Char() {"("c, ")"c, "{"c, "}"c, "["c, "]"c, """"c, """"c, "'"c, "'"c}
        fctbScript.AutoIndentCharsPatterns = "^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);" & vbCrLf & "^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);"
        fctbScript.AutoScrollMinSize = New Size(31, 18)
        fctbScript.BackBrush = Nothing
        fctbScript.BackColor = Color.LightGray
        fctbScript.CharHeight = 18
        fctbScript.CharWidth = 10
        fctbScript.DisabledColor = Color.FromArgb(CByte(100), CByte(180), CByte(180), CByte(180))
        fctbScript.Font = New Font("Courier New", 12F)
        fctbScript.Hotkeys = resources.GetString("fctbScript.Hotkeys")
        fctbScript.IsReplaceMode = False
        fctbScript.Location = New Point(281, 80)
        fctbScript.Name = "fctbScript"
        fctbScript.Paddings = New Padding(0)
        fctbScript.SelectionColor = Color.FromArgb(CByte(60), CByte(0), CByte(0), CByte(255))
        fctbScript.ServiceColors = CType(resources.GetObject("fctbScript.ServiceColors"), ServiceColors)
        fctbScript.Size = New Size(1326, 694)
        fctbScript.TabIndex = 6
        fctbScript.Visible = False
        fctbScript.Zoom = 100
        ' 
        ' lblClient
        ' 
        lblClient.AutoSize = True
        lblClient.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblClient.Location = New Point(12, 41)
        lblClient.Name = "lblClient"
        lblClient.Size = New Size(50, 20)
        lblClient.TabIndex = 7
        lblClient.Text = "Client:"
        ' 
        ' lblLibraryType
        ' 
        lblLibraryType.AutoSize = True
        lblLibraryType.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblLibraryType.Location = New Point(12, 80)
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
        lblEventFunction.Location = New Point(12, 404)
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
        lblScriptVersion.Location = New Point(404, 41)
        lblScriptVersion.Name = "lblScriptVersion"
        lblScriptVersion.Size = New Size(53, 20)
        lblScriptVersion.TabIndex = 10
        lblScriptVersion.Text = "Label1"
        lblScriptVersion.Visible = False
        ' 
        ' btnVersionReport
        ' 
        btnVersionReport.FlatStyle = FlatStyle.System
        btnVersionReport.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnVersionReport.Location = New Point(1260, 33)
        btnVersionReport.Name = "btnVersionReport"
        btnVersionReport.Size = New Size(183, 36)
        btnVersionReport.TabIndex = 11
        btnVersionReport.Text = "Script Version By Client"
        btnVersionReport.UseVisualStyleBackColor = True
        ' 
        ' lblVersion
        ' 
        lblVersion.AutoSize = True
        lblVersion.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblVersion.Location = New Point(1522, 38)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(53, 20)
        lblVersion.TabIndex = 12
        lblVersion.Text = "Label1"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {tsMenu})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1662, 24)
        MenuStrip1.TabIndex = 13
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' tsMenu
        ' 
        tsMenu.DropDownItems.AddRange(New ToolStripItem() {LinksToolStripMenuItem, ImportDataToolStripMenuItem})
        tsMenu.Name = "tsMenu"
        tsMenu.Size = New Size(44, 20)
        tsMenu.Text = "Help"
        ' 
        ' LinksToolStripMenuItem
        ' 
        LinksToolStripMenuItem.Name = "LinksToolStripMenuItem"
        LinksToolStripMenuItem.Size = New Size(180, 22)
        LinksToolStripMenuItem.Text = "Links"
        ' 
        ' ImportDataToolStripMenuItem
        ' 
        ImportDataToolStripMenuItem.Name = "ImportDataToolStripMenuItem"
        ImportDataToolStripMenuItem.Size = New Size(180, 22)
        ImportDataToolStripMenuItem.Text = "Import Data..."
        ' 
        ' frmMain
        ' 
        BackColor = SystemColors.ActiveCaption
        ClientSize = New Size(1662, 871)
        Controls.Add(lblVersion)
        Controls.Add(btnVersionReport)
        Controls.Add(lblScriptVersion)
        Controls.Add(dgvEventLibraryEvents)
        Controls.Add(lblEventFunction)
        Controls.Add(lblLibraryType)
        Controls.Add(dgvEventLibraries)
        Controls.Add(lblClient)
        Controls.Add(fctbScript)
        Controls.Add(btnCustomLibraries)
        Controls.Add(btnEventLibraries)
        Controls.Add(cmbClients)
        Controls.Add(MenuStrip1)
        Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        FormBorderStyle = FormBorderStyle.Fixed3D
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        MaximizeBox = False
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Script Viewer"
        CType(dgvEventLibraries, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvEventLibraryEvents, ComponentModel.ISupportInitialize).EndInit()
        CType(fctbScript, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
        ' Me.Text = "Form1"
    End Sub

    Friend WithEvents lblClient As Label
    Friend WithEvents lblLibraryType As Label
    Friend WithEvents SqlCommand1 As Microsoft.Data.SqlClient.SqlCommand
    Friend WithEvents lblEventFunction As Label
    Friend WithEvents lblScriptVersion As Label
    Friend WithEvents btnVersionReport As System.Windows.Forms.Button
    Friend WithEvents lblVersion As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents tsMenu As ToolStripMenuItem
    Friend WithEvents LinksToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportDataToolStripMenuItem As ToolStripMenuItem



End Class
