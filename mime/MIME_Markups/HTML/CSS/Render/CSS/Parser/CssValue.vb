Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Reflection
Imports System.Diagnostics
Imports System.Globalization
Imports System.Drawing

Public NotInheritable Class CssValue

    Private Sub New()
    End Sub

    Shared Sub New()
        'Add this assembly as a reference
        References.Add(Assembly.GetExecutingAssembly())
    End Sub

    ''' <summary>
    ''' Evals a number and returns it. If number is a percentage, it will be multiplied by <paramref name="hundredPercent"/>
    ''' </summary>
    ''' <param name="number">Number to be parsed</param>
    ''' <param name="hundredPercent">Number that represents the 100% if parsed number is a percentage</param>
    ''' <returns>Parsed number. Zero if error while parsing.</returns>
    Public Shared Function ParseNumber(number As String, hundredPercent As Single) As Single
        If String.IsNullOrEmpty(number) Then
            Return 0F
        End If

        Dim toParse As String = number
        Dim isPercent As Boolean = number.EndsWith("%")
        Dim result As Single = 0F

        If isPercent Then
            toParse = number.Substring(0, number.Length - 1)
        End If

        If Not Single.TryParse(toParse, NumberStyles.Number, NumberFormatInfo.InvariantInfo, result) Then
            Return 0F
        End If

        If isPercent Then
            result = (result / 100.0F) * hundredPercent
        End If

        Return result
    End Function

    ''' <summary>
    ''' Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
    ''' </summary>
    ''' <param name="length">Specified length</param>
    ''' <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
    ''' <param name="box"></param>
    ''' <returns></returns>
    Public Shared Function ParseLength(length As String, hundredPercent As Single, box As CssBox) As Single
        Return ParseLength(length, hundredPercent, box, box.GetEmHeight(), False)
    End Function

    ''' <summary>
    ''' Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
    ''' </summary>
    ''' <param name="length">Specified length</param>
    ''' <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
    ''' <param name="box"></param>
    ''' <param name="returnPoints">Allows the return float to be in points. If false, result will be pixels</param>
    ''' <returns></returns>
    Public Shared Function ParseLength(length As String, hundredPercent As Single, box As CssBox, emFactor As Single, returnPoints As Boolean) As Single
        'Return zero if no length specified, zero specified
        If String.IsNullOrEmpty(length) OrElse length = "0" Then
            Return 0F
        End If

        'If percentage, use ParseNumber
        If length.EndsWith("%") Then
            Return ParseNumber(length, hundredPercent)
        End If

        'If no units, return zero
        If length.Length < 3 Then
            Return 0F
        End If

        'Get units of the length
        Dim unit As String = length.Substring(length.Length - 2, 2)

        'Factor will depend on the unit
        Dim factor As Single = 1.0F

        'Number of the length
        Dim number As String = length.Substring(0, length.Length - 2)

        'TODO: Units behave different in paper and in screen!
        Select Case unit
            Case CssConstants.Em
                factor = emFactor
                Exit Select
            Case CssConstants.Px
                factor = 1.0F
                Exit Select
            Case CssConstants.Mm
                factor = 3.0F
                '3 pixels per millimeter
                Exit Select
            Case CssConstants.Cm
                factor = 37.0F
                '37 pixels per centimeter
                Exit Select
            Case CssConstants.[In]
                factor = 96.0F
                '96 pixels per inch
                Exit Select
            Case CssConstants.Pt
                factor = 96.0F / 72.0F
                ' 1 point = 1/72 of inch
                If returnPoints Then
                    Return ParseNumber(number, hundredPercent)
                End If

                Exit Select
            Case CssConstants.Pc
                factor = 96.0F / 72.0F * 12.0F
                ' 1 pica = 12 points
                Exit Select
            Case Else
                factor = 0F
                Exit Select
        End Select



        Return factor * ParseNumber(number, hundredPercent)
    End Function

    ''' <summary>
    ''' Parses a color value in CSS style; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)
    ''' </summary>
    ''' <param name="colorValue">Specified color value; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)</param>
    ''' <returns>System.Drawing.Color value</returns>
    Public Shared Function GetActualColor(colorValue As String) As Color
        Dim r As Integer = 0
        Dim g As Integer = 0
        Dim b As Integer = 0
        Dim onError As Color = Color.Empty

        If String.IsNullOrEmpty(colorValue) Then
            Return onError
        End If

        colorValue = colorValue.ToLower().Trim()

        If colorValue.StartsWith("#") Then
            '#Region "hexadecimal forms"
            Dim hex As String = colorValue.Substring(1)

            If hex.Length = 6 Then
                r = Integer.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)
                g = Integer.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                b = Integer.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
            ElseIf hex.Length = 3 Then
                r = Integer.Parse(New [String](hex.Substring(0, 1)(0), 2), System.Globalization.NumberStyles.HexNumber)
                g = Integer.Parse(New [String](hex.Substring(1, 1)(0), 2), System.Globalization.NumberStyles.HexNumber)
                b = Integer.Parse(New [String](hex.Substring(2, 1)(0), 2), System.Globalization.NumberStyles.HexNumber)
            Else
                Return onError
                '#End Region
            End If
        ElseIf colorValue.StartsWith("rgb(") AndAlso colorValue.EndsWith(")") Then
            '#Region "RGB forms"

            Dim rgb As String = colorValue.Substring(4, colorValue.Length - 5)
            Dim chunks As String() = rgb.Split(","c)

            If chunks.Length = 3 Then
                ' unchecked
                If True Then
                    r = Convert.ToInt32(ParseNumber(chunks(0).Trim(), 255.0F))
                    g = Convert.ToInt32(ParseNumber(chunks(1).Trim(), 255.0F))
                    b = Convert.ToInt32(ParseNumber(chunks(2).Trim(), 255.0F))
                End If
            Else
                Return onError

                '#End Region
            End If
        Else
            '#Region "Color Constants"

            Dim hex As String = String.Empty

            Select Case colorValue
                Case CssConstants.Maroon
                    hex = "#800000"
                    Exit Select
                Case CssConstants.Red
                    hex = "#ff0000"
                    Exit Select
                Case CssConstants.Orange
                    hex = "#ffA500"
                    Exit Select
                Case CssConstants.Olive
                    hex = "#808000"
                    Exit Select
                Case CssConstants.Purple
                    hex = "#800080"
                    Exit Select
                Case CssConstants.Fuchsia
                    hex = "#ff00ff"
                    Exit Select
                Case CssConstants.White
                    hex = "#ffffff"
                    Exit Select
                Case CssConstants.Lime
                    hex = "#00ff00"
                    Exit Select
                Case CssConstants.Green
                    hex = "#008000"
                    Exit Select
                Case CssConstants.Navy
                    hex = "#000080"
                    Exit Select
                Case CssConstants.Blue
                    hex = "#0000ff"
                    Exit Select
                Case CssConstants.Aqua
                    hex = "#00ffff"
                    Exit Select
                Case CssConstants.Teal
                    hex = "#008080"
                    Exit Select
                Case CssConstants.Black
                    hex = "#000000"
                    Exit Select
                Case CssConstants.Silver
                    hex = "#c0c0c0"
                    Exit Select
                Case CssConstants.Gray
                    hex = "#808080"
                    Exit Select
                Case CssConstants.Yellow
                    hex = "#FFFF00"
                    Exit Select
            End Select

            If String.IsNullOrEmpty(hex) Then
                Return onError
            Else
                Dim c As Color = GetActualColor(hex)
                r = c.R
                g = c.G
                b = c.B

                '#End Region
            End If
        End If

        Return Color.FromArgb(r, g, b)
    End Function

    ''' <summary>
    ''' Parses a border value in CSS style; e.g. 1px, 1, thin, thick, medium
    ''' </summary>
    ''' <param name="borderValue"></param>
    ''' <returns></returns>
    Public Shared Function GetActualBorderWidth(borderValue As String, b As CssBox) As Single
        If String.IsNullOrEmpty(borderValue) Then
            Return GetActualBorderWidth(CssConstants.Medium, b)
        End If

        Select Case borderValue
            Case CssConstants.Thin
                Return 1.0F
            Case CssConstants.Medium
                Return 2.0F
            Case CssConstants.Thick
                Return 4.0F
            Case Else
                Return Math.Abs(ParseLength(borderValue, 1, b))
        End Select
    End Function

    ''' <summary>
    ''' Split the value by spaces; e.g. Useful in values like 'padding:5 4 3 inherit'
    ''' </summary>
    ''' <param name="value">Value to be splitted</param>
    ''' <returns>Splitted and trimmed values</returns>
    Public Shared Function SplitValues(value As String) As String()
        Return SplitValues(value, " "c)
    End Function

    ''' <summary>
    ''' Split the value by the specified separator; e.g. Useful in values like 'padding:5 4 3 inherit'
    ''' </summary>
    ''' <param name="value">Value to be splitted</param>
    ''' <returns>Splitted and trimmed values</returns>
    Public Shared Function SplitValues(value As String, separator As Char) As String()
        'TODO: CRITICAL! Don't split values on parenthesis (like rgb(0, 0, 0)) or quotes ("strings")


        If String.IsNullOrEmpty(value) Then
            Return New String() {}
        End If

        Dim values As String() = value.Split(separator)
        Dim result As New List(Of String)()

        For i As Integer = 0 To values.Length - 1
            Dim val As String = values(i).Trim()

            If Not String.IsNullOrEmpty(val) Then
                result.Add(val)
            End If
        Next

        Return result.ToArray()
    End Function

    ''' <summary>
    ''' Gets a list of Assembly references used to search for external references
    ''' </summary>
    ''' <remarks>
    ''' This references are used when loading images and other content, when
    ''' rendering a piece of HTML/CSS
    ''' </remarks>
    Public Shared ReadOnly Property References() As New List(Of Assembly)

    ''' <summary>
    ''' Detects the type name in a path. 
    ''' E.g. Gets System.Drawing.Graphics from a path like System.Drawing.Graphics.Clear
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Private Shared Function GetTypeInfo(path As String, ByRef moreInfo As String) As Type
        Dim lastDot As Integer = path.LastIndexOf("."c)

        If lastDot < 0 Then
            Return Nothing
        End If

        Dim type As String = path.Substring(0, lastDot)
        moreInfo = path.Substring(lastDot + 1)
        moreInfo = moreInfo.Replace("(", String.Empty).Replace(")", String.Empty)


        For Each a As Assembly In References
            Dim t As Type = a.[GetType](type, False, True)

            If t IsNot Nothing Then
                Return t
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the object specific to the path
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns>One of the following possible objects: FileInfo, MethodInfo, PropertyInfo</returns>
    Private Shared Function DetectSource(path As String) As Object
        If path.StartsWith("method:", StringComparison.CurrentCultureIgnoreCase) Then
            Dim methodName As String = String.Empty
            Dim t As Type = GetTypeInfo(path.Substring(7), methodName)
            If t Is Nothing Then
                Return Nothing
            End If
            Dim method As MethodInfo = t.GetMethod(methodName)

            If Not method.IsStatic OrElse method.GetParameters().Length > 0 Then
                Return Nothing
            End If

            Return method
        ElseIf path.StartsWith("property:", StringComparison.CurrentCultureIgnoreCase) Then
            Dim propName As String = String.Empty
            Dim t As Type = GetTypeInfo(path.Substring(9), propName)
            If t Is Nothing Then
                Return Nothing
            End If
            Dim prop As PropertyInfo = t.GetProperty(propName)

            Return prop
        ElseIf Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute) Then
            Return New Uri(path)
        Else
            Return New FileInfo(path)
        End If
    End Function

    ''' <summary>
    ''' Gets the image of the specified path
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function GetImage(path As String) As Image
        Dim source As Object = DetectSource(path)

        Dim finfo As FileInfo = TryCast(source, FileInfo)
        Dim prop As PropertyInfo = TryCast(source, PropertyInfo)
        Dim method As MethodInfo = TryCast(source, MethodInfo)

        Try
            If finfo IsNot Nothing Then
                If Not finfo.Exists Then
                    Return Nothing
                End If


                Return Image.FromFile(finfo.FullName)
            ElseIf prop IsNot Nothing Then
                If Not prop.PropertyType.IsSubclassOf(GetType(Image)) AndAlso Not prop.PropertyType.Equals(GetType(Image)) Then
                    Return Nothing
                End If

                Return TryCast(prop.GetValue(Nothing, Nothing), Image)
            ElseIf method IsNot Nothing Then
                If Not method.ReturnType.IsSubclassOf(GetType(Image)) Then
                    Return Nothing
                End If

                Return TryCast(method.Invoke(Nothing, Nothing), Image)
            Else
                Return Nothing
            End If
        Catch
            'TODO: Return error image
            Return New Bitmap(50, 50)
        End Try
    End Function

    ''' <summary>
    ''' Gets the content of the stylesheet specified in the path
    ''' </summary>
    ''' <param name="path"></param>
    ''' <returns></returns>
    Public Shared Function GetStyleSheet(path As String) As String
        Dim source As Object = DetectSource(path)

        Dim finfo As FileInfo = TryCast(source, FileInfo)
        Dim prop As PropertyInfo = TryCast(source, PropertyInfo)
        Dim method As MethodInfo = TryCast(source, MethodInfo)

        Try
            If finfo IsNot Nothing Then
                If Not finfo.Exists Then
                    Return Nothing
                End If

                Dim sr As New StreamReader(finfo.FullName)
                Dim result As String = sr.ReadToEnd()
                sr.Dispose()

                Return result
            ElseIf prop IsNot Nothing Then
                If Not prop.PropertyType.Equals(GetType(String)) Then
                    Return Nothing
                End If

                Return TryCast(prop.GetValue(Nothing, Nothing), String)
            ElseIf method IsNot Nothing Then
                If Not method.ReturnType.Equals(GetType(String)) Then
                    Return Nothing
                End If

                Return TryCast(method.Invoke(Nothing, Nothing), String)
            Else
                Return String.Empty
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Executes the desired action when the user clicks a link
    ''' </summary>
    ''' <param name="href"></param>
    Public Shared Sub GoLink(href As String)
        Dim source As Object = DetectSource(href)

        Dim finfo As FileInfo = TryCast(source, FileInfo)
        Dim prop As PropertyInfo = TryCast(source, PropertyInfo)
        Dim method As MethodInfo = TryCast(source, MethodInfo)
        Dim uri As Uri = TryCast(source, Uri)

        Try
            If finfo IsNot Nothing OrElse uri IsNot Nothing Then
                Dim nfo As New ProcessStartInfo(href)
                nfo.UseShellExecute = True


                Process.Start(nfo)
            ElseIf method IsNot Nothing Then
                method.Invoke(Nothing, Nothing)
                'Nothing to do.
            Else
            End If
        Catch
            Throw
        End Try
    End Sub
End Class
