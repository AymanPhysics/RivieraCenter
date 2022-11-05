Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ProductionItemCollectionMotion

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "ItemCollectionMotionMaster"
    Public TableDetailsName2 As String = "ItemCollectionMotionDetailsTo"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Public Flag As Integer

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = False
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        txtFlag.Visibility = Windows.Visibility.Hidden
        LoadResource()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate()

        RdoGrouping_Checked(Nothing, Nothing)
        LoadWFH(WFHTo, G)

        bm.FillCombo("Employees", SystemUser, "", , True)
        bm.FillCombo("Employees", EntryUser, "", , True)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        bm.Fields = New String() {"Flag", MainId, SubId, "DayDate", "Notes", "DocNo", "MotionTypeId", "Temp", "ItemId", "Qty", "EmpId1", "EmpId2", "EmpId3", "EntryUser", "EntryDate", "SystemUser", "UpdateDate", "NewInvoiceNo"}
        bm.control = New Control() {txtFlag, StoreId, InvoiceNo, DayDate, Notes, DocNo, MotionTypeId, Temp, ItemId, Qty, EmpId1, EmpId2, EmpId3, EntryUser, EntryDate, SystemUser, UpdateDate, NewInvoiceNo}
        bm.KeyFields = New String() {"Flag", MainId, SubId}

        bm.Table_Name = TableName


        lblEntryUser.Visibility = Windows.Visibility.Hidden
        EntryUser.Visibility = Windows.Visibility.Hidden
        EntryDate.Visibility = Windows.Visibility.Hidden
        lblSystemUser.Visibility = Windows.Visibility.Hidden
        SystemUser.Visibility = Windows.Visibility.Hidden
        UpdateDate.Visibility = Windows.Visibility.Hidden
        NewInvoiceNo.Visibility = Windows.Visibility.Hidden

        LoadGroups()
        LoadAllItems()

        StoreId.Text = Md.DefaultERPStore
        StoreId_LostFocus(Nothing, Nothing)

        btnNew_Click(Nothing, Nothing)
    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared Qty As String = "Qty"
        Shared TotalQty As String = "TotalQty"
    End Structure

    Private Sub LoadWFH(WFH As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.ItemId, "كود الصنف")
        G.Columns.Add(GC.ItemName, "اسم الصنف")

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.TotalQty, "إجمالى الكمية")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.ItemId).FillWeight = 110
        G.Columns(GC.ItemName).FillWeight = 300

        G.Columns(GC.ItemName).ReadOnly = True
        G.Columns(GC.TotalQty).ReadOnly = True

        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.GotFocus, AddressOf GridGotFocus
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

            TabItems.Header = It & " - " & xx.Content.ToString.Trim

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


    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        Try
            If RdoGrouping.IsChecked Then
                txtID.Visibility = Visibility.Hidden
                txtName.Visibility = Visibility.Hidden
                txtPrice.Visibility = Visibility.Hidden
                HelpGD.Visibility = Visibility.Hidden
                PanelGroups.Visibility = Visibility.Visible
                PanelTypes.Visibility = Visibility.Visible
                PanelTypes2.Visibility = Visibility.Visible
                PanelItems.Visibility = Visibility.Visible
            ElseIf RdoSearch.IsChecked Then
                txtID.Visibility = Visibility.Visible
                txtName.Visibility = Visibility.Visible
                txtPrice.Visibility = Visibility.Visible
                HelpGD.Visibility = Visibility.Visible
                PanelGroups.Visibility = Visibility.Hidden
                PanelTypes.Visibility = Visibility.Hidden
                PanelTypes2.Visibility = Visibility.Hidden
                PanelItems.Visibility = Visibility.Hidden
            End If
        Catch
        End Try
    End Sub


    Sub LoadAllItems()
        Try
            HelpDt = bm.ExecuteAdapter("Select cast(Code as nvarchar(100))Id,DescA Name,SPrice Price From " & Md.ERPDatabase & "Items  where IsActive='Y' " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrice.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtPrice.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
        Catch
        End Try
    End Sub


    Private Sub HelpGD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.PreviewKeyDown, txtName.PreviewKeyDown, txtPrice.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub HelpGD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelpGD.MouseDoubleClick
        Try
            AddItem(HelpGD.Items(HelpGD.SelectedIndex)(0))
        Catch ex As Exception
        End Try
    End Sub



    Function ItemWhere() As String
        Dim st As String = ""
        Return st
    End Function

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag)
    End Sub

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        If IsCurrentMain Then
            ItemId.Text = Id
            ItemId_LostFocus(Nothing, Nothing)
            Return
        End If

        Try
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Rows.Count - 1
                    If Not G.Rows(x).Cells(GC.ItemId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.ItemId).Value.ToString.Trim = Id.ToString.Trim AndAlso Not G.Rows(x).ReadOnly Then
                        i = x
                        Exists = True
                        GoTo Br
                    End If
                Next
                i = G.Rows.Add()
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select cast(Code as nvarchar(100))Id,DescA Name,* From " & Md.ERPDatabase & "Items where /*IsActive='Y' and*/ Code='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.ItemId).Value Is Nothing Or G.Rows(i).Cells(GC.ItemId).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)

                Return
            End If
            G.Rows(i).Cells(GC.ItemId).Value = dr(0)("Id")
            G.Rows(i).Cells(GC.ItemName).Value = dr(0)("Name")


            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Qty).Index, i))
            Try
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
            Catch ex As Exception
            End Try

            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        If lop Then Return
        Try
            If G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString.Trim = "" Then
                ClearRow(i)
                Return
            ElseIf G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString.Trim = "" Then

            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
            G.Rows(i).Cells(GC.TotalQty).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(Qty.Text)


        Catch ex As Exception
            ClearRow(i)
        End Try

    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.ItemId).Value = Nothing
        G.Rows(i).Cells(GC.ItemName).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.TotalQty).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            Dim G As MyGrid = sender
            If G.Columns(e.ColumnIndex).Name = GC.ItemId Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.ItemId).Value, e.RowIndex, 0)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
        'G.Rows(e.RowIndex).Cells(GC.TotalQty).Value = Val(MainQty.Text) * Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value)

    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Code as varchar(100)) Id,DescA Name from " & Md.ERPDatabase & "Stores") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select DescA Name from " & Md.ERPDatabase & "Stores where Code='" & StoreId.Text.Trim() & "'")
        If Not sender Is Nothing Then ClearControls()
    End Sub


    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls()
        ItemId_LostFocus(Nothing, Nothing)
        EmpId1_LostFocus(Nothing, Nothing)
        EmpId2_LostFocus(Nothing, Nothing)
        EmpId3_LostFocus(Nothing, Nothing)

        dt = bm.ExecuteAdapter("select SD.* from " & TableDetailsName2 & " SD where SD.Flag=" & Flag & " and SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text)
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString.Trim
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId").ToString.Trim
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName").ToString.Trim
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString.Trim
            G.Rows(i).Cells(GC.TotalQty).Value = dt.Rows(i)("TotalQty").ToString.Trim
            CalcRow(i)
        Next
        G.RefreshEdit()

        Notes.Focus()
        lop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {"Flag", MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        If Val(ItemId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الصنف")
            ItemId.Focus()
            Return
        End If

        If Val(Qty.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد العدد")
            Qty.Focus()
            Return
        End If

        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where Flag=" & Flag & " and " & MainId & "='" & StoreId.Text & "'")
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Red
            'System.Threading.Thread.Sleep(500)
            'lblLastEntry.Foreground = System.Windows.Media.Brushes.Blue
            State = BasicMethods.SaveState.Insert
        End If


        SystemUser.SelectedValue = Md.UserName
        UpdateDate.Text = bm.ExecuteScalar("Select GETDATE()")

        If EntryUser.SelectedValue = 0 Then EntryUser.SelectedValue = Md.UserName
        If EntryDate.Text.Trim = "" Then EntryDate.Text = bm.ExecuteScalar("Select GETDATE()")


        bm.DefineValues()
        If Not bm.Save(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName2, New String() {"Flag", "StoreId", "InvoiceNo"}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, New String() {"Barcode", "ItemId", "ItemName", "Qty", "TotalQty"}, New String() {GC.Barcode, GC.ItemId, GC.ItemName, GC.Qty, GC.TotalQty}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.ItemId}) Then Return


        If Flag > 10 Then
            bm.ExecuteNonQuery("GenerateERPMotion", {"Flag", "StoreId", "InvoiceNo"}, {Flag, StoreId.Text, InvoiceNo.Text})
        End If

        If sender Is btnPrint Then
            PrintPone(sender, 0)
            'txtID_Leave(Nothing, Nothing)
            'AllowClose = True
            Return
        End If

        If Not DontClear Then btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"Flag", MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        Try

            NewId()
            Dim d As DateTime = Nothing
            Try
                If d.Year = 1 Then d = bm.MyGetDate
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            Dim st As String = StoreId.Text
            If st = "" Then st = "001"

            bm.ClearControls(False)
            txtFlag.Text = Flag
            ItemName.Clear()
            EmpName1.Clear()
            EmpName2.Clear()
            EmpName3.Clear()

            MotionTypeId.SelectedIndex = 1

            DayDate.SelectedDate = bm.MyGetDate()

            SystemUser.SelectedValue = Md.UserName
            EntryUser.SelectedValue = Md.UserName

            StoreId.Text = st
            StoreId_LostFocus(Nothing, Nothing)
            G.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Flag > 10 Then
                bm.ExecuteNonQuery("DeleteERPMotion", {"Flag", "StoreId", "InvoiceNo"}, {Flag, StoreId.Text, InvoiceNo.Text})
            End If

            bm.ExecuteNonQuery("delete from " & TableName & " where Flag=" & Flag & " and " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName2 & " where Flag=" & Flag & " and " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            btnNew_Click(sender, e)
        End If
    End Sub


    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {"Flag", MainId, SubId}, New String() {Flag, StoreId.Text, InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, txtID.KeyDown, EmpId1.KeyDown, EmpId2.KeyDown, EmpId3.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtPrice.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub EmpId1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId1.KeyUp
        bm.ShowHelp("Employees", EmpId1, EmpName1, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId2.KeyUp
        bm.ShowHelp("Employees", EmpId2, EmpName2, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId3.KeyUp
        bm.ShowHelp("Employees", EmpId3, EmpName3, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Stopped=0")
    End Sub

    Private Sub EmpId1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId1.LostFocus
        bm.LostFocus(EmpId1, EmpName1, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId1.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId2.LostFocus
        bm.LostFocus(EmpId2, EmpName2, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId2.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub EmpId3_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId3.LostFocus
        bm.LostFocus(EmpId3, EmpName3, "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId3.Text.Trim() & " and Stopped=0 ")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Function UnitCount(dt As DataTable, i As Integer) As String
        Select Case i
            Case 1
                Return dt.Rows(0)("UnitCount")
            Case 2
                Return dt.Rows(0)("UnitCount2")
            Case Else
                Return 1
        End Select
    End Function

    Private Sub PrintPone(ByVal sender As System.Object, ByVal NewItemsOnly As Integer)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@ItemId", "@TypeId", "Manager"}
        rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, Flag, StoreId.Text, InvoiceNo.Text, InvoiceNo.Text, CType(Parent, Page).Title, ItemId.Text, 0, IIf(Md.Manager, 1, 0)}
        rpt.Rpt = "ItemCollectionMotion.rpt"
        rpt.Show()

    End Sub


    Dim DontClear As Boolean = False


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Dim G As MyGrid = sender
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.ItemId).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.ItemName).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "select cast(Code as nvarchar(100))Id,DescA Name,SPrice 'السعر' from " & Md.ERPDatabase & "Items where IsActive='Y' " & ItemWhere()) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If


            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.ItemId).Value)) Then
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub SetStyle(ByVal x As Button)
        x.Style = Application.Current.FindResource("GlossyCloseButton")
        x.VerticalContentAlignment = Windows.VerticalAlignment.Center
        x.Width = 180
        x.Height = 30
        x.Margin = New Thickness(5, 5, 0, 0)
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


    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Code as nvarchar(100))Id,DescA Name from " & Md.ERPDatabase & "Items") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select DescA Name from " & Md.ERPDatabase & "Items where Code='" & ItemId.Text.Trim() & "'")
    End Sub

    Private Sub btnCalc_Click(sender As Object, e As RoutedEventArgs) Handles btnCalc.Click
        dt = bm.ExecuteAdapter("SELECT ItemId,ItemName,Qty FROM ItemComponants where MainItemId='" & ItemId.Text & "'")
        G.Rows.Clear()
        If dt.Rows.Count = 0 Then Return
        G.Rows.Add(dt.Rows.Count)
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId")
            G.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName")
            G.Rows(i).Cells(GC.Qty).Value = Val(dt.Rows(i)("Qty"))
            CalcRow(i)
        Next
    End Sub

    Dim IsCurrentMain As Boolean = True
    Private Sub ItemId_GotFocus(sender As Object, e As RoutedEventArgs) Handles ItemId.GotFocus
        IsCurrentMain = True
    End Sub

    Private Sub GridGotFocus(sender As Object, e As EventArgs)
        IsCurrentMain = False
    End Sub
End Class
