﻿#Region "Microsoft.VisualBasic::234de4ab1ceb4f346c3b85bc8f68776e, Data\BinaryData\msgpack\ObjectExtensions.vb"

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

    ' Module ObjectExtensions
    ' 
    '     Function: ToMsgPack
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

<HideModuleName>
Public Module ObjectExtensions

    <Extension()>
    Public Function ToMsgPack(o As Object) As Byte()
        If o Is Nothing Then
            Throw New ArgumentException("Can't serialize null references", "o")
        Else
            Return MsgPackSerializer.SerializeObject(o)
        End If
    End Function
End Module

