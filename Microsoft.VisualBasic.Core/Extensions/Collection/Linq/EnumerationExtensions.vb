﻿Imports System.Runtime.CompilerServices

Namespace Linq

    <HideModuleName>
    Public Module EnumerationExtensions

        ''' <summary>
        ''' 将一个<see cref="Array"/>对象转换为一个<see cref="Object"/>对象的枚举序列
        ''' </summary>
        ''' <param name="enums"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 使用这个拓展函数的原因是<see cref="Array"/>对象不能够产生对象的枚举序列用于Linq拓展函数
        ''' </remarks>
        <Extension>
        Public Iterator Function AsObjectEnumerator(enums As Array) As IEnumerable(Of Object)
            For i As Integer = 0 To enums.Length - 1
                Yield enums.GetValue(i)
            Next
        End Function

        ''' <summary>
        ''' 将一个<see cref="Array"/>对象转换为一个<see cref="Object"/>对象的枚举序列
        ''' </summary>
        ''' <param name="enums"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 使用这个拓展函数的原因是<see cref="Array"/>对象不能够产生对象的枚举序列用于Linq拓展函数
        ''' </remarks>
        <Extension>
        Public Iterator Function AsObjectEnumerator(Of T)(enums As Array) As IEnumerable(Of T)
            For i As Integer = 0 To enums.Length - 1
                Yield DirectCast(enums.GetValue(i), T)
            Next
        End Function

        ''' <summary>
        ''' Returns the input typed as <see cref="IEnumerable(Of T)"/>.
        ''' </summary>
        ''' <typeparam name="T">The type of the elements of source.</typeparam>
        ''' <param name="enums">The sequence to type as <see cref="IEnumerable(Of T)"/></param>
        ''' <returns>The input sequence typed as <see cref="IEnumerable(Of T)"/>.</returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function AsEnumerable(Of T)(enums As Enumeration(Of T)) As IEnumerable(Of T)
            If enums Is Nothing Then
                Return {}
            Else
                Return New Enumerator(Of T) With {
                    .Enumeration = enums
                }
            End If
        End Function
    End Module
End Namespace