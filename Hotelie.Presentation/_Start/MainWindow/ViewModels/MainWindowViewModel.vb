﻿Imports Caliburn.Micro
Imports Hotelie.Presentation.Common

Namespace Start.MainWindow.ViewModels
	Public Class MainWindowViewModel
		Inherits Conductor(Of IShell)
		Implements IMainWindow

		Private _title As String
		Private _width As Double
		Private _height As Double
		Private _windowState As WindowState

		Public Sub New()
			Title = "Hotelie"

			Width = 1000

			Height = 600
		End Sub

		Public Property Title As String Implements IMainWindow.Title
			Get
				Return _title
			End Get
			Set
				If String.Equals( _title, Value ) Then Return

				_title = value
				NotifyOfPropertyChange( Function() Title )
			End Set
		End Property

		Public Property Width As Double Implements IMainWindow.Width
			Get
				Return _width
			End Get
			Set
				If Double.Equals( _width, Value ) Then Return

				_width = value
				NotifyOfPropertyChange( Function() Width )
			End Set
		End Property

		Public Property Height As Double Implements IMainWindow.Height
			Get
				Return _height
			End Get
			Set
				If Double.Equals( _height, Value ) Then Return

				_height = value
				NotifyOfPropertyChange( Function() Height )
			End Set
		End Property

		Public Property WindowState As WindowState Implements IMainWindow.WindowState
			Get
				Return _windowState
			End Get
			Set
				If Equals( _windowState, Value ) Then Return

				_windowState = value
				NotifyOfPropertyChange( Function() WindowState )
			End Set
		End Property

		Public ReadOnly Property Shell As IShell Implements IMainWindow.Shell
			Get
				Return ActiveItem
			End Get
		End Property

		Public Sub ShowLoginShell() Implements IMainWindow.ShowLoginShell
			Title = "Hotelie - Login"

			Width = 1000

			Height = 600

			WindowState = WindowState.Normal

			ActivateItem( IoC.Get(Of IShell)( "login-shell" ) )
		End Sub

		Public Sub ShowWorkspaceShell() Implements IMainWindow.ShowWorkspaceShell
			Title = "Hotelie - Workspace"

			WindowState = WindowState.Maximized

			ActivateItem( IoC.Get(Of IShell)( "workspace-shell" ) )
		End Sub

		Protected Overrides Sub ChangeActiveItem( newItem As IShell,
		                                          closePrevious As Boolean )
			MyBase.ChangeActiveItem( newItem, closePrevious )

			' update view
			NotifyOfPropertyChange( Function() Shell )

			' update window title
			If Shell Is Nothing
				Title = "Hotelie"
			Else
				Title = $"Hotelie - {Shell.DisplayName}"
			End If
		End Sub

		Public Sub ToggleZoomState() Implements IMainWindow.ToggleZoomState
			WindowState = 2 - WindowState
		End Sub

		Public Sub Hide() Implements IMainWindow.Hide
			WindowState = WindowState.Minimized
		End Sub

		Public Sub Close() Implements IMainWindow.Close
			Windows.Application.Current.MainWindow.Close()
		End Sub
	End Class
End Namespace
