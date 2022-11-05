Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ItemComponants

    Public TableName As String = "ItemComponants"
    Public SubId As String = "MainItemId"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public MyId As String = ""

    WithEvents G As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public Flag As Integer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        btnPrint.Visibility = Windows.Visibility.Hidden

        LoadWFH()
        RdoGrouping_Checked(Nothing, Nothing)

        TabItem1.Header = ""
        LoadVisibility()

        bm.Fields = New String() {SubId}
        bm.control = New Control() {ItemId}
        bm.KeyFields = New String() {SubId}

        bm.Table_Name = TableName
         
        LoadGroups()
        LoadAllItems()

        btnNew_Click(Nothing, Nothing)

        ItemId.Text = MyId
        ItemId_LostFocus(Nothing, Nothing)

    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared Qty As String = "Qty"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

    
        Dim GCUnitId As New Forms.DataGridViewComboBoxColumn
        GCUnitId.HeaderText = "الوحدة"
        GCUnitId.Name = GC.UnitId
        bm.FillCombo("select 0 Id,'' Name", GCUnitId)
        G.Columns.Add(GCUnitId)

        G.Columns.Add(GC.UnitQty, "عدد الفرعى")

        G.Columns.Add(GC.Qty, "الكمية")
    
        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.UnitId).FillWeight = 100
        G.Columns(GC.UnitQty).FillWeight = 100
        G.Columns(GC.Qty).FillWeight = 100
        
        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True

        G.Columns(GC.Barcode).Visible = False
        G.Columns(GC.UnitQty).Visible = False
        
        G.Columns(GC.UnitId).Visible = Md.ShowQtySub


        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
            btnPrint.Visibility = Windows.Visibility.Hidden
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
            HelpDt = bm.ExecuteAdapter("Select cast(Code as nvarchar(100))Id,DescA Name,SPrice Price From " & Md.ERPDatabase & "Items  where IsActive='Y'")
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
        st = "  "
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
            Id = Id.Trim
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            For x As Integer = 0 To G.Rows.Count - 1
                If x <> i AndAlso G.Rows(x).Cells(GC.Id).Value = Id Then
                    i = x
                End If
            Next

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select cast(Code as nvarchar(100))Id,DescA Name,SPrice Price From " & Md.ERPDatabase & "Items  where IsActive='Y' and Code='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id).ToString.Trim
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name).ToString.Trim

            For x As Integer = 0 To G.Rows.Count - 1
                If x <> i AndAlso G.Rows(x).Cells(GC.Id).Value = Id Then
                    bm.ShowMSG("تم تكرار الصنف بالسطر رقم " & (x + 1).ToString.Trim)
                    ClearRow(i)
                    Return
                End If
            Next

            'G.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            LoadItemUint(i)

            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)
            G.CurrentCell = G.Rows(i).Cells(GC.Name)
            G.CurrentCell = G.Rows(i).Cells(GC.Qty)

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
        Try
            G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString.Trim = "" Then
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)


        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
    End Sub


    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
    End Sub



    Sub FillControls()
        If lop Then Return
        lop = True
        bm.FillControls()
        GetItemName()


        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & SubId & "=" & ItemId.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString.Trim
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString.Trim
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("ItemName").ToString.Trim
            LoadItemUint(i)
            G.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString.Trim
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString.Trim
            CalcRow(i)
        Next
        G.RefreshEdit()
        lop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {itemid.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If ItemId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Id).Value) > 0 Then
                Exit For
            End If
            If i = G.Rows.Count - 1 Then Return
        Next


        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {SubId}, New String() {ItemId.Text}, New String() {"Barcode", "ItemId", "ItemName", "UnitId", "UnitQty", "Qty"}, New String() {GC.Barcode, GC.Id, GC.Name, GC.UnitId, GC.UnitQty, GC.Qty}, New VariantType() {VariantType.String, VariantType.String, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.Id}) Then Return

        If sender Is btnPrint Then
            'PrintPone(sender, 0)
        End If

        If Not DontClear Then btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        ItemId.Clear()
        ItemName.Clear()
        ItemId.Focus()
    End Sub

    Sub ClearControls()
        Try
            bm.ClearControls(False)
            GetItemName()
            G.Rows.Clear()
            'G.Focus()
        Catch
        End Try

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then

            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & ItemId.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub


    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {itemid.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemId.LostFocus
        If lv Then
            Return
        End If
        lv = True
        GetItemName()
        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {ItemId.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub
     

    Dim DontClear As Boolean = False
    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            'If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
            '    If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select cast(Id as varchar(100)) Id,Name,SalesPrice 'السعر' from Items where IsStopped=0 " & ItemWhere()) Then
            '        GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
            '        If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
            '            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
            '        ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
            '            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
            '        End If

            '    End If
            'End If


            'If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
            '    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
            '    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
            '        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
            '    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
            '        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
            '    End If
            'End If
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
     

    Private Sub LoadItemUint(i As Integer)
        Dim Id As Integer = G.Rows(i).Cells(GC.Id).Value
        'Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere() & "")

        'bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 1 Id,UnitSub Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' " & ItemWhere() & "", G.Rows(i).Cells(GC.UnitId))

        If G.Rows(i).Cells(GC.UnitId).Value Is Nothing Then G.Rows(i).Cells(GC.UnitId).Value = 0


    End Sub

    Private Sub LoadVisibility()
        btnDelete.Visibility = Windows.Visibility.Visible
        btnFirst.Visibility = Windows.Visibility.Visible
        btnLast.Visibility = Windows.Visibility.Visible
        btnNext.Visibility = Windows.Visibility.Visible
        btnPrevios.Visibility = Windows.Visibility.Visible
        btnPrint.Visibility = Windows.Visibility.Visible
         
    End Sub
     
    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "Select cast(Code as nvarchar(100))Id,DescA Name,SPrice Price From " & Md.ERPDatabase & "Items  where IsActive='Y'") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub GetItemName()
        bm.LostFocus(ItemId, ItemName, "Select DescA Name From " & Md.ERPDatabase & "Items  where IsActive='Y' and Code=" & ItemId.Text.Trim())
    End Sub

    Dim IsCurrentMain As Boolean = True
    Private Sub ItemId_GotFocus(sender As Object, e As RoutedEventArgs) Handles ItemId.GotFocus
        IsCurrentMain = True
    End Sub

    Private Sub GridGotFocus(sender As Object, e As EventArgs)
        IsCurrentMain = False
    End Sub

End Class
