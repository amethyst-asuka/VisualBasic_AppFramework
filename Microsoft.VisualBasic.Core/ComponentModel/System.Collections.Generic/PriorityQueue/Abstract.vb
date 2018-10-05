﻿#Region "Microsoft.VisualBasic::604828e680c05d92bd2ec4e2cffbb20e, Microsoft.VisualBasic.Core\ComponentModel\System.Collections.Generic\PriorityQueue\Abstract.vb"

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

    '     Interface IPriorityQueue
    ' 
    '         Function: Peek, Pop, Push
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace ComponentModel.Collection

    Public Interface IPriorityQueue(Of T)
        Inherits ICollection
        Inherits ICloneable
        Inherits IList

        Function Push(O As T) As Integer
        Function Pop() As T
        Function Peek() As T

    End Interface
End Namespace