﻿Imports Caliburn.Micro
Imports Hotelie.Presentation.Common

Namespace Leases.ViewModels
	Public Class LeasesWorkspaceViewModel
		Inherits PropertyChangedBase
		Implements IWorkspace

		Private _displayName As String

		Public Property DisplayName As String Implements IHaveDisplayName.DisplayName
			Get
				Return _displayName
			End Get
			Set
				_displayName = value
			End Set
		End Property

		Public Sub New()
			DisplayName = "Thuê phòng"
		End Sub
	End Class
End Namespace
