﻿Imports Microsoft.VisualBasic.Imaging.Math2D

Namespace Layouts.Cola

    Public Class TangentVisibilityGraph

        Public V As New List(Of VisibilityVertex)
        Public E As New List(Of VisibilityEdge)
        Public P As TVGPoint()()

        Public Sub New(P__1 As TVGPoint()(), Optional g0 As any = Nothing)
            If g0 Is Nothing Then
                Dim n = P__1.Length
                ' For each node...
                For i As Integer = 0 To n - 1
                    Dim p__2 = P__1(i)
                    ' For each node vertex.
                    For j As Integer = 0 To p__2.Length - 1
                        Dim pj__3 = p__2(j)
                        Dim vv = New VisibilityVertex(Me.V.Count, i, j, pj__3)
                        Me.V.Add(vv)
                        ' For the every iteration but the first, generate an
                        ' edge from the previous visibility vertex to the
                        ' current one.
                        If j > 0 Then
                            Me.E.Add(New VisibilityEdge(p__2(j - 1).vv, vv))
                        End If
                    Next
                    ' Add a visibility edge from the first vertex to the last.
                    If p__2.Length > 1 Then
                        Me.E.Add(New VisibilityEdge(p__2(0).vv, p__2(p__2.Length - 1).vv))
                    End If
                Next
                For i As Integer = 0 To n - 2
                    Dim Pi = P__1(i)
                    For j As Integer = i + 1 To n - 1
                        Dim Pj__4 = P__1(j)
                        Dim t = tangents(Pi, Pj__4)
                        For Each q As var In t.keys
                            Dim c = t(q)
                            Dim source = Pi(c.t1)
                            Dim target = Pj__4(c.t2)
                            Me.addEdgeIfVisible(source, target, i, j)
                        Next
                    Next
                Next
            Else
                Me.V = g0.V.slice(0)
                Me.E = g0.E.slice(0)
            End If
        End Sub

        Public Sub addEdgeIfVisible(u As TVGPoint, v As TVGPoint, i1 As Double, i2 As Double)
            If Not Me.intersectsPolys(New Line(u.X, u.Y, v.X, v.Y), i1, i2) Then
                Me.E.Add(New VisibilityEdge(u.vv, v.vv))
            End If
        End Sub

        Private Function addPoint(p As TVGPoint, i1 As Double) As VisibilityVertex
            Dim n = Me.P.Length
            Me.V.Add(New VisibilityVertex(Me.V.Count, n, 0, p))
            For i As Integer = 0 To n - 1
                If i = i1 Then
                    Continue For
                End If
                Dim poly = Me.P(i)
                Dim t = tangent_PointPolyC(p, poly)

                Me.addEdgeIfVisible(p, poly(t.ltan), i1, i)
                Me.addEdgeIfVisible(p, poly(t.rtan), i1, i)
            Next
            Return p.vv
        End Function

        Private Function intersectsPolys(l As Line, i1 As Double, i2 As Double) As Boolean
            Dim i As Integer = 0, n As Integer = Me.P.Length
            While i < n
                If i <> i1 AndAlso i <> i2 AndAlso intersects(l, Me.P(i)).Length > 0 Then
                    Return True
                End If
                i += 1
            End While
            Return False
        End Function
    End Class

End Namespace