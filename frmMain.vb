Imports System.Data.SqlTypes
Imports System.Reflection
Imports FastColoredTextBoxNS
Imports Microsoft.Data.SqlClient
Imports System.Configuration


Public Class frmMain
    'Public Shared _connIntellidact As SqlConnection
    Public Shared _connScriptViewer As SqlConnection
    Public Shared librarytype As String
    Public Shared connectionString

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            CreateConnection()
            LoadClientList()

            lblVersion.Text = "Version: " & Assembly.GetExecutingAssembly().GetName().Version.ToString

            'Me.WindowState = FormWindowState.Maximized

        Catch ex As Exception
            MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub CreateConnection()
#If DEBUG Then
        connectionString = ConfigurationManager.ConnectionStrings("ScriptViewerTest").ConnectionString
#ElseIf RELEASE Then
        connectionString = ConfigurationManager.ConnectionStrings("ScriptViewerProd").ConnectionString
#End If



        Dim environment As String = ConfigurationManager.AppSettings("Environment")
        'Dim connectionString As String

        'If environment = "test" Then
        '    connectionString = ConfigurationManager.ConnectionStrings("ScriptViewerTest").ConnectionString
        '    '_connScriptViewer = New SqlConnection(connectionString)
        'ElseIf environment = "prod" Then
        '    connectionString = ConfigurationManager.ConnectionStrings("ScriptViewerProd").ConnectionString
        '    '_connScriptViewer = New SqlConnection(connectionString)
        'End If



    End Sub

    Private Sub LoadClientList()

        Dim dt As New DataTable()

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try

                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@querytype", 0)

                    _connScriptViewer.Open()
                    'Dim reader As SqlDataReader = cmd.ExecuteReader()

                    cmbClients.Items.Clear()

                    cmbClients.Items.Add("--Select client...--")

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using

                    cmbClients.DataSource = dt
                    cmbClients.DisplayMember = "ClientName"
                    cmbClients.ValueMember = "DBName"

                    cmbClients.SelectedIndex = 0

                End Using


            Catch ex As Exception
                MessageBox.Show("Error loading client list. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub LoadClientListOld()

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try

                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@querytype", 0)

                    _connScriptViewer.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    cmbClients.Items.Clear()

                    cmbClients.Items.Add("--Select client...--")

                    While reader.Read()
                        cmbClients.Items.Add(reader("clientname").ToString())
                    End While

                    _connScriptViewer.Close()

                    cmbClients.SelectedIndex = 0

                End Using


            Catch ex As Exception
                MessageBox.Show("Error loading client list. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub cmbClients_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbClients.SelectionChangeCommitted
        dgvEventLibraries.Visible = False
        dgvEventLibraryEvents.Visible = False
        fctbScript.Visible = False
        If cmbClients.SelectedIndex > 0 Then
            btnCustomLibraries.Enabled = True
            btnEventLibraries.Enabled = True
            GetScriptVersion()
        Else
            btnCustomLibraries.Enabled = False
            btnEventLibraries.Enabled = False
            lblLibraryType.Visible = False
            lblEventFunction.Visible = False
            lblScriptVersion.Visible = False
        End If

        lblLibraryType.Visible = False
        lblEventFunction.Visible = False

    End Sub



    Private Sub GetScriptVersion()
        'Dim databasename = cmbClients.SelectedItem & "_intellidact"
        Dim databasename = cmbClients.SelectedValue
        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@querytype", 4)

                    Using adapter As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable
                        _connScriptViewer.Open()

                        Dim result As Object = cmd.ExecuteScalar

                        _connScriptViewer.Close()

                        lblScriptVersion.Text = result.ToString
                        lblScriptVersion.Visible = True
                    End Using

                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading version info. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnEventLibraries_Click(sender As Object, e As EventArgs) Handles btnEventLibraries.Click
        dgvEventLibraryEvents.Visible = False
        fctbScript.Visible = False
        librarytype = "event"
        lblLibraryType.Visible = True
        lblLibraryType.Text = "Event Libraries:"
        lblEventFunction.Visible = False

        'Dim databasename = cmbClients.SelectedItem & "_intellidact"
        Dim databasename = cmbClients.SelectedValue

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        If cmbClients.SelectedIndex = 0 Then
            MsgBox("Please select a valid client.", Title:="")
            cmbClients.Focus()
            Exit Sub
        End If

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try

                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    cmd.Parameters.AddWithValue("@querytype", 1)

                    Using adapter As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable

                        _connScriptViewer.Open()

                        adapter.Fill(dt)

                        _connScriptViewer.Close()

                        dgvEventLibraries.DataSource = dt

                        dgvEventLibraries.ShowCellToolTips = True

                        dgvEventLibraries.Columns(1).Visible = False
                    End Using

                End Using

                dgvEventLibraries.Columns(0).Width = 200
                dgvEventLibraries.Visible = True
                dgvEventLibraries.ClearSelection()
            Catch ex As Exception
                MessageBox.Show("Error loading event libraries. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub btnCustomLibraries_Click(sender As Object, e As EventArgs) Handles btnCustomLibraries.Click
        dgvEventLibraryEvents.Visible = False
        fctbScript.Visible = False
        librarytype = "custom"
        lblLibraryType.Visible = True
        lblLibraryType.Text = "Custom Libraries:"
        lblEventFunction.Visible = False


        'Dim databasename = cmbClients.SelectedItem & "_intellidact"
        Dim databasename = cmbClients.SelectedValue

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        If cmbClients.SelectedIndex = 0 Then
            MsgBox("Please select a valid client.", Title:="")
            cmbClients.Focus()
            Exit Sub
        End If

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    cmd.Parameters.AddWithValue("@querytype", 1)

                    Using adapter As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable

                        _connScriptViewer.Open()

                        adapter.Fill(dt)

                        _connScriptViewer.Close()

                        dgvEventLibraries.DataSource = dt

                        dgvEventLibraries.Columns(1).Visible = False
                    End Using

                End Using

                dgvEventLibraries.ClearSelection()
                dgvEventLibraries.Columns(0).Width = 200
                dgvEventLibraries.Visible = True
            Catch ex As Exception
                MessageBox.Show("Error loading custom libraries. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub dgvEventLibraries_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEventLibraries.CellClick
        fctbScript.Visible = False

        'Dim databasename = cmbClients.SelectedItem & "_intellidact"
        Dim databasename = cmbClients.SelectedValue

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    cmd.Parameters.AddWithValue("@querytype", 2)
                    cmd.Parameters.AddWithValue("@library", dgvEventLibraries.CurrentCell.Value)

                    Using adapter As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable

                        _connScriptViewer.Open()

                        adapter.Fill(dt)

                        _connScriptViewer.Close()

                        dgvEventLibraryEvents.DataSource = dt

                        dgvEventLibraryEvents.ShowCellToolTips = True

                        dgvEventLibraryEvents.Columns(1).Visible = False

                        ' dgvEventLibraries.Columns(0).Width = 200
                        dgvEventLibraryEvents.Visible = True
                    End Using

                End Using

                dgvEventLibraries.Columns(0).Width = 200
                dgvEventLibraries.Visible = True
                lblEventFunction.Visible = True
                If librarytype = "event" Then
                    lblEventFunction.Text = "Events:"
                Else
                    lblEventFunction.Text = "Functions:"
                End If

                dgvEventLibraryEvents.ClearSelection()
            Catch ex As Exception
                MessageBox.Show("Error loading library events. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub dgvEventLibraryEvents_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEventLibraryEvents.CellClick
        'fctbScript.Dock = DockStyle.Fill

        Dim cellvalue = dgvEventLibraryEvents.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString

        fctbScript.Text = ""
        fctbScript.ReadOnly = True

        fctbScript.Language = Language.JS


        'Dim databasename = cmbClients.SelectedItem & "_intellidact"
        Dim databasename = cmbClients.SelectedValue

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        Using _connScriptViewer = New SqlConnection(connectionString)
            Try


                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    cmd.Parameters.AddWithValue("@querytype", 3)
                    cmd.Parameters.AddWithValue("@library", dgvEventLibraries.CurrentCell.Value)
                    cmd.Parameters.AddWithValue("@eventfunctionname", cellvalue)

                    _connScriptViewer.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        While reader.Read
                            fctbScript.AppendText(reader("eventcode").ToString() & Environment.NewLine)

                        End While
                    End Using
                    _connScriptViewer.Close()

                End Using

                fctbScript.Visible = True

            Catch ex As Exception
                MessageBox.Show("Error loading event script. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub btnVersionReport_Click(sender As Object, e As EventArgs) Handles btnVersionReport.Click
        frmVersionReport.ShowDialog()

    End Sub

    Private Sub dgvEventLibraries_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvEventLibraries.CellFormatting
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            Dim tooltipValue As Object = dgvEventLibraries.Rows(e.RowIndex).Cells(1).Value

            ' Ensure tooltip is set only for valid (non-null) values
            If tooltipValue IsNot DBNull.Value AndAlso tooltipValue IsNot Nothing Then
                dgvEventLibraries.Rows(e.RowIndex).Cells(e.ColumnIndex).ToolTipText = tooltipValue.ToString()
            Else
                dgvEventLibraries.Rows(e.RowIndex).Cells(e.ColumnIndex).ToolTipText = "No additional info"
            End If
        End If
    End Sub

    Private Sub dgvEventLibraryEvents_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvEventLibraryEvents.CellFormatting
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            Dim tooltipValue As Object = dgvEventLibraryEvents.Rows(e.RowIndex).Cells(1).Value

            ' Ensure tooltip is set only for valid (non-null) values
            If tooltipValue IsNot DBNull.Value AndAlso tooltipValue IsNot Nothing Then
                dgvEventLibraryEvents.Rows(e.RowIndex).Cells(e.ColumnIndex).ToolTipText = tooltipValue.ToString()
            Else
                dgvEventLibraryEvents.Rows(e.RowIndex).Cells(e.ColumnIndex).ToolTipText = "No additional info"
            End If
        End If
    End Sub

    Private Sub LinksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LinksToolStripMenuItem.Click
        MsgBox("open links")
    End Sub

    Private Sub ImportDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportDataToolStripMenuItem.Click
        MsgBox("open import data")
    End Sub

    Friend WithEvents cmbClients As System.Windows.Forms.ComboBox
    Friend WithEvents dgvEventLibraries As DataGridView
    Friend WithEvents btnEventLibraries As System.Windows.Forms.Button
    Friend WithEvents btnCustomLibraries As System.Windows.Forms.Button
    Friend WithEvents dgvEventLibraryEvents As DataGridView
    Friend WithEvents fctbScript As FastColoredTextBoxNS.FastColoredTextBox


End Class
