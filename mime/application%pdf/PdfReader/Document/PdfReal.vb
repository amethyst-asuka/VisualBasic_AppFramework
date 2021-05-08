﻿#Region "Microsoft.VisualBasic::8b087d84eaff714a628a3ac469716720, mime\application%pdf\PdfReader\Document\PdfReal.vb"

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

    '     Class PdfReal
    ' 
    '         Properties: ParseReal, Value
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: ToString
    ' 
    '         Sub: Visit
    ' 
    ' 
    ' /********************************************************************************/

#End Region


Namespace PdfReader
    Public Class PdfReal
        Inherits PdfObject

        Public Sub New(ByVal parent As PdfObject, ByVal real As ParseReal)
            MyBase.New(parent, real)
        End Sub

        Public Overrides Function ToString() As String
            Return Value.ToString()
        End Function

        Public Overrides Sub Visit(ByVal visitor As IPdfObjectVisitor)
            visitor.Visit(Me)
        End Sub

        Public ReadOnly Property ParseReal As ParseReal
            Get
                Return TryCast(ParseObject, ParseReal)
            End Get
        End Property

        Public ReadOnly Property Value As Single
            Get
                Return ParseReal.Value
            End Get
        End Property

        Public Shared Narrowing Operator CType(f As PdfReal) As Single
            Return f.Value
        End Operator
    End Class
End Namespace
