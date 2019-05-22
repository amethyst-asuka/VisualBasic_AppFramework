﻿#Region "Microsoft.VisualBasic::c69f7289e2d0dd95768872279f411e93, Data_science\Mathematica\Plot\test\ROCplotTest.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Module ROCplotTest
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.ChartPlots.Statistics
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.DataMining

Module ROCplotTest

    Sub Main()
        ' Dim data = EntityObject.LoadDataSet("D:\GCModeller\src\runtime\sciBASIC#\Data_science\Mathematica\Plot\data\ROC\identify.csv").ToArray
        ' Dim test = Validation.ROC(data, Function(d, p) d!class = "p", Function(d, p) d!score >= p).CreateSerial

        Dim data = EntityObject.LoadDataSet("D:\smartnucl_integrative\biodeepDB\smartnucl_integrative\build_tools\CVD_kb\duke\out\CHD_20190411_IVD\[ALL-validates]validate-result_4markers.csv").ToArray
        Dim test = Validation.ROC(data, Function(d, p) d!CHD > 0, Function(d, p) d("CHD(predicted)") >= p).CreateSerial

        Call ROCPlot.Plot(test).Save("D:\GCModeller\src\runtime\sciBASIC#\Data_science\Mathematica\Plot\data\ROC\identify_ROC2.png")
    End Sub
End Module