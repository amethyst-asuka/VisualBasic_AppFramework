﻿#Region "Microsoft.VisualBasic::3e0065aaf90998c0e658e3bb5045e19a, Data\GraphQuery\TextParser\ParserFunction.vb"

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

    '     Delegate Function
    ' 
    ' 
    '     Class ParserFunction
    ' 
    '         Function: ParseDocument
    ' 
    '     Class InternalInvoke
    ' 
    '         Properties: method, name
    ' 
    '         Function: GetToken
    ' 
    '     Class CustomFunction
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetToken
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MIME.Html.Document

Namespace TextParser

    Public Delegate Function IParserPipeline(document As InnerPlantText) As InnerPlantText

    Public MustInherit Class ParserFunction

        Public MustOverride Function GetToken(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText

        Public Shared Function ParseDocument(document As InnerPlantText,
                                             pip As IParserPipeline,
                                             isArray As Boolean,
                                             <CallerMemberName>
                                             Optional callFunc As String = Nothing) As InnerPlantText
            If Not isArray Then
                Return pip(document)
            End If

            Dim array As New HtmlElement With {.TagName = callFunc}

            If TypeOf document Is HtmlElement Then
                For Each element In DirectCast(document, HtmlElement).HtmlElements
                    array.Add(pip(element))
                Next
            Else
                array.Add(pip(document))
            End If

            Return array
        End Function

    End Class

    Public Class InternalInvoke : Inherits ParserFunction

        Public Property name As String
        Public Property method As MethodInfo

        Public Overrides Function GetToken(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Return method.Invoke(Nothing, {document, parameters, isArray})
        End Function
    End Class

    Public Class CustomFunction : Inherits ParserFunction

        Dim parse As Func(Of InnerPlantText, InnerPlantText)

        Sub New(parse As Func(Of InnerPlantText, InnerPlantText))
            Me.parse = parse
        End Sub

        Public Overrides Function GetToken(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace
