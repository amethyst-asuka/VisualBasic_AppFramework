﻿Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging

Public Module g

    Public Function GraphicsPlots(ByRef size As Size, ByRef margin As Size, bg As String, plot As Action(Of Graphics)) As Bitmap
        If size.IsEmpty Then
            size = New Size(4300, 2000)
        End If
        If margin.IsEmpty Then
            margin = New Size(100, 100)
        End If

        Dim bmp As New Bitmap(size.Width, size.Height)
        Dim bgColor As Color = bg.ToColor(onFailure:=Color.White)

        Using g As Graphics = Graphics.FromImage(bmp)
            Dim rect As New Rectangle(New Point, size)

            g.FillRectangle(New SolidBrush(bgColor), rect)
            g.CompositingQuality = CompositingQuality.HighQuality

            Call plot(g)
        End Using

        Return bmp
    End Function

    '<Extension>
    'Public Sub DrawLegend(Of T)(g As Graphics,
    '                            data As T(),
    '                            getName As Func(Of T, String),
    '                            getColor As Func(Of T, Color),
    '                            top As Single,
    '                            left As Single,
    '                            font As Font)

    '    Dim rl = 200, rh = g.MeasureString("123", font).Height, d = 10

    '    For Each x In data
    '        Call g.FillRectangle(New SolidBrush(getColor(x)), New Rectangle(left - rl - 20, top, rl, rh))
    '        Call g.DrawString(getName(x), font, Brushes.Black, New Point(left, top))

    '        top += rh + d
    '    Next
    'End Sub
End Module
