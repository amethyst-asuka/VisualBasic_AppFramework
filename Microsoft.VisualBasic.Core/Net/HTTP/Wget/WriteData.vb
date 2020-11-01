﻿Imports System.IO
Imports Microsoft.VisualBasic.Parallel

Namespace Net.Http

    Public Class WriteData : Implements IDisposable

        Private disposedValue As Boolean

        ReadOnly fs As Stream
        ReadOnly pipeline As DuplexPipe

        Public ReadOnly Property Length As Long
            Get
                If Not fs Is Nothing Then
                    Return fs.Length
                Else
                    Return pipeline.Length
                End If
            End Get
        End Property

        Sub New(fs As Stream)
            Me.fs = fs
        End Sub

        Sub New(pipe As DuplexPipe)
            Me.pipeline = pipe
        End Sub

        Public Sub Write(bytes As Byte())
            If Not fs Is Nothing Then
                Call fs.Write(bytes, Scan0, bytes.Length)
            Else
                Call pipeline.Write(bytes)
            End If
        End Sub

        Public Sub Flush()
            If Not fs Is Nothing Then
                Call fs.Flush()
            Else
                ' do nothing
            End If
        End Sub

        Public Overrides Function ToString() As String
            If Not fs Is Nothing Then
                Return "<stream>"
            Else
                Return "<pipeline>"
            End If
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                    If Not fs Is Nothing Then
                        Call fs.Flush()
                        Call fs.Close()
                        Call fs.Dispose()
                    Else
                        Call pipeline.Wait()
                        Call pipeline.Close()
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class

End Namespace