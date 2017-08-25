﻿#Region "Microsoft.VisualBasic::7d3d817888f51b77b2431de29d304d93, ..\sciBASIC#\Data_science\Mathematica\Plot\Plots-statistics\ZScores.vb"

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
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime

''' <summary>
''' Plot of the <see cref="Bootstraping.Z"/>
''' </summary>
Public Module ZScoresPlot

    <Extension>
    Public Function Plot(data As ZScores,
                         Optional size$ = "2700,3000",
                         Optional margin$ = g.DefaultPadding,
                         Optional bg$ = "white",
                         Optional title$ = "Z-scores",
                         Optional titleFontCSS$ = CSSFont.Win7VeryLarge,
                         Optional serialLabelFontCSS$ = CSSFont.Win7LargerNormal,
                         Optional legendLabelFontCSS$ = CSSFont.Win7LittleLarge,
                         Optional tickFontCSS$ = CSSFont.Win7Normal,
                         Optional pointWidth! = 5,
                         Optional axisStrokeCSS$ = Stroke.AxisStroke) As GraphicsData

        Dim ticks#() = data.Range.CreateAxisTicks
        Dim range As DoubleRange = ticks
        Dim maxGroupLabel$ = data.groups.Keys.MaxLengthString
        Dim maxSerialsLabel$ = data.serials.Keys.MaxLengthString
        Dim serialLabelFont As Font = CSSFont.TryParse(serialLabelFontCSS)
        Dim legendLabelFont As Font = CSSFont.TryParse(legendLabelFontCSS)
        Dim titleFont As Font = CSSFont.TryParse(titleFontCSS)
        Dim tickFont As Font = CSSFont.TryParse(tickFontCSS)
        Dim groups = data.groups
        Dim colors = data.colors
        Dim pointSize As New SizeF(pointWidth, pointWidth)
        Dim axisStroke As Pen = Stroke.TryParse(axisStrokeCSS).GDIObject

        Dim plotInternal =
            Sub(ByRef g As IGraphics, rect As GraphicsRegion)

                Dim maxSerialLabelSize As SizeF = g.MeasureString(maxSerialsLabel, serialLabelFont)
                Dim maxLegendLabelSize As SizeF = g.MeasureString(maxGroupLabel, legendLabelFont)

                ' 计算出layout信息
                Dim plotWidth% = rect.PlotRegion.Width - maxSerialLabelSize.Width - maxLegendLabelSize.Width - maxLegendLabelSize.Height - 30
                Dim plotHeight = rect.PlotRegion.Height - titleFont.Height - tickFont.Height - 20
                Dim plotWidthRange As DoubleRange = {0, plotWidth}
                Dim X = Function(Z#)
                            Return rect.Padding.Left + maxSerialLabelSize.Width + 5 + range.ScaleMapping(Z, plotWidthRange)
                        End Function
                Dim dy! = plotHeight / (data.serials.Length - 1)
                Dim yTop! = rect.Padding.Top
                Dim left! = X(0)
                Dim labelSize As SizeF
                Dim labelPosition As PointF
                Dim pt As PointF

                ' 绘制出每一个系列的点和相应的标签字符串
                For Each serial As DataSet In data.serials
                    Dim labelY = yTop + (dy - serialLabelFont.Height) / 2
                    Dim yPoints! = yTop + (dy - pointWidth) / 2

                    labelSize = g.MeasureString(serial.ID, serialLabelFont)
                    labelPosition = New PointF(left - labelSize.Width, labelY)
                    g.DrawString(serial.ID, serialLabelFont, Brushes.Black, labelPosition)

                    For Each group In groups
                        Dim color As New SolidBrush(colors(group.Key))

                        For Each Z As Double In serial.TakeValues(group.Value)
                            pt = New PointF(X(Z), yPoints)
                            g.FillEllipse(color, New RectangleF(pt, pointSize))
                        Next
                    Next
                Next

                ' 绘制出X轴的ticks
                For Each tick As Double In ticks
                    labelSize = g.MeasureString(tick, tickFont)
                    pt = New PointF(X(tick), yTop)
                    labelPosition = New PointF With {
                        .X = pt.X - labelSize.Width / 2,
                        .Y = yTop + 10
                    }

                    g.DrawString(tick, tickFont, Brushes.Black, labelPosition)
                    g.DrawLine(Pens.Black, New PointF(pt.X, yTop), New PointF(pt.X, yTop + 8))
                Next

                ' 分别绘制出X坐标轴和Y坐标轴
                yTop = rect.Padding.Top
                g.DrawLine(axisStroke, New PointF(left, yTop), New PointF(left, yTop + plotHeight))
                g.DrawLine(axisStroke,
                           New PointF(left, yTop + plotHeight),
                           New PointF(left + plotWidth, yTop + plotHeight))

                ' 绘制出标题
                labelSize = g.MeasureString(title, titleFont)
                labelPosition = New PointF With {
                    .X = left + (plotWidth - labelSize.Width) / 2,
                    .Y = yTop + plotHeight + tickFont.Height + 20
                }

                g.DrawString(title, titleFont, Brushes.Black, labelPosition)
            End Sub

        Return g.GraphicsPlots(
            size.SizeParser, margin,
            bg,
            plotInternal)
    End Function
End Module

Public Structure ZScores

    Dim serials As DataSet()
    Dim groups As Dictionary(Of String, String())
    ''' <summary>
    ''' Colors for the <see cref="groups"/>
    ''' </summary>
    Dim colors As Dictionary(Of String, Color)

    Public Function Range() As DoubleRange
        Return serials _
            .Select(Function(d) d.Properties.Values) _
            .IteratesALL _
            .Range
    End Function

    Public Shared Function Load(path$, groups As Dictionary(Of String, String()), colors As Color()) As ZScores
        Dim colorlist As LoopArray(Of Color) = colors
        Dim datalist As DataSet() = DataSet.LoadDataSet(path)
        Dim names As New NamedVectorFactory(datalist.PropertyNames)
        Dim zscores = datalist _
            .Select(Function(serial)
                        Dim z As Vector = names _
                            .AsVector(serial.Properties) _
                            .Z()
                        Return New DataSet With {
                            .ID = serial.ID,
                            .Properties = names.Translate(z)
                        }
                    End Function) _
            .ToArray
        Return New ZScores With {
            .serials = zscores,
            .groups = groups,
            .colors = groups.ToDictionary(Function(x) x.Key,
                                          Function(x) colorlist.Next)
        }
    End Function

    Public Shared Function Load(path$, groups As Dictionary(Of String, String()), Optional colors$ = ColorBrewer.QualitativeSchemes.Paired12) As ZScores
        Return ZScores.Load(path, groups, Designer.GetColors(colors))
    End Function
End Structure