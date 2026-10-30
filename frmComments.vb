Imports Microsoft.Data.SqlClient

Public Class frmComments
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property ClientName As String

    Private Sub frmComments_Load(sender As Object, e As EventArgs) Handles Me.Load
        If String.IsNullOrWhiteSpace(ClientName) Then
            MessageBox.Show("Select a client before viewing comments.", "Comments", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Close()
            Return
        End If

        'Text = "Comments - " & ClientName

        Try
            Dim comments As New DataTable()
            Using connection As New SqlConnection(frmMain.connectionString)
                Using command As New SqlCommand("GetScripts", connection)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("@database", SqlDbType.NVarChar, 200).Value = ClientName
                    command.Parameters.AddWithValue("@querytype", 7)

                    Using adapter As New SqlDataAdapter(command)
                        adapter.Fill(comments)
                    End Using
                End Using
            End Using

            If Not comments.Columns.Contains("VersionNumber") OrElse Not comments.Columns.Contains("Description") Then
                Throw New InvalidOperationException("GetScripts did not return VersionNumber and Description columns.")
            End If

            dgvComments.DataSource = comments
        Catch ex As Exception
            MessageBox.Show("Error loading comments. " & ex.Message, "Comments", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
