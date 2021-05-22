﻿#Region "Microsoft.VisualBasic::aceef90c7762210d87c1531fa3a5830f, Data\GraphQuery\Query\Parser\AttributeSelector.vb"

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

    ' Class AttributeSelector
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: ParseImpl
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MIME.Html.Document

Public Class AttributeSelector : Inherits Parser

    Sub New(func As String, parameters As String())
        Call MyBase.New(func, parameters)
    End Sub

    Protected Overrides Function ParseImpl(document As InnerPlantText, isArray As Boolean, env As Engine) As InnerPlantText
        If isArray Then
            Return New HtmlElement With {
                .HtmlElements = DirectCast(document, HtmlElement)(parameters(Scan0)).Values _
                    .Select(Function(a)
                                Return New InnerPlantText With {
                                    .InnerText = a
                                }
                            End Function) _
                    .ToArray
            }
        ElseIf document.GetType Is GetType(InnerPlantText) Then
            Return document
        Else
            Return New InnerPlantText With {
                .InnerText = DirectCast(document, HtmlElement)(parameters(Scan0)).Value
            }
        End If
    End Function
End Class

