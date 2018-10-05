﻿#Region "Microsoft.VisualBasic::c37c3ac26ae4be2ecee532f59140223d, mime\application%rdf+xml\TestProject\Test1.vb"

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

    ' Class RDFD
    ' 
    '     Properties: CDList
    '     Class CD
    ' 
    '         Properties: Artist, Company, Country, IgnoredProperty, Price
    '                     Year
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.DocumentFormat.RDF
Imports Microsoft.VisualBasic.DocumentFormat.RDF.Serialization

<RDFNamespaceImports("cd", "http://www.recshop.fake/cd#")>
Public Class RDFD

    <RDFElement("cd")> Public Property CDList As CD()

    <RDFDescription(About:="http://www.recshop.fake/cd/Empire Burlesque")>
    <RDFType("cd")>
    Public Class CD : Inherits RDFEntity
        <RDFElement("artist")> Public Property Artist As String
        <RDFElement("country")> Public Property Country As String
        <RDFElement("company")> Public Property Company As String
        <RDFElement("price")> Public Property Price As String
        <RDFElement("year")> Public Property Year As String

        <RDFIgnore> Public Property IgnoredProperty As KeyValuePair(Of Integer, String)
    End Class
End Class