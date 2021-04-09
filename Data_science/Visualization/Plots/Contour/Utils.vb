﻿Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Scripting.MathExpression
Imports Microsoft.VisualBasic.Math.Scripting.MathExpression.Impl
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports stdNum = System.Math

Namespace Contour

    Public Module Utils

        ''' <summary>
        ''' Compile the math expression as a lambda expression for producing numeric values.
        ''' </summary>
        ''' <param name="exp$">A string math function expression: ``f(x,y)``</param>
        ''' <returns></returns>
        Public Function Compile(exp As String) As Func(Of Double, Double, Double)
            With New ExpressionEngine()
                !x = 0
                !y = 0

                Dim func As Expression = New ExpressionTokenIcer(exp) _
                    .GetTokens _
                    .ToArray _
                    .DoCall(AddressOf BuildExpression)

                Return Function(x, y)
                           !x = x
                           !y = y

                           Return .DoCall(AddressOf func.Evaluate)
                       End Function
            End With
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="exp$"></param>
        ''' <param name="xrange"></param>
        ''' <param name="yrange"></param>
        ''' <param name="colorMap$"></param>
        ''' <param name="mapLevels%"></param>
        ''' <param name="bg$"></param>
        ''' <param name="size"></param>
        ''' <param name="legendTitle$"></param>
        ''' <param name="legendFont"></param>
        ''' <param name="xsteps!"></param>
        ''' <param name="ysteps!"></param>
        ''' <param name="matrix">
        ''' 请注意：假若想要获取得到原始的矩阵数据，这个列表对象还需要实例化之后再传递进来，否则仍然会返回空集合
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Function Plot(exp$, xrange As DoubleRange, yrange As DoubleRange,
                         Optional colorMap$ = "Spectral:c10",
                         Optional mapLevels% = 25,
                         Optional bg$ = "white",
                         Optional size$ = "3000,2700",
                         Optional padding$ = "padding: 100 400 100 400;",
                         Optional unit% = 5,
                         Optional legendTitle$ = "",
                         Optional legendFont$ = CSSFont.Win7Large,
                         Optional xsteps! = Single.NaN,
                         Optional ysteps! = Single.NaN,
                         Optional ByRef matrix As List(Of DataSet) = Nothing) As GraphicsData

            Dim fun As Func(Of Double, Double, Double) = Compile(exp)

            Try
                Return fun.Plot(
                    xrange, yrange,
                    colorMap,
                    mapLevels,
                    bg, size, padding,
                    unit,
                    legendTitle, legendFont,
                    xsteps, ysteps,
                    matrix:=matrix
                )
            Catch ex As Exception
                Throw New Exception(exp, ex)
            End Try
        End Function

        ''' <summary>
        ''' steps步长值默认值为长度平分到每一个像素点
        ''' </summary>
        ''' <param name="fun"></param>
        ''' <param name="xrange"></param>
        ''' <param name="yrange"></param>
        ''' <param name="colorMap$">
        ''' Default using colorbrewer ``Spectral:c10`` schema.
        ''' </param>
        ''' <param name="size">3000, 2400</param>
        ''' <param name="xsteps!"></param>
        ''' <param name="matrix">
        ''' 请注意：假若想要获取得到原始的矩阵数据，这个列表对象还需要实例化之后再传递进来，否则仍然会返回空集合
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Public Function Plot(fun As Func(Of Double, Double, Double),
                             xrange As DoubleRange,
                             yrange As DoubleRange,
                             Optional colorMap$ = "Spectral:c10",
                             Optional mapLevels% = 25,
                             Optional bg$ = "white",
                             Optional size$ = "3000,2700",
                             Optional padding$ = "padding: 100 400 100 400",
                             Optional unit% = 5,
                             Optional legendTitle$ = "Scatter Heatmap",
                             Optional legendFont$ = CSSFont.Win7Large,
                             Optional xsteps! = Single.NaN,
                             Optional ysteps! = Single.NaN,
                             Optional parallel As Boolean = False,
                             Optional ByRef matrix As List(Of DataSet) = Nothing,
                             Optional minZ# = Double.MinValue,
                             Optional maxZ# = Double.MaxValue,
                             Optional xlabel$ = "X",
                             Optional ylabel$ = "Y",
                             Optional logbase# = -1.0R,
                             Optional scale# = 1.0#,
                             Optional tickFont$ = CSSFont.Win7Normal) As GraphicsData

            Dim theme As New Theme With {.padding = padding, .axisTickCSS = tickFont, .legendLabelCSS = legendFont, .colorSet = colorMap, .background = bg}
            Dim plotInternal As New ContourPlot(theme) With {
                .offset = New Point(-300, 0),
                .xrange = xrange,
                .yrange = yrange,
                .parallel = parallel,
                .xsteps = xsteps,
                .ysteps = ysteps,
                .legendTitle = legendTitle,
                .mapLevels = mapLevels,
                .matrix = New FormulaEvaluate With {.formula = fun},
                .unit = unit,
                .xlabel = xlabel,
                .ylabel = ylabel,
                .logBase = logbase,
                .maxZ = maxZ,
                .minZ = minZ,
                .scale = scale
            }

            Return plotInternal.Plot(size)
        End Function

        ''' <summary>
        ''' 从现有的矩阵数据之中绘制等高线图
        ''' </summary>
        ''' <param name="matrix"></param>
        ''' <param name="colorMap$"></param>
        ''' <param name="mapLevels%"></param>
        ''' <param name="bg$"></param>
        ''' <param name="size$"></param>
        ''' <param name="padding$"></param>
        ''' <param name="legendTitle$"></param>
        ''' <param name="legendFont"></param>
        ''' <param name="xlabel$"></param>
        ''' <param name="ylabel$"></param>
        ''' <param name="minZ#"></param>
        ''' <param name="maxZ#"></param>
        ''' <returns></returns>
        <Extension>
        Public Function Plot(matrix As IEnumerable(Of DataSet),
                         Optional colorMap$ = "Spectral:c10",
                         Optional mapLevels% = 25,
                         Optional bg$ = "white",
                         Optional size$ = "3000,2500",
                         Optional padding$ = "padding: 100 400 100 400;",
                         Optional unit% = 5,
                         Optional legendTitle$ = "Scatter Heatmap",
                         Optional legendFont$ = CSSFont.Win10NormalLarge,
                         Optional tickFont$ = CSSFont.Win7Normal,
                         Optional xlabel$ = "X",
                         Optional ylabel$ = "Y",
                         Optional minZ# = Double.MinValue,
                         Optional maxZ# = Double.MaxValue) As GraphicsData

            Dim margin As Padding = padding
            Dim theme As New Theme With {
                .colorSet = colorMap,
                .background = bg,
                .legendLabelCSS = legendFont,
                .axisTickCSS = tickFont,
                .padding = padding
            }

            Return New ContourPlot(theme) With {
                .offset = New Point(-300, 0),
                .legendTitle = legendTitle,
                .mapLevels = mapLevels,
                .matrix = New MatrixEvaluate(matrix, 1),
                .xlabel = xlabel,
                .ylabel = ylabel,
                .minZ = minZ,
                .maxZ = maxZ,
                .unit = unit
           }.Plot(size)
        End Function

        ''' <summary>
        ''' 返回去的数据是和<paramref name="size"/>每一个像素点相对应的
        ''' </summary>
        ''' <param name="fun"></param>
        ''' <param name="size"></param>
        ''' <param name="xrange"></param>
        ''' <param name="yrange"></param>
        ''' <param name="xsteps!"></param>
        ''' <param name="ysteps!"></param>
        ''' <param name="parallel">
        ''' 对于例如ODEs计算这类比较重度的计算，可以考虑在这里使用并行
        ''' </param>
        ''' <returns></returns>
        <Extension>
        Friend Function __getData(fun As EvaluatePoints,
                               size As Size,
                               xrange As DoubleRange,
                               yrange As DoubleRange,
                               ByRef xsteps!,
                               ByRef ysteps!,
                               parallel As Boolean,
                               ByRef matrix As List(Of DataSet), unit%) As (X#, y#, z#)()


            xsteps = xsteps Or (xrange.Length / size.Width).AsDefault(Function(n) Single.IsNaN(CSng(n)))
            ysteps = ysteps Or (yrange.Length / size.Height).AsDefault(Function(n) Single.IsNaN(CSng(n)))
            xsteps *= unit%
            ysteps *= unit%

            ' x: a -> b
            ' 每一行数据都是y在发生变化
            Dim data As (X#, y#, Z#)()() = DataProvider.Evaluate(
                AddressOf fun.Evaluate, xrange, yrange,
                xsteps, ysteps,
                parallel, matrix).ToArray

            If data.Length > size.Width + 10 Then
                Dim stepDelta = data.Length / size.Width
                Dim splt = data.Split(stepDelta)

            Else ' 数据不足


            End If

            Return data.ToVector
        End Function
    End Module
End Namespace