﻿Namespace Users
    Public Interface IUserModel
        ReadOnly Property CouldConfigRoom As Boolean
        ReadOnly Property CouldAddLease As Boolean
        ReadOnly Property CouldEditLease As Boolean
        ReadOnly Property CouldRemoveLease As Boolean
        ReadOnly Property CouldManageUser As Boolean
        ReadOnly Property CouldEditRule As Boolean
    End Interface
End NameSpace