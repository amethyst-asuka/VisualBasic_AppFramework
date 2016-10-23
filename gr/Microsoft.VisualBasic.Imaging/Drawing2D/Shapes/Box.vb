﻿#Region "Microsoft.VisualBasic::8937c1269465dfe1dbf8aebfced2e87c, ..\visualbasic_App\gr\Microsoft.VisualBasic.Imaging\Drawing2D\Shapes\Box.vb"

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
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Drawing2D.Vector.Shapes

    Public Class Box : Inherits Shape

        Sub New(Location As Point, Size As Size, Color As Color)
            Call MyBase.New(Location)
        End Sub

        Public Overrides ReadOnly Property Size As Size

        Public Shared Sub DrawRectangle(ByRef g As Graphics,
                                        topLeft As Point,
                                        size As Size,
                                        Optional br As Brush = Nothing,
                                        Optional border As Border = Nothing)

            Call g.FillRectangle(If(br Is Nothing, Brushes.Black, br), New Rectangle(topLeft, size))

            If Not border Is Nothing Then
                Call g.DrawRectangle(border.GetPen, New Rectangle(topLeft, size))
            End If
        End Sub
    End Class
End Namespace
