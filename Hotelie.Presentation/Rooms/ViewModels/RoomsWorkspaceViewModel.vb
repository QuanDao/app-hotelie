﻿Imports Caliburn.Micro
Imports Hotelie.Presentation.Common

Namespace Rooms.ViewModels
	Public Class RoomsWorkspaceViewModel
		Inherits Screen
		Implements IWorkspace

		Protected Overrides Sub OnInitialize()
			MyBase.OnInitialize()

			DisplayName = "Danh sách phòng"
		End Sub
	End Class
End Namespace
