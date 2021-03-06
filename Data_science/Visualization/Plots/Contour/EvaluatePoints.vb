﻿#Region "Microsoft.VisualBasic::ad46399089eb724abff0e910aec668c9, Data_science\Visualization\Plots\Contour\EvaluatePoints.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
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



    ' /********************************************************************************/

    ' Summaries:

    '     Class EvaluatePoints
    ' 
    ' 
    ' 
    '     Class FormulaEvaluate
    ' 
    '         Function: Evaluate
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Contour

    Public MustInherit Class EvaluatePoints

        Public MustOverride Function Evaluate(x As Double, y As Double) As Double

    End Class

    Public Class FormulaEvaluate : Inherits EvaluatePoints

        ''' <summary>
        ''' evaluate of the z from [x, y]
        ''' </summary>
        Public formula As Func(Of Double, Double, Double)

        ''' <summary>
        ''' 得到通过计算返回来的数据
        ''' </summary>
        ''' <param name="x"></param>
        ''' <param name="y"></param>
        ''' <returns></returns>
        Public Overrides Function Evaluate(x As Double, y As Double) As Double
            Return formula(x, y)
        End Function
    End Class
End Namespace
