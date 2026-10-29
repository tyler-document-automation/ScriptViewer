Imports FastColoredTextBoxNS

Public Class CompareTextBox
    Inherits FastColoredTextBox

    Public Event SharedWheel As MouseEventHandler

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        RaiseEvent SharedWheel(Me, e)
    End Sub
End Class
