' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml
Imports System.IO.Ports
Imports System.Threading

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}


    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

        'If Md.MyProject = Client.ClothesRed Then
        '    bm.SetColor(SampleDisplayBorder)
        '    btnBack.Background = System.Windows.Media.Brushes.White
        'End If
    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)

        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)

            Dim theFrame
            If TypeOf theButton.Tag Is Page Then
                theFrame = CType(theButton.Tag, Page)
                theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
            ElseIf TypeOf theButton.Tag Is Window Then
                theFrame = CType(theButton.Tag, MyWindow)
                theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
            End If

            theButton.IsTabStop = False
            CType(args.Source, RadioButton).IsChecked = False

            If TypeOf theButton.Tag Is Window Then
                CType(theFrame, MyWindow).Show()
            ElseIf m.layoutSwitcher.SelectedIndex = 1 Then
                Dim frm As New MyWindow With {.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag), .WindowState = WindowState.Maximized}

                frm.Content = theButton.Tag
                frm.Show()
            Else

                SampleDisplayFrame.Content = theButton.Tag
                SampleDisplayBorder.Visibility = Visibility.Visible
                Try
                    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Catch ex As Exception
                End Try
                sampleDisplayFrameLoaded(theFrame, args)

            End If

        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        If TypeOf sender Is MyWindow Then
            Try
                If Not Resources.Item(CType(sender, MyWindow).Tag) Is Nothing Then
                    CType(sender, MyWindow).Title = Resources.Item(CType(sender, MyWindow).Tag)
                    Md.Currentpage = CType(sender, MyWindow).Title
                End If
            Catch ex As Exception
            End Try
        ElseIf TypeOf sender Is Page Then
            Try
                CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        ElseIf TypeOf CType(sender, Frame).Content Is Page Then
            Try
                If Not Resources.Item(CType(CType(sender, Frame).Content, Page).Tag) Is Nothing Then
                    CType(CType(sender, Frame).Content, Page).Title = Resources.Item(CType(CType(sender, Frame).Content, Page).Tag)
                    Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
                End If
            Catch ex As Exception
            End Try
            Try
                CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        End If

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DesignDt As New DataTable
    Sub LoadLabel(ByVal G As WrapPanel, Ttl As String)
        CurrentMenuitem += 1
        'If Md.MyProject = Client.Clothes Then Return

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim lbl0 As New Label With {.Height = ActualHeight, .Margin = New Windows.Thickness(24, 0, 0, 0)}
        G.Children.Add(lbl0)

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1), .Height = 90}

        If Resources.Item(Ttl) = "" Then
            lbl.Content = Ttl
        Else
            lbl.SetResourceReference(Label.ContentProperty, Ttl)
        End If

        G.Children.Add(lbl)
        lbl.Width = 240
        lbl.Height = 70
        lbl.FontSize = 24
        
        If Ttl = "" Then lbl.Height = 0
    End Sub

    'Function AddHandler LoadRadio(ByVal G As WrapPanel, ByVal frm As UserControl, ByVal Ttl As String) As RadioButton
    Function LoadRadio(ByVal G As WrapPanel, ByVal Ttl As String) As RadioButton
        CurrentMenuitem += 1

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle"), .Width = 180, .Height = 90}
        'r.Tag = New Page With {.Content = frm}
        r.Width = 140
        r.Height = 70
        
        Dim t As New TranslateTextAnimationExample
        t.RealText.Tag = Ttl

        If Resources.Item(Ttl) = "" Then
            t.RealText.Text = Ttl
        Else
            t.RealText.SetResourceReference(TextBlock.TextProperty, Ttl)
        End If

        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.SetResourceReference(RadioButton.ToolTipProperty, Ttl)
        Return r
    End Function


    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
        While Md.DictionaryCurrent.Item(Ttl).Length < 16
            Md.DictionaryCurrent.Item(Ttl) = " " & Md.DictionaryCurrent.Item(Ttl) & " "
        End While
    End Sub


    Public Lvl As Boolean = False
    Dim CurrentTab As Integer = 0
    Dim CurrentMenuitem As Integer = 0
    Public Sub Load()

        DesignDt = bm.ExecuteAdapter("select * from PLevels where id='" & Md.UserName & "'")

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs()
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            Dim dt As DataTable = bm.ExecuteAdapter("select * from nlevels where id=" & Md.LevelId)
            If dt.Rows.Count = 0 Then Return

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dt.Rows(0)(CType(tab.Items(i), TabItem).Name).ToString = "" Then
                    item.Visibility = Windows.Visibility.Collapsed
                Else
                    item.Visibility = IIf(dt.Rows(0)(item.Name), Visibility.Visible, Visibility.Collapsed)
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dt.Rows(0)(t.Name).ToString = "" Then
                            t.Visibility = Windows.Visibility.Collapsed
                        Else
                            t.Visibility = IIf(dt.Rows(0)(t.Name), Visibility.Visible, Visibility.Collapsed)
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Windows.Visibility.Visible
                        ElseIf dt.Rows(0)(t.Name).ToString = "" Then
                            t.Visibility = Windows.Visibility.Collapsed
                        Else
                            t.Visibility = IIf(dt.Rows(0)(t.Name), Visibility.Visible, Visibility.Collapsed)
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Windows.Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Function MakePanel(MyHeader As String, ImagePath As String) As WrapPanel
        CurrentTab += 1
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12"
        t.Style = FindResource("MyTabItem")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel

        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))
        ResizeHeader(G)
        t.SetResourceReference(TabItem.HeaderProperty, t.Header)
        Return G
    End Function

    Private Sub LoadGPCs()
        Dim G As WrapPanel = MakePanel("File", "Omega.jpg")

        AddHandler LoadRadio(G, "PCs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                    Dim frm As New BasicForm With {.TableName = "PCs"}
                                                    frm.txtName.MaxLength = 1000
                                                    m.TabControl1.Items.Clear()
                                                    sender.Tag = New Page With {.Content = frm}
                                                End Sub

    End Sub

    Private Sub LoadGFile()
        Dim s As String ="MainOMEGA.jpg"
        
        Dim G As WrapPanel = MakePanel("File", s)
        Dim frm As UserControl

        AddHandler LoadRadio(G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          sender.Tag = New Page With {.Content = New Employees}
                                                      End Sub

        
        


        AddHandler LoadRadio(G, "Attachment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "AttachmentTypes"}
                                                                 sender.Tag = New Page With {.Content = frm}
                                                             End Sub


    End Sub

    Private Sub LoadGStores()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Stores", s)
        Dim frm As UserControl

        LoadLabel(G, "البيانات الأساسية")

        AddHandler LoadRadio(G, "ItemComponants").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New ItemComponants
                                                               sender.Tag = New Page With {.Content = frm}
                                                           End Sub

        Dim FlagsDt As DataTable = bm.ExecuteAdapter("select * From ItemCollectionMotionFlags")
        For i As Integer = 0 To FlagsDt.Rows.Count - 1
            If FlagsDt.Rows(i)("Id") = 1 Then
                LoadLabel(G, "أوامر التشغيل")
            ElseIf FlagsDt.Rows(i)("Id") = 11 Then
                LoadLabel(G, "أوامر الانتاج")
            End If

            Dim r As RadioButton = LoadRadio(G, FlagsDt.Rows(i)("Name"))
            frm = New ProductionItemCollectionMotion With {.Flag = FlagsDt.Rows(i)("Id")}
            r.Tag = New Page With {.Content = frm}
        Next

        AddHandler LoadRadio(G, "ProductionPlan").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New ProductionPlan
                                                               sender.Tag = New Page With {.Content = frm}
                                                           End Sub


        LoadLabel(G, "الحسابات")

        AddHandler LoadRadio(G, "ProfitRatio").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New ProfitRatio
                                                            sender.Tag = New Page With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(G, "ProductionOrderCreation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New OrderStatus
                                                                        sender.Tag = New Page With {.Content = frm}
                                                                    End Sub

    End Sub

    Private Sub LoadGSales()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Sales", s)
        Dim frm As UserControl

        LoadLabel(G, "البيانات الأساسية")

        AddHandler LoadRadio(G, "Groups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New BasicForm With {.TableName = "Groups"}
                                                       sender.Tag = New Page With {.Content = frm}
                                                   End Sub


        AddHandler LoadRadio(G, "Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New BasicForm2 With {.MainTableName = "Groups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "Types", .MainId = "GroupId", .SubId = "Id", .SubName = "Name"} 
                                                      sender.Tag = New Page With {.Content = frm}
                                                  End Sub

        AddHandler LoadRadio(G, "Itemunits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New BasicForm With {.TableName = "Itemunits"}
                                                          sender.Tag = New Page With {.Content = frm}
                                                      End Sub

        AddHandler LoadRadio(G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New Items
                                                      sender.Tag = New Page With {.Content = frm}
                                                  End Sub

        AddHandler LoadRadio(G, "Branches").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New Stores
                                                         sender.Tag = New Page With {.Content = frm}
                                                     End Sub



        LoadLabel(G, "Sales")

        AddHandler LoadRadio(G, "Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New Sales With {.Flag = Sales.FlagState.المبيعات}
                                                      sender.Tag = New Page With {.Content = frm}
                                                  End Sub

        LoadLabel(G, "OutCome")

        AddHandler LoadRadio(G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New EmpOutcome
                                                        sender.Tag = New Page With {.Content = frm}
                                                    End Sub

    End Sub

    Private Sub LoadGBanks()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Banks", s)
        Dim frm As UserControl

        LoadLabel(G, "البيانات الأساسية")

        AddHandler LoadRadio(G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New BasicForm With {.TableName = "Banks"}
                                                      sender.Tag = New Page With {.Content = frm}
                                                  End Sub

        AddHandler LoadRadio(G, "Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New BankCash2 With {.Flag = 1}
                                                       sender.Tag = New Page With {.Content = frm}
                                                   End Sub

        AddHandler LoadRadio(G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New BankCash2 With {.Flag = 2}
                                                        sender.Tag = New Page With {.Content = frm}
                                                    End Sub

    End Sub

    Private Sub LoadGSecurity()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Options", s)
        Dim frm As UserControl

        AddHandler LoadRadio(G, "Change Password").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New ChangePassword
                                                                sender.Tag = New Page With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(G, "Levels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Levels
                                                       sender.Tag = New Page With {.Content = frm}
                                                   End Sub

        AddHandler LoadRadio(G, "Attachement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Attachments
                                                            sender.Tag = New Page With {.Content = frm}
                                                        End Sub

    End Sub

    Private Sub LoadGStoresReports()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Stores Reports", s)
        Dim frm As UserControl

        Dim FlagsDt As DataTable = bm.ExecuteAdapter("select * From ItemCollectionMotionFlags")
        For i As Integer = 0 To FlagsDt.Rows.Count - 1
            If FlagsDt.Rows(i)("Id") = 1 Then
                LoadLabel(G, "أوامر التشغيل")
            ElseIf FlagsDt.Rows(i)("Id") = 11 Then
                LoadLabel(G, "أوامر الانتاج")
            End If

            Dim r As RadioButton = LoadRadio(G, FlagsDt.Rows(i)("Name"))
            frm = New RPT60 With {.Flag = FlagsDt.Rows(i)("Id")}
            r.Tag = New Page With {.Content = frm}
        Next

        LoadLabel(G, "الحسابات")
        AddHandler LoadRadio(G, "ProfitRatioTeachers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT01 With {.Flag = 1}
                                                                    sender.Tag = New Page With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(G, "ProfitRatioAll").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT01 With {.Flag = 2}
                                                               sender.Tag = New Page With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(G, "Store Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT02 With {.Flag = 1}
                                                                               sender.Tag = New Page With {.Content = frm}
                                                                           End Sub

        AddHandler LoadRadio(G, "StoreDailyReport").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT03 With {.Flag = 1}
                                                                 sender.Tag = New Page With {.Content = frm}
                                                             End Sub

    End Sub

    Private Sub LoadGSalesReports()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Sales Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(G, "Items Printing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT13 With {.Flag = 1}
                                                               sender.Tag = New Page With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(G, "Sales Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT6 With {.Flag = 3, .Detail = 1}
                                                                        sender.Tag = New Page With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(G, "Sales Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT6 With {.Flag = 3, .Detail = 0}
                                                                     sender.Tag = New Page With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(G, "Stores Sales Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                   sender.Tag = New Page With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(G, "Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT6 With {.Flag = 3, .Detail = 3}
                                                            sender.Tag = New Page With {.Content = frm}
                                                        End Sub

        LoadLabel(G, "OutCome")


        AddHandler LoadRadio(G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New RPT25 With {.Flag = 1}
                                                        sender.Tag = New Page With {.Content = frm}
                                                    End Sub


        AddHandler LoadRadio(G, "NetIncome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New RPT25 With {.Flag = 2}
                                                          sender.Tag = New Page With {.Content = frm}
                                                      End Sub

    End Sub

    Private Sub LoadGBanksReports()
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel("Banks Reports", s)
        Dim frm As UserControl
        
        AddHandler LoadRadio(G, "OutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New RPT25 With {.Flag = 1}
                                                        sender.Tag = New Page With {.Content = frm}
                                                    End Sub


        AddHandler LoadRadio(G, "BankAccountMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 2}
                                                                  sender.Tag = New Page With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(G, "BankBalances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New RPT25 With {.Flag = 2}
                                                             sender.Tag = New Page With {.Content = frm}
                                                         End Sub

    End Sub



    Private Sub LoadTabs()

        LoadGFile()

        LoadGStores()

        LoadGSales()

        LoadGBanks()
        
        LoadGSecurity()

        LoadGStoresReports()

        LoadGSalesReports()

        LoadGBanksReports()

        'bm.SetModem()

    End Sub


End Class

