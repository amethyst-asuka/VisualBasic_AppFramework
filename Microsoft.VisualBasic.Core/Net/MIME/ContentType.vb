﻿#Region "Microsoft.VisualBasic::05811d12baae321f2a46bf4ec36ad30a, Net\MIME\ContentType.vb"

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

    '     Structure ContentType
    ' 
    '         Properties: Details, FileExt, IsEmpty, MIMEType, Name
    ' 
    '         Function: __createObject, ToString
    ' 
    '     Class ContentTypeAttribute
    ' 
    '         Properties: Description, Type
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data.Linq.Mapping
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Text

Namespace Net.Protocols.ContentTypes

    ''' <summary>
    ''' MIME types / Internet Media Types
    ''' </summary>
    Public Structure ContentType : Implements IsEmpty

        ''' <summary>
        ''' Type name or brief info
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
        ''' <summary>
        ''' MIME Type / Internet Media Type
        ''' </summary>
        ''' <returns></returns>
        <Column(Name:="MIME Type / Internet Media Type")> Public Property MIMEType As String

        ''' <summary>
        ''' File Extension
        ''' </summary>
        ''' <returns></returns>
        <Column(Name:="File Extension")>
        Public Property FileExt As String

        ''' <summary>
        ''' More Details
        ''' </summary>
        ''' <returns></returns>
        <Column(Name:="More Details")> Public Property Details As String

        Public ReadOnly Property IsEmpty() As Boolean Implements IsEmpty.IsEmpty
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Name Is Nothing AndAlso
                    MIMEType Is Nothing AndAlso
                    FileExt Is Nothing AndAlso
                    Details Is Nothing
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return $"{MIMEType} (*{FileExt})"
        End Function

        Friend Shared Function __createObject(line As String) As ContentType
            Dim tokens As String() = line.Split(ASCII.TAB)

            If tokens.IsNullOrEmpty OrElse tokens.Length < 3 Then
                Call line.Warning
                Return Nothing
            Else
                Dim mime As New ContentType With {
                    .Name = tokens(Scan0),
                    .MIMEType = tokens(1),
                    .FileExt = tokens(2),
                    .Details = tokens(3)
                }
                Return mime
            End If
        End Function
    End Structure

    ''' <summary>
    ''' MIME types / Internet Media Types attribute data that tagged on the content generator class or module
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Struct, AllowMultiple:=True, Inherited:=True)>
    Public Class ContentTypeAttribute : Inherits Attribute

        ''' <summary>
        ''' Internet media type
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Type As String
        ''' <summary>
        ''' Type of format
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Description As String

        ''' <summary>
        ''' A media type (formerly known as MIME type) is a two-part identifier for file formats and format contents transmitted on the Internet. 
        ''' The Internet Assigned Numbers Authority (IANA) is the official authority for the standardization and publication of these 
        ''' classifications. Media types were originally defined in Request for Comments 2045 in November 1996 as a part of MIME (Multipurpose 
        ''' Internet Mail Extensions) specification, for denoting type of email message content and attachments; hence the name MIME type. 
        ''' Media types are also used by other internet protocols such as HTTP and document file formats such as HTML, for similar purpose.
        ''' </summary>
        ''' <param name="type">
        ''' A media type consists of a type and a subtype, which is further structured into a tree. A media type can optionally define a 
        ''' suffix and parameters:
        ''' 
        ''' ```
        ''' type "/" [tree "."] subtype ["+" suffix] *[";" parameter]
        ''' ```
        ''' 
        ''' The currently registered types are: ``application``, ``audio``, ``example``, ``font``, ``image``, ``message``, ``model``, 
        ''' ``multipart``, ``text`` And ``video``.
        ''' </param>
        ''' <param name="description"></param>
        Sub New(type$, Optional description$ = Nothing)
            Me.Type = type
            Me.Description = description
        End Sub

        Public Overrides Function ToString() As String
            Return Type
        End Function
    End Class

End Namespace
