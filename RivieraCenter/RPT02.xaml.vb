Imports System.Data

Public Class RPT02
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ToDate", "Header", "@StoreId", "@TypeId"}
        rpt.paravalue = New String() {ToDate.SelectedDate, CType(Parent, Page).Title, StoreId.Text, TypeId.Text}
        Select Case Flag
            Case 1
                rpt.Rpt = "ERPStoresItemsBal.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({StoreId})

        LoadGroups()

        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        StoreId.Text = Md.DefaultERPStore
        StoreId_LostFocus(Nothing, Nothing)

    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Code as varchar(100)) Id,DescA Name from " & Md.ERPDatabase & "Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select DescA Name from " & Md.ERPDatabase & "Stores where Code='" & StoreId.Text.Trim() & "'")
    End Sub


    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WTypes2.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
    
            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories where Parent_Code is null")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString.Trim
                x.Tag = dt.Rows(i)("Id").ToString.Trim
                x.Content = dt.Rows(i)("Name").ToString.Trim
                x.ToolTip = dt.Rows(i)("Name").ToString.Trim
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub


    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WTypes2.Children.Clear()

            TypeId.Text = xx.Tag.ToString.Trim
            TypeId_LostFocus(Nothing, Nothing)

            TabTypes.Header = Tp & " - " & xx.Content.ToString.Trim
    
            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories T where T.Parent_Code=(select TT.Code from " & Md.ERPDatabase & "Categories TT where TT.Ser='" & xx.Tag.ToString.Trim() & "')")


            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString.Trim & "_" & dt.Rows(i)("Id").ToString.Trim
                x.Tag = dt.Rows(i)("Id").ToString.Trim
                x.Content = dt.Rows(i)("Name").ToString.Trim
                x.ToolTip = dt.Rows(i)("Name").ToString.Trim
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes2
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadTypes2(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes2.Tag = xx.Tag
            WTypes2.Children.Clear()

            TypeId.Text = xx.Tag.ToString.Trim
            TypeId_LostFocus(Nothing, Nothing)

            TabTypes2.Header = Tp & " - " & xx.Content.ToString.Trim
            
            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories T where T.Parent_Code=(select TT.Code from " & Md.ERPDatabase & "Categories TT where TT.Ser='" & xx.Tag.ToString.Trim() & "')")

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                x.Name = "TabItem_" & xx.Tag.ToString.Trim & "_" & dt.Rows(i)("Id").ToString.Trim
                x.Tag = dt.Rows(i)("Id").ToString.Trim
                x.Content = dt.Rows(i)("Name").ToString.Trim
                x.ToolTip = dt.Rows(i)("Name").ToString.Trim
                WTypes2.Children.Add(x)
                AddHandler x.Click, Sub()
                                        TypeId.Text = x.Tag.ToString.Trim
                                        TypeId_LostFocus(Nothing, Nothing)
                                    End Sub
            Next
        Catch
        End Try
    End Sub

    Sub SetStyle(ByVal x As Button)
        x.Style = Application.Current.FindResource("GlossyCloseButton")
        x.VerticalContentAlignment = Windows.VerticalAlignment.Center
        x.Width = 180
        x.Height = 30
        x.Margin = New Thickness(5, 5, 0, 0)
    End Sub

    Private Sub TypeId_LostFocus(sender As Object, e As RoutedEventArgs) Handles TypeId.LostFocus
        TypeName.Text = bm.ExecuteScalar("select DescA Name from " & Md.ERPDatabase & "Categories where Ser='" & TypeId.Text.Trim() & "'")
    End Sub
End Class
