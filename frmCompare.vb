Imports FastColoredTextBoxNS
Imports Microsoft.Data.SqlClient

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
    Private ReadOnly _leftScrollMask As New Panel With {.BackColor = SystemColors.Control, .TabStop = False}
    Private _wheelRemainder As Integer
    Private ReadOnly _removedStyle As New TextStyle(Brushes.Black, Brushes.LightPink, FontStyle.Regular)
    Private ReadOnly _addedStyle As New TextStyle(Brushes.Black, Brushes.LightGreen, FontStyle.Regular)

    Private Sub frmCompare_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetUpEditorSplitter()
        lblCompareClient.Text = "Client: " & ClientName
        lblCompareLibrary.Text = "Library: " & LibraryName
        Dim itemKind As String = If(String.Equals(LibraryType, "custom", StringComparison.OrdinalIgnoreCase), "Function", "Event")
        lblCompareItem.Text = itemKind & ": " & EventFunctionName

        LoadVersions(cmbVersionLeft)
        LoadVersions(cmbVersionRight)
        cmbVersionLeft.SelectedIndex = If(cmbVersionLeft.Items.Count > 0, 0, -1)
        cmbVersionRight.SelectedIndex = If(cmbVersionRight.Items.Count > 1, 1, -1)

        RefreshScripts()
    End Sub

    Private Sub RefreshScripts()
        _synchronizingScroll = True

        Try
            LoadScript(cmbVersionLeft, fctbLeft)
            LoadScript(cmbVersionRight, fctbRight)
            HighlightDifferences()

            ScrollEditor(fctbLeft, 0)
            ScrollEditor(fctbRight, 0)
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
        .Padding = New Padding(24, 124, 35, 12)
    }

        Dim splitter As New SplitContainer With {
        .Dock = DockStyle.Fill,
        .Orientation = Orientation.Vertical,
        .SplitterWidth = 6,
        .IsSplitterFixed = True
    }

        'Leave both editors' native scroll ranges intact; only the right scrollbar is exposed.
        splitter.Panel1.Controls.Add(fctbLeft)
        splitter.Panel2.Controls.Add(fctbRight)

        fctbLeft.Dock = DockStyle.Fill
        fctbRight.Dock = DockStyle.Fill
        splitter.Panel1.Controls.Add(_leftScrollMask)
        AddHandler fctbLeft.SizeChanged, AddressOf PositionLeftScrollMask
        AddHandler fctbLeft.VisibleChanged, AddressOf PositionLeftScrollMask
        PositionLeftScrollMask(Nothing, EventArgs.Empty)

        editorArea.Controls.Add(splitter)
        Controls.Add(editorArea)
        editorArea.SendToBack()
        AddHandler splitter.SizeChanged, AddressOf EditorArea_SizeChanged
        AddHandler splitter.SplitterMoved, Sub(sender, e) AlignVersionDropdowns()

        If splitter.ClientSize.Width - splitter.SplitterWidth >= 300 Then
            splitter.SplitterDistance = (splitter.ClientSize.Width - splitter.SplitterWidth) \ 2
            splitter.Panel1MinSize = 150
            splitter.Panel2MinSize = 150
        End If
        AlignVersionDropdowns()
    End Sub

    Private Sub frmCompare_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        AlignVersionDropdowns()
    End Sub

    Private Sub AlignVersionDropdowns()
        If fctbLeft.Parent Is Nothing OrElse fctbRight.Parent Is Nothing Then Return

        cmbVersionLeft.Left = PointToClient(fctbLeft.PointToScreen(Point.Empty)).X
        cmbVersionRight.Left = PointToClient(fctbRight.PointToScreen(Point.Empty)).X
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
        Dim amount As Integer = If(linesPerNotch < 0, fctbRight.ClientSize.Height, linesPerNotch * fctbRight.CharHeight)
        Dim position As Integer = Math.Max(0, Math.Min(CommonScrollMaximum(), fctbRight.VerticalScroll.Value - notches * amount))
        ScrollBoth(position)
    End Sub

    Private Function CommonScrollMaximum() As Integer
        Dim leftScroll = fctbLeft.VerticalScroll
        Dim rightScroll = fctbRight.VerticalScroll
        Dim leftMaximum As Integer = Math.Max(0, leftScroll.Maximum - leftScroll.LargeChange + 1)
        Dim rightMaximum As Integer = Math.Max(0, rightScroll.Maximum - rightScroll.LargeChange + 1)
        Return Math.Min(leftMaximum, rightMaximum)
    End Function

    Private Sub PositionLeftScrollMask(sender As Object, e As EventArgs)
        _leftScrollMask.Bounds = New Rectangle(
            fctbLeft.Right - SystemInformation.VerticalScrollBarWidth, fctbLeft.Top,
            SystemInformation.VerticalScrollBarWidth, fctbLeft.Height)
        _leftScrollMask.Visible = fctbLeft.Visible
        _leftScrollMask.BringToFront()
    End Sub

    Private Sub EditorArea_SizeChanged(sender As Object, e As EventArgs)
        AlignVersionDropdowns()
        If _synchronizingScroll Then Return
        ScrollBoth(Math.Min(fctbRight.VerticalScroll.Value, CommonScrollMaximum()))
    End Sub

    Private Sub fctb_ScrollbarsUpdated(sender As Object, e As EventArgs) Handles fctbLeft.ScrollbarsUpdated, fctbRight.ScrollbarsUpdated
        If Not _synchronizingScroll AndAlso fctbLeft.Visible AndAlso fctbRight.Visible Then
            ScrollBoth(Math.Min(fctbRight.VerticalScroll.Value, CommonScrollMaximum()))
        End If
    End Sub

    Private Sub ScrollBoth(position As Integer)
        _synchronizingScroll = True
        Try
            Dim sharedPosition As Integer = Math.Max(0, Math.Min(position, CommonScrollMaximum()))
            ScrollEditor(fctbRight, sharedPosition)
            ScrollEditor(fctbLeft, sharedPosition)
        Finally
            _synchronizingScroll = False
        End Try
    End Sub

    Private Shared Sub ScrollEditor(editor As FastColoredTextBox, position As Integer)
        Dim scroll = editor.VerticalScroll
        Dim maximum As Integer = Math.Max(scroll.Minimum, scroll.Maximum - scroll.LargeChange + 1)
        Dim desired As Integer = Math.Max(scroll.Minimum, Math.Min(position, maximum))
        If scroll.Value = desired Then Return

        'Use the editor's scroll handler, not just its scrollbar thumb, to move the text.
        editor.OnScroll(New ScrollEventArgs(ScrollEventType.ThumbPosition, scroll.Value, desired,
            ScrollOrientation.VerticalScroll), False)
    End Sub

    Private Sub SynchronizeScroll(source As FastColoredTextBox)
        If _synchronizingScroll OrElse Not source.Visible Then Return

        ScrollBoth(Math.Max(0, Math.Min(CommonScrollMaximum(), source.VerticalScroll.Value)))
    End Sub

    Private Sub lblCompareItem_Click(sender As Object, e As EventArgs) Handles lblCompareItem.Click

    End Sub
End Class