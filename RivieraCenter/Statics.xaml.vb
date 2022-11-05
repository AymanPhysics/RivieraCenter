Imports System.Data
Imports System.IO

Public Class Statics

    Dim bm As New BasicMethods

    Private Sub Statics_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        Dim dt As DataTable = bm.ExecuteAdapter("select PoneComment,PoneHeader from Statics")
        If dt.Rows.Count > 0 Then
            PoneComment.Text = dt.Rows(0)("PoneComment").ToString
            PoneHeader.Text = dt.Rows(0)("PoneHeader").ToString
        End If

        BarcodePrinter.Items.Clear()
        PonePrinter.Items.Clear()
        For i As Integer = 0 To System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1
            BarcodePrinter.Items.Add(System.Drawing.Printing.PrinterSettings.InstalledPrinters(i))
            PonePrinter.Items.Add(System.Drawing.Printing.PrinterSettings.InstalledPrinters(i))
        Next

        BarcodePrinter.Text = Md.BarcodePrinter
        PonePrinter.Text = Md.PonePrinter

    End Sub


    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        bm.ExecuteNonQuery("update Statics set PoneComment='" & PoneComment.Text.Replace("'", "''") & "',PoneHeader='" & PoneHeader.Text.Replace("'", "''") & "'")

        Try
            Md.BarcodePrinter = BarcodePrinter.Text
            Dim st As New StreamWriter("BarcodePrinter.dll")
            st.WriteLine(BarcodePrinter.Text)
            st.Flush()
            st.Close()
        Catch ex As Exception
        End Try

        Try
            Md.PonePrinter = PonePrinter.Text
            Dim st As New StreamWriter("PonePrinter.dll")
            st.WriteLine(PonePrinter.Text)
            st.Flush()
            st.Close()
        Catch ex As Exception
        End Try

        bm.ShowMSG("تم الحفظ بنجاح")
    End Sub
End Class
