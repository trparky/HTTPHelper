﻿Imports System.Security.Cryptography.X509Certificates

Public Class Form1
    Private Const urlToDownload As String = "http://releases.ubuntu.com/16.04.2/ubuntu-16.04.2-desktop-amd64.iso"
    Private Const localFilePathToDownloadFileTo As String = "S:\ubuntu-16.04.2-desktop-amd64.iso"

    Private Sub btnGetWebPageData_Click(sender As Object, e As EventArgs) Handles btnGetWebPageData.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.httpHelper() With {.setUserAgent = "Microsoft .NET"} ' Set our User Agent String.
            httpHelper.addGETData("test3", "value3")
            httpHelper.addHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.addHTTPHeader("myheader", "my header contents")
            httpHelper.setHTTPCredentials("test", "test")
            httpHelper.SetURLPreProcessor = Function(strURLInput As String) As String
                                                Debug.WriteLine($"strURLInput = {strURLInput}")
                                                Return strURLInput
                                            End Function

            httpHelper.SetCustomErrorHandler = Function(ex As Exception, classInstance As HTTPHelper.HttpHelper)
                                                   MessageBox.Show(ex.Message)
                                                   Return False
                                               End Function

            If httpHelper.GetWebData(If(String.IsNullOrWhiteSpace(TxtURL.Text), "https://www.toms-world.org/blog", TxtURL.Text), strServerResponse) Then
                WebView21.NavigateToString(strServerResponse)
                TextBox1.Text = httpHelper.GetHTTPResponseHeaders(True).ToString

                Dim certDetails As X509Certificate2 = httpHelper.GetCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If
            End If
        Catch ex As HTTPHelper.HttpProtocolException
            ' You can handle httpProtocolExceptions different than normal exceptions with this code.
        Catch ex As Net.WebException
            ' You can handle web exceptions different than normal exceptions with this code.
        Catch ex As Exception
            MessageBox.Show($"{ex.Message} {ex.StackTrace}")
        End Try
    End Sub

    Private Sub postDataExample_Click(sender As Object, e As EventArgs) Handles postDataExample.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.HttpHelper() With {.SetUserAgent = "Microsoft .NET"} ' Set our User Agent String.
            httpHelper.AddHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.AddHTTPHeader("myheader", "my header contents")
            httpHelper.AddPOSTData("test1", "value1")
            httpHelper.AddPOSTData("test2", "value2")
            httpHelper.AddGETData("test3", "value3")
            httpHelper.AddPOSTData("major", "3")
            httpHelper.AddPOSTData("minor", "9")
            httpHelper.AddPOSTData("build", "6")
            httpHelper.SetURLPreProcessor = Function(strURLInput As String) As String
                                                Debug.WriteLine($"strURLInput = {strURLInput}")
                                                Return strURLInput
                                            End Function

            If httpHelper.GetWebData("https://www.toms-world.org/httphelper.php", strServerResponse) Then
                WebView21.NavigateToString(strServerResponse)
                TextBox1.Text = httpHelper.GetHTTPResponseHeaders().ToString

                Dim certDetails As X509Certificate2 = httpHelper.GetCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If
            End If
        Catch ex As Net.WebException
            MessageBox.Show($"{ex.Message} {ex.StackTrace}")
        End Try
    End Sub

    Private Sub btnStopDownload_Click(sender As Object, e As EventArgs) Handles btnStopDownload.Click
        downloadThread.Abort()
        If statusThread IsNot Nothing Then statusThread.Abort()
    End Sub

    Private downloadThread, statusThread As Threading.Thread

    Private Sub btnDownloadFile_Click(sender As Object, e As EventArgs) Handles btnDownloadFile.Click
        ' First we create our httpHelper Class instance.
        Dim httpHelper As New HTTPHelper.HttpHelper() With {
            .SetUserAgent = "Microsoft .NET", ' Set our User Agent String.
            .EnableMultiThreadedDownloadStatusUpdates = True
        }
        Dim oldFileSize As ULong = 0

        ' Now we set up our download status updating Lambda function that's passed to the Class instance to execute within the memory space of the Class.
        httpHelper.SetDownloadStatusUpdateRoutine = Sub(downloadStatusDetails As HTTPHelper.DownloadStatusDetails)
                                                        If httpHelper.EnableMultiThreadedDownloadStatusUpdates Then
                                                            Label1.Invoke(Sub() Label1.Text = String.Format("Downloaded {0} of {1} ({2}/s)", httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.LocalFileSize), httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.RemoteFileSize), httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.LocalFileSize - oldFileSize)))

                                                            oldFileSize = downloadStatusDetails.LocalFileSize
                                                        Else
                                                            Label1.Invoke(Sub() Label1.Text = String.Format("Downloaded {0} of {1}", httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.LocalFileSize), httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.RemoteFileSize)))
                                                        End If

                                                        Label2.Invoke(Sub() Label2.Text = $"{downloadStatusDetails.PercentageDownloaded}%")
                                                        ProgressBar1.Invoke(Sub() ProgressBar1.Value = downloadStatusDetails.PercentageDownloaded)
                                                    End Sub

        ' Now we need to create our download thread.
        downloadThread = New Threading.Thread(Sub()
                                                  Dim urlToFileToBeDownloaded As String = urlToDownload
                                                  Dim pathToDownloadFileTo As String = localFilePathToDownloadFileTo
                                                  Dim memStream As New IO.MemoryStream

                                                  Try
                                                      btnStopDownload.Enabled = True
                                                      btnDownloadFile.Enabled = False
                                                      btnDownloadFile2.Enabled = False

                                                      ' We use the downloadFile() function which first calls for the URL and then the path to a place on the local file system to save it. This function is why we need multithreading, this will take a long time to do.
                                                      If httpHelper.DownloadFile(urlToFileToBeDownloaded, memStream, True) Then
                                                          Dim fileStream As New IO.FileStream(pathToDownloadFileTo, IO.FileMode.Create)
                                                          memStream.CopyTo(fileStream)
                                                          memStream.Close()
                                                          memStream.Dispose()
                                                          fileStream.Close()
                                                          fileStream.Dispose()

                                                          btnDownloadFile.Enabled = True
                                                          btnStopDownload.Enabled = False
                                                          MessageBox.Show("Download complete.") ' And tell the user that the download is complete.
                                                      End If

                                                  Catch ex As Net.WebException
                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      MessageBox.Show($"{ex.Message} {ex.StackTrace}")
                                                  Catch ex As Threading.ThreadAbortException

                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      If IO.File.Exists(pathToDownloadFileTo) Then IO.File.Delete(pathToDownloadFileTo)
                                                      MessageBox.Show("Download aborted.") ' And tell the user that the download is aborted.
                                                  End Try
                                              End Sub) With {
            .IsBackground = True
        }
        downloadThread.Start() ' Starts our download thread.
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            OpenFileDialog.Title = "Browse for file to upload..."
            OpenFileDialog.FileName = Nothing
            OpenFileDialog.Filter = "Image Files (JPEG, PNG)|*.png;*.jpg;*.jpeg"

            If OpenFileDialog.ShowDialog() = DialogResult.OK Then
                Dim strServerResponse As String = Nothing

                Dim httpHelper As New HTTPHelper.HttpHelper() With {
                    .SetHTTPTimeout = 10,
                    .SetUserAgent = "Microsoft .NET" ' Set our User Agent String.
                }
                httpHelper.AddHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
                httpHelper.AddHTTPHeader("myheader", "my header contents")
                httpHelper.AddPOSTData("test1", "value1")
                httpHelper.AddPOSTData("test2", "value2")
                httpHelper.AddGETData("test3", "value3")
                httpHelper.AddFileUpload("myfileupload", OpenFileDialog.FileName, Nothing, Nothing)

                If httpHelper.UploadData("https://www.toms-world.org/httphelper.php", strServerResponse) Then
                    WebView21.NavigateToString(strServerResponse)
                    TextBox1.Text = httpHelper.GetHTTPResponseHeaders().ToString

                    Dim certDetails As X509Certificate2 = httpHelper.GetCertificateDetails(False)
                    If certDetails IsNot Nothing Then
                        TextBox1.Text &= certDetails.ToString
                    End If
                End If
            End If
        Catch ex As Net.WebException
            MessageBox.Show($"{ex.Message} {ex.StackTrace}")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        WebView21.EnsureCoreWebView2Async(Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim strServerResponse As String = Nothing

            Dim httpHelper As New HTTPHelper.HttpHelper() With {.SetUserAgent = "Microsoft .NET"} ' Set our User Agent String.
            httpHelper.AddGETData("test3", "value3")
            httpHelper.AddHTTPCookie("mycookie", "my cookie contents", "www.toms-world.org", "/")
            httpHelper.AddHTTPHeader("myheader", "my header contents")

            If httpHelper.GetWebData("https://www.toms-world.org/download/restorepointcreatorchangelog.rtf", strServerResponse) Then
                WebView21.NavigateToString(strServerResponse)
                TextBox1.Text = httpHelper.GetHTTPResponseHeaders(True).ToString

                Dim certDetails As X509Certificate2 = httpHelper.GetCertificateDetails(False)
                If certDetails IsNot Nothing Then
                    TextBox1.Text &= certDetails.ToString
                End If
            End If
        Catch ex As HTTPHelper.HttpProtocolException
            ' You can handle httpProtocolExceptions different than normal exceptions with this code.
        Catch ex As Net.WebException
            ' You can handle web exceptions different than normal exceptions with this code.
        Catch ex As Exception
            MessageBox.Show($"{ex.Message} {ex.StackTrace}")
        End Try
    End Sub

    Private Sub btnDownloadFile2_Click(sender As Object, e As EventArgs) Handles btnDownloadFile2.Click
        ' First we create our httpHelper Class instance.
        Dim httpHelper As New HTTPHelper.HttpHelper() With {
            .SetUserAgent = "Microsoft .NET" ' Set our User Agent String.
        }

        ' Now we need to create our download thread.
        downloadThread = New Threading.Thread(Sub()
                                                  Dim urlToFileToBeDownloaded As String = urlToDownload
                                                  Dim pathToDownloadFileTo As String = localFilePathToDownloadFileTo
                                                  Dim memStream As New IO.MemoryStream

                                                  Try
                                                      btnStopDownload.Enabled = True
                                                      btnDownloadFile.Enabled = False
                                                      btnDownloadFile2.Enabled = False

                                                      ' We use the downloadFile() function which first calls for the URL and then the path to a place on the local file system to save it. This function is why we need multithreading, this will take a long time to do.
                                                      If httpHelper.DownloadFile(urlToFileToBeDownloaded, memStream, True) Then
                                                          Dim fileStream As New IO.FileStream(pathToDownloadFileTo, IO.FileMode.Create)
                                                          memStream.CopyTo(fileStream)
                                                          memStream.Close()
                                                          memStream.Dispose()
                                                          fileStream.Close()
                                                          fileStream.Dispose()

                                                          btnDownloadFile.Enabled = True
                                                          btnStopDownload.Enabled = False
                                                          MessageBox.Show("Download complete.") ' And tell the user that the download is complete.
                                                      End If

                                                  Catch ex As Net.WebException
                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      MessageBox.Show($"{ex.Message} {ex.StackTrace}")
                                                  Catch ex As Threading.ThreadAbortException

                                                      btnDownloadFile.Enabled = True
                                                      btnDownloadFile2.Enabled = True
                                                      btnStopDownload.Enabled = False
                                                      If IO.File.Exists(pathToDownloadFileTo) Then IO.File.Delete(pathToDownloadFileTo)
                                                      MessageBox.Show("Download aborted.") ' And tell the user that the download is aborted.
                                                  End Try
                                              End Sub) With {
            .IsBackground = True
        }
        downloadThread.Start() ' Starts our download thread.

        statusThread = New Threading.Thread(Sub()
                                                Dim oldFileSize As ULong = 0
                                                Dim downloadStatusDetails As HTTPHelper.DownloadStatusDetails
startAgain:
                                                downloadStatusDetails = httpHelper.GetDownloadStatusDetails

                                                If downloadStatusDetails IsNot Nothing Then
                                                    Label1.Invoke(Sub() Label1.Text = String.Format("Downloaded {0} of {1} ({2}/s)", httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.LocalFileSize), httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.RemoteFileSize), httpHelper.FileSizeToHumanReadableFormat(downloadStatusDetails.LocalFileSize - oldFileSize)))

                                                    oldFileSize = downloadStatusDetails.LocalFileSize

                                                    Label2.Invoke(Sub() Label2.Text = $"{downloadStatusDetails.PercentageDownloaded}%")
                                                    ProgressBar1.Invoke(Sub() ProgressBar1.Value = downloadStatusDetails.percentageDownloaded)
                                                End If

                                                Threading.Thread.Sleep(1000)
                                                GoTo startAgain
                                            End Sub) With {
            .IsBackground = True
        }
        statusThread.Start()
    End Sub
End Class
