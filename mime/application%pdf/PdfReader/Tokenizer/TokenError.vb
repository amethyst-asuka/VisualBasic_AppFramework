﻿#Region "Microsoft.VisualBasic::45f3b9d6e8d97509da72e0d0c81e030a, mime\application%pdf\PdfReader\Tokenizer\TokenError.vb"

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

    '     Class TokenError
    ' 
    '         Properties: Message, Position
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace PdfReader
    Public Class TokenError
        Inherits TokenObject

        Private _Position As Long, _Message As String

        Public Sub New(ByVal position As Long, ByVal message As String)
            Me.Position = position
            Me.Message = message
        End Sub

        Public Property Position As Long
            Get
                Return _Position
            End Get
            Private Set(ByVal value As Long)
                _Position = value
            End Set
        End Property

        Public Property Message As String
            Get
                Return _Message
            End Get
            Private Set(ByVal value As String)
                _Message = value
            End Set
        End Property
    End Class
End Namespace
