Imports System.Web.Script.Serialization
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Net
Imports System.Text
Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim WF As New System.Collections.Specialized.NameValueCollection
        Dim Server As New System.Uri("http://localhost/pdoapi/read.php")
        Dim WC As New Net.WebClient

        Me.Cursor = Cursors.WaitCursor

        Try
            WF.Clear()
            WF.Add("title", TextBox1.Text.Trim())

            Dim RV() As Byte = WC.UploadValues(Server, "POST", WF)

            Dim JsonText As String = System.Text.Encoding.ASCII.GetString(RV)
            Dim JSS As New JavaScriptSerializer
            Dim Tpost = JSS.Deserialize(Of List(Of Posting))(JsonText)

            Dim BS As New BindingSource
            BS.DataSource = Tpost

            DataGridView1.AutoGenerateColumns = True
            DataGridView1.DataSource = BS

            Me.Cursor = Cursors.Default


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show(ex.Message, "informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim row As DataGridViewRow = DataGridView1.CurrentRow

            Dim fedit As New frmedit


            'Dim id = DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value
            ' Dim F As Form = New frmedit(id)
            fedit.TextBox1.Text = row.Cells(0).Value.ToString
            fedit.TextBox2.Text = row.Cells(1).Value.ToString
            fedit.TextBox3.Text = row.Cells(2).Value.ToString
            fedit.TextBox4.Text = row.Cells(3).Value.ToString
            fedit.ShowDialog()
            If fedit.Tag = "OK" Then
                Button1.PerformClick()
            End If


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show("Pilih Data Yang Akan Di Edit....", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub




    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        Dim row As DataGridViewRow = DataGridView1.CurrentRow

        Dim fedit As New frmedit


        'Dim id = DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value
        ' Dim F As Form = New frmedit(id)
        fedit.TextBox1.Text = row.Cells(0).Value.ToString
        fedit.TextBox2.Text = row.Cells(1).Value.ToString
        fedit.TextBox3.Text = row.Cells(2).Value.ToString
        fedit.TextBox4.Text = row.Cells(3).Value.ToString
        fedit.ShowDialog()
        If fedit.Tag = "OK" Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim F As Form = New FrmAdd
        F.ShowDialog()
        If F.Tag = "OK" Then
            Button1.PerformClick()
        End If



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            Me.Cursor = Cursors.Default

            Try
                Dim WC As New Net.WebClient
                Dim Server As New System.Uri("http://localhost/pdoapi/delete.php")

                Dim jobject As New JObject

                Dim id = DataGridView1(0, DataGridView1.CurrentCell.RowIndex).Value

                jobject.Add(New JProperty("id", id))
                Dim Str As String

                Str = jobject.ToString

                Me.Cursor = Cursors.WaitCursor
                Dim data = Encoding.UTF8.GetBytes(Str)
                Dim RV() As Byte = WC.UploadData(Server, "POST", data)
                Me.Cursor = Cursors.Default

                Dim strMSG As String

                strMSG = Encoding.UTF8.GetString(RV)

                If strMSG.Contains("OK") Then
                    MessageBox.Show("Data Berhasil dihapus ! ", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button1.PerformClick()

                Else
                    MessageBox.Show("Data Gagal Diedit ! ", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Me.Cursor = Cursors.Default
            Catch ex As Exception

                Me.Cursor = Cursors.Default
                MessageBox.Show("Pilih Data Yang Akan Di Hapus....", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show(ex.Message, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim WF As New System.Collections.Specialized.NameValueCollection
        Dim Server As New System.Uri("http://localhost/pdoapi/read.php")
        Dim WC As New Net.WebClient

        Me.Cursor = Cursors.WaitCursor

        Try
            WF.Clear()
            WF.Add("title", TextBox1.Text.Trim())

            Dim RV() As Byte = WC.UploadValues(Server, "POST", WF)

            Dim JsonText As String = System.Text.Encoding.ASCII.GetString(RV)
            Dim JSS As New JavaScriptSerializer
            Dim Tpost = JSS.Deserialize(Of List(Of Posting))(JsonText)

            Dim dt As DataTable = New DataTable
            'dt.Columns.Add(New DataColumn("id", GetType(System.String)))
            dt.Columns.Add(New DataColumn("title", GetType(System.String)))
            dt.Columns.Add(New DataColumn("body", GetType(System.String)))
            dt.Columns.Add(New DataColumn("author", GetType(System.String)))

            Dim strkoneksi As String = "server=localhost;uid=root;pwd=;database=lat1"

            Dim con As New MySqlConnection(strkoneksi)

            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            con.Open()

            For Each item In Tpost
                Dim dr As DataRow = dt.NewRow()
                ' dr("id") = item.id
                dr("title") = item.title
                dr("body") = item.body
                dr("author") = item.author

                Dim strcmd = "insert into posts(title,body,author) values('" & dr("title") & "','" & dr("body") & "','" & dr("author") & "')"
                Dim cmd As New MySqlCommand(strcmd, con)
                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()

            Next
            con.Close()


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show(ex.Message, "informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub
End Class
