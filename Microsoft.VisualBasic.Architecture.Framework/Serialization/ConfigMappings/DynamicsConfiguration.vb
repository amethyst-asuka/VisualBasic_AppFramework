﻿#Region "Microsoft.VisualBasic::32a8da7f23d0a7b3ab13321641a04bb7, ..\visualbasic_App\Microsoft.VisualBasic.Architecture.Framework\Serialization\ConfigMappings\DynamicsConfiguration.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xieguigang (xie.guigang@live.com)
'       xie (genetics@smrucc.org)
' 
' Copyright (c) 2016 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.DataFramework
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Serialization

    Public Class DynamicsConfiguration : Inherits Dynamic.DynamicObject

        ''' <summary>
        ''' 加载完数据之后返回其自身
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadDocument(Of T)(path As String) As Object
            Dim TextChunk As String() = IO.File.ReadAllLines(path)

            Return Me
        End Function

        Public Shared Function GetProperties(Of T)(access As PropertyAccess) As Dictionary(Of String, PropertyInfo)
            Return Schema(Of T)(access)
        End Function

        ''' <summary>
        ''' 将目标dump为一个字典
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="onlyPrimitive">
        ''' 假若这个为真的话，则只有初级类型的才会被读取，反之，复杂类型会被序列化为json作为字符串value
        ''' </param>
        ''' <returns></returns>
        Public Shared Function ToDictionary(Of T)(Optional onlyPrimitive As Boolean = True) As Func(Of T, Dictionary(Of String, String))
            Dim ps = Schema(Of T)(PropertyAccess.Readable)

            For Each p As String In ps.Keys.ToArray
                If Not ps(p).GetIndexParameters.IsNullOrEmpty Then
                    Call ps.Remove(p)
                End If
            Next

            Return Function(o)
                       Dim out As New Dictionary(Of String, String)
                       For Each p As PropertyInfo In ps.Values
                           Dim pType As Type = p.PropertyType
                           Dim value As Object = p.GetValue(o, Nothing)

                           If IsPrimitive(pType) Then
                               out(p.Name) = Scripting.ToString(value)
                           Else
                               If Not onlyPrimitive Then
                                   out(p.Name) = GetObjectJson(value, type:=pType)
                               End If
                           End If
                       Next
                       Return out
                   End Function
        End Function

#Region "Dynamics Support"

        Public Overrides Function GetDynamicMemberNames() As IEnumerable(Of String)
            Return MyBase.GetDynamicMemberNames()
        End Function

        Public Overrides Function TryGetMember(binder As Dynamic.GetMemberBinder, ByRef result As Object) As Boolean
            Return MyBase.TryGetMember(binder, result)
        End Function

        Public Overrides Function TrySetMember(binder As Dynamic.SetMemberBinder, value As Object) As Boolean
            Return MyBase.TrySetMember(binder, value)
        End Function

        Public Overrides Function TryConvert(binder As Dynamic.ConvertBinder, ByRef result As Object) As Boolean
            Return MyBase.TryConvert(binder, result)
        End Function
#End Region
    End Class
End Namespace
