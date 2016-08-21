Imports System.IO

Public Class insight
    Public currentprojectpath, USerDir As String
    Private Sub insight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.FileExists(currentprojectpath & "\package.json") Then
            If My.Computer.FileSystem.FileExists(currentprojectpath & "\node_modules") Then
                Button3.Enabled = False
                Button3.Text = "Node Configured"
            End If
        Else
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start("cmd", String.Format("/k {0} & {1}", "cd /d " & currentprojectpath, "npm init"))
        Button3.Enabled = False
        Button3.Text = "Node Configured"
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form1.Show()
        Me.Close()
    End Sub
    Dim listman As New ListBox
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        listman.Items.Clear()

        For Each OneProcess As Process In Process.GetProcesses
            If OneProcess.ProcessName = "mongod" Then
                Button1.Enabled = False
                listman.Items.Add("mongod")
            End If
            If OneProcess.ProcessName = "mongo" Then
                Button4.Enabled = False
                listman.Items.Add("mongo")
            End If
        Next
        If CheckItem("mongod") = False Then
            Button1.Enabled = True
        End If
        If CheckItem("mongo") = False Then
            Button4.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongod.exe")
        Catch ex As Exception
            MsgBox("Please reinstall MongoDB.", MsgBoxStyle.Exclamation, "MongoDB Corrupt")
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            For Each OneProcess As Process In Process.GetProcesses
                If OneProcess.ProcessName = "mongod" Then
                    Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongo.exe")
                    Exit Sub
                End If
            Next
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongod.exe")
            wait(2000)
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongo.exe")

        Catch ex As Exception
            MsgBox("Please reinstall MongoDB.", MsgBoxStyle.Exclamation, "MongoDB Corrupt")
        End Try
    End Sub

    Private Function CheckItem(ByVal Name As String)
        For Each item As String In listman.Items
            If item = Name Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub
End Class