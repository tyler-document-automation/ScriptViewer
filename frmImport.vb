Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Text
Imports System.IO


Public Class frmImport
    Public Shared connectionString
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using folderDialog As New FolderBrowserDialog()

            folderDialog.Description = "Select the folder"
            folderDialog.ShowNewFolderButton = True

            If folderDialog.ShowDialog() = DialogResult.OK Then
                txtFolderPath.Text = folderDialog.SelectedPath
            End If

        End Using
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If txtFolderPath.Text = "" Or txtClient.Text = "" Then
            MessageBox.Show("Please enter a client and folder path")
            Return
        End If

        Dim client As String = txtClient.Text.Trim()
        Dim folderPath As String = txtFolderPath.Text.Trim()

        txtOutput.Clear()

        Try
            Dim tempFolder As String = String.Empty
            CopyFiles(folderPath, tempFolder)
            ImportData(client, tempFolder)




        Catch ex As Exception
            txtOutput.Text =
            "The import failed:" &
            Environment.NewLine &
            ex.Message
        End Try

    End Sub



    Private Sub CopyFiles(sourceFolder As String, ByRef tempFolder As String)

        Dim destinationRoot As String = "\\dbksvcsifsvai\Development\BrianV\ScriptViewer\ImportFiles"

        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")

        tempFolder = Path.Combine(destinationRoot, timestamp)

        'Create the timestamped destination folder.
        Directory.CreateDirectory(tempFolder)

        AddOutput("Copying files to " & tempFolder & "...")

        'Copy only the files directly inside the selected folder.
        For Each sourceFile As String In Directory.GetFiles(sourceFolder)

            Dim fileName As String = Path.GetFileName(sourceFile)

            Dim destinationFile As String = Path.Combine(tempFolder, fileName)

            File.Copy(sourceFile, destinationFile, overwrite:=True)

        Next

        AddOutput("Files copied successfully.")

    End Sub

    Private Sub ImportData(client As String, folderpath As String)

        connectionString = ConfigurationManager.ConnectionStrings("ScriptViewerProd").ConnectionString


        AddOutput("Import running...")

        Dim procedureOutput As New StringBuilder()

        Using connection As New SqlConnection(connectionString)

            AddHandler connection.InfoMessage,
            Sub(infoSender As Object, infoArgs As SqlInfoMessageEventArgs)
                procedureOutput.AppendLine(infoArgs.Message)
            End Sub

            Using command As New SqlCommand("ImportDataNew", connection)

                command.CommandType = CommandType.StoredProcedure

                command.Parameters.Add(
                "@ClientName",
                SqlDbType.VarChar,
                255
            ).Value = client

                command.Parameters.Add(
                "@FilePath",
                SqlDbType.VarChar,
                200
            ).Value = folderpath

                connection.Open()
                command.ExecuteNonQuery()

            End Using
        End Using

        If procedureOutput.Length > 0 Then
            AddOutput(procedureOutput.ToString())
        Else
            AddOutput("The procedure completed without producing output.")
        End If
    End Sub

    Private Sub AddOutput(message As String)
        txtOutput.AppendText(message & Environment.NewLine)
        txtOutput.AppendText(Environment.NewLine)

        'Automatically scroll to the latest output.
        txtOutput.SelectionStart = txtOutput.TextLength
        txtOutput.ScrollToCaret()
        txtOutput.Refresh()
    End Sub

End Class