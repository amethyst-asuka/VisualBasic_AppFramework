﻿#Region "Microsoft.VisualBasic::bfc45608afda5ea5ebfe48b599f4264e, ..\sciBASIC#\gr\Microsoft.VisualBasic.Imaging\Drawing2D\Shapes\Line.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.Math

Namespace Drawing2D.Vector.Shapes

    Public Class Line : Inherits Shape

        ''' <summary>
        ''' 线段的起点和终点
        ''' </summary>
        Dim pt1 As PointF, pt2 As PointF
        Dim pen As Pen

        Public Overrides ReadOnly Property Size As Size
            Get
                Return New Size(pt2.X - pt1.X, pt2.Y - pt1.Y)
            End Get
        End Property

        ''' <summary>
        ''' 返回线段和X轴的夹角，夹角值为弧度值
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Alpha As Double
            Get
                Dim dx! = pt2.X - pt1.X
                Dim dy! = pt2.Y - pt1.Y
                Dim cos = dx / Math.Sqrt(dx ^ 2 + dy ^ 2)
                Dim a = Arccos(cos)

                If dy < 0 Then
                    ' y 小于零的时候是第三和第4象限的
                    ' cos(170) = cos(190)
                    ' 则假设通过判断这个y坐标值知道点是在第三和第四象限
                    ' 那么 190 = 180 + (180-170)
                    '      350 = 180 + (180-10)
                    a = PI + (PI - a)
                End If

                Return a
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="a">在进行位移的时候，这两个点之间的相对位置不会发生改变</param>
        ''' <param name="b"></param>
        ''' <param name="c"></param>
        ''' <param name="width"></param>
        ''' <remarks></remarks>
        Sub New(a As PointF, b As PointF, c As Color, width%)
            Call MyBase.New(a.ToPoint)

            Me.pt1 = a
            Me.pt2 = b
            Me.pen = New Pen(New SolidBrush(c), width)
        End Sub

        Sub New(a As PointF, b As PointF)
            Call Me.New(a, b, Color.Black, 1)
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{pt1.X}, {pt1.Y}] --> [{pt2.X}, {pt2.Y}] alpha:{Alpha.ToDegrees} degree"
        End Function

        Public Overrides Function Draw(ByRef g As IGraphics, Optional overridesLoci As Point = Nothing) As RectangleF
            Dim rect As RectangleF = MyBase.Draw(g, overridesLoci)
            Dim p1 = Location
            Dim p2 = New Point(Me.Location.X + pt2.X - pt1.X, Me.Location.Y + pt2.Y - pt1.Y)
            Call g.DrawLine(pen, p1, p2)
            Return rect
        End Function

        ''' <summary>
        ''' 对这条线段进行平行位移
        ''' </summary>
        ''' <param name="d#">位移的距离</param>
        ''' <returns></returns>
        Public Function ParallelShift(d#) As Line
            Dim tanA = Tan(Alpha)
            Dim offset As PointF

            If Abs(tanA) <= 0.0000000001 Then
                ' 水平往下平移
                ' x不变，只变换y
                offset = New PointF(0, d)
            Else
                Dim c = d / Tan(Alpha)
                Dim dx = c * Cos(Alpha)
                Dim dy = c * Sin(Alpha)

                offset = New PointF(dx, dy)
            End If

            Dim color As Color = DirectCast(pen.Brush, SolidBrush).Color
            Return New Line(pt1.OffSet2D(offset), pt2.OffSet2D(offset), color, pen.Width)
        End Function
    End Class
End Namespace
