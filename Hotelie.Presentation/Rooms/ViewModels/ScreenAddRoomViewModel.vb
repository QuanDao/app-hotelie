﻿Imports Caliburn.Micro
Imports Hotelie.Application.Rooms.Queries.GetRoomCategoriesList
Imports Hotelie.Presentation.Common
Imports Hotelie.Presentation.Common.Controls
Imports Hotelie.Presentation.Start.MainWindow.Models
Imports MaterialDesignThemes.Wpf

Namespace Rooms.ViewModels
	Public Class ScreenAddRoomViewModel
		Inherits PropertyChangedBase
		Implements IChild(Of RoomsWorkspaceViewModel)

		' Dependencies
		Private ReadOnly _getRoomCategoriesList As IGetRoomCategoriesListQuery

		' Backing fields
		Private _roomName As String
		Private _roomCategory As RoomCategoryModel
		Private _roomNote As String

		' Parent
		Public Property Parent As Object Implements IChild.Parent

		Public Property ParentWorkspace As RoomsWorkspaceViewModel Implements IChild(Of RoomsWorkspaceViewModel).Parent

		' Initialization
		Sub New( workspace As RoomsWorkspaceViewModel,
		         getRoomCategoriesList As IGetRoomCategoriesListQuery )
			ParentWorkspace = workspace

			_getRoomCategoriesList = getRoomCategoriesList

			InitRoomCategories( RoomCategories )

			InitValues()
		End Sub

		Private Sub InitValues()
			_roomName = String.Empty
			_roomCategory = RoomCategories.FirstOrDefault()
			_roomNote = String.Empty
		End Sub

		Private Sub InitRoomCategories( ByRef roomCategoryCollection As IObservableCollection(Of RoomCategoryModel) )
			roomCategoryCollection = New BindableCollection(Of RoomCategoryModel)
			roomCategoryCollection.AddRange( _getRoomCategoriesList.Execute() )
		End Sub

		' Data
		Public Property RoomName As String
			Get
				Return _roomName
			End Get
			Set
				If String.Equals( Value, _roomName ) Then Return
				_roomName = value
				NotifyOfPropertyChange( Function() RoomName )
			End Set
		End Property

		Public Property RoomCategory As RoomCategoryModel
			Get
				Return _roomCategory
			End Get
			Set
				If Equals( Value, _roomCategory ) Then Return
				_roomCategory = value
				NotifyOfPropertyChange( Function() RoomCategory )
			End Set
		End Property

		Public Property RoomNote As String
			Get
				Return _roomNote
			End Get
			Set
				If String.Equals( Value, _roomNote ) Then Return
				_roomNote = value
				NotifyOfPropertyChange( Function() RoomNote )
			End Set
		End Property

		Public ReadOnly Property RoomState As Integer
			Get
				Return 0
			End Get
		End Property

		' ReSharper disable once CollectionNeverUpdated.Global
		' ReSharper disable once UnassignedGetOnlyAutoProperty
		Public ReadOnly Property RoomCategories As IObservableCollection(Of RoomCategoryModel)

		' Exit
		Private Sub ResetValues()
			RoomName = String.Empty
			RoomCategory = RoomCategories.FirstOrDefault()
			RoomNote = String.Empty
		End Sub

		Public Async Sub PreviewExit()
			If CheckForPendingChanges()
				Dim result = Await ConfirmExit()

				If Equals( result, 1 )
					PreviewSave()
					Return
				ElseIf Equals( result, 2 )
					Return
				End If
			End If

			[Exit]()
		End Sub

		Private Sub [Exit]()
			ResetValues()
			ParentWorkspace.NavigateToScreenRoomsList()
		End Sub

		Private Function CheckForPendingChanges()
			Return (Not String.IsNullOrWhiteSpace( RoomName )) Or
			       (Not String.IsNullOrWhiteSpace( RoomNote ))
		End Function

		Private Async Function ConfirmExit() As Task(Of Integer)
			' show dialog
			Dim dialog = New ThreeButtonDialog( "Thoát mà không lưu các thay đổi?",
			                                    "THOÁT",
			                                    "LƯU & THOÁT",
			                                    "HỦY",
			                                    False,
			                                    True,
			                                    False )
			Dim result = Await DialogHost.Show( dialog, "window" )

			If String.Equals( result, "THOÁT" ) Then Return 0
			If String.Equals( result, "HỦY" ) Then Return 2
			Return 1
		End Function

		' Save
		Public Sub PreviewSave()
			If Not ValidateData() Then Return
			Save()
		End Sub

		Private Sub Save()
			' try update
			IoC.Get(Of IMainWindow).ShowStaticWindowDialog( New LoadingDialog() )
			' TODO: call insert here
			IoC.Get(Of IMainWindow).CloseStaticWindowDialog()

			[Exit]()
		End Sub

		Private Function ValidateData() As Boolean
			If String.IsNullOrWhiteSpace( RoomName )
				IoC.Get(Of IMainWindow).ShowStaticBottomNotification( StaticNotificationType.Information, "Vui lòng nhập tên phòng!" )
				Return False
			End If

			Return True
		End Function
	End Class
End Namespace