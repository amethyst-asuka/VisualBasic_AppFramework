﻿#Region "Microsoft.VisualBasic::532761f83fc4f6e6e888b5a96d75fd5b, ..\core.test"

    ' Author:
    ' 
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 


    ' Source file summaries:

    ' Module Module1
    ' 
    '     Sub: Main
    ' 
    ' 
    ' 

#End Region

Module Module1

    Sub Main()

        Call App.Shell(App.HOME & "/child.exe", "/test /123455 sdfgdshgjkfdhgjkdf").Run()

        Pause()
    End Sub

End Module
