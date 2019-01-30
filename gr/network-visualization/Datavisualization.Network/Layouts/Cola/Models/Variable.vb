﻿Namespace Layouts.Cola

    Public Class Variable
        Public offset As Double = 0
        Public block As Block
        Public cIn As Constraint()
        Public cOut As Constraint()
        Public weight As Double = 1
        Public scale As Double = 1
        Public desiredPosition As Double

        Public ReadOnly Property dfdv As Double
            Get
                Return 2 * weight * (position - desiredPosition)
            End Get
        End Property

        Public ReadOnly Property position As Double
            Get
                Return (block.ps.scale * block.posn + offset) / scale
            End Get
        End Property

        Sub New(desiredPosition As Double, Optional weight As Double = 1, Optional scale As Double = 1)
            Me.desiredPosition = desiredPosition
            Me.weight = weight
            Me.scale = scale
        End Sub

        Public Sub visitNeighbours(prev As Variable, f As Action(Of Constraint, Variable))
            Dim ff = Sub(c As Constraint, [next] As Variable)
                         If c.active AndAlso Not prev Is [next] Then
                             Call f(c, [next])
                         End If
                     End Sub

            cOut.ForEach(Sub(c, i) ff(c, c.right))
            cIn.ForEach(Sub(c, i) ff(c, c.left))
        End Sub

        Public Shared Operator IsTrue(v As Variable) As Boolean
            Return Not v Is Nothing
        End Operator

        Public Shared Operator IsFalse(v As Variable) As Boolean
            Return v Is Nothing
        End Operator
    End Class
End Namespace