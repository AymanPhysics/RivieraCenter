Imports System.Data

Public Class OrderStatus


    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}

    Dim m As MainWindow = Application.Current.MainWindow
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        FillData(0)
        bm.FillCombo("ItemCollectionMotionFlags", Flag, " Where Id<11 ", , True)
        If bm.TestIsLoaded(Me) Then Return
        Dim t As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New System.TimeSpan(0, 0, 1)}
        AddHandler t.Tick, AddressOf t_Tick

    End Sub

    Private Sub FillData(Flg As Integer)
        dt = bm.ExecuteAdapter("GetPreProductionOrder " & Flg)
        dt.TableName = "tbl"

        dv.Table = dt
        DataGridView1.ItemsSource = dv
        'DataGridView1.IsReadOnly = True
        DataGridView1.Columns(1).Visibility = Windows.Visibility.Collapsed
        DataGridView1.Columns(3).Visibility = Windows.Visibility.Collapsed
        DataGridView1.RowHeaderWidth = 0
        DataGridView1.MinColumnWidth = 100
        'DataGridView1.SelectedIndex = 0 

        For i As Integer = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Columns(i).IsReadOnly = True
        Next
        DataGridView1.Columns(8).IsReadOnly = False
    End Sub


    Private Sub DataGridView1_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles DataGridView1.PreviewMouseDoubleClick

        If Not DataGridView1.CurrentColumn.IsReadOnly Then Return
        If DataGridView1.Items.Count = 0 Then Return

        If bm.ShowDeleteMSG("تحويل إلى أمر انتاج ..؟") Then
            Dim str As String = bm.ExecuteScalar("exec GenerateProductionOrder " & DataGridView1.CurrentItem("Flag") & ",'" & DataGridView1.CurrentItem("StoreId") & "'," & DataGridView1.CurrentItem("المسلسل") & "," & DataGridView1.CurrentItem("الكمية"))
            bm.ShowMSG("تم إنشاء أمر انتاج برقم " & str)
            Flag_SelectionChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub t_Tick(sender As Object, e As EventArgs)
        If Not sender Is Nothing Then CType(sender, System.Windows.Threading.DispatcherTimer).Stop()

        'Dim CurrentActualWidth As Integer = 0
        Try
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
                MyTextBoxes(i) = txt
                'Dim d = DataGridView1.Columns(i).ActualWidth
                'txt.Width = d
                'txt.SetResourceReference(TextBox.WidthProperty, Val(txt.Text))

                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DataGridView1.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)


                txt.Visibility = DataGridView1.Columns(i).Visibility

                'CurrentActualWidth += DataGridView1.Columns(i).ActualWidth
                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
                txt.VerticalAlignment = Windows.VerticalAlignment.Top
                SC.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            DataGridView1.SelectedIndex = 0
        Catch
        End Try
        'DataGridView1.IsReadOnly = True

    End Sub


    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = DataGridView1.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            For i As Integer = 0 To dt.Columns.Count - 1
                dv.RowFilter &= " and [" & dt.Columns(i).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
            Next
        Catch
        End Try
    End Sub

    Private Sub Flag_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Flag.SelectionChanged
        Try
            FillData(Flag.SelectedValue)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "Header"}
        rpt.paravalue = New String() {Val(Flag.SelectedValue), CType(Parent, Page).Title}
        rpt.Rpt = "PreProductionOrder.rpt"
        rpt.Show()
    End Sub
End Class
