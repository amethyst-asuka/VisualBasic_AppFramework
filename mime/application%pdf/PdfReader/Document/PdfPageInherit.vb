﻿#Region "Microsoft.VisualBasic::5a0c55c1f0b1c2dcc3c5273ba14994c8, mime\application%pdf\PdfReader\Document\PdfPageInherit.vb"

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

    '     Class PdfPageInherit
    ' 
    '         Properties: Inherit
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: InheritableMandatoryRefValue, InheritableMandatoryValue, InheritableOptionalRefValue, InheritableOptionalValue
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic

Namespace PdfReader
    Public MustInherit Class PdfPageInherit
        Inherits PdfDictionary

        Public Sub New(ByVal parent As PdfObject, ByVal dictionary As ParseDictionary)
            MyBase.New(parent, dictionary)
        End Sub

        Public ReadOnly Property Inherit As PdfPageInherit
            Get
                Return TypedParent(Of PdfPageInherit)()
            End Get
        End Property

        Public MustOverride Sub FindLeafPages(ByVal pages As List(Of PdfPage))

        Public Function InheritableOptionalValue(Of T As PdfObject)(ByVal name As String) As T
            ' Try and get the value from this dictionary
            Dim here = OptionalValue(Of T)(name)

            ' If not present then inherit it from the parent
            If here Is Nothing AndAlso Inherit IsNot Nothing Then here = Inherit.InheritableOptionalValue(Of T)(name)
            Return here
        End Function

        Public Function InheritableOptionalRefValue(Of T As PdfObject)(ByVal name As String) As T
            ' Try and get the value from this dictionary
            Dim here = OptionalValueRef(Of T)(name)

            ' If not present then inherit it from the parent
            If here Is Nothing AndAlso Inherit IsNot Nothing Then here = Inherit.InheritableOptionalRefValue(Of T)(name)
            Return here
        End Function

        Public Function InheritableMandatoryValue(Of T As PdfObject)(ByVal name As String) As T
            ' Try and get the value from this dictionary
            Dim here = OptionalValue(Of T)(name)

            ' If not present then inherit it from the parent
            If here Is Nothing AndAlso Inherit IsNot Nothing Then here = Inherit.InheritableMandatoryValue(Of T)(name)

            ' Enforce mandatory existence
            If here Is Nothing Then Throw New ApplicationException($"Page is missing a mandatory inheritable value for '{name}'.")
            Return here
        End Function

        Public Function InheritableMandatoryRefValue(Of T As PdfObject)(ByVal name As String) As T
            ' Try and get the value from this dictionary
            Dim here = OptionalValueRef(Of T)(name)

            ' If not present then inherit it from the parent
            If here Is Nothing AndAlso Inherit IsNot Nothing Then here = Inherit.InheritableMandatoryRefValue(Of T)(name)

            ' Enforce mandatory existence
            If here Is Nothing Then Throw New ApplicationException($"Page is missing a mandatory inheritable value for '{name}'.")
            Return here
        End Function
    End Class
End Namespace
