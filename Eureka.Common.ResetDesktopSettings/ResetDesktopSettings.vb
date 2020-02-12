Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Serialization
Imports Sprache

Public Class ResetDesktopSettings
    Public Shared ReadOnly Property ActiveUserNumber As Integer
        Get
            Return Sage.Accounting.Application.ActiveUserNumber
        End Get
    End Property

    Public Sub Populate()
        TreeView1.Nodes.Clear()

        Dim pairs = GetFormstateFiles _
            .SelectMany(AddressOf ParseFile) _
            .OrderBy(Function(pair) String.Concat(pair.Value))

        For Each pair In pairs
            AddNode(Nothing, pair.Value, pair.Key)
        Next

        For Each node In TreeView1.Nodes
            node.Expand
        Next

        Dim anyChecked As Boolean = False
        For Each root As FormStateFileTreeNode In TreeView1.Nodes
            anyChecked = anyChecked Or root.GetAllChildren.Any(Function(child) child.Checked)
        Next

        ExportButton.Enabled = anyChecked
        ResetButton.Enabled = anyChecked
    End Sub

    Public Shared Function GetFormstateFiles() As IEnumerable(Of FileInfo)
        Dim isolatedStoragePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\IsolatedStorage\"
        Dim isolatedStorageDirectory = New DirectoryInfo(isolatedStoragePath)

        Return isolatedStorageDirectory _
            .GetAllDirectories _
            .SelectMany(Function(dir) dir.GetFiles) _
            .Where(Function(file) file.Name.StartsWith(ActiveUserNumber.ToString) And file.Extension = ".formstate")
    End Function

    Sub AddNode(root As FormStateFileTreeNode, keys As IEnumerable(Of String), value As FileInfo)
        Select Case keys.Count
            Case 0
                Throw New Exception
            Case 1
                If Not root.Nodes.ContainsKey(keys.First) Then
                    root.Nodes.Add(New FormStateFileTreeNode(keys.First, value))
                End If
            Case Else
                Dim child As FormStateFileTreeNode = Nothing
                If IsNothing(root) Then
                    Dim existingNode = TreeView1.Nodes.Cast(Of FormStateFileTreeNode).FirstOrDefault(Function(node) node.Text = keys.First)
                    If IsNothing(existingNode) Then
                        child = New FormStateFileTreeNode(keys.First)
                        TreeView1.Nodes.Add(child)
                    Else
                        child = existingNode
                    End If
                Else
                    Dim existingNode = root.Nodes.Cast(Of FormStateFileTreeNode).FirstOrDefault(Function(node) node.Text = keys.First)
                    If IsNothing(existingNode) Then
                        child = New FormStateFileTreeNode(keys.First)
                        root.Nodes.Add(child)
                    Else
                        child = existingNode
                    End If
                End If
                AddNode(child, keys.Skip(1), value)
        End Select
    End Sub

    Private Sub ResetDesktopSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Populate()
    End Sub

    Private Sub TreeView_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCheck
        If e.Action <> TreeViewAction.Unknown Then
            Dim anyChecked As Boolean = False
            For Each root As FormStateFileTreeNode In TreeView1.Nodes
                UpdateChecked(root)
                anyChecked = anyChecked Or root.GetAllChildren.Any(Function(child) child.Checked)
            Next

            ExportButton.Enabled = anyChecked
            ResetButton.Enabled = anyChecked
        End If
    End Sub

    Private Sub UpdateChecked(root As FormStateFileTreeNode)
        For Each child As FormStateFileTreeNode In root.Nodes
            UpdateChecked(child)
        Next

        If root.Nodes.Count > 0 Then
            root.Checked = root.GetAllChildren.All(Function(child) child.Checked)
        End If
    End Sub

    Private Sub TreeView_BeforeCheck(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView1.BeforeCheck
        If e.Action <> TreeViewAction.Unknown Then
            TreeView1.SelectedNode = e.Node
            For Each child In CType(e.Node, FormStateFileTreeNode).GetAllChildren
                child.Checked = Not e.Node.Checked
            Next
        End If
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        For Each node In TreeView1.Nodes.Cast(Of FormStateFileTreeNode).SelectMany(Function(root) root.GetAllChildren)
            If node.Checked And node.IsLeaf Then
                node.File.Delete()
            End If
        Next

        Populate()
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub ImportButton_Click(sender As Object, e As EventArgs) Handles ImportButton.Click
        Dim openFileDialog As New OpenFileDialog() With {
            .InitialDirectory = BackupsDirectoryPath,
            .Filter = "Backup Files (*.backup)|*.backup"
        }

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Import(openFileDialog.FileName)
        End If

        Populate()
    End Sub

    Private Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click
        Dim selectedFiles = TreeView1 _
            .Nodes _
            .Cast(Of FormStateFileTreeNode) _
            .SelectMany(Function(root) root.GetAllChildren) _
            .Where(Function(node) node.Checked And node.IsLeaf) _
            .Select(Function(node) node.File)

        Dim settingsBackup = Settings.FromFiles(selectedFiles.ToArray)
        settingsBackup.Export()
    End Sub
End Class

Module Formatting
    Private ReadOnly _substitutions As New Dictionary(Of String, String) From {
        {"S O P", "Sales Order Processing"},
        {"P O P", "Purchase Order Processing"},
        {"W O P", "Work Order Processing"},
        {"A A P3", "AAP3"},
        {"B O M", "Bill of Materials"},
        {"Bom ", "Bill of Materials "},
        {"Op ", "Operation "},
        {"W Order", "Work Order"},
        {"Desk Top", "Desktop"},
        {" And ", " and "},
        {" Or ", " or "},
        {" Of ", " of "},
        {" By ", " by "},
        {" To ", " to "}
    }

    Public Function MakeSubstitutions(s As String) As String
        Dim result As String = s
        For Each substitution In _substitutions
            Dim pattern As New Regex(substitution.Key)
            result = pattern.Replace(result, substitution.Value)
        Next
        Return result
    End Function
End Module

Module Parsing
    Private ReadOnly _formNameParser As Parser(Of IEnumerable(Of String))

    Sub New()
        Dim activeUserNumberPrefixParser As Parser(Of String) =
            Parse.Text(Parse.String($"{ResetDesktopSettings.ActiveUserNumber}_"))

        Dim formStateExtensionParser As Parser(Of String) =
            Parse.Text(Parse.String(".formstate"))

        Dim pascalCaseWordParser As Parser(Of String) =
            From firstUppercaseCharacter In Parse.Upper
            From remainingLowercaseCharactersOrDigits In Parse.Lower.Or(Parse.Digit).Many
            Select firstUppercaseCharacter + String.Concat(remainingLowercaseCharactersOrDigits)

        Dim pascalCaseNameParser As Parser(Of IEnumerable(Of IEnumerable(Of String))) =
            Parse.DelimitedBy(pascalCaseWordParser.AtLeastOnce, Parse.Char("."))

        Dim desktopListFormNameParser As Parser(Of IEnumerable(Of IEnumerable(Of String))) =
            From prefix In Parse.String("Sage.Desktop.Lists.")
            From name In pascalCaseNameParser
            From suffix In Parse.String("_desktop")
            Select Enumerable.Concat(New IEnumerable(Of String)() {New String() {"Sage"}.AsEnumerable, New String() {"Desktop", "Lists"}.AsEnumerable}.AsEnumerable, name)

        Dim mmsFormNameParser As Parser(Of IEnumerable(Of IEnumerable(Of String))) =
            From prefix In Parse.String("Sage.MMS.")
            From name In pascalCaseNameParser
            Select Enumerable.Concat(New IEnumerable(Of String)() {New String() {"Sage"}.AsEnumerable}.AsEnumerable, name)

        Dim genericFormNameParser As Parser(Of IEnumerable(Of IEnumerable(Of String))) =
            From name In pascalCaseNameParser
            From desktopSuffix In Parse.Optional(Parse.String("_desktop"))
            Select name

        _formNameParser =
            From activeUserNumberPrefix In activeUserNumberPrefixParser
            From formName In desktopListFormNameParser.Or(mmsFormNameParser).Or(genericFormNameParser)
            From formStateExtension In formStateExtensionParser
            Select formName.Select(Function(name) String.Join(" ", name)).Select(AddressOf MakeSubstitutions)
    End Sub

    Function ParseFile(file As FileInfo) As IEnumerable(Of KeyValuePair(Of FileInfo, IEnumerable(Of String)))
        Dim result = _formNameParser.TryParse(file.Name)
        If result.WasSuccessful Then
            Return New KeyValuePair(Of FileInfo, IEnumerable(Of String))() {New KeyValuePair(Of FileInfo, IEnumerable(Of String))(file, result.Value)}.AsEnumerable
        Else
            Return Enumerable.Empty(Of KeyValuePair(Of FileInfo, IEnumerable(Of String)))
        End If
    End Function
End Module

Public Module Backup
    Public Class Settings
        Public Class FormState
            Public FullName As String
            Public InnerText As String
        End Class

        Public Timestamp As Date
        Public FormStates As New List(Of FormState)

        Public Shared Function FromFiles(ParamArray files As FileInfo()) As Settings
            Dim settings As New Settings With {.Timestamp = Now}
            For Each file In files
                settings.FormStates.Add(New FormState With {.FullName = String.Concat(file.Name.SkipWhile(Function(c) Char.IsDigit(c))), .InnerText = IO.File.ReadAllText(file.FullName)})
            Next

            Return settings
        End Function

        Public Sub Export()
            Dim serializer As New XmlSerializer(GetType(Settings))
            Dim backupsDirectory = Directory.CreateDirectory(BackupsDirectoryPath)
            Dim fileName = $"{backupsDirectory.FullName}\Desktop Settings {Now.ToString("dd.MM.yyyy HH.mm.ss")}.backup"

            Using fileStream = File.Create(fileName, 1024)
                serializer.Serialize(fileStream, Me)
            End Using
        End Sub
    End Class

    Public ReadOnly Property BackupsDirectoryPath As String
        Get
            Return $"{Sage.Accounting.Application.LogonPath}\DesktopBackups"
        End Get
    End Property

    Public Sub Import(path As String)
        Dim settings As Settings = Nothing
        Using fileStream = File.OpenRead(path)
            Dim serializer As New XmlSerializer(GetType(Settings))
            settings = serializer.Deserialize(fileStream)
        End Using

        If Not IsNothing(settings) Then
            Dim restoreDirectories = GetRestoreDirectories()
            If Not restoreDirectories.Any Then
                Dim folderBrowserDialog As New FolderBrowserDialog With {
                    .RootFolder = Environment.SpecialFolder.ApplicationData,
                    .SelectedPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\IsolatedStorage\"
                }
                If folderBrowserDialog.ShowDialog = DialogResult.OK Then
                    Console.WriteLine($"got dir {folderBrowserDialog.SelectedPath}")
                    restoreDirectories = New DirectoryInfo() {New DirectoryInfo(folderBrowserDialog.SelectedPath)}
                    If Not restoreDirectories.Any Then
                        Return
                    End If
                Else
                    Return
                End If
            End If

            For Each directory In restoreDirectories
                Console.WriteLine($"writing to {directory.FullName}")
                For Each formState In settings.FormStates
                    Console.WriteLine($"writing {formState.FullName}")
                    Using fileStream = File.Create($"{directory.FullName}\{ResetDesktopSettings.ActiveUserNumber}{formState.FullName}")
                        Using writer = New StreamWriter(fileStream)
                            writer.Write(formState.InnerText)
                        End Using
                    End Using
                Next
            Next
        End If
    End Sub

    Function GetRestoreDirectories() As IEnumerable(Of DirectoryInfo)
        Return ResetDesktopSettings _
            .GetFormstateFiles _
            .Select(Function(file) file.Directory) _
            .DistinctBy(Function(dir) dir.FullName)
    End Function
End Module