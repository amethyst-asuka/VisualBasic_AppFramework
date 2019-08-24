﻿Imports HuffmanNode = org.nlp.util.HuffmanNode

Namespace org.nlp.vec

    ''' <summary>
    ''' Created by fangy on 13-12-20.
    ''' </summary>
    Public Class HuffmanNeuron
        Implements HuffmanNode

        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
        Protected Friend frequency_Renamed As Integer = 0
        Protected Friend parentNeuron As HuffmanNode
        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
        Protected Friend code_Renamed As Integer = 0
        Protected Friend vector As Double()

        Public Overridable WriteOnly Property code As Integer Implements HuffmanNode.code
            Set(value As Integer)
                code_Renamed = value
            End Set
        End Property

        Public Overridable Property frequency As Integer Implements HuffmanNode.frequency
            Set(value As Integer)
                frequency_Renamed = value
            End Set
            Get
                Return frequency_Renamed
            End Get
        End Property

        Public Overridable Property parent As HuffmanNode Implements HuffmanNode.parent
            Set(value As HuffmanNode)
                parentNeuron = value
            End Set
            Get
                Return parentNeuron
            End Get
        End Property

        Public Overridable Function merge(right As HuffmanNode) As HuffmanNode Implements HuffmanNode.merge
            Dim parent As HuffmanNode = New HuffmanNeuron(frequency_Renamed + right.frequency, vector.Length)
            parentNeuron = parent
            code_Renamed = 0
            right.parent = parent
            right.code = 1
            Return parent
        End Function

        Public Overrides Function compareTo(hn As HuffmanNode) As Integer
            If frequency_Renamed > hn.frequency Then
                Return 1
            Else
                Return -1
            End If
        End Function

        Public Sub New(freq As Integer, vectorSize As Integer)
            frequency_Renamed = freq
            vector = New Double(vectorSize - 1) {}
            parentNeuron = Nothing
        End Sub
    End Class
End Namespace
