﻿#ExternalChecksum("..\..\..\MainPage.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","BF8C87D7FDBE69BD7475DEC9DE64781245FFD2557C1014636724D11A9B7DA946")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Forms.Integration
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell
Imports WpfApplication1


'''<summary>
'''MainPage
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class MainPage
    Inherits System.Windows.Controls.Page
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\MainPage.xaml",13)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents GridSampleViewerPage As MainPage
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",18)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleGridBorder As System.Windows.Controls.Border
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",21)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleGrid As System.Windows.Controls.Grid
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",22)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents tab As System.Windows.Controls.TabControl
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",26)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleGridTranslateTransform As System.Windows.Media.TranslateTransform
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",31)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleDisplayBorder As System.Windows.Controls.Border
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",37)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents btnBack As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",68)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleDisplayFrame As System.Windows.Controls.Frame
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\MainPage.xaml",75)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SampleDisplayBorderTranslateTransform As System.Windows.Media.TranslateTransform
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/OMEGA;component/mainpage.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\MainPage.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.GridSampleViewerPage = CType(target,MainPage)
            
            #ExternalSource("..\..\..\MainPage.xaml",12)
            AddHandler Me.GridSampleViewerPage.Loaded, New System.Windows.RoutedEventHandler(AddressOf Me.galleryLoaded)
            
            #End ExternalSource
            
            #ExternalSource("..\..\..\MainPage.xaml",14)
            AddHandler Me.GridSampleViewerPage.SizeChanged, New System.Windows.SizeChangedEventHandler(AddressOf Me.pageSizeChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me.SampleGridBorder = CType(target,System.Windows.Controls.Border)
            Return
        End If
        If (connectionId = 3) Then
            Me.SampleGrid = CType(target,System.Windows.Controls.Grid)
            Return
        End If
        If (connectionId = 4) Then
            Me.tab = CType(target,System.Windows.Controls.TabControl)
            Return
        End If
        If (connectionId = 5) Then
            Me.SampleGridTranslateTransform = CType(target,System.Windows.Media.TranslateTransform)
            Return
        End If
        If (connectionId = 6) Then
            Me.SampleDisplayBorder = CType(target,System.Windows.Controls.Border)
            Return
        End If
        If (connectionId = 7) Then
            Me.btnBack = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 8) Then
            Me.SampleDisplayFrame = CType(target,System.Windows.Controls.Frame)
            
            #ExternalSource("..\..\..\MainPage.xaml",69)
            AddHandler Me.SampleDisplayFrame.ContentRendered, New System.EventHandler(AddressOf Me.sampleDisplayFrameLoaded)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 9) Then
            Me.SampleDisplayBorderTranslateTransform = CType(target,System.Windows.Media.TranslateTransform)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

