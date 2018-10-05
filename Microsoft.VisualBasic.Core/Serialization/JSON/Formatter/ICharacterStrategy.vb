﻿#Region "Microsoft.VisualBasic::e5a7ba64525ce64c0a225cea14e5f7d8, Microsoft.VisualBasic.Core\Serialization\JSON\Formatter\ICharacterStrategy.vb"

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

    '     Interface ICharacterStrategy
    ' 
    '         Properties: ForWhichCharacter
    ' 
    '         Sub: Execute
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Serialization.JSON.Formatter.Internals

    Friend Interface ICharacterStrategy
        Sub Execute(context As JsonFormatterStrategyContext)

        ReadOnly Property ForWhichCharacter() As Char
    End Interface
End Namespace