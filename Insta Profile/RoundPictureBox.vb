Imports System.IO

Public Class RoundPictureBox
    Public Shared Function GetRoundImageFromMemoryStream(ms As MemoryStream) As Bitmap

        Dim originalImage As Bitmap = New Bitmap(ms)

        Dim roundImage As New Bitmap(originalImage.Width, originalImage.Height)

        Using g As Graphics = Graphics.FromImage(roundImage)

            Dim path As New Drawing2D.GraphicsPath()
            path.AddEllipse(0, 0, originalImage.Width, originalImage.Height)

            g.SetClip(path)

            g.DrawImage(originalImage, 0, 0)
        End Using

        originalImage.Dispose()

        Return roundImage
    End Function
End Class
