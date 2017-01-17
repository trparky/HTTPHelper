Imports System.Security.Cryptography.X509Certificates

Public Class Form1
    Private Sub btnGetWebPageData_Click(sender As Object, e As EventArgs) Handles btnGetWebPageData.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.httpHelper()
            httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.
            httpHelper.addGETData("test3", "value3")
            httpHelper.addHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.addHTTPHeader("myheader", "my header contents")
            httpHelper.setHTTPCredentials("test", "test")

            httpHelper.setCustomErrorHandler = Function(ex As Exception, classInstance As HTTPHelper.httpHelper)
                                                   MsgBox(ex.Message)
                                                   Return False
                                               End Function

            If httpHelper.getWebData("https://www.toms-world.org/php/phpinfo.php", strServerResponse) = True Then
                WebBrowser1.DocumentText = strServerResponse
                TextBox1.Text = httpHelper.getHTTPResponseHeaders(True).ToString

                Dim certDetails As X509Certificate2 = httpHelper.getCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If
            End If
        Catch ex As HTTPHelper.httpProtocolException
            ' You can handle httpProtocolExceptions different than normal exceptions with this code.
        Catch ex As Net.WebException
            ' You can handle web exceptions different than normal exceptions with this code.
        Catch ex As Exception
            MsgBox(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Private Sub postDataExample_Click(sender As Object, e As EventArgs) Handles postDataExample.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.httpHelper()
            httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.
            httpHelper.addHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.addHTTPHeader("myheader", "my header contents")
            httpHelper.addPOSTData("test1", "value1")
            httpHelper.addPOSTData("test2", "value2")
            httpHelper.addGETData("test3", "value3")
            httpHelper.addPOSTData("major", "3")
            httpHelper.addPOSTData("minor", "9")
            httpHelper.addPOSTData("build", "6")

            If httpHelper.getWebData("https://www.toms-world.org/httphelper.php", strServerResponse) = True Then
                WebBrowser1.DocumentText = strServerResponse
                TextBox1.Text = httpHelper.getHTTPResponseHeaders().ToString

                Dim certDetails As X509Certificate2 = httpHelper.getCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If

                'For Each strHeaderName As String In httpHelper.getHTTPResponseHeaders
                '    MsgBox(strHeaderName & " = " & httpHelper.getHTTPResponseHeaders.Item(strHeaderName))
                'Next
            End If
        Catch ex As Net.WebException
            MsgBox(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    'Sub updateStatus(ByVal httpHelper As Tom.httpHelper)
    '    ' This gets our percentage of the file that's been downloaded.
    '    Dim percentage As Short = httpHelper.getHTTPDownloadProgressPercentage()

    '    While percentage <> 100 ' We loop while the percentage is not 100.
    '        Label1.Text = String.Format("Downloaded {0} of {1} ({2}%)", httpHelper.getHTTPDownloadLocalFileSize(), httpHelper.getHTTPDownloadRemoteFileSize(), percentage)
    '        ProgressBar1.Value = percentage ' Set the progress bar the percentage value.
    '        Threading.Thread.Sleep(1000) ' Let's sleep for a second.
    '        percentage = httpHelper.getHTTPDownloadProgressPercentage() ' Get the new percentage value.
    '    End While
    'End Sub

    Private Sub btnStopDownload_Click(sender As Object, e As EventArgs) Handles btnStopDownload.Click
        downloadThread.Abort()
        If statusThread IsNot Nothing Then statusThread.Abort()
    End Sub

    Private downloadThread, statusThread As Threading.Thread

    Private Sub btnDownloadFile_Click(sender As Object, e As EventArgs) Handles btnDownloadFile.Click
        ' First we create our httpHelper Class instance.
        Dim httpHelper As New HTTPHelper.httpHelper()
        httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.

        ' Now we set up our download status updating Lambda function that's passed to the Class instance to execute within the memory space of the Class.
        httpHelper.setDownloadStatusUpdateRoutine = Sub(ByVal downloadStatusDetails As HTTPHelper.downloadStatusDetails)
                                                        Label1.Invoke(Sub() Label1.Text = String.Format("Downloaded {0} of {1}", httpHelper.fileSizeToHumanReadableFormat(downloadStatusDetails.localFileSize), httpHelper.fileSizeToHumanReadableFormat(downloadStatusDetails.remoteFileSize)))
                                                        Label2.Invoke(Sub() Label2.Text = downloadStatusDetails.percentageDownloaded.ToString & "%")
                                                        ProgressBar1.Invoke(Sub() ProgressBar1.Value = downloadStatusDetails.percentageDownloaded)
                                                    End Sub

        ' Now we need to create our download thread.
        downloadThread = New Threading.Thread(Sub()
                                                  Dim urlToFileToBeDownloaded As String = "http://releases.ubuntu.com/16.04.1/ubuntu-16.04.1-desktop-amd64.iso"
                                                  Dim pathToDownloadFileTo As String = "S:\ubuntu-16.04.1-desktop-amd64.iso"
                                                  Dim memStream As New IO.MemoryStream

                                                  Try
                                                      btnStopDownload.Enabled = True
                                                      btnDownloadFile.Enabled = False
                                                      btnDownloadFile2.Enabled = False

                                                      ' We use the downloadFile() function which first calls for the URL and then the path to a place on the local file system to save it. This function is why we need multithreading, this will take a long time to do.
                                                      If httpHelper.downloadFile(urlToFileToBeDownloaded, memStream, True) = True Then
                                                          Dim fileStream As New IO.FileStream(pathToDownloadFileTo, IO.FileMode.Create)
                                                          memStream.CopyTo(fileStream)
                                                          memStream.Close()
                                                          memStream.Dispose()
                                                          fileStream.Close()
                                                          fileStream.Dispose()

                                                          btnDownloadFile.Enabled = True
                                                          btnStopDownload.Enabled = False
                                                          MsgBox("Download complete.") ' And tell the user that the download is complete.
                                                      End If

                                                  Catch ex As Net.WebException
                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      MsgBox(ex.Message & " " & ex.StackTrace)
                                                  Catch ex As Threading.ThreadAbortException

                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      If IO.File.Exists(pathToDownloadFileTo) Then IO.File.Delete(pathToDownloadFileTo)
                                                      MsgBox("Download aborted.") ' And tell the user that the download is aborted.
                                                  End Try
                                              End Sub)
        downloadThread.IsBackground = True
        downloadThread.Start() ' Starts our download thread.
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            OpenFileDialog.Title = "Browse for file to upload..."
            OpenFileDialog.FileName = Nothing
            OpenFileDialog.Filter = "Image Files (JPEG, PNG)|*.png;*.jpg;*.jpeg"

            If OpenFileDialog.ShowDialog() = DialogResult.OK Then
                Dim strServerResponse As String = Nothing

                Dim httpHelper As New HTTPHelper.httpHelper()
                httpHelper.setHTTPTimeout = 10
                httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.
                httpHelper.addHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
                httpHelper.addHTTPHeader("myheader", "my header contents")
                httpHelper.addPOSTData("test1", "value1")
                httpHelper.addPOSTData("test2", "value2")
                httpHelper.addGETData("test3", "value3")
                httpHelper.addFileUpload("myfileupload", OpenFileDialog.FileName, Nothing, Nothing)

                If httpHelper.uploadData("https://www.toms-world.org/httphelper.php", strServerResponse) = True Then
                    WebBrowser1.DocumentText = strServerResponse
                    TextBox1.Text = httpHelper.getHTTPResponseHeaders().ToString

                    Dim certDetails As X509Certificate2 = httpHelper.getCertificateDetails(False)
                    If certDetails IsNot Nothing Then
                        TextBox1.Text &= certDetails.ToString
                    End If
                End If
            End If
        Catch ex As Net.WebException
            MsgBox(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.httpHelper()
            httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.
            httpHelper.addGETData("test3", "value3")
            httpHelper.addHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.addHTTPHeader("myheader", "my header contents")

            If httpHelper.getWebData("https://www.toms-world.org/download/restorepointcreatorchangelog.rtf", strServerResponse) = True Then
                WebBrowser1.DocumentText = strServerResponse
                TextBox1.Text = httpHelper.getHTTPResponseHeaders(True).ToString

                Dim certDetails As X509Certificate2 = httpHelper.getCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If
            End If
        Catch ex As HTTPHelper.httpProtocolException
            ' You can handle httpProtocolExceptions different than normal exceptions with this code.
        Catch ex As Net.WebException
            ' You can handle web exceptions different than normal exceptions with this code.
        Catch ex As Exception
            MsgBox(ex.Message & " " & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnDownloadFile2_Click(sender As Object, e As EventArgs) Handles btnDownloadFile2.Click
        ' First we create our httpHelper Class instance.
        Dim httpHelper As New HTTPHelper.httpHelper()
        httpHelper.setUserAgent = "Microsoft .NET" ' Set our User Agent String.

        ' Now we need to create our download thread.
        downloadThread = New Threading.Thread(Sub()
                                                  Dim urlToFileToBeDownloaded As String = "http://releases.ubuntu.com/16.04.1/ubuntu-16.04.1-desktop-amd64.iso"
                                                  Dim pathToDownloadFileTo As String = "S:\ubuntu-16.04.1-desktop-amd64.iso"
                                                  Dim memStream As New IO.MemoryStream

                                                  Try
                                                      btnStopDownload.Enabled = True
                                                      btnDownloadFile.Enabled = False
                                                      btnDownloadFile2.Enabled = False

                                                      ' We use the downloadFile() function which first calls for the URL and then the path to a place on the local file system to save it. This function is why we need multithreading, this will take a long time to do.
                                                      If httpHelper.downloadFile(urlToFileToBeDownloaded, memStream, True) = True Then
                                                          Dim fileStream As New IO.FileStream(pathToDownloadFileTo, IO.FileMode.Create)
                                                          memStream.CopyTo(fileStream)
                                                          memStream.Close()
                                                          memStream.Dispose()
                                                          fileStream.Close()
                                                          fileStream.Dispose()

                                                          btnDownloadFile.Enabled = True
                                                          btnStopDownload.Enabled = False
                                                          MsgBox("Download complete.") ' And tell the user that the download is complete.
                                                      End If

                                                  Catch ex As Net.WebException
                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      MsgBox(ex.Message & " " & ex.StackTrace)
                                                  Catch ex As Threading.ThreadAbortException

                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      If IO.File.Exists(pathToDownloadFileTo) Then IO.File.Delete(pathToDownloadFileTo)
                                                      MsgBox("Download aborted.") ' And tell the user that the download is aborted.
                                                  End Try
                                              End Sub)
        downloadThread.IsBackground = True
        downloadThread.Start() ' Starts our download thread.

        statusThread = New Threading.Thread(Sub()
                                                Dim oldFileSize As ULong = 0
                                                Dim downloadStatusDetails As HTTPHelper.downloadStatusDetails
startAgain:
                                                downloadStatusDetails = httpHelper.getDownloadStatusDetails

                                                If downloadStatusDetails IsNot Nothing Then
                                                    Label1.Invoke(Sub() Label1.Text = String.Format("Downloaded {0} of {1} ({2}/s)", httpHelper.fileSizeToHumanReadableFormat(downloadStatusDetails.localFileSize), httpHelper.fileSizeToHumanReadableFormat(downloadStatusDetails.remoteFileSize), httpHelper.fileSizeToHumanReadableFormat(downloadStatusDetails.localFileSize - oldFileSize)))

                                                    oldFileSize = downloadStatusDetails.localFileSize

                                                    Label2.Invoke(Sub() Label2.Text = downloadStatusDetails.percentageDownloaded.ToString & "%")
                                                    ProgressBar1.Invoke(Sub() ProgressBar1.Value = downloadStatusDetails.percentageDownloaded)
                                                End If

                                                Threading.Thread.Sleep(1000)
                                                GoTo startAgain
                                            End Sub)
        statusThread.IsBackground = True
        statusThread.Start()
    End Sub
End Class
