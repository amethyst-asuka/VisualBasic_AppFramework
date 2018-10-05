﻿#Region "Microsoft.VisualBasic::05de739cb085fc85f552bf7dde34b415, Microsoft.VisualBasic.Core\ApplicationServices\VBDev\XmlDoc\Extensions\VBSpecific.vb"

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

    '     Module VBSpecific
    ' 
    '         Function: IsMyResource
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace ApplicationServices.Development.XmlDoc.Serialization

    Public Module VBSpecific

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function IsMyResource([declare] As String) As Boolean
            Return InStr([declare], "My.Resources", CompareMethod.Binary) > 0
        End Function
    End Module
End Namespace