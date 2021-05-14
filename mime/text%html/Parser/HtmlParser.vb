﻿Imports Microsoft.VisualBasic.Emit.Marshal
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Markup.HTML

Public Class HtmlParser

    Public Shared Function ParseTree(document As String) As HtmlElement
        Dim tokens = New TokenIcer(document).GetTokens.ToArray
        Dim i As Pointer(Of Token) = tokens
        Dim html As New HtmlElement With {.Name = "!DOCTYPE html"}
        Dim tagStack As New Stack(Of HtmlElement)
        Dim a As New Value(Of Token)

        tagStack.Push(html)

        Do While i
            Select Case (a = ++i).name
                Case HtmlTokens.openTag
                    Dim name As String = (++i).text

                    If name = "/" Then
                        name = (++i).text

                        If name = tagStack.Peek.Name Then
                            tagStack.Pop()
                            i.MoveNext()
                        End If
                    Else
                        Dim newTag As New HtmlElement With {.Name = name}
                        Dim tagClosed As Boolean = False

                        Do While (a = ++i).name <> HtmlTokens.closeTag
                            ' name=value
                            If i.Current.name = HtmlTokens.equalsSymbol Then
                                i.MoveNext()
                                newTag.Add(CType(a, Token).text, (++i).text)
                            ElseIf CType(a, Token).name = HtmlTokens.splash AndAlso i.Current.name = HtmlTokens.closeTag Then
                                ' <.../>
                                i.MoveNext()
                                tagClosed = True
                                Exit Do
                            Else
                                newTag.Add(CType(a, Token).text, "")
                            End If
                        Loop

                        tagStack.Peek.Add(newTag)

                        If Not tagClosed Then
                            If Not (newTag.Name = "img" OrElse newTag.Name = "br" OrElse newTag.Name = "hr") Then
                                tagStack.Push(newTag)
                            End If
                        End If
                    End If
                Case Else
                    tagStack.Peek.Add(New InnerPlantText(CType(a, Token).text))
            End Select
        Loop

        Return html
    End Function
End Class