﻿Imports Caliburn.Micro
Imports Hotelie.Application.Services.Authentication
Imports Hotelie.Presentation.Bills.ViewModels
Imports Hotelie.Presentation.Common
Imports Hotelie.Presentation.Common.Controls
Imports Hotelie.Presentation.Leases.ViewModels
Imports Hotelie.Presentation.Rooms.ViewModels
Imports Hotelie.Presentation.Rules.ViewModels
Imports MaterialDesignThemes.Wpf

Namespace Start.WorkspaceShell.ViewModels
	Public Class WorkspaceShellViewModel
		Inherits Screen
		Implements IShell
		Implements INeedWindowModals

		' Dependencies
		Private ReadOnly _authentication As IAuthentication

		Private _activeWorkspace As IScreen
		Private _displayWorkspaceCode As Integer
		Private _displayCode As Integer

		' Parent window
		Public Property ParentWindow As IMainWindow Implements IChild(Of IMainWindow).Parent
			Get
				Return CType(Parent, IMainWindow)
			End Get
			Set
				Parent = Value
			End Set
		End Property

		' Initialization
		Public Sub New( authentication As IAuthentication )
			_authentication = authentication

			DisplayName = "Bàn làm việc"

			' load command bar
			CommandsBar = New WorkspaceShellCommandsBarViewModel( Me )

			' load workspace
			WorkspaceRooms = IoC.Get(Of RoomsWorkspaceViewModel)
			WorkspaceLeases = IoC.Get(Of LeasesWorkspaceViewModel)
			WorkspaceBills = IoC.Get(Of BillsWorkspaceViewModel)
			Workspaces = New BindableCollection(Of IAppScreen) From {WorkspaceRooms, WorkspaceLeases, WorkspaceBills}

			' load other screens
			ScreenChangeRules = IoC.Get(Of ScreenChangeRulesViewModel)
			Screens = New BindableCollection(Of IAppScreen) From {
				WorkspaceRooms, WorkspaceLeases, WorkspaceBills,
				ScreenChangeRules}

			' subcribe exited event
			For Each screen As IAppScreen In Screens
				AddHandler screen.OnExited, AddressOf OnScreenExited
			Next

			' initial screen
			DisplayWorkspaceCode = 0
		End Sub

		' Display
		Public ReadOnly Property CommandsBar As IWindowCommandsBar Implements IShell.CommandsBar

		Public ReadOnly Property WorkspaceRooms As RoomsWorkspaceViewModel

		Public ReadOnly Property WorkspaceLeases As LeasesWorkspaceViewModel

		Public ReadOnly Property WorkspaceBills As BillsWorkspaceViewModel

		Public ReadOnly Property ScreenChangeRules As ScreenChangeRulesViewModel

		Public ReadOnly Property Workspaces As IObservableCollection(Of IAppScreen)

		Public ReadOnly Property Screens As IObservableCollection(Of IAppScreen)

		Public Property DisplayWorkspaceCode As Integer
			Get
				Return _displayWorkspaceCode
			End Get
			Set
				If Equals( Value, _displayWorkspaceCode ) Then Return
				_displayWorkspaceCode = value
				DisplayCode = DisplayWorkspaceCode
				NotifyOfPropertyChange( Function() DisplayWorkspaceCode )
			End Set
		End Property

		Public Property DisplayCode As Integer
			Get
				Return _displayCode
			End Get
			Set
				If Equals( Value, _displayCode ) Then Return
				If Value < 0 OrElse Value >= Screens.Count Then Return
				_displayCode = value
				UpdateScreenAsync()
			End Set
		End Property

		' Navigation
		Private Sub UpdateScreen()
			Dim screen = Screens( DisplayCode )

			screen.Show()
			NotifyOfPropertyChange( Function() DisplayCode )
			ParentWindow.TitleMode = screen.ColorMode
		End Sub

		Private Async Sub UpdateScreenAsync()
			Dim screen = Screens( DisplayCode )
			Await screen.ShowAsync()
			NotifyOfPropertyChange( Function() DisplayCode )
			ParentWindow.TitleMode = screen.ColorMode
		End Sub

		Private Sub OnScreenExited( sender As Object,
		                            e As EventArgs )
			If Not Workspaces.Contains( CType(sender, IAppScreen) )
				DisplayCode	= DisplayWorkspaceCode
			End If
		End Sub

		Public Sub NavigateToScreenAddLease( roomId As String )
			DisplayWorkspaceCode = 1
			WorkspaceLeases.NavigateToScreenAddLease( roomId )
		End Sub

		Public Sub NavigateToScreenChangeRules()
			DisplayCode = 3
		End Sub

		' Closing
		Public Overrides Async Sub CanClose( callback As Action(Of Boolean) )
			Dim dialog = New TwoButtonDialog( "Thoát khỏi bàn làm việc?", "THOÁT", "HỦY", True, False )

			Dim result = Await ShowDynamicWindowDialog( dialog )

			If String.Equals( result, "THOÁT" )
				callback( True )
			Else
				callback( False )
			End If
		End Sub

		Protected Overrides Sub OnDeactivate( close As Boolean )
			ParentWindow.TitleMode = ColorZoneMode.PrimaryDark

			MyBase.OnDeactivate( close )

			If close
				_authentication.Logout()
			End If
		End Sub
	End Class
End Namespace
