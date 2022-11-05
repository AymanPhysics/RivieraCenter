Imports System.Data

Public Class RPT60
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If ComboBox1.SelectedIndex < 0 Then ComboBox1.SelectedIndex = 0

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@ItemId", "@TypeId", "Manager"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, ComboBox1.SelectedValue.ToString, Val(StoreId.Text), Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, ItemId.Text, Val(TypeId.Text), IIf(Md.Manager, 1, 0)}
        rpt.Rpt = "ItemCollectionMotion.rpt"

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("ItemCollectionMotionFlags", ComboBox1, "", , True)
        bm.Addcontrol_MouseDoubleClick({ItemId, StoreId})

        If Flag > 0 Then
            ComboBox1.SelectedValue = Flag
            ComboBox1.Visibility = Windows.Visibility.Hidden
            Label2.Visibility = Windows.Visibility.Hidden
        End If

        LoadGroups()

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        StoreId.Text = ""
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



    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Code as nvarchar(100))Id,DescA Name from " & Md.ERPDatabase & "Items") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select DescA Name from " & Md.ERPDatabase & "Items where Code='" & ItemId.Text.Trim() & "'")
    End Sub




    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WTypes2.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories where Parent_Code is null")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString.Trim
                x.Tag = dt.Rows(i)("Code").ToString.Trim
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
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content.ToString.Trim
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories where Parent_Code='" & xx.Tag.ToString.Trim() & "'")

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
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadTypes2(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes2.Tag = xx.Tag
            WTypes2.Children.Clear()
            WItems.Children.Clear()

            TabTypes2.Header = Tp & " - " & xx.Content.ToString.Trim
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("select Ser Id,Code,DescA Name from " & Md.ERPDatabase & "Categories T where T.Parent_Code=(select TT.Code from " & Md.ERPDatabase & "Categories TT where TT.Ser='" & xx.Tag.ToString.Trim() & "')")

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                x.Name = "TabItem_" & xx.Tag.ToString.Trim & "_" & dt.Rows(i)("Id").ToString.Trim
                x.Tag = dt.Rows(i)("Id").ToString.Trim
                x.Content = dt.Rows(i)("Name").ToString.Trim
                x.ToolTip = dt.Rows(i)("Name").ToString.Trim
                WTypes2.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TypeId.Text = xx.Tag.ToString.Trim
            TypeId_LostFocus(Nothing, Nothing)
            TabItems.Header = It & " - " & xx.Content.ToString.Trim

            ItemId.Clear()
            ItemName.Clear()

            Dim dt As DataTable = bm.ExecuteAdapter("Select cast(Code as nvarchar(100))Id,DescA Name,SPrice Price From " & Md.ERPDatabase & "Items where IsActive='Y' and Cat_Ser=" & xx.Tag.ToString.Trim)

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString.Trim
                x.Content = dt.Rows(i)("Name").ToString.Trim
                x.ToolTip = dt.Rows(i)("Name").ToString.Trim
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
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

    Private Sub TabItem(sender As Object, e As RoutedEventArgs)
        ItemId.Text = sender.Tag
        ItemId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TypeId_LostFocus(sender As Object, e As RoutedEventArgs) Handles TypeId.LostFocus
        TypeName.Text = bm.ExecuteScalar("select DescA Name from " & Md.ERPDatabase & "Categories where Ser='" & TypeId.Text.Trim() & "'")
    End Sub
End Class
