﻿Imports Hotelie.Application.Services.Persistence

Namespace Services.Authentication
    Public Class Authentication
        Implements IAuthentication

        Private _loggedIn As Boolean

        Private ReadOnly _userRepository As IUserRepository

        Public Property LoggedAccount As Account Implements IAuthentication.LoggedAccount

        Public ReadOnly Property LoggedIn As Boolean Implements IAuthentication.LoggedIn
            Get
                Return _loggedIn
            End Get
        End Property

        ''' <summary>
        ''' Login to Hotelie
        ''' </summary>
        Public Function TryLogin(account As Account) As IEnumerable(Of String) Implements IAuthentication.TryLogin
            Dim errorLog = New List(Of String)

            ' Currently logged in
            If (LoggedIn) 
                errorLog.Add("Đăng nhập lỗi, Hotelie đang được đăng nhập với tài khoản: " + LoggedAccount.Username)
                Return errorLog
            End If

            ' Account currently logged in
            If (account.Username=LoggedAccount.Username) And (account.Password=LoggedAccount.Password)
                errorLog.Add("Tài khoản này đang đăng nhập")
                Return errorLog
            End If

            ' Username is invalid
            Dim loginId = _userRepository.Find(Function(p)(p.Id=account.Username)).FirstOrDefault()
            If (IsNothing(loginId))
                errorLog.Add("Tên tài khoản không tồn tại.")
                Return errorLog
            End If

            ' Password is invalid
            Dim loginUser = _userRepository.Find(Function(p)(p.Id=loginId.Id And p.Password=account.Password)).FirstOrDefault()
            If (IsNothing(loginUser))
                errorLog.Add("Sai mật khẩu")
                Return errorLog
            End If

            ' Logging in
            _loggedIn = True
            LoggedAccount = account

            Return Nothing
        End Function

        ''' <summary>
        ''' Logout current user
        ''' </summary>
        Public Sub Logout() Implements IAuthentication.Logout
            _loggedIn = False
            LoggedAccount = Nothing
        End Sub

        Public Sub New(userRepository As IUserRepository)
            Logout()
            _userRepository = userRepository
        End Sub

       
    End Class
End Namespace