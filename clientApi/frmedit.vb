Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Net
Imports System.Text

Public Class frmedit

    Private Sub TESTRUN()
        Dim s As HttpWebRequest
        Dim enc As UTF8Encoding
        Dim postdata As String
        Dim postdatabytes As Byte()
        s = HttpWebRequest.Create("http://localhost/pdoapi/update.php")
        enc = New System.Text.UTF8Encoding()
        'postdata = "username=*****&password=*****&message=test+message&orig=test&number=447712345678"
        postdata = "id=2&title=dsadd&body=dsad&author=dadsads"
        postdatabytes = enc.GetBytes(postdata)
        s.Method = "POST"
        s.ContentType = "application/x-www-form-urlencoded"
        s.ContentLength = postdatabytes.Length

        Using stream = s.GetRequestStream()
            stream.Write(postdatabytes, 0, postdatabytes.Length)
        End Using
        Dim result = s.GetResponse()
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(id As String)
        Me.New
        Dim WF As New System.Collections.Specialized.NameValueCollection
        Dim server As New System.Uri("http://localhost/pdoapi/read.php")
        Dim WC As New Net.WebClient

        Me.Cursor = Cursors.WaitCursor

        Try
            TextBox1.Text = id
            WF.Clear()
            WF.Add("id", TextBox1.Text)

            Dim RV() As Byte = WC.UploadValues(server, "POST", WF)
            Dim JsonText As String = System.Text.Encoding.ASCII.GetString(RV)
            Dim JSS As New JavaScriptSerializer

            Dim Tpost = JSS.Deserialize(Of List(Of Posting))(JsonText)
            TextBox2.Text = Tpost(0).title
            TextBox3.Text = Tpost(0).body
            TextBox4.Text = Tpost(0).author

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MessageBox.Show(ex.Message, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Call TESTRUN()
        Call ubah()


    End Sub

    Private Sub ubah()

        Dim WC As New Net.WebClient
        Dim Server As New System.Uri("http://localhost/pdoapi/update.php")

        Dim jobject As New JObject
        jobject.Add(New JProperty("id", TextBox1.Text))
        jobject.Add(New JProperty("title", TextBox2.Text))
        jobject.Add(New JProperty("body", TextBox3.Text))
        jobject.Add(New JProperty("author", TextBox4.Text))

        Dim Str As String

        Str = jobject.ToString


            Me.Cursor = Cursors.WaitCursor

        Try
            Dim data = Encoding.UTF8.GetBytes(Str)
            Dim RV() As Byte = WC.UploadData(Server, "POST", data)
            Me.Cursor = Cursors.Default

            Dim strMSG As String

            strMSG = Encoding.UTF8.GetString(RV)

            If strMSG.Contains("OK") Then
                MessageBox.Show("Data Berhasil Diedit ! ", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
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