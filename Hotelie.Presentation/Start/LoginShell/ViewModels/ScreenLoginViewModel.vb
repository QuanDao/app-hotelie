﻿Imports Caliburn.Micro
Imports Hotelie.Application.Services.Authentication
Imports Hotelie.Application.Services.Persistence
Imports Hotelie.Presentation.Common
Imports Hotelie.Presentation.Common.Controls
Imports Hotelie.Presentation.Start.MainWindow.Models

Namespace Start.LoginShell.ViewModels
	Public Class ScreenLoginViewModel
		' Dependencies
		Private ReadOnly _authentication As IAuthentication

		' Initilization
		Public ReadOnly Property InitialAccount As String

		Public ReadOnly Property InitialPassword As String

		Public Sub New( authentication As IAuthentication )
			_authentication = authentication
			InitialAccount = My.Settings.SavedAccount
			InitialPassword = My.Settings.SavedPassword
		End Sub

		Public Async Sub Login( username As String,
		                        password As String )
			If Not ValidateAccount( username, password ) Then Return

			' login

			Dim err As String
			Try
				' try login
				IoC.Get(Of IMainWindow).ShowStaticWindowDialog( New LoadingDialog() )
				err = (Await _authentication.TryLoginAsync( username, password )).FirstOrDefault()
				IoC.Get(Of IMainWindow).CloseStaticWindowDialog()
			Catch ex As DatabaseConnectionFailedException
				' connection errors
				err = "Mất kết nối máy chủ. Không thể đăng nhập!"
			Catch ex As Exception
				' other errors
				err = ex.Message
			End Try

			If String.IsNullOrEmpty( err )
				' success
				IoC.Get(Of IMainWindow).SwitchShell( "workspace-shell" )
			Else
				' fail
				IoC.Get(Of IMainWindow).ShowStaticNotification( StaticNotificationType.Error, err )
			End If
		End Sub

		Private Function ValidateAccount( username As String,
		                                  password As String ) As Boolean
			' short username
			If username.Length = 0
				IoC.Get(Of IMainWindow).ShowStaticNotification( StaticNotificationType.Information,
				                                                "Nhập tên tài khoản để đăng nhập" )
				Return False
			End If

			' short password
			If password.Length = 0
				IoC.Get(Of IMainWindow).ShowStaticNotification( StaticNotificationType.Information, "Nhập mật khẩu để đăng nhập" )
				Return False
			End If

			Return True
		End Function
	End Class
End Namespace
