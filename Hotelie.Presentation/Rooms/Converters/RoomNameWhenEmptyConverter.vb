﻿Imports System.Globalization

Namespace Rooms.Converters
	Public Class RoomNameWhenEmptyConverter
		Implements IValueConverter

		Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
			If String.IsNullOrWhiteSpace(value) 
				Return "(Chưa có tên)"
			End If

			Return value
		End Function

		Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace
