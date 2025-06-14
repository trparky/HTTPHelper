<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnGetWebPageData = New System.Windows.Forms.Button()
        Me.postDataExample = New System.Windows.Forms.Button()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnDownloadFile = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnUpload = New System.Windows.Forms.Button()
        Me.btnStopDownload = New System.Windows.Forms.Button()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnDownloadFile2 = New System.Windows.Forms.Button()
        Me.TxtURL = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGetWebPageData
        '
        Me.btnGetWebPageData.Location = New System.Drawing.Point(12, 41)
        Me.btnGetWebPageData.Name = "btnGetWebPageData"
        Me.btnGetWebPageData.Size = New System.Drawing.Size(158, 23)
        Me.btnGetWebPageData.TabIndex = 0
        Me.btnGetWebPageData.Text = "Get Web Page Data"
        Me.btnGetWebPageData.UseVisualStyleBackColor = True
        '
        'postDataExample
        '
        Me.postDataExample.Location = New System.Drawing.Point(176, 41)
        Me.postDataExample.Name = "postDataExample"
        Me.postDataExample.Size = New System.Drawing.Size(158, 23)
        Me.postDataExample.TabIndex = 1
        Me.postDataExample.Text = "Post Data to Web Site"
        Me.postDataExample.UseVisualStyleBackColor = True
        '
        'WebView21
        '
        Me.WebView21.AllowExternalDrop = True
        Me.WebView21.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView21.Location = New System.Drawing.Point(15, 122)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(801, 374)
        Me.WebView21.TabIndex = 14
        Me.WebView21.ZoomFactor = 1.0R
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(12, 502)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(804, 186)
        Me.TextBox1.TabIndex = 3
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 70)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(485, 23)
        Me.ProgressBar1.TabIndex = 4
        '
        'btnDownloadFile
        '
        Me.btnDownloadFile.Location = New System.Drawing.Point(558, 12)
        Me.btnDownloadFile.Name = "btnDownloadFile"
        Me.btnDownloadFile.Size = New System.Drawing.Size(108, 23)
        Me.btnDownloadFile.TabIndex = 5
        Me.btnDownloadFile.Text = "Download File"
        Me.btnDownloadFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Label1"
        '
        'btnUpload
        '
        Me.btnUpload.Location = New System.Drawing.Point(340, 41)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(75, 23)
        Me.btnUpload.TabIndex = 7
        Me.btnUpload.Text = "Upload File"
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'btnStopDownload
        '
        Me.btnStopDownload.Enabled = False
        Me.btnStopDownload.Location = New System.Drawing.Point(672, 12)
        Me.btnStopDownload.Name = "btnStopDownload"
        Me.btnStopDownload.Size = New System.Drawing.Size(108, 23)
        Me.btnStopDownload.TabIndex = 8
        Me.btnStopDownload.Text = "Stop Download"
        Me.btnStopDownload.UseVisualStyleBackColor = True
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.FileName = "OpenFileDialog1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(558, 79)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(222, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(458, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Label2"
        '
        'btnDownloadFile2
        '
        Me.btnDownloadFile2.Location = New System.Drawing.Point(558, 41)
        Me.btnDownloadFile2.Name = "btnDownloadFile2"
        Me.btnDownloadFile2.Size = New System.Drawing.Size(222, 23)
        Me.btnDownloadFile2.TabIndex = 11
        Me.btnDownloadFile2.Text = "Multi-Threaded Download File"
        Me.btnDownloadFile2.UseVisualStyleBackColor = True
        '
        'TxtURL
        '
        Me.TxtURL.Location = New System.Drawing.Point(46, 12)
        Me.TxtURL.Name = "TxtURL"
        Me.TxtURL.Size = New System.Drawing.Size(506, 20)
        Me.TxtURL.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "URL"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(828, 700)
        Me.Controls.Add(Me.WebView21)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtURL)
        Me.Controls.Add(Me.btnDownloadFile2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnStopDownload)
        Me.Controls.Add(Me.btnUpload)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDownloadFile)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.postDataExample)
        Me.Controls.Add(Me.btnGetWebPageData)
        Me.Name = "Form1"
        Me.Text = "HTTPHelper Example"
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnGetWebPageData As Button
    Friend WithEvents postDataExample As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents btnDownloadFile As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnUpload As Button
    Friend WithEvents btnStopDownload As Button
    Friend WithEvents OpenFileDialog As OpenFileDialog
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents btnDownloadFile2 As Button
    Friend WithEvents TxtURL As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
End Class
