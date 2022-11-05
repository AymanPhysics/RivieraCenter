Imports System.Data
Imports System.Windows.Controls.Primitives

Public Class Login

    Dim bm As New BasicMethods
    Public Flag As Integer = 1
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogin.Click

        If Username.Text.Trim = "" Or Username.Text.Trim = "-" Or Username.SelectedIndex < 0 Then
            Username.Focus()
            Return
        End If

        If Password.Password.Trim = "" Then
            Password.Focus()
            Return
        End If

        Dim dt As DataTable

        If Not bm.StopPro() Then Return
        Dim paraname() As String = {"Id", "Password"}
        Dim paravalue() As String = {Username.SelectedValue.ToString, bm.Encrypt(Password.Password)}
        dt = bm.ExecuteAdapter("TestLogin", paraname, paravalue)
        If dt.Rows.Count = 0 Then
            bm.ShowMSG("Invalid Password ...")
            Password.Focus()
            Password.SelectAll()
            Exit Sub
        End If
        Md.UserName = Username.SelectedValue.ToString
        Md.ArName = dt.Rows(0)("Name").ToString
        Md.EnName = dt.Rows(0)("EnName").ToString
        Md.LevelId = dt.Rows(0)("LevelId").ToString
        Md.Password = bm.Decrypt(dt.Rows(0)("Password").ToString)
        Md.CompanyName = dt.Rows(0)("CompanyName").ToString
        Md.CompanyTel = dt.Rows(0)("CompanyTel").ToString
        Md.Manager = IIf(dt.Rows(0)("Manager").ToString() = "1", True, False)
        Md.Nurse = dt.Rows(0)("Nurse").ToString
        Md.Receptionist = IIf(dt.Rows(0)("Receptionist").ToString() = "1", True, False)

        Md.DefaultStore = dt.Rows(0)("DefaultStore")
        If Val(Md.DefaultERPStore) = 0 Then Md.DefaultERPStore = "001"

        Md.DefaultSave = dt.Rows(0)("DefaultSave")
        Md.EditPrices = IIf(dt.Rows(0)("EditPrices").ToString() = "1", True, False)

        Dim m As MainWindow = Application.Current.MainWindow
        m.LoadTabs(New MainPage)
        IsLogedIn = True
    End Sub

    Private Sub Login_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()


        Dim dt As New DataTable("ddtt")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        For Each file In IO.Directory.GetFiles(System.Windows.Forms.Application.StartupPath)
            If file.ToLower.EndsWith(".udl") AndAlso Not file.ToLower.EndsWith("connect.udl") Then
                dt.Rows.Add(file.ToLower.Split("\").Last.Replace(".udl", ""), file.ToLower.Split("\").Last.Replace(".udl", ""))
            End If
        Next
        bm.FillCombo(dt, AccYear)
        If dt.Rows.Count = 0 Then
            lblAccYear.Visibility = Windows.Visibility.Hidden
            AccYear.Visibility = Windows.Visibility.Hidden
            LoadEmployees()
        Else
            AccYear.SelectedIndex = AccYear.Items.Count - 1
        End If


        LoadResource()
    End Sub

    Private Sub LoadResource()
        btnLogin.SetResourceReference(Button.ContentProperty, "Login")
        lblUsername.SetResourceReference(Label.ContentProperty, "Username")
        lblPassword.SetResourceReference(Label.ContentProperty, "Password")
        lblAccYear.SetResourceReference(Label.ContentProperty, "AccYear")
    End Sub

    Private Sub LoadEmployees()
        Dim CboName As String = Resources.Item("CboName")
        bm.FillCombo("select Id," & CboName & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", Username)
        Username.Focus()
    End Sub


    Private Sub AccYear_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles AccYear.SelectionChanged
        Try
            Dim m As MainWindow = Application.Current.MainWindow
            Md.UdlName = AccYear.SelectedValue
            If con.State = ConnectionState.Open Then con.Close()
            m.LoadConnection()
            bm = New BasicMethods
            LoadEmployees()
        Catch ex As Exception
        End Try
    End Sub
End Class
