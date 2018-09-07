﻿#Region "Microsoft.VisualBasic::0b6714930826e413b5320efe5892ef2d, Microsoft.VisualBasic.Core\Extensions\IO\IO.vb"

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

    ' Module IOExtensions
    ' 
    '     Function: FixPath, FlushAllLines, (+3 Overloads) FlushStream, Open, OpenReader
    '               OpenTextWriter, ReadBinary, ReadVector
    ' 
    '     Sub: ClearFileBytes, FlushTo
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Text

''' <summary>
''' The extension API for system file io.(IO函数拓展)
''' </summary>
<Package("IO")>
Public Module IOExtensions

    ReadOnly UTF8 As DefaultValue(Of Encoding) = Encoding.UTF8

    ''' <summary>
    ''' Open text writer interface from a given <see cref="Stream"/> <paramref name="s"/>. 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="encoding">By default is using <see cref="UTF8"/> text encoding.</param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function OpenTextWriter(s As Stream, Optional encoding As Encoding = Nothing) As StreamWriter
        Return New StreamWriter(s, encoding Or UTF8) With {
            .NewLine = ASCII.LF
        }
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="stream">
    ''' 必须要能够支持<see cref="Stream.Length"/>，对于有些网络服务器的HttpResponseStream可能不支持
    ''' <see cref="Stream.Length"/>的话，这个函数将会报错
    ''' </param>
    ''' <param name="path$"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function FlushStream(stream As Stream, path$) As Boolean
        Dim buffer As Byte() = New Byte(stream.Length - 1) {}
        Call stream.Read(buffer, Scan0, stream.Length)
        Return buffer.FlushStream(path)
    End Function

    ''' <summary>
    ''' 将指定的字符串的数据值写入到目标可写的输出流之中
    ''' </summary>
    ''' <param name="data$">所需要写入的字符串数据</param>
    ''' <param name="out">输出流</param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Sub FlushTo(data$, out As StreamWriter)
        Call out.WriteLine(data)
    End Sub

    ''' <summary>
    ''' 为了方便在linux上面使用，这里会处理一下file://这种情况，请注意参数是ByRef引用的
    ''' </summary>
    ''' <param name="path$"></param>
    ''' <returns></returns>
    ''' 
    <Extension> Public Function FixPath(ByRef path$) As String
        If InStr(path, "file://", CompareMethod.Text) = 1 Then
            If App.IsMicrosoftPlatform AndAlso InStr(path, "file:///", CompareMethod.Text) = 1 Then
                path = Mid(path, 9)
            Else
                path = Mid(path, 8)
            End If
        Else
            path = FileIO.FileSystem.GetFileInfo(path).FullName
        End If

        Return path$
    End Function

    ''' <summary>
    ''' Read target text file as a numeric vector, each line in the target text file should be a number, 
    ''' so that if the target text file have n lines, then the returned vector have n elements.
    ''' (这个文本文件之中的每一行都是一个数字，所以假设这个文本文件有n行，那么所返回的向量的长度就是n)
    ''' </summary>
    ''' <param name="path">The file path of the target text file.</param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ReadVector(path As String) As Double()
        Return File.ReadAllLines(path) _
            .Select(Function(x) CDbl(x)) _
            .ToArray
    End Function

    ''' <summary>
    ''' Safe open a local file handle.
    ''' (打开本地文件指针，这是一个安全的函数，会自动创建不存在的文件夹。这个函数默认是写模式的)
    ''' </summary>
    ''' <param name="path">文件的路径</param>
    ''' <param name="mode">File open mode, default is create a new file.(文件指针的打开模式)</param>
    ''' <param name="doClear">
    ''' By default is clear all of the data in source file.
    ''' (写模式下默认将原来的文件数据清空)
    ''' 是否将原来的文件之中的数据清空？默认是，否则将会以追加模式工作
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("Open.File")>
    <Extension>
    Public Function Open(path$, Optional mode As FileMode = FileMode.OpenOrCreate, Optional doClear As Boolean = True) As FileStream
        With path.ParentPath
            If Not .DirectoryExists Then
                Call .MkDIR()
            End If
        End With

        If doClear Then
            ' 在这里调用FlushStream函数的话会导致一个循环引用的问题
            Call ClearFileBytes(path)
        End If

        Return File.Open(path, mode)
    End Function

    ''' <summary>
    ''' 将文件之中的所有数据都清空
    ''' </summary>
    ''' <param name="path"></param>
    Public Sub ClearFileBytes(path As String)
        Call IO.File.WriteAllBytes(path, New Byte() {})
    End Sub

    ''' <summary>
    ''' Open a text file and returns its file handle.
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="encoding">使用系统默认的编码方案</param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <ExportAPI("Open.Reader")>
    <Extension>
    Public Function OpenReader(path$, Optional encoding As Encoding = Nothing) As StreamReader
        Return New StreamReader(IO.File.Open(path, FileMode.OpenOrCreate), encoding Or UTF8)
    End Function

    ''' <summary>
    ''' <see cref="IO.File.ReadAllBytes"/>, if the file is not exists on the filesystem, then a empty array will be return.
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    <Extension>
    Public Function ReadBinary(path As String) As Byte()
        If Not path.FileExists Then
            Return {}
        Else
            Return File.ReadAllBytes(path)
        End If
    End Function

    ''' <summary>
    ''' Write all object into a text file by using its <see cref="Object.ToString"/> method.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="data"></param>
    ''' <param name="saveTo"></param>
    ''' <param name="encoding"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension> Public Function FlushAllLines(Of T)(data As IEnumerable(Of T), saveTo$, Optional encoding As Encodings = Encodings.Default) As Boolean
        Return data.FlushAllLines(saveTo, encoding.CodePage)
    End Function

    ''' <summary>
    ''' Save the binary data into the filesystem.(保存二进制数据包值文件系统)
    ''' </summary>
    ''' <param name="buf">The binary bytes data of the target package's data.(目标二进制数据)</param>
    ''' <param name="path">The saved file path of the target binary data chunk.(目标二进制数据包所要进行保存的文件名路径)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''
    <ExportAPI("FlushStream")>
    <Extension> Public Function FlushStream(buf As IEnumerable(Of Byte), <Parameter("Path.Save")> path$) As Boolean
        Using write As New BinaryWriter(path.Open)
            For Each b As Byte In buf
                Call write.Write(b)
            Next
        End Using

        Return True
    End Function

    <ExportAPI("FlushStream")>
    <Extension> Public Function FlushStream(stream As Net.Protocols.ISerializable, savePath$) As Boolean
        Dim rawStream As Byte() = stream.Serialize
        If rawStream Is Nothing Then
            rawStream = New Byte() {}
        End If
        Return rawStream.FlushStream(savePath)
    End Function
End Module