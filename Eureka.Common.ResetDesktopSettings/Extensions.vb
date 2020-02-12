Imports System.IO
Imports System.Runtime.CompilerServices

Module Extensions
    <Extension>
    Public Iterator Function GetAllDirectories(root As DirectoryInfo) As IEnumerable(Of DirectoryInfo)
        Yield root
        For Each subdirectory In root.GetDirectories.SelectMany(AddressOf GetAllDirectories)
            Yield subdirectory
        Next
    End Function

    <Extension>
    Public Function GetAllChildren(node As FormStateFileTreeNode) As IEnumerable(Of FormStateFileTreeNode)
        Return node.Nodes.Cast(Of FormStateFileTreeNode).SelectMany(Function(child) New FormStateFileTreeNode() {child}.AsEnumerable.Concat(child.GetAllChildren))
    End Function

    <Extension>
    Public Iterator Function DistinctBy(Of TSource, TKey)(source As IEnumerable(Of TSource), keySelector As Func(Of TSource, TKey)) As IEnumerable(Of TSource)
        Dim observedKeys As New HashSet(Of TKey)
        For Each element In source
            If observedKeys.Add(keySelector(element)) Then
                Yield element
            End If
        Next
    End Function
End Module
