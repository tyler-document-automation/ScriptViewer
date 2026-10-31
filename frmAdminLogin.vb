Public Class frmAdminLogin
    Private Sub frmAdminLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtPassword.UseSystemPasswordChar = True
        txtPassword.Focus()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please enter the password.")
            txtPassword.Focus()
            Return
        End If

        If txtPassword.Text <> "test" Then
            MessageBox.Show(
                "The password is incorrect.",
                "Login Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPassword.Clear()
            txtPassword.Focus()
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class