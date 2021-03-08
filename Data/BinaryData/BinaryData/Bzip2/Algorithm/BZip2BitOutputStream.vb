﻿' Bzip2 library for .net
' By Jaime Olivares
' Location: http://github.com/jaime-olivares/bzip2
' Ported from the Java implementation by Matthew Francis: https://github.com/MateuszBartosiewicz/bzip2

Imports System.IO
Imports stdNum = System.Math

Namespace Bzip2
    ''' <summary>Implements a bit-wise output stream</summary>
    ''' <remarks>
    ''' Allows the writing of single bit booleans, unary numbers, bit
    ''' strings of arbitrary length(up to 24 bits), and bit aligned 32-bit integers.A single byte at a
    ''' time is written to the wrapped stream when sufficient bits have been accumulated
    ''' </remarks>
    Friend Class BZip2BitOutputStream
#Region "Private fields"
        ' The stream to which bits are written
        Private ReadOnly outputStream As Stream

        ' A buffer of bits waiting to be written to the output stream	 
        Private bitBuffer As UInteger

        ' The number of bits currently buffered in bitBuffer
        Private bitCount As Integer
#End Region

#Region "Public methods"
        ' *
        '  Public constructor
        ' 	     * @param outputStream The OutputStream to wrap
        ' 

        Public Sub New(outputStream As Stream)
            Me.outputStream = outputStream
        End Sub

        ' *
        ' 		 * Writes a single bit to the wrapped output stream
        ' 		 * @param value The bit to write
        ' 		 * @Exception if an error occurs writing to the stream
        ' 

        Public Sub WriteBoolean(value As Boolean)
            bitCount += 1
            bitBuffer = bitBuffer Or If(value, 1UI, 0UI) << 32 - bitCount

            If bitCount = 8 Then
                outputStream.WriteByte(bitBuffer >> 24)
                bitBuffer = 0
                bitCount = 0
            End If
        End Sub

        ' *
        ' 	     * Writes a zero-terminated unary number to the wrapped output stream
        ' 	     * @param value The number to write (must be non-negative)
        ' 	     * @Exception if an error occurs writing to the stream
        ' 

        Public Sub WriteUnary(value As Integer)
            While stdNum.Max(Threading.Interlocked.Decrement(value), value + 1) > 0
                WriteBoolean(True)
            End While

            WriteBoolean(False)
        End Sub

        ' *
        ' 	     * Writes up to 24 bits to the wrapped output stream
        ' 	     * @param count The number of bits to write (maximum 24)
        ' 	     * @param value The bits to write
        ' 	     * @Exception if an error occurs writing to the stream
        ' 

        Public Sub WriteBits(count As Integer, value As UInteger)
            bitBuffer = bitBuffer Or value << 32 - count >> bitCount
            bitCount += count

            While bitCount >= 8
                outputStream.WriteByte(bitBuffer >> 24)
                bitBuffer <<= 8
                bitCount -= 8
            End While
        End Sub

        ' *
        ' 	     * Writes an integer as 32 bits of output
        ' 	     * @param value The integer to write
        ' 	     * @Exception if an error occurs writing to the stream
        ' 

        Public Sub WriteInteger(value As UInteger)
            WriteBits(16, value >> 16 And &HfffF)
            WriteBits(16, value And &HfffF)
        End Sub

        ' *
        ' 	     * Writes any remaining bits to the output stream, zero padding to a whole byte as required
        ' 	     * @Exception if an error occurs writing to the stream
        ' 

        Public Sub Flush()
            If bitCount > 0 Then WriteBits(8 - bitCount, 0)
        End Sub
#End Region
    End Class
End Namespace