﻿Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text.Xml.Models

Module VectorTest

    Sub Main()

        Dim vector = {
            New NamedValue With {.name = "name", .text = "dddd"},
            New NamedValue With {.name = "name2222222", .text = "1234"}
        }.VectorShadows

        Dim textArray$() = vector.text

        Call textArray.GetJson.__DEBUG_ECHO

        Dim newText$() = {"eeeed", "ffffffff"}

        vector.text = newText



        Pause()
    End Sub
End Module
