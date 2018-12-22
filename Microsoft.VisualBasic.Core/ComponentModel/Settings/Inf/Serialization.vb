﻿#Region "Microsoft.VisualBasic::deb0fe73b458011a0eee5f2b51528cd5, Microsoft.VisualBasic.Core\ComponentModel\Settings\Inf\Serialization.vb"

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

    '     Class GenericINIProfile
    ' 
    '         Properties: Sections
    ' 
    '         Function: __getDefaultPath, Load, Save
    ' 
    '     Class ClassName
    ' 
    '         Properties: Name
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    '     Class IniMapIO
    ' 
    '         Properties: Path
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Language.UnixBash.FileSystem
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ComponentModel.Settings.Inf

    ''' <summary>
    ''' 通用的配置文件模型
    ''' </summary>
    Public Class GenericINIProfile : Inherits ITextFile

        <XmlElement> Public Property Sections As Section()

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Load(path As String) As GenericINIProfile
            Return New GenericINIProfile With {
                .FilePath = path,
                .Sections = INIProfile _
                    .PopulateSections(path) _
                    .ToArray
            }
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            Return Sections _
                .Select(Function(sec) sec.CreateDocFragment) _
                .SaveTo(getPath(FilePath), Encoding)
        End Function

        Protected Overrides Function __getDefaultPath() As String
            Return FilePath
        End Function
    End Class

    ''' <summary>
    ''' 定义在Ini配置文件之中的Section的名称
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class ClassName : Inherits Attribute

        Public ReadOnly Property Name As String

        ''' <summary>
        ''' Defines the section name in the ini profile data.(定义在Ini配置文件之中的Section的名称)
        ''' </summary>
        ''' <param name="name"></param>
        Sub New(name As String)
            Me.Name = name
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    ''' <summary>
    ''' The path parameter can be shortcut by method <see cref="PathMapper.GetMapPath"/>.
    ''' additional, using ``@fileName`` for using <see cref="App.GetFile(String)"/> API.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class IniMapIO : Inherits Attribute

        Public ReadOnly Property Path As String

        ''' <summary>
        ''' The path parameter can be shortcut by method <see cref="PathMapper.GetMapPath"/>
        ''' </summary>
        ''' <param name="path"></param>
        Sub New(path As String)
            If path.First = "@"c Then
                Me.Path = App.GetFile(Mid(path, 2))
            Else
                Me.Path = PathMapper.GetMapPath(path)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

End Namespace
