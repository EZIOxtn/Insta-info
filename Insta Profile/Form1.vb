Imports System.Drawing.Drawing2D
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private username As String
    Private alreadyused As String
    Private followers As Integer
    Private strt As Boolean = False
    Private counter As Integer = 20
    Private fortooltip As String
    Private lastuseTime As DateTime
    Private toolTip1 As New ToolTip()
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        username = TextBox1.Text
        Dim client As New HttpClient()
        client.DefaultRequestHeaders.Add("accept", "*/*")
        client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5")
        client.DefaultRequestHeaders.Add("origin", "https://www.tucktools.com")
        client.DefaultRequestHeaders.Add("referer", "https://www.tucktools.com/")
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36")

        Dim url As String = $"https://fanhub.pro/tucktools_user?username={username}"
        Dim r As String
        Try
            Button1.Enabled = False
            Label10.Text = "fetching..."
            Dim response = Await client.GetStringAsync(url)
            r = response
            Dim js = JObject.Parse(response)
            Dim fare9 As Integer
            If strt = True Then
                fare9 = Integer.Parse(js("user_followers")) - followers
                Dim elapsed As TimeSpan = Date.Now - lastuseTime

                If fare9 < 0 Then
                    Label12.ForeColor = Color.Red
                    Label12.Text = fare9.ToString().Replace("-", "↓") + vbNewLine + elapsed.Hours.ToString + " H: " + elapsed.Minutes.ToString + " M: " + elapsed.Seconds.ToString + " s "
                ElseIf fare9 = 0 Then
                    Label12.ForeColor = Color.White
                    Label12.Text = "↑↓ 0" + vbNewLine + elapsed.Hours.ToString + " H: " + elapsed.Minutes.ToString + " M: " + elapsed.Seconds.ToString + " s "
                ElseIf fare9 > 0 Then
                    Label12.ForeColor = Color.Lime
                    Label12.Text = "↑ " + fare9.ToString() + vbNewLine + elapsed.Hours.ToString + " H: " + elapsed.Minutes.ToString + " M: " + elapsed.Seconds.ToString + " s "
                Else
                    Label12.Text = "error"
                End If
            End If
            Dim th As Threading.Thread = New Threading.Thread(Sub()
                                                                  Dim textt As String = "Followers " & js("user_followers").ToString()
                                                                  Label1.Text = ""
                                                                  Dim textu As String = js("username").ToString()
                                                                  For i As Integer = 1 To textt.Length Step +1
                                                                      Label2.Text = textt.Substring(0, i)
                                                                      Threading.Thread.Sleep(50)

                                                                  Next
                                                                  Threading.Thread.Sleep(500)
                                                                  For j As Integer = 1 To textu.Length Step +1
                                                                      Label1.Text = textu.Substring(0, j)
                                                                      Threading.Thread.Sleep(20)
                                                                  Next

                                                              End Sub)
            'Label1.Text = js("username").ToString()
            Label3.Text = "Full Name: " & js("user_fullname").ToString()

            Label4.Text = "Private: " & js("is_private").ToString()
            Label5.Text = "ID: " & js("id").ToString()
            Label6.Text = "Following: " & js("user_following").ToString()
            Label7.Text = "Description: " & vbNewLine & js("user_description").ToString()
            Label8.Text = "Posts: " & js("total_posts").ToString()
            Label9.Text = "Status: " & js("status").ToString()
            fortooltip = js("user_description").ToString()
            th.Start()
            followers = Integer.Parse(js("user_followers"))
            toolTip1.SetToolTip(Label7, fortooltip)
            strt = True
            lastuseTime = DateTime.Now
            Timer1.Start()
            Dim isit As String = js("username").ToString()
            Try
                If isit <> alreadyused Then
                    Dim pictureUrl As String = js("user_profile_pic").ToString()
                    If Not String.IsNullOrEmpty(pictureUrl) Then
                        Label10.Text = "fetching User image..."
                        Dim imageBytes As Byte() = Await client.GetByteArrayAsync(pictureUrl)
                        Using ms As New IO.MemoryStream(imageBytes)
                            Dim g As Image = Image.FromStream(ms)
                            g = New Bitmap(g, New Size(160, 160))
                            PictureBox1.Image = g
                        End Using
                    End If
                End If
            Catch ex As Exception
                Label10.Text = "An error occurred: fetch image error"
            End Try


            Label10.Text = ""
            Button1.Enabled = False

            Label11.Visible = True
            Label11.Visible = True

        Catch ex As Exception
            Label10.Text = "An error occurred: fetch error or use not found "
            Button1.Enabled = True


        End Try


    End Sub

    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint

        Dim radius As Integer = 150
        Dim path As New GraphicsPath()
        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(PictureBox1.Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(PictureBox1.Width - radius, PictureBox1.Height - radius, radius, radius, 0, 90)
        path.AddArc(0, PictureBox1.Height - radius, radius, radius, 90, 90)
        path.CloseFigure()

        PictureBox1.Region = New Region(path)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

        If PictureBox1.Image IsNot Nothing Then
            e.Graphics.DrawImage(PictureBox1.Image, New Rectangle(0, 0, PictureBox1.Width, PictureBox1.Height))
        End If
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load




        toolTip1.AutoPopDelay = 5000
        toolTip1.InitialDelay = 100
        toolTip1.ReshowDelay = 50
        toolTip1.ShowAlways = True




        Label3.Text = "Full Name: " & "........"
        Label4.Text = "Private: " & "........"
        Label5.Text = "ID: " & "........"
        Label6.Text = "Following: " & "........"
        Label7.Text = "Description: " & vbNewLine & "........"
        Label8.Text = "Posts: " & "........"
        Label9.Text = "Status: " & "........"
        ' DoWork("eziox01")

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If counter > 0 Then
            counter -= 1
            Label11.Text = counter.ToString()
        Else
            counter = 21
            Label11.Visible = False
            Button1.Enabled = True
            Timer1.Stop()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub
End Class
