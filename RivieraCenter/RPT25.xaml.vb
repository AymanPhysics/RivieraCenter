Imports System.Data
Imports Microsoft.Office.Interop
Imports System.IO

Public Class RPT25
    Dim bm As New BasicMethods
    Dim dt As New DataTable

    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer
        Select Case Flag
            Case 1
                rpt.Rpt = "EmpOutcome.rpt"
            Case 2
                rpt.Rpt = "NetSales.rpt"
        End Select

        rpt.paraname = New String() {"@StoreId", "@EmpId", "@FromDate", "@ToDate", "Header", "IsDetailed"}
        rpt.paravalue = New String() {Val(StoreId.Text), Val(EmpId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, IIf(IsDetailed.IsChecked, 1, 0)}
        rpt.Show()

    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({EmpId, StoreId})
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        IsDetailed.Visibility = Visibility.Hidden
        IsDetailed2.Visibility = Visibility.Hidden
              

    End Sub
    Private Sub LoadResource()

        lblStoreId.SetResourceReference(Label.ContentProperty, "Branch")
        lblEmpId.SetResourceReference(Label.ContentProperty, "Employee")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub EmpId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles EmpId.KeyUp
        Dim str As String = "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from Employees where 1=1 "
     
        If bm.ShowHelp("Employees", EmpId, EmpName, e, str) Then
            EmpId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub EmpId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles EmpId.LostFocus
        If Val(EmpId.Text.Trim) = 0 Then
            EmpId.Clear()
            EmpName.Clear()
            Return
        End If

        Dim str As String = "select " & Resources.Item("CboName") & " Name from Employees where Id=" & EmpId.Text.Trim()
        
        bm.LostFocus(EmpId, EmpName, str)
    End Sub
     

    Private Sub IsDetailed_Checked(sender As Object, e As RoutedEventArgs) Handles IsDetailed.Checked
        IsDetailed2.IsChecked = False
    End Sub

    Private Sub IsDetailed2_Checked(sender As Object, e As RoutedEventArgs) Handles IsDetailed2.Checked
        IsDetailed.IsChecked = False
    End Sub



    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Dim StoreUnitId As Integer = 0
    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

End Class