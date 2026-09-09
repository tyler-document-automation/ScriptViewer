Imports System.Configuration
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Reflection
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar
Imports FastColoredTextBoxNS
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Text

Imports Newtonsoft.Json.Linq
Imports System.Threading
Imports System.Drawing.Drawing2D



Public Class frmMain
    Public Shared _connScriptViewer As SqlConnection
    Public Shared librarytype As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            LoadClientList()
        Catch ex As Exception
            MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub LoadClientList()

        Dim currentClient As String = cmbClients.Text

        Dim dt As New DataTable()
        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
            Try

                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@querytype", 0)
                    If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                        cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text)
                    End If

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using

                    Dim newRow As DataRow = dt.NewRow()
                    newRow("ClientName") = "--Select client--"
                    dt.Rows.InsertAt(newRow, 0)


                    cmbClients.DataSource = dt
                    cmbClients.DisplayMember = "ClientName"

                    Dim foundIndex As Integer = cmbClients.FindStringExact(currentClient)

                    If foundIndex >= 0 Then
                        cmbClients.SelectedIndex = foundIndex
                    Else
                        cmbClients.SelectedIndex = 0
                    End If


                End Using


            Catch ex As Exception
                MessageBox.Show("Error loading client list. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub



    Private Sub cmbClients_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbClients.SelectionChangeCommitted


        dgvEventLibraryEvents.Visible = False
        fctbScript.Visible = False
        If cmbClients.SelectedIndex > 0 Then
            btnCustomLibraries.Enabled = True
            btnEventLibraries.Enabled = True
            GetScriptVersion()
        Else
            btnCustomLibraries.Enabled = False
            btnEventLibraries.Enabled = False
        End If

        FillEvents()

    End Sub



    Private Sub GetScriptVersion()

        Dim databasename = cmbClients.Text


        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
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


        If cmbClients.SelectedIndex = 0 Then
            MsgBox("Please select a valid client.", Title:="")
            cmbClients.Focus()
            Exit Sub
        End If


        FillEvents()

    End Sub

    Private Sub FillEvents()
        dgvEventLibraryEvents.Visible = False
        fctbScript.Visible = False
        librarytype = "event"
        lblLibraryType.Visible = True
        lblLibraryType.Text = "Event Libraries:"
        lblEventFunction.Visible = False

        Dim databasename = cmbClients.Text

        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
            Try

                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                        cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text)
                    End If
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


        Dim databasename = cmbClients.Text

        If cmbClients.SelectedIndex = 0 Then
            MsgBox("Please select a valid client.", Title:="")
            cmbClients.Focus()
            Exit Sub
        End If

        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                        cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text)
                    End If
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

        Dim databasename = cmbClients.Text
        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@database", databasename)
                    cmd.Parameters.AddWithValue("@eventorcustom", librarytype)
                    If Not String.IsNullOrWhiteSpace(txtSearch.Text) Then
                        cmd.Parameters.AddWithValue("@searchstring", txtSearch.Text)
                    End If
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
                        dgvEventLibraryEvents.Columns(2).Visible = False

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

        Dim cellvalue = dgvEventLibraryEvents.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString

        fctbScript.Text = ""
        fctbScript.ReadOnly = True

        fctbScript.Language = Language.JS


        Dim databasename = cmbClients.Text

        Using _connScriptViewer As New SqlConnection(DatabaseConfig.ConnectionString)
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

    Private Sub LinksToolStripMenuItem_Click(sender As Object, e As EventArgs)
        MsgBox("open links")
    End Sub

    Private Sub ImportDataToolStripMenuItem_Click(sender As Object, e As EventArgs)
        MsgBox("open import data")
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            MsgBox("Please enter a search value")
        Else
            dgvEventLibraryEvents.Visible = False
            fctbScript.Visible = False
            LoadClientList()
            FillEvents()

        End If
    End Sub

    Private Sub btnUndo_Click(sender As Object, e As EventArgs) Handles btnUndo.Click
        txtSearch.Text = ""
        dgvEventLibraries.Visible = False
        dgvEventLibraryEvents.Visible = False
        lblLibraryType.Visible = False
        lblEventFunction.Visible = False
        fctbScript.Visible = False
        LoadClientList()

    End Sub

    Private Async Sub btnGetBatches_Click(sender As Object, e As EventArgs) Handles btnAPI.Click

        Try
            Await GetBatchesAsync()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Async Function GetBatchesAsync() As Task

        Dim apiUrl As String =
        "https://dev-us-east-1.prod.docauto.tylerapp.com/d-nd-state/core-api/api/transactional/batches"

        Using client As New HttpClient()

            client.DefaultRequestHeaders.Clear()

            client.DefaultRequestHeaders.TryAddWithoutValidation(
            "Accept",
            "application/json;odata.metadata=minimal;odata.streaming=true"
        )

            client.DefaultRequestHeaders.TryAddWithoutValidation("csi-AppName", "csi-ApiKey")
            client.DefaultRequestHeaders.TryAddWithoutValidation("csi-ApiKey", "bvarnell")

            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            Dim body As String = Await response.Content.ReadAsStringAsync()

            MessageBox.Show(
            "Status: " & CInt(response.StatusCode).ToString() &
            " " & response.ReasonPhrase &
            vbCrLf & vbCrLf &
            body
        )

        End Using

    End Function

    Private Sub btnCloudSites_Click(sender As Object, e As EventArgs) Handles btnCloudSites.Click

        Using frm As New frmCloudSites()
            frm.ShowDialog()
        End Using
    End Sub
    Private Sub tsVersionByClient_Click(sender As Object, e As EventArgs) Handles tsVersionByClient.Click
        frmVersionReport.ShowDialog()
    End Sub

    Private Sub tsAbout_Click(sender As Object, e As EventArgs) Handles tsAbout.Click
        MsgBox(Assembly.GetExecutingAssembly().GetName().Version.ToString, Title:="ScriptViewer Version")
    End Sub

    Private Sub mnuImport_Click(sender As Object, e As EventArgs) Handles mnuImport.Click
        frmImport.ShowDialog()
    End Sub
End Class
