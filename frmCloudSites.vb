Imports Microsoft.Data.SqlClient

Public Class frmCloudSites
    Private ReadOnly connectionString As String =
        ConfigurationManager.ConnectionStrings("ScriptViewerProd").ConnectionString

    Private Sub fmCloudSites_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCloudSites()
    End Sub

    Private Sub LoadCloudSites()

        tlpCloudSites.SuspendLayout()
        tlpCloudSites.Controls.Clear()
        tlpCloudSites.RowStyles.Clear()

        tlpCloudSites.ColumnCount = 5
        tlpCloudSites.RowCount = 1

        tlpCloudSites.ColumnStyles.Clear()
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 1000))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlpCloudSites.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))

        AddHeaderRow()

        Const sql As String =
            "SELECT Client, URLPrefix " &
            "FROM ClientAPI " &
            "WHERE URLPrefix IS NOT NULL " &
            "ORDER BY Client;"

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, conn)

                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            Dim clientName As String =
                                reader("Client").ToString().Trim()

                            Dim baseUrl As String =
                                reader("URLPrefix").ToString().Trim()

                            AddClientRow(clientName, baseUrl)

                        End While

                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show(
                $"Unable to load cloud sites.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Cloud Sites",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        Finally
            tlpCloudSites.ResumeLayout()
        End Try

    End Sub

    Private Sub AddHeaderRow()

        'tlpCloudSites.Controls.Add(CreateHeaderLabel("Client"), 0, 0)
        'tlpCloudSites.Controls.Add(CreateHeaderLabel("Admin"), 1, 0)
        'tlpCloudSites.Controls.Add(CreateHeaderLabel("Workflow"), 2, 0)
        'tlpCloudSites.Controls.Add(CreateHeaderLabel("Extraction"), 3, 0)
        'tlpCloudSites.Controls.Add(CreateHeaderLabel("Scripting"), 4, 0)

    End Sub

    Private Function CreateHeaderLabel(text As String) As Label

        Return New Label With {
            .Text = text,
            .AutoSize = True,
            .Font = New Font(Font, FontStyle.Bold),
            .Margin = New Padding(6),
            .Anchor = AnchorStyles.Left
        }

    End Function

    Private Sub AddClientRow(clientName As String, baseUrl As String)

        Dim rowIndex As Integer = tlpCloudSites.RowCount

        tlpCloudSites.RowCount += 1

        tlpCloudSites.RowStyles.Add(New RowStyle(SizeType.Absolute, 40)
)
        Dim lblClient As New Label With {
            .Text = clientName,
            .AutoSize = False,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Margin = New Padding(6, 4, 6, 4)
        }

        Dim btnAdmin As Button =
            CreateSiteButton("Admin", baseUrl, "admin/")

        Dim btnWorkflow As Button =
            CreateSiteButton("Workflow", baseUrl, "workflow/")

        Dim btnExtraction As Button =
            CreateSiteButton("Extraction", baseUrl, "extraction/")

        Dim btnScripting As Button =
            CreateSiteButton("Scripting", baseUrl, "scripting/")


        Dim rowColor As Color

        If rowIndex Mod 2 = 0 Then
            rowColor = Color.White
        Else
            rowColor = Color.Black
        End If

        tlpCloudSites.Controls.Add(lblClient, 0, rowIndex)
        tlpCloudSites.Controls.Add(btnAdmin, 1, rowIndex)
        tlpCloudSites.Controls.Add(btnWorkflow, 2, rowIndex)
        tlpCloudSites.Controls.Add(btnExtraction, 3, rowIndex)
        tlpCloudSites.Controls.Add(btnScripting, 4, rowIndex)

    End Sub

    Private Function CreateSiteButton(
    buttonText As String,
    baseUrl As String,
    sitePath As String) As Button

        Dim btn As New Button With {
            .Text = buttonText,
            .AutoSize = True,
            .Tag = BuildUrl(baseUrl, sitePath),
            .Margin = New Padding(4)
        }

        AddHandler btn.Click, AddressOf SiteButton_Click

        Return btn

    End Function

    Private Function BuildUrl(baseUrl As String, sitePath As String) As String

        Return $"{baseUrl.TrimEnd("/"c)}/{sitePath.TrimStart("/"c)}"

    End Function

    Private Sub SiteButton_Click(sender As Object, e As EventArgs)

        Dim clickedButton As Button = DirectCast(sender, Button)
        Dim url As String = clickedButton.Tag.ToString()

        Try
            Process.Start(New ProcessStartInfo(url) With {
                .UseShellExecute = True
            })

        Catch ex As Exception
            MessageBox.Show(
                $"Unable to open:{Environment.NewLine}{url}{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Cloud Sites",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try

    End Sub
End Class