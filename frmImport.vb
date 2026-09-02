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
            If Not FileCheck(folderPath) Then
                AddOutput("File check failed. Process halted.")
            Else
                CopyFiles(folderPath, tempFolder)
                'ImportData(client, tempFolder)
            End If

        Catch ex As Exception
            txtOutput.Text = "The import failed:" & Environment.NewLine & ex.Message
        End Try

    End Sub

    Private Function FileCheck(folderPath As String) As Boolean

        If String.IsNullOrWhiteSpace(folderPath) OrElse Not Directory.Exists(folderPath) Then
            AddOutput("The selected folder does not exist.")
            Return False
        End If

        If rdoNonCloud.Checked Then

            'NonCloud validation checks go here.
            Dim jsonFiles As String() = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly)

            If jsonFiles.Length <> 3 Then
                AddOutput("File check failed: The source folder must contain exactly three JSON files.")
                Return False
            End If

            Dim hasCustomLibraries As Boolean = Array.Exists(jsonFiles, Function(filePath) Path.GetFileName(filePath).StartsWith("custom_libraries", StringComparison.OrdinalIgnoreCase))
            Dim hasEventLibraries As Boolean = Array.Exists(jsonFiles, Function(filePath) Path.GetFileName(filePath).StartsWith("event_libraries", StringComparison.OrdinalIgnoreCase))
            Dim hasRelease As Boolean = Array.Exists(jsonFiles, Function(filePath) String.Equals(Path.GetFileName(filePath), "release.json", StringComparison.OrdinalIgnoreCase))

            If Not hasCustomLibraries Then
                AddOutput("File check failed: A JSON file starting with 'custom_libraries' was not found.")
                Return False
            End If

            If Not hasEventLibraries Then
                AddOutput("File check failed: A JSON file starting with 'event_libraries' was not found.")
                Return False
            End If

            If Not hasRelease Then
                AddOutput("File check failed: release.json was not found.")
                Return False
            End If

            AddOutput("Non-cloud file check passed.")


        Else 'Cloud validation checks go here.

            Dim files As String() = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly)
            Dim jsonFiles As String() = Array.FindAll(files, Function(filePath) String.Equals(Path.GetExtension(filePath), ".json", StringComparison.OrdinalIgnoreCase))

            If jsonFiles.Length = 0 Then
                AddOutput("File check failed: The source folder does not contain a JSON file.")
                Return False
            End If

            If jsonFiles.Length > 1 Then
                AddOutput("File check failed: The source folder contains more than one JSON file.")
                Return False
            End If

            AddOutput("Cloud file check passed: " & Path.GetFileName(jsonFiles(0)))

        End If

        'All checks passed.
        Return True

    End Function

    Private Sub CopyFiles(sourceFolder As String, ByRef tempFolder As String)

        Dim destinationRoot As String = "\\dbksvcsifsvai\CustomerImages\ScriptViewer\ImportFiles\"

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