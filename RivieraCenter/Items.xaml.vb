Imports System.Data

Public Class Items
    Public TableName As String = "Items"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        lblStoreId.Visibility = Visibility.Hidden
        StoreId.Visibility = Visibility.Hidden
        StoreName.Visibility = Visibility.Hidden
        lblPrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupName.Visibility = Visibility.Hidden

        ItemType.Visibility = Visibility.Hidden
        lblItemType.Visibility = Visibility.Hidden
        btnItemComponants.Visibility = Windows.Visibility.Hidden

        lblUnitSub_Copy.Visibility = Windows.Visibility.Hidden
        lblUnitSub_Copy1.Visibility = Windows.Visibility.Hidden
        ImportPrice.Visibility = Windows.Visibility.Hidden
        ImportPriceSub.Visibility = Windows.Visibility.Hidden
        ImportPriceSub2.Visibility = Windows.Visibility.Hidden
        lblImportPrice.Visibility = Windows.Visibility.Hidden
        lblImportPriceSub.Visibility = Windows.Visibility.Hidden

        bm.FillCombo("ItemUnits", ItemUnitId, "")
        If Md.ShowQtySub Then
            ItemUnitId.Visibility = Windows.Visibility.Hidden
        Else
            Unit.Visibility = Windows.Visibility.Hidden

            lblUnitSub.Visibility = Visibility.Hidden
            lblPurchasePriceSub.Visibility = Visibility.Hidden
            lblSalesPriceSub.Visibility = Visibility.Hidden
            lblUnitCount.Visibility = Visibility.Hidden

            UnitSub.Visibility = Visibility.Hidden
            PurchasePriceSub.Visibility = Visibility.Hidden
            SalesPriceSub.Visibility = Visibility.Hidden
            UnitCount.Visibility = Visibility.Hidden

            UnitSub2.Visibility = Visibility.Hidden
            PurchasePriceSub2.Visibility = Visibility.Hidden
            SalesPriceSub2.Visibility = Visibility.Hidden
            UnitCount2.Visibility = Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, SubName, "EnName", "GroupId", "TypeId", "ItemUnitId", "PrintingGroupId", "StoreId", "PurchasePrice", "PurchasePriceSub", "PurchasePriceSub2", "SalesPrice", "SalesPriceSub", "SalesPriceSub2", "ItemType", "Unit", "UnitSub", "UnitSub2", "UnitCount", "UnitCount2", "Adding", "IsTables", "IsTakeAway", "IsDelivary", "Limit", "Barcode", "IsStopped", "Flag", "ImportPrice", "ImportPriceSub", "ImportPriceSub2", "IsKidneysWash", "CodeOnPackage", "IsService", "CountryId"}
        bm.control = New Control() {txtID, txtName, txtEnName, GroupId, TypeId, ItemUnitId, PrintingGroupId, StoreId, PurchasePrice, PurchasePriceSub, PurchasePriceSub2, SalesPrice, SalesPriceSub, SalesPriceSub2, ItemType, Unit, UnitSub, UnitSub2, UnitCount, UnitCount2, Adding, IsTables, IsTakeAway, IsDelivary, Limit, Barcode, IsStopped, Flag, ImportPrice, ImportPriceSub, ImportPriceSub2, IsKidneysWash, CodeOnPackage, IsService, CountryId}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        IsKidneysWash.Visibility = Windows.Visibility.Hidden
        Flag.Visibility = Windows.Visibility.Hidden
        
        If Md.ShowBarcode Then
            lblBarcode.Visibility = Windows.Visibility.Visible
            Barcode.Visibility = Windows.Visibility.Visible
        Else
            lblBarcode.Visibility = Windows.Visibility.Hidden
            Barcode.Visibility = Windows.Visibility.Hidden
        End If

        Image1.Visibility = Windows.Visibility.Hidden
        btnSetImage.Visibility = Windows.Visibility.Hidden
        btnSetNoImage.Visibility = Windows.Visibility.Hidden

        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls()
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        GroupId_LostFocus(Nothing, Nothing)
        TypeId_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        PrintingGroupId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowPrint = False
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        UnitCount.Text = Val(UnitCount.Text)
        PurchasePrice.Text = Val(PurchasePrice.Text)
        PurchasePriceSub.Text = Val(PurchasePriceSub.Text)
        SalesPrice.Text = Val(SalesPrice.Text)
        SalesPriceSub.Text = Val(SalesPriceSub.Text)

        UnitCount2.Text = Val(UnitCount2.Text)
        PurchasePriceSub2.Text = Val(PurchasePriceSub2.Text)
        SalesPriceSub2.Text = Val(SalesPriceSub2.Text)
        Limit.Text = Val(Limit.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        'bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

        If Not DontClear Then
            If ItemType.SelectedIndex = 3 AndAlso Not bm.IF_Exists("select * from ItemComponants where MainItemId='" & txtID.Text & "'") Then
                bm.ShowMSG("برجاء تحديد مكونات الصنف")
                Return
            End If
        End If

        If Not DontClear Then btnNew_Click(sender, e)
        AllowPrint = True
    End Sub

    Dim AllowPrint As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        ItemType.SelectedIndex = 2

        bm.SetNoImage(Image1)

        GroupName.Clear()
        TypeName.Clear()
        CountryName.Clear()
        PrintingGroupName.Clear()
        StoreName.Clear()

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetItemUsingCount('" & txtID.Text.Trim & "')")) > 0 Then
                bm.ShowMSG("غير مسموح بمسح أصناف عليها حركات")
                Exit Sub
            End If
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            Barcode.Text = bm.ean13(txtID.Text)
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")")
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, GroupId.KeyDown, CountryId.KeyDown, TypeId.KeyDown, PrintingGroupId.KeyDown, StoreId.KeyDown, ItemType.KeyDown, Limit.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            If bm.ShowHelp("Items", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Items") Then txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub PrintingGroupId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PrintingGroupId.KeyUp
        bm.ShowHelp("PrintingGroups", PrintingGroupId, PrintingGroupName, e, "select cast(Id as varchar(100)) Id,Name from PrintingGroups")
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PurchasePrice.KeyDown, PurchasePriceSub.KeyDown, SalesPrice.KeyDown, SalesPriceSub.KeyDown, PurchasePriceSub2.KeyDown, SalesPriceSub2.KeyDown, UnitCount.KeyDown, UnitCount2.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
    End Sub

    Private Sub PrintingGroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles PrintingGroupId.LostFocus
        bm.LostFocus(PrintingGroupId, PrintingGroupName, "select Name from PrintingGroups where Id=" & PrintingGroupId.Text.Trim())
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CountryId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CountryId.KeyUp
        If bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries") Then
            CountryId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
    End Sub

    Private Sub SalesUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                SalesPriceSub.Text = 0
            Else
                SalesPriceSub.Text = Val(SalesPrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            SalesPriceSub.Text = 0
        End Try
    End Sub

    Private Sub SalesUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                SalesPriceSub2.Text = 0
            Else
                SalesPriceSub2.Text = Val(SalesPrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            SalesPriceSub2.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                PurchasePriceSub.Text = 0
            Else
                PurchasePriceSub.Text = Val(PurchasePrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                PurchasePriceSub2.Text = 0
            Else
                PurchasePriceSub2.Text = Val(PurchasePrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub2.Text = 0
        End Try
    End Sub

    Private Sub ItemType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ItemType.SelectionChanged
        Select Case ItemType.SelectedIndex
            Case 0, 1
                IsDelivary.IsChecked = False
                IsTables.IsChecked = False
                IsTakeAway.IsChecked = False
            Case 2, 3
                IsDelivary.IsChecked = True
                IsTables.IsChecked = True
                IsTakeAway.IsChecked = True
        End Select

        If ItemType.SelectedIndex = 3 Then
            btnItemComponants.Visibility = Windows.Visibility.Visible
        Else
            btnItemComponants.Visibility = Windows.Visibility.Hidden
        End If

    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")


    End Sub


    Private Sub btnItemComponants_Click(sender As Object, e As RoutedEventArgs) Handles btnItemComponants.Click

        DontClear = True
        btnSave_Click(sender, Nothing)
        DontClear = False

        If Not AllowPrint Then Return

        Dim frm As New MyWindow With {.Title = CType(sender, Button).Content, .WindowState = WindowState.Maximized}
        Dim c As New ItemComponants With {.MyId = txtID.Text}
        frm.Content = c
        frm.Hide()
        frm.ShowDialog()
    End Sub

    Private Sub ItemUnitId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ItemUnitId.LostFocus
        Unit.Text = ItemUnitId.Text
    End Sub


End Class
