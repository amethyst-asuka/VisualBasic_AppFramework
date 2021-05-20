﻿Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language.C
Imports Microsoft.VisualBasic.MIME.Html.Document
Imports Microsoft.VisualBasic.Text

Namespace TextParser

    Module BaseInvoke

        ''' <summary>
        ''' Extract the text of the current node
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters"></param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        <ExportAPI("text")>
        Public Function text(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Return ParserFunction.ParseDocument(document, getText, isArray)
        End Function

        Private Function getText() As IParserPipeline
            Return Function(i)
                       If TypeOf i Is HtmlElement Then
                           Return New InnerPlantText With {
                               .InnerText = i.GetPlantText
                           }
                       Else
                           Return i
                       End If
                   End Function
        End Function

        ''' <summary>
        ''' Clear spaces and line breaks before and after the string
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters"></param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        <ExportAPI("trim")>
        Public Function trim(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Return ParserFunction.ParseDocument(document, trim(parameters), isArray)
        End Function

        Private Function trim(parameters As String()) As IParserPipeline
            Dim chars As Char()

            If parameters.IsNullOrEmpty Then
                chars = {" "c, ASCII.TAB, ASCII.CR, ASCII.LF}
            Else
                chars = parameters _
                    .JoinBy("") _
                    .ReplaceMetaChars _
                    .ToArray
            End If

            Return Function(i)
                       Dim text As String = i.GetPlantText

                       text = Strings.Trim(text)
                       text = text.Trim(chars)

                       Return New InnerPlantText With {.InnerText = text}
                   End Function
        End Function

        ''' <summary>
        ''' Take the nth element in the current node collection
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters"></param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        <ExportAPI("eq")>
        Public Function eq(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Dim n As Integer = Integer.Parse(parameters(Scan0))
            Dim nItem As InnerPlantText = DirectCast(document, HtmlElement).HtmlElements(n)

            Return nItem
        End Function

        ''' <summary>
        ''' removes of the text string that matched the pattern of given regexp list
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters"></param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        <ExportAPI("filter")>
        Public Function filter(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Dim patterns As Regex() = parameters _
                .Select(Function(p) New Regex(p)) _
                .ToArray

        End Function

        ''' <summary>
        ''' match the string collection with given regexp pattern
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters"></param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        <ExportAPI("match")>
        Public Function match(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText

        End Function

        ''' <summary>
        ''' parse the sub-string with given regexp pattern
        ''' </summary>
        ''' <param name="document"></param>
        ''' <param name="parameters">
        ''' [regexp, opts]
        ''' </param>
        ''' <param name="isArray"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Regular_Expressions#advanced_searching_with_flags
        ''' </remarks>
        <ExportAPI("regexp")>
        Public Function regexp(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Dim options As String = parameters(1)
            Dim pattern As String = parameters(Scan0)
            Dim opts As RegexOptions = RegexOptions.Compiled
            Dim r As New Regex(pattern, opts)

            Return ParserFunction.ParseDocument(document, regexpParseText(r), isArray)
        End Function

        Private Function regexpParseText(r As Regex) As IParserPipeline
            Return Function(i)
                       Dim text As String = i.GetPlantText
                       text = r.Match(text).Value
                       Return New InnerPlantText With {.InnerText = text}
                   End Function
        End Function

        <ExportAPI("html")>
        Public Function html(document As InnerPlantText, parameters As String(), isArray As Boolean) As InnerPlantText
            Return ParserFunction.ParseDocument(document, Function(i) New InnerPlantText With {.InnerText = i.GetHtmlText}, isArray)
        End Function
    End Module
End Namespace