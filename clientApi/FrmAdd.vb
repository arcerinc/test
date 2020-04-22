Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Net
Imports System.Text
Public Class FrmAdd
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim WC As New Net.WebClient
        Dim Server As New System.Uri("http://localhost/pdoapi/insert.php")

        Dim jobject As New JObject
        'jobject.Add(New JProperty("id", TextBox1.Text))
        jobject.Add(New JProperty("title", TextBox1.Text))
        jobject.Add(New JProperty("body", TextBox2.Text))
        jobject.Add(New JProperty("author", TextBox3.Text))

        Dim Str As String
        Try
            Str = jobject.ToString
        Catch ex As Exception

        End Try

        Me.Cursor = Cursors.WaitCursor

        Try
            Dim data = Encoding.UTF8.GetBytes(Str)
            Dim RV() As Byte = WC.UploadData(Server, "POST", data)
            Me.Cursor = Cursors.Default

            Dim strMSG As String

            strMSG = Encoding.UTF8.GetString(RV)

            If strMSG.Contains("OK") Then
                MessageBox.Show("Data Berhasil DiTambah ! ", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Tag = "OK"
                Me.Close()
            Else
                MessageBox.Show("Data Gagal Diedit ! ", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show(ex.Message, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class