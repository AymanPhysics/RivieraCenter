Public Class PCs

    Dim bm As New BasicMethods

    Private Sub TextBox2_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TextBox2.TextChanged
        Try
            If TextBox2.Text = s Then
                bm.ExecuteNonQuery("insert PCs(Id,Name,MyGetDate,UserName) select isnull(MAX(Id),0)+1,'" & s & "',GETDATE(),0 from PCs")
                Forms.Application.Restart()
                Close()
                Application.Current.Run()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim s As String

    Private Sub PCs_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        s = bm.Encrypt(bm.ProtectionSerial())
    End Sub
End Class
