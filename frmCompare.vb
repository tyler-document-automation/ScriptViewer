Imports FastColoredTextBoxNS
Imports Microsoft.Data.SqlClient

Public Class CompareTextBox
    Inherits FastColoredTextBox

    Public Event SharedWheel As MouseEventHandler

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        RaiseEvent SharedWheel(Me, e)
    End Sub
End Class

Public Class frmCompare

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property ClientName As String

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property LibraryType As String

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property LibraryName As String

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property EventFunctionName As String

    Private _leftScript As String
    Private _rightScript As String
    Private _synchronizingScroll As Boolean
    Private WithEvents _sharedScroll As New VScrollBar()
    Private _wheelRemainder As Integer
    Private ReadOnly _removedStyle As New TextStyle(Brushes.Black, Brushes.LightPink, FontStyle.Regular)
    Private ReadOnly _addedStyle As New TextStyle(Brushes.Black, Brushes.LightGreen, FontStyle.Regular)

    Private Sub frmCompare_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetUpEditorSplitter()

        LoadVersions(cmbVersionLeft)
        LoadVersions(cmbVersionRight)

        RefreshScripts()
    End Sub

    Private Sub RefreshScripts()
        _synchronizingScroll = True

        Try
            LoadScript(cmbVersionLeft, fctbLeft)
            LoadScript(cmbVersionRight, fctbRight)
            HighlightDifferences()

            fctbLeft.VerticalScroll.Value = fctbLeft.VerticalScroll.Minimum
            fctbRight.VerticalScroll.Value = fctbRight.VerticalScroll.Minimum
            UpdateScrollRanges()
            _sharedScroll.Value = 0
        Finally
            _synchronizingScroll = False
        End Try
    End Sub

    Private Sub LoadVersions(target As ComboBox)
        Dim dt As New DataTable()

        Using conn As New SqlConnection(frmMain.connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@database", ClientName)
                    cmd.Parameters.AddWithValue("@querytype", 6)

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using

                target.DisplayMember = "VersionInfo"
                target.ValueMember = "VersionNumber"
                target.DataSource = dt
            Catch ex As Exception
                MessageBox.Show("Error loading version list. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub LoadScript(versionList As ComboBox, editor As FastColoredTextBox)
        editor.Clear()
        editor.Visible = False

        If editor Is fctbLeft Then
            _leftScript = Nothing
        Else
            _rightScript = Nothing
        End If

        If versionList.SelectedValue Is Nothing OrElse TypeOf versionList.SelectedValue Is DataRowView Then
            Return
        End If

        editor.Language = Language.JS

        Using conn As New SqlConnection(frmMain.connectionString)
            Try
                Using cmd As New SqlCommand("GetScripts", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@database", ClientName)
                    cmd.Parameters.AddWithValue("@eventorcustom", LibraryType)
                    cmd.Parameters.AddWithValue("@querytype", 3)
                    cmd.Parameters.AddWithValue("@library", LibraryName)
                    cmd.Parameters.AddWithValue("@eventfunctionname", EventFunctionName)
                    cmd.Parameters.AddWithValue("@versionnumber", versionList.SelectedValue)

                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            editor.AppendText(reader("eventcode").ToString() & Environment.NewLine)
                        End While
                    End Using
                End Using

                If editor Is fctbLeft Then
                    _leftScript = editor.Text
                Else
                    _rightScript = editor.Text
                End If

                editor.Visible = True
            Catch ex As Exception
                MessageBox.Show("Error loading event script. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub cmbVersionLeft_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbVersionLeft.SelectionChangeCommitted
        RefreshScripts()
    End Sub

    Private Sub cmbVersionRight_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbVersionRight.SelectionChangeCommitted
        RefreshScripts()
    End Sub

    Private Sub SetUpEditorSplitter()
        Dim editorArea As New Panel With {
        .Dock = DockStyle.Fill,
        .Padding = New Padding(24, 96, 35, 12)
    }

        Dim splitter As New SplitContainer With {
        .Dock = DockStyle.Fill,
        .Orientation = Orientation.Vertical,
        .SplitterWidth = 6,
        .IsSplitterFixed = True
    }

        _sharedScroll.Dock = DockStyle.Right
        _sharedScroll.Width = SystemInformation.VerticalScrollBarWidth
        _sharedScroll.SmallChange = fctbLeft.CharHeight

        'The editor needs its native scrollbars to maintain its internal scroll range.
        splitter.Panel1.Controls.Add(fctbLeft)
        splitter.Panel2.Controls.Add(fctbRight)

        fctbLeft.Dock = DockStyle.Fill
        fctbRight.Dock = DockStyle.Fill

        editorArea.Controls.Add(splitter)
        editorArea.Controls.Add(_sharedScroll)
        Controls.Add(editorArea)
        editorArea.SendToBack()
        AddHandler splitter.SizeChanged, AddressOf EditorArea_SizeChanged

        If splitter.ClientSize.Width - splitter.SplitterWidth >= 300 Then
            splitter.SplitterDistance = (splitter.ClientSize.Width - splitter.SplitterWidth) \ 2
            splitter.Panel1MinSize = 150
            splitter.Panel2MinSize = 150
        End If
    End Sub

    Private Sub HighlightDifferences()
        If _leftScript Is Nothing OrElse _rightScript Is Nothing Then
            If _leftScript IsNot Nothing Then
                fctbLeft.Text = _leftScript
            End If

            If _rightScript IsNot Nothing Then
                fctbRight.Text = _rightScript
            End If

            Return
        End If

        Dim leftLines As String() = _leftScript.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        Dim rightLines As String() = _rightScript.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        Dim leftCount As Integer = leftLines.Length
        Dim rightCount As Integer = rightLines.Length
        Dim matches(leftCount, rightCount) As Integer

        For leftIndex As Integer = leftCount - 1 To 0 Step -1
            For rightIndex As Integer = rightCount - 1 To 0 Step -1
                If leftLines(leftIndex) = rightLines(rightIndex) Then
                    matches(leftIndex, rightIndex) = matches(leftIndex + 1, rightIndex + 1) + 1
                Else
                    matches(leftIndex, rightIndex) = Math.Max(
                        matches(leftIndex + 1, rightIndex),
                        matches(leftIndex, rightIndex + 1))
                End If
            Next
        Next

        Dim alignedLeft As New List(Of String)()
        Dim alignedRight As New List(Of String)()
        Dim removedLines As New List(Of Integer)()
        Dim addedLines As New List(Of Integer)()
        Dim leftPosition As Integer = 0
        Dim rightPosition As Integer = 0

        While leftPosition < leftCount OrElse rightPosition < rightCount
            If leftPosition < leftCount AndAlso rightPosition < rightCount AndAlso
               leftLines(leftPosition) = rightLines(rightPosition) Then
                alignedLeft.Add(leftLines(leftPosition))
                alignedRight.Add(rightLines(rightPosition))
                leftPosition += 1
                rightPosition += 1
            ElseIf leftPosition < leftCount AndAlso
                   (rightPosition = rightCount OrElse
                    matches(leftPosition + 1, rightPosition) >= matches(leftPosition, rightPosition + 1)) Then
                removedLines.Add(alignedLeft.Count)
                alignedLeft.Add(leftLines(leftPosition))
                alignedRight.Add(String.Empty)
                leftPosition += 1
            Else
                addedLines.Add(alignedRight.Count)
                alignedLeft.Add(String.Empty)
                alignedRight.Add(rightLines(rightPosition))
                rightPosition += 1
            End If
        End While

        fctbLeft.Text = String.Join(Environment.NewLine, alignedLeft)
        fctbRight.Text = String.Join(Environment.NewLine, alignedRight)

        For Each lineIndex As Integer In removedLines
            HighlightLine(fctbLeft, lineIndex, _removedStyle)
        Next

        For Each lineIndex As Integer In addedLines
            HighlightLine(fctbRight, lineIndex, _addedStyle)
        Next
    End Sub

    Private Sub HighlightLine(editor As FastColoredTextBox, lineIndex As Integer, style As TextStyle)
        Dim lineRange As New FastColoredTextBoxNS.Range(
            editor,
            New Place(0, lineIndex),
            New Place(editor.Lines(lineIndex).Length, lineIndex))

        lineRange.SetStyle(style)
    End Sub

    Private Sub fctbLeft_VisibleRangeChanged(sender As Object, e As EventArgs) Handles fctbLeft.VisibleRangeChanged
        SynchronizeScroll(fctbLeft)
    End Sub

    Private Sub fctbRight_VisibleRangeChanged(sender As Object, e As EventArgs) Handles fctbRight.VisibleRangeChanged
        SynchronizeScroll(fctbRight)
    End Sub

    Private Sub fctb_SharedWheel(sender As Object, e As MouseEventArgs) Handles fctbLeft.SharedWheel, fctbRight.SharedWheel
        _wheelRemainder += e.Delta
        Dim notches As Integer = CInt(Math.Truncate(_wheelRemainder / 120.0))
        _wheelRemainder -= notches * 120
        If notches = 0 Then Return

        Dim linesPerNotch As Integer = SystemInformation.MouseWheelScrollLines
        Dim amount As Integer = If(linesPerNotch < 0, _sharedScroll.LargeChange, linesPerNotch * fctbLeft.CharHeight)
        Dim maximum As Integer = ScrollMaximum(_sharedScroll)
        _sharedScroll.Value = Math.Max(0, Math.Min(maximum, _sharedScroll.Value - notches * amount))
    End Sub

    Private Shared Function ScrollMaximum(scrollbar As ScrollBar) As Integer
        Return Math.Max(scrollbar.Minimum, scrollbar.Maximum - scrollbar.LargeChange + 1)
    End Function

    Private Sub UpdateScrollRanges()
        If Not fctbLeft.IsHandleCreated OrElse Not fctbRight.IsHandleCreated Then Return

        _sharedScroll.LargeChange = Math.Max(1, fctbLeft.ClientSize.Height)
        Dim leftMaximum As Integer = Math.Max(0, fctbLeft.VerticalScroll.Maximum - fctbLeft.VerticalScroll.LargeChange + 1)
        Dim rightMaximum As Integer = Math.Max(0, fctbRight.VerticalScroll.Maximum - fctbRight.VerticalScroll.LargeChange + 1)
        Dim maximum As Integer = Math.Min(leftMaximum, rightMaximum)
        _sharedScroll.Maximum = maximum + _sharedScroll.LargeChange - 1
        _sharedScroll.Enabled = maximum > 0
    End Sub

    Private Sub EditorArea_SizeChanged(sender As Object, e As EventArgs)
        If _synchronizingScroll Then Return
        UpdateScrollRanges()
        ScrollBoth(_sharedScroll.Value)
    End Sub

    Private Sub fctb_ScrollbarsUpdated(sender As Object, e As EventArgs) Handles fctbLeft.ScrollbarsUpdated, fctbRight.ScrollbarsUpdated
        If Not _synchronizingScroll Then UpdateScrollRanges()
    End Sub

    Private Sub _sharedScroll_ValueChanged(sender As Object, e As EventArgs) Handles _sharedScroll.ValueChanged
        If Not _synchronizingScroll Then ScrollBoth(_sharedScroll.Value)
    End Sub

    Private Sub ScrollBoth(position As Integer)
        _synchronizingScroll = True
        Try
            fctbLeft.VerticalScroll.Value = Math.Min(position,
                Math.Max(0, fctbLeft.VerticalScroll.Maximum - fctbLeft.VerticalScroll.LargeChange + 1))
            fctbRight.VerticalScroll.Value = Math.Min(position,
                Math.Max(0, fctbRight.VerticalScroll.Maximum - fctbRight.VerticalScroll.LargeChange + 1))
        Finally
            _synchronizingScroll = False
        End Try
    End Sub

    Private Sub SynchronizeScroll(source As FastColoredTextBox)
        If _synchronizingScroll OrElse Not source.Visible Then Return

        Dim position As Integer = Math.Max(0, Math.Min(ScrollMaximum(_sharedScroll), source.VerticalScroll.Value))
        _synchronizingScroll = True
        Try
            _sharedScroll.Value = position
        Finally
            _synchronizingScroll = False
        End Try
        ScrollBoth(position)
    End Sub

End Class