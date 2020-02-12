<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResetDesktopSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ResetDesktopSettings))
        Me.ResetButton = New Sage.Common.Controls.Button()
        Me.CloseButton = New Sage.Common.Controls.Button()
        Me.ImportButton = New Sage.Common.Controls.Button()
        Me.ExportButton = New Sage.Common.Controls.Button()
        Me.TreeView1 = New Eureka.Common.ResetDesktopSettings.FormStateFileTreeView()
        Me.SuspendLayout()
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ResetButton.Enabled = False
        Me.ResetButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResetButton.Location = New System.Drawing.Point(12, 583)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(75, 28)
        Me.ResetButton.TabIndex = 1
        Me.ResetButton.Text = "Reset"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseButton.Location = New System.Drawing.Point(499, 583)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 28)
        Me.CloseButton.TabIndex = 2
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'ImportButton
        '
        Me.ImportButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ImportButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImportButton.Location = New System.Drawing.Point(93, 583)
        Me.ImportButton.Name = "ImportButton"
        Me.ImportButton.Size = New System.Drawing.Size(75, 28)
        Me.ImportButton.TabIndex = 3
        Me.ImportButton.Text = "Import"
        Me.ImportButton.UseVisualStyleBackColor = True
        '
        'ExportButton
        '
        Me.ExportButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ExportButton.Enabled = False
        Me.ExportButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExportButton.Location = New System.Drawing.Point(174, 583)
        Me.ExportButton.Name = "ExportButton"
        Me.ExportButton.Size = New System.Drawing.Size(75, 28)
        Me.ExportButton.TabIndex = 4
        Me.ExportButton.Text = "Export"
        Me.ExportButton.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.CheckBoxes = True
        Me.TreeView1.Location = New System.Drawing.Point(12, 12)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(562, 558)
        Me.TreeView1.TabIndex = 0
        '
        'ResetDesktopSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(586, 623)
        Me.Controls.Add(Me.ExportButton)
        Me.Controls.Add(Me.ImportButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.TreeView1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ResetDesktopSettings"
        Me.Text = "Reset Desktop Settings"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ResetButton As Sage.Common.Controls.Button
    Friend WithEvents CloseButton As Sage.Common.Controls.Button
    Friend WithEvents ImportButton As Sage.Common.Controls.Button
    Friend WithEvents ExportButton As Sage.Common.Controls.Button
    Friend WithEvents TreeView1 As FormStateFileTreeView
End Class
