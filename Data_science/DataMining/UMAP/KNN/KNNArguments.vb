﻿#Region "Microsoft.VisualBasic::e583a1b26100778a3107407af47ee17f, Data_science\DataMining\UMAP\KNN\KNNArguments.vb"

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

    ' Structure KNNArguments
    ' 
    '     Properties: bandwidth, k, localConnectivity, nIter
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Serialization.JSON

Public Structure KNNArguments

    ''' <summary>
    ''' nNeighbors
    ''' </summary>
    ''' <returns></returns>
    Public Property k As Integer
    Public Property localConnectivity As Double
    Public Property nIter As Integer
    Public Property bandwidth As Double

    Sub New(k As Integer, Optional localConnectivity As Double = 1, Optional nIter As Integer = 64, Optional bandwidth As Double = 1)
        Me.k = k
        Me.localConnectivity = localConnectivity
        Me.nIter = nIter
        Me.bandwidth = bandwidth
    End Sub

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function

End Structure

