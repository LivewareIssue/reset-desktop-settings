Imports System.IO

Public Class FormStateFileTreeView
    Inherits TreeView

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H203 And CheckBoxes Then
            Dim localPosition = PointToClient(Cursor.Position)
            Dim hitTestInfo = HitTest(localPosition)
            If hitTestInfo.Location = TreeViewHitTestLocations.StateImage Then
                m.Msg = &H201
            End If
        End If

        MyBase.WndProc(m)
    End Sub
End Class

Public Class FormStateFileTreeNode
    Inherits TreeNode

    Public ReadOnly File As FileInfo

    Public Sub New(text As String)
        MyBase.New(text)
        Me.File = Nothing
    End Sub

    Public Sub New(text As String, file As FileInfo)
        MyBase.New(text)
        Me.File = file
    End Sub

    Public ReadOnly Property IsLeaf As Boolean
        Get
            Return Not IsNothing(File)
        End Get
    End Property
End Class
