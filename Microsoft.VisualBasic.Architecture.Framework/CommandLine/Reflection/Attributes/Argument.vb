﻿Imports System.Reflection
Imports System.Text
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace CommandLine.Reflection

    ''' <summary>
    ''' Use for the detail description for a specific commandline switch.(用于对某一个命令的开关参数的具体描述帮助信息)
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True, Inherited:=True)>
    Public Class Argument : Inherits CLIToken

        ''' <summary>
        ''' The name of this command line parameter switch.(该命令开关的名称)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property Name As String
            Get
                Return MyBase.Name
            End Get
        End Property

        Dim _Description As String

        ''' <summary>
        ''' The description and brief help information about this parameter switch, 
        ''' you can using the \n escape string to gets a VbCrLf value.
        ''' (对这个开关参数的具体的描述以及帮助信息，可以使用\n转义字符进行换行)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description As String
            Get
                Return _Description
            End Get
            Set(value As String)
                Dim Tokens As String() = Strings.Split(value, "\n")
                Dim sBuilder As StringBuilder = New StringBuilder(Tokens.First & vbCrLf)
                For i As Integer = 1 To Tokens.Length - 1
                    Call sBuilder.AppendLine("              " & Tokens(i))
                Next

                _Description = sBuilder.ToString
            End Set
        End Property

        ''' <summary>
        ''' The usage example of this parameter switch.(该开关的值的示例)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Example As String
        ''' <summary>
        ''' The usage syntax information about this parameter switch.(本开关参数的使用语法)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Usage As String

        ''' <summary>
        ''' Is this parameter switch is an optional value.(本开关是否为可选的参数)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property [Optional] As Boolean
        Public ReadOnly Property TokenType As CLITypes
        Public ReadOnly Property Pipeline As PipelineTypes

        ''' <summary>
        ''' Is this parameter is using for the output
        ''' </summary>
        ''' <returns></returns>
        Public Property Out As Boolean = False
        ''' <summary>
        ''' Accept these types as input or output data in this types if <see cref="Out"/> is true.
        ''' </summary>
        ''' <returns></returns>
        Public Property AcceptTypes As Type()

        ''' <summary>
        ''' 对命令行之中的某一个参数进行描述性信息的创建，包括用法和含义
        ''' </summary>
        ''' <param name="Name">The name of this command line parameter switch.(该命令开关的名称)</param>
        ''' <param name="Optional">Is this parameter switch is an optional value.(本开关是否为可选的参数)</param>
        ''' <remarks></remarks>
        Sub New(Name As String, Optional [Optional] As Boolean = False, Optional type As CLITypes = CLITypes.String, Optional pip As PipelineTypes = PipelineTypes.undefined)
            Call MyBase.New(Name)

            Me.[Optional] = [Optional]
            Me.TokenType = type
            Me.Pipeline = pip
        End Sub

        Public Overrides Function ToString() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)
            If [Optional] Then
                sBuilder.AppendLine(String.Format("   [{0}]", Name))
            Else
                sBuilder.AppendLine(Name)
            End If
            sBuilder.AppendLine(String.Format("    Description:  {0}", Description))
            sBuilder.AppendLine(String.Format("    Example:      {0} ""{1}""", Name, Example))

            Return sBuilder.ToString
        End Function
    End Class
End Namespace