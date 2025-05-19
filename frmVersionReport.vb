Imports Microsoft.Data.SqlClient

Public Class frmVersionReport
    'Public Shared _connScriptViewer As SqlConnection

    Private Sub frmVersionReport_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("ScriptViewer").ConnectionString

        Using _connScriptViewer = New SqlConnection(frmMain.connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", _connScriptViewer)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@querytype", 5)

                    Using adapter As New SqlDataAdapter(cmd)

                        Dim dt As New DataTable

                        _connScriptViewer.Open()

                        adapter.Fill(dt)

                        _connScriptViewer.Close()

                        dgvVersionReport.DataSource = dt

                    End Using

                End Using


                dgvVersionReport.Columns(0).Width = 150
                dgvVersionReport.Columns(1).Width = 50
                dgvVersionReport.Columns(2).Width = 150

            Catch ex As Exception
                MessageBox.Show("Error loading version report. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End Try
        End Using
    End Sub
End Class