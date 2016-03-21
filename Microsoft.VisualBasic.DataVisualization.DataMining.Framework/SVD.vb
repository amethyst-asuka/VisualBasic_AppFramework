' AForge Math Library
' AForge.NET framework
' http://www.aforgenet.com/framework/
'
' Copyright © AForge.NET, 2007-2014
' aforge.net@gmail.com
'

' Just a copy-paste of SVD algorithm from Numerical Recipes but updated for C#
' (as authors state, the code is aimed to be machine readable, so blame them
' for all those c/f/g/h/s variable)
Public Module SVD

    ''' <summary>
    ''' Singular Value Decomposition
    ''' </summary>
    ''' <param name="a">Number of rows in A must be greater or equal to number of columns</param>
    ''' <param name="w"></param>
    ''' <param name="v"></param>
    Public Sub SVDecomposition(a As Double(,), ByRef w As Double(), ByRef v As Double(,))
        ' number of rows in A
        Dim m As Integer = a.GetLength(0)
        ' number of columns in A
        Dim n As Integer = a.GetLength(1)

        If m < n Then
            Throw New ArgumentException("Number of rows in A must be greater or equal to number of columns")
        End If

        w = New Double(n - 1) {}
        v = New Double(n - 1, n - 1) {}


        Dim flag As Integer, i As Integer, its As Integer, j As Integer, jj As Integer, k As Integer,
                l As Integer = 0, nm As Integer = 0
        Dim anorm As Double, c As Double, f As Double, g As Double, h As Double, s As Double,
                scale As Double, x As Double, y As Double, z As Double

        Dim rv1 As Double() = New Double(n - 1) {}

        ' householder reduction to bidiagonal form
        anorm = 0R
        scale = 0R
        g = scale

        For i = 0 To n - 1
            l = i + 1
            rv1(i) = scale * g
            scale = 0R
            s = scale
            g = s

            If i < m Then
                For k = i To m - 1
                    scale += System.Math.Abs(a(k, i))
                Next

                If scale <> 0.0 Then
                    For k = i To m - 1
                        a(k, i) /= scale
                        s += a(k, i) * a(k, i)
                    Next

                    f = a(i, i)
                    g = -Sign(System.Math.Sqrt(s), f)
                    h = f * g - s
                    a(i, i) = f - g

                    If i <> n - 1 Then
                        For j = l To n - 1
                            s = 0.0
                            k = i
                            While k < m
                                s += a(k, i) * a(k, j)
                                k += 1
                            End While

                            f = s / h

                            For k = i To m - 1
                                a(k, j) += f * a(k, i)
                            Next
                        Next
                    End If

                    For k = i To m - 1
                        a(k, i) *= scale
                    Next
                End If
            End If

            w(i) = scale * g
            scale = 0R
            s = scale
            g = s

            If (i < m) AndAlso (i <> n - 1) Then
                For k = l To n - 1
                    scale += System.Math.Abs(a(i, k))
                Next

                If scale <> 0.0 Then
                    For k = l To n - 1
                        a(i, k) /= scale
                        s += a(i, k) * a(i, k)
                    Next

                    f = a(i, l)
                    g = -Sign(System.Math.Sqrt(s), f)
                    h = f * g - s
                    a(i, l) = f - g

                    For k = l To n - 1
                        rv1(k) = a(i, k) / h
                    Next

                    If i <> m - 1 Then
                        For j = l To m - 1
                            s = 0.0
                            k = l
                            While k < n
                                s += a(j, k) * a(i, k)
                                k += 1
                            End While
                            For k = l To n - 1
                                a(j, k) += s * rv1(k)
                            Next
                        Next
                    End If

                    For k = l To n - 1
                        a(i, k) *= scale
                    Next
                End If
            End If
            anorm = System.Math.Max(anorm, (System.Math.Abs(w(i)) + System.Math.Abs(rv1(i))))
        Next

        ' accumulation of right-hand transformations
        For i = n - 1 To 0 Step -1
            If i < n - 1 Then
                If g <> 0.0 Then
                    For j = l To n - 1
                        v(j, i) = (a(i, j) / a(i, l)) / g
                    Next

                    For j = l To n - 1
                        s = 0
                        k = l
                        While k < n
                            s += a(i, k) * v(k, j)
                            k += 1
                        End While
                        For k = l To n - 1
                            v(k, j) += s * v(k, i)
                        Next
                    Next
                End If
                For j = l To n - 1
                    v(j, i) = 0R
                    v(i, j) = 0R
                Next
            End If
            v(i, i) = 1
            g = rv1(i)
            l = i
        Next

        ' accumulation of left-hand transformations
        For i = n - 1 To 0 Step -1
            l = i + 1
            g = w(i)

            If i < n - 1 Then
                For j = l To n - 1
                    a(i, j) = 0.0
                Next
            End If

            If g <> 0 Then
                g = 1.0 / g

                If i <> n - 1 Then
                    For j = l To n - 1
                        s = 0
                        k = l
                        While k < m
                            s += a(k, i) * a(k, j)
                            k += 1
                        End While

                        f = (s / a(i, i)) * g

                        For k = i To m - 1
                            a(k, j) += f * a(k, i)
                        Next
                    Next
                End If

                For j = i To m - 1
                    a(j, i) *= g
                Next
            Else
                For j = i To m - 1
                    a(j, i) = 0
                Next
            End If
            a(i, i) += 1
        Next

        ' diagonalization of the bidiagonal form: Loop over singular values
        ' and over allowed iterations
        For k = n - 1 To 0 Step -1
            For its = 1 To 30
                flag = 1

                For l = k To 0 Step -1
                    ' test for splitting
                    nm = l - 1

                    If System.Math.Abs(rv1(l)) + anorm = anorm Then
                        flag = 0
                        Exit For
                    End If

                    If System.Math.Abs(w(nm)) + anorm = anorm Then
                        Exit For
                    End If
                Next

                If flag <> 0 Then
                    c = 0.0
                    s = 1.0
                    For i = l To k
                        f = s * rv1(i)

                        If System.Math.Abs(f) + anorm <> anorm Then
                            g = w(i)
                            h = Pythag(f, g)
                            w(i) = h
                            h = 1.0 / h
                            c = g * h
                            s = -f * h

                            For j = 0 To m - 1
                                y = a(j, nm)
                                z = a(j, i)
                                a(j, nm) = y * c + z * s
                                a(j, i) = z * c - y * s
                            Next
                        End If
                    Next
                End If

                z = w(k)

                If l = k Then
                    ' convergence
                    If z < 0.0 Then
                        ' singular value is made nonnegative
                        w(k) = -z

                        For j = 0 To n - 1
                            v(j, k) = -v(j, k)
                        Next
                    End If
                    Exit For
                End If

                If its = 30 Then
                    Throw New ApplicationException("No convergence in 30 svdcmp iterations")
                End If

                ' shift from bottom 2-by-2 minor
                x = w(l)
                nm = k - 1
                y = w(nm)
                g = rv1(nm)
                h = rv1(k)
                f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y)
                g = Pythag(f, 1.0)
                f = ((x - z) * (x + z) + h * ((y / (f + Sign(g, f))) - h)) / x

                ' next QR transformation
                s = 1.0
                c = s

                For j = l To nm
                    i = j + 1
                    g = rv1(i)
                    y = w(i)
                    h = s * g
                    g = c * g
                    z = Pythag(f, h)
                    rv1(j) = z
                    c = f / z
                    s = h / z
                    f = x * c + g * s
                    g = g * c - x * s
                    h = y * s
                    y *= c

                    For jj = 0 To n - 1
                        x = v(jj, j)
                        z = v(jj, i)
                        v(jj, j) = x * c + z * s
                        v(jj, i) = z * c - x * s
                    Next

                    z = Pythag(f, h)
                    w(j) = z

                    If z <> 0 Then
                        z = 1.0 / z
                        c = f * z
                        s = h * z
                    End If

                    f = c * g + s * y
                    x = c * y - s * g

                    For jj = 0 To m - 1
                        y = a(jj, j)
                        z = a(jj, i)
                        a(jj, j) = y * c + z * s
                        a(jj, i) = z * c - y * s
                    Next
                Next

                rv1(l) = 0.0
                rv1(k) = f
                w(k) = x
            Next
        Next
    End Sub

    Private Function Sign(a As Double, b As Double) As Double
        Return If((b >= 0.0), System.Math.Abs(a), -System.Math.Abs(a))
    End Function

    Private Function Pythag(a As Double, b As Double) As Double
        Dim at As Double = System.Math.Abs(a), bt As Double = System.Math.Abs(b), ct As Double, result As Double

        If at > bt Then
            ct = bt / at
            result = at * System.Math.Sqrt(1.0 + ct * ct)
        ElseIf bt > 0.0 Then
            ct = at / bt
            result = bt * System.Math.Sqrt(1.0 + ct * ct)
        Else
            result = 0.0
        End If

        Return result
    End Function
End Module
