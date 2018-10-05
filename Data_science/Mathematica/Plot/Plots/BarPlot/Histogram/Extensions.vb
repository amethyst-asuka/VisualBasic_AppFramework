﻿#Region "Microsoft.VisualBasic::685dbf379544f99c950e9f4cc3b0c0b4, Data_science\Mathematica\Plot\Plots\BarPlot\Histogram\Extensions.vb"

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

    '     Module Extensions
    ' 
    '         Function: NewModel
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.TagData
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend

Namespace BarPlot.Histogram

    Module Extensions

        ''' <summary>
        ''' Syntax helper
        ''' </summary>
        ''' <param name="hist"></param>
        ''' <param name="step!"></param>
        ''' <param name="legend"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function NewModel(hist As Dictionary(Of Double, IntegerTagged(Of Double)), step!, legend As Legend) As HistProfile
            Return New HistProfile(hist, [step]) With {
                .legend = legend
            }
        End Function
    End Module
End Namespace