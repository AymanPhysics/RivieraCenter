Imports System.Data

Public Class RPT6
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detail As Integer = 0
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If ComboBox1.SelectedIndex < 0 Then ComboBox1.SelectedIndex = 0
        If Detail = 7 OrElse Detail = 8 Then
            If StoreId.Visibility = Windows.Visibility.Visible AndAlso StoreId.Text.Trim = "" Then
                bm.ShowMSG("برجاء تحديد المخزن")
                StoreId.Focus()
                Return
            End If
            If ItemId.Visibility = Windows.Visibility.Visible AndAlso ItemId.Text.Trim = "" Then
                bm.ShowMSG("برجاء تحديد الصنف")
                ItemId.Focus()
                Return
            End If

        End If


        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "ShiftName", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "@CashierId", "@IsClosedOnly", "Header", "@ToId", "@ItemId", "@GroupId", "@TypeId", "@ColorId", "ColorName", "@SizeId", "SizeName", "@SaveId", "ItemName"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, Shift.SelectedValue.ToString, Shift.Text, ComboBox1.SelectedValue.ToString, Val(StoreId.Text), Val(FromInvoice.Text), Val(ToInvoice.Text), 0, Flag, ComboBox1.SelectedValue.ToString, 0, 0, IIf(IsClosedOnly.IsChecked, 1, 0), CType(Parent, Page).Title, Val(ToId.Text), Val(ItemId.Text), 0, 0, 0, "", 0, "", 0, ItemName.Text} ', Val(ColorId.Text), ColorName.Text, Val(SizeId.Text), SizeName.Text

        Select Case Detail
            Case 0
                rpt.Rpt = "Sales2.rpt"
            Case 1
                rpt.Rpt = "Sales.rpt"
            Case 2
                rpt.Rpt = "DeletedSales.rpt"
            Case 3
                rpt.Rpt = "ItemsSales5.rpt"
            Case 4
                rpt.Rpt = "SalesProfit.rpt"
            Case 5
                rpt.Rpt = "SalesPone22.rpt"
            Case 6
                rpt.Rpt = "ItemsSales4.rpt"
            Case 7
                rpt.Rpt = "ItemMotion2.rpt"
            Case 8
                rpt.Rpt = "ItemMotion3.rpt"
            Case 9
                rpt.Rpt = "Sales3.rpt"
            Case 10
                rpt.Rpt = "ItemsSales6.rpt"
            Case 11
                rpt.Rpt = "ItemsSales7.rpt"
            Case 12
                rpt.Rpt = "ItemsSales8.rpt"
            Case 13
                rpt.Rpt = "ItemsSales9.rpt"
            Case 14
                rpt.Rpt = "ItemsSales51.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("Shifts", Shift, "")
        bm.Addcontrol_MouseDoubleClick({ItemId, StoreId, ToId})
        If Flag = 5 OrElse Flag = 6 Then
            Label2.Visibility = Windows.Visibility.Hidden
            ComboBox1.Visibility = Windows.Visibility.Hidden
            IsClosedOnly.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 6 Then
            lblItemId.Visibility = Windows.Visibility.Hidden
            ItemId.Visibility = Windows.Visibility.Hidden
            ItemName.Visibility = Windows.Visibility.Hidden
            Image1.Visibility = Windows.Visibility.Hidden
        End If

        lblShift.Visibility = Windows.Visibility.Hidden
        Shift.Visibility = Windows.Visibility.Hidden

        LoadCbo()

        Select Case Detail
            Case 11
                lblToId.Visibility = Windows.Visibility.Hidden
                ToId.Visibility = Windows.Visibility.Hidden
                ToName.Visibility = Windows.Visibility.Hidden
            Case 12, 13
                lblItemId.Visibility = Windows.Visibility.Hidden
                ItemId.Visibility = Windows.Visibility.Hidden
                ItemName.Visibility = Windows.Visibility.Hidden
                Image1.Visibility = Windows.Visibility.Hidden

                lblToId.Visibility = Windows.Visibility.Hidden
                ToId.Visibility = Windows.Visibility.Hidden
                ToName.Visibility = Windows.Visibility.Hidden
        End Select

        Dim MyNow As DateTime = bm.MyGetDate()
        Shift.SelectedValue = 0
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        StoreId.Text = ""
        StoreId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub


    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub LoadCbo()
        Dim dt As New DataTable("tbl")
        dt.Columns.Add("Id")
        dt.Columns.Add("Name")
        dt.Rows.Add(New String() {0, "الكل"})
        Select Case Flag
            Case 1
                dt.Rows.Add(New String() {1, "أرصدة افتتاحية"})
                dt.Rows.Add(New String() {2, "إضافة"})
                dt.Rows.Add(New String() {3, "تسوية إضافة"})
                dt.Rows.Add(New String() {4, "صرف"})
                dt.Rows.Add(New String() {5, "تسوية صرف"})
                dt.Rows.Add(New String() {6, "هدايا"})
                dt.Rows.Add(New String() {7, "هالك"})
                dt.Rows.Add(New String() {8, "تحويل إلى مخزن"})

                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden
                lblToId.Content = "المخزن المحول إليه"

            Case 2
                dt.Rows.Add(New String() {9, "مشتريات"})
                dt.Rows.Add(New String() {10, "مردودات مشتريات"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المورد"

            Case 7, 10
                dt.Rows.Add(New String() {19, "الاستيراد"})
                dt.Rows.Add(New String() {20, "مردودات الاستيراد"})

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المورد"

            Case 3
                'dt.Rows.Add(New String() {11, "مبيعات الصالة"})
                'dt.Rows.Add(New String() {12, "مردودات مبيعات الصالة"})
                'dt.Rows.Add(New String() {13, "مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {14, "مردودات مبيعات التيك أواى"})
                'dt.Rows.Add(New String() {15, "مبيعات التوصيل"})
                'dt.Rows.Add(New String() {16, "مردودات مبيعات التوصيل"})
                'IsClosedOnly.Visibility = Visibility.Visible
                dt.Rows.Add(New String() {13, "المبيعات"})
                dt.Rows.Add(New String() {14, "مردودات المبيعات"})

                If 1 = 2 Then
                    dt.Rows.Add(New String() {23, "مبيعات نصف الجملة"})
                    dt.Rows.Add(New String() {24, "مردودات مبيعات نصف الجملة"})
                End If

                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"
                
            Case 4
                dt.Rows.Add(New String() {17, "المستهلكات"})
                dt.Rows.Add(New String() {18, "مردودات المستهلكات"})
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "المريض"

            Case 5
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
                lblToId.Content = "العميل"

        End Select


        Select Case Flag
            Case 3, 4, 10
            Case Else
                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden
        End Select

        Dim dv As New DataView
        dv.Table = dt
        ComboBox1.ItemsSource = dv
        ComboBox1.SelectedValuePath = "Id"
        ComboBox1.DisplayMemberPath = "Name"
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub ComboBox1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBox1.SelectionChanged
        If Flag = 1 Then
            If ComboBox1.SelectedValue = 8 Then
                lblToId.Visibility = Visibility.Visible
                ToId.Visibility = Visibility.Visible
                ToName.Visibility = Visibility.Visible
            Else
                lblToId.Visibility = Visibility.Hidden
                ToId.Visibility = Visibility.Hidden
                ToName.Visibility = Visibility.Hidden

                ToId.Clear()
                ToName.Clear()
            End If
            
        End If
    End Sub


    Private Sub ToId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ToId.KeyUp
        Dim Title, tbl As String
        If Flag = 1 Then
            tbl = "Stores"
            Title = "المخازن"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = 2 OrElse Flag = 7 OrElse Flag = 10 Then
            tbl = "Suppliers"
            Title = "الموردين"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = 3 OrElse Flag = 5 Then
            tbl = "Customers"
            Title = "العملاء"
            bm.ShowHelp(Title, ToId, ToName, e, "select cast(Id as varchar(100)) Id,Name from " & tbl)
        ElseIf Flag = 4 Then
            bm.ShowHelpCases(ToId, ToName, e)
        End If
    End Sub

    Private Sub ToId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToId.LostFocus
        Dim tbl As String
        If Flag = 1 Then
            tbl = "Stores"
        ElseIf Flag = 2 OrElse Flag = 7 OrElse Flag = 10 Then
            tbl = "Suppliers"
        ElseIf Flag = 3 OrElse Flag = 5 Then
            tbl = "Customers"
        ElseIf Flag = 4 Then
            bm.LostFocus(ToId, ToName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & ToId.Text.Trim())
            ToId.ToolTip = ""
            ToName.ToolTip = ""
            Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile from Cases where Id=" & ToId.Text.Trim())
            If dt.Rows.Count > 0 Then
                ToId.ToolTip = Resources.Item("Id") & ": " & ToId.Text & vbCrLf & Resources.Item("Name") & ": " & ToName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
                ToName.ToolTip = ToId.ToolTip
            End If
            Return
        Else
            Return
        End If
        bm.LostFocus(ToId, ToName, "select Name from " & tbl & " where Id=" & ToId.Text.Trim())
    End Sub


    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from Items") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
        bm.GetImage("Items", New String() {"Id"}, New String() {ItemId.Text.Trim()}, "Image", Image1)
    End Sub

End Class
