Imports System
Imports System.Threading
Public Class MSG
    Dim bm As New BasicMethods
    Public Ok As Boolean
    Public DelMsg As Boolean = False
    Public MSG As String

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Ok = False
        If Not DelMsg Then
            btnNo.Width = 0
            btnNo.Height = 0
            btnYes.Content = Application.Current.MainWindow.Resources.Item("Exit")
            'btnYes.Focus()
        End If
        lblMSG.Text = MSG
        'lblMSG.wrap()
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNo.Click
        Ok = False
        Close()
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnYes.Click
        Ok = True
        Close()
    End Sub

    Private Sub LoadResource()
        btnYes.SetResourceReference(Button.ContentProperty, "Yes")
        btnNo.SetResourceReference(Button.ContentProperty, "No")

    End Sub
End Class
