﻿#Region "Microsoft.VisualBasic::e9e751fb0c36ed5fa7c7e67d5d93ff8b, Microsoft.VisualBasic.Core\Language\Value\Value.vb"

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

    '     Class Value
    ' 
    '         Properties: HasValue, Value
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: Equals, GetUnderlyingType, (+2 Overloads) GetValueOrDefault, IsNothing, ToString
    '         Operators: -, (+3 Overloads) +, <=, <>, =
    '                    >=
    '         Interface IValueOf
    ' 
    '             Properties: Value
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Scripting

Namespace Language

    ''' <summary>
    ''' You can applying this data type into a dictionary object to makes the mathematics calculation more easily.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class Value(Of T) ': Implements ValueType
        Implements IValueOf

        ''' <summary>
        ''' This object have a <see cref="IValueOf.value"/> property for stores its data
        ''' </summary>
        Public Interface IValueOf

            ''' <summary>
            ''' value property for this object stores its data
            ''' </summary>
            ''' <returns></returns>
            Property Value As T
        End Interface

        ''' <summary>
        ''' Gets a value indicating whether the current System.Nullable`1 object has a valid
        ''' value of its underlying type.
        ''' </summary>
        ''' <returns>true if the current System.Nullable`1 object has a value; false if the current
        ''' System.Nullable`1 object has no value.</returns>
        Public ReadOnly Property HasValue As Boolean
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Not Value Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the value of the current System.Nullable`1 object, or the object's
        ''' default value.
        ''' </summary>
        ''' <returns>The value of the System.Nullable`1.Value property if the System.Nullable`1.HasValue
        ''' property is true; otherwise, the default value of the current System.Nullable`1
        ''' object. The type of the default value is the type argument of the current System.Nullable`1
        ''' object, and the value of the default value consists solely of binary zeroes.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetValueOrDefault() As T
            Return GetValueOrDefault(Nothing)
        End Function

        ''' <summary>
        ''' Retrieves the value of the current System.Nullable`1 object, or the specified
        ''' default value.
        ''' </summary>
        ''' <param name="defaultValue">A value to return if the System.Nullable`1.HasValue property is false.</param>
        ''' <returns>The value of the System.Nullable`1.Value property if the System.Nullable`1.HasValue
        ''' property is true; otherwise, the defaultValue parameter.</returns>
        Public Function GetValueOrDefault(defaultValue As T) As T
            If Value Is Nothing Then
                Return defaultValue
            Else
                Return Value
            End If
        End Function

        ''' <summary>
        ''' Indicates whether the current System.Nullable`1 object is equal to a specified
        ''' object.
        ''' </summary>
        ''' <param name="other">An object.</param>
        ''' <returns>true if the other parameter is equal to the current System.Nullable`1 object;
        ''' otherwise, false. This table describes how equality is defined for the compared
        ''' values: Return ValueDescriptiontrueThe System.Nullable`1.HasValue property is
        ''' false, and the other parameter is null. That is, two null values are equal by
        ''' definition.-or-The System.Nullable`1.HasValue property is true, and the value
        ''' returned by the System.Nullable`1.Value property is equal to the other parameter.falseThe
        ''' System.Nullable`1.HasValue property for the current System.Nullable`1 structure
        ''' is true, and the other parameter is null.-or-The System.Nullable`1.HasValue property
        ''' for the current System.Nullable`1 structure is false, and the other parameter
        ''' is not null.-or-The System.Nullable`1.HasValue property for the current System.Nullable`1
        ''' structure is true, and the value returned by the System.Nullable`1.Value property
        ''' is not equal to the other parameter.</returns>
        Public Overrides Function Equals(other As Object) As Boolean
            If other Is Nothing Then
                Return False
            ElseIf Not other.GetType Is GetType(T) Then
                Return False
            Else
                Return Value.Equals(other)
            End If
        End Function

        ''' <summary>
        ''' The object value with a specific type define.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Property Value As T Implements IValueOf.Value

        ''' <summary>
        ''' Creates an reference value object with the specific object value
        ''' </summary>
        ''' <param name="value"></param>
        Sub New(value As T)
            Me.Value = value
        End Sub

        ''' <summary>
        ''' Value is Nothing
        ''' </summary>
        Sub New()
            Call MyBase.New
            Value = Nothing
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetUnderlyingType() As Type
            Return GetType(T)
        End Function

        ''' <summary>
        ''' Is the value is nothing.
        ''' </summary>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function IsNothing() As Boolean
            Return Value Is Nothing
        End Function

        ''' <summary>
        ''' Display <see cref="value"/> ``ToString()`` function value.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return InputHandler.ToString(Value)
        End Function

        Public Overloads Shared Operator +(list As Generic.List(Of Value(Of T)), x As Value(Of T)) As Generic.List(Of Value(Of T))
            Call list.Add(x)
            Return list
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overloads Shared Operator +(x As Value(Of T), list As IEnumerable(Of T)) As List(Of T)
            Return (+x).Join(list)
        End Operator

        Public Overloads Shared Operator -(list As Generic.List(Of Value(Of T)), x As Value(Of T)) As Generic.List(Of Value(Of T))
            Call list.Remove(x)
            Return list
        End Operator

        Public Shared Operator <=(value As Value(Of T), o As T) As T
            value.Value = o
            Return o
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(x As Value(Of T)) As T
            Return x.Value
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(x As T) As Value(Of T)
            Return New Value(Of T)(x)
        End Operator

        ''' <summary>
        ''' Gets the <see cref="Value"/> property value.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator +(x As Value(Of T)) As T
            Return x.Value
        End Operator

        ''' <summary>
        ''' Inline value assignment: ``Dim s As String = Value(Of String) = var``
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="o"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Operator =(value As Value(Of T), o As T) As T
            value.Value = o
            Return o
        End Operator

        Public Shared Operator <>(value As Value(Of T), o As T) As T
            Throw New NotSupportedException
        End Operator

        Public Shared Operator >=(value As Value(Of T), o As T) As T
            Throw New NotSupportedException
        End Operator

        'Public Shared Operator &(o As Value(Of T)) As T
        '    Return o.value
        'End Operator
    End Class
End Namespace