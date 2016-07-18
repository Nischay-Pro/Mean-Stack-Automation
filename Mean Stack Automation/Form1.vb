Imports Microsoft.Win32

Public Class Form1
    Public USerDir As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = "Built by Nischay Pro | " & "Build " & My.Application.Info.Version.ToString
        Dim OScurrentpath As String =
        Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.exe", "Path", "Key does not exist")
        If OScurrentpath <> "Key does not exist" Then
            USerDir = OScurrentpath.Substring(0, 3)
            Me.Show()
            wait(1000)
            SetupSystem()
        Else
            MsgBox("Unable to detect directory. Contact Developer.", MsgBoxStyle.Exclamation, "Error")
            End
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            Process.Start(USerDir & "Program Files\nodejs\node.exe")
        Catch ex As Exception
            MsgBox("Please reinstall Node.", MsgBoxStyle.Exclamation, "NodeJS Corrupt")
        End Try
    End Sub
    Dim MongoDB As Boolean = False
    Dim NodeJS As Boolean = False
    Dim Brackets As Boolean = False
    Dim S7zip As Boolean = False
    Dim AdvancedBrowser As Boolean = False
    Private Sub SetupSystem()
        Label4.Text = "Current Status : Checking System"
        wait(500)
        Label4.Text = "Current Status : Detecting Brackets"
        Dim Bracketsman As String =
Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Brackets.exe", "(Default)", "Key does not exist")
        If Bracketsman <> "Key does not exist" Then
            Brackets = True
        End If
        wait(500)
        Label4.Text = "Current Status : Detecting 7-Zip Archive Manager"
        Dim S7zipsman As String =
Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\7zFM.exe", "(Default)", "Key does not exist")
        If S7zipsman <> "Key does not exist" Then
            S7zip = True
        End If
        wait(500)
        Label4.Text = "Current Status : Detecting MongoDB"
        If My.Computer.FileSystem.DirectoryExists(USerDir & "Program Files\MongoDB\Server\3.2\bin") Then
            MongoDB = True
            GroupBox1.Enabled = True
        End If
        wait(500)
        Label4.Text = "Current Status : Detecting NodeJS"
        If My.Computer.FileSystem.DirectoryExists(USerDir & "Program Files\nodejs") Then
            NodeJS = True
            GroupBox2.Enabled = True
        End If
        wait(500)
        Label4.Text = "Current Status : Detecting HTML5 Support Browser"
        Dim browserKeys As RegistryKey
        browserKeys = Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Clients\StartMenuInternet")
        If browserKeys Is Nothing Then
            browserKeys = Registry.LocalMachine.OpenSubKey("SOFTWARE\Clients\StartMenuInternet")
        End If
        Dim browserNames As String() = browserKeys.GetSubKeyNames()

        For Each item As String In browserNames
            If item.ToLower = "firefox.exe" Or item = "Google Chrome" Or item = "Opera" Or item = "OperaStable" Then
                AdvancedBrowser = True
            End If
        Next
        wait(500)
        Label4.Text = "Current Status : Detection Complete"
        Timer1.Start()
        Label5.Visible = True
        GenerateReport()
    End Sub

    Private Sub GenerateReport()
        Dim count As Integer = 0
        If MongoDB = False Then
            Label5.Text += vbNewLine & "MongoDB was not detected. Please install MongoDB to enable full MEAN Stack Development." & vbNewLine
            count += 1
        End If
        If NodeJS = False Then
            Label5.Text += vbNewLine & "NodeJS was not detected. Please install NodeJS to enable full MEAN Stack Development." & vbNewLine
            count += 1
        End If
        If AdvancedBrowser = False Then
            Label5.Text += vbNewLine & "A browser with decent HTML5 rendering capabilities was not detected. Please install Firefox or Chrome." & vbNewLine
            count += 1
        End If
        If count = 0 Then
            Label5.Text += vbNewLine & "Congratulations your computer has everything ready and installed to support MEAN Stack Development."
        End If
    End Sub

    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            My.Computer.FileSystem.CreateDirectory(USerDir & "data")
            My.Computer.FileSystem.CreateDirectory(USerDir & "data\db")
            MsgBox("Successfully Configured MongoDB.", MsgBoxStyle.Information, "MongoDB Configured")
        Catch ex As Exception
            MsgBox("MongoDB has already been configured to use.", MsgBoxStyle.Exclamation, "MongoDB Configured")
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongod.exe")
        Catch ex As Exception
            MsgBox("Please reinstall MongoDB.", MsgBoxStyle.Exclamation, "MongoDB Corrupt")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            For Each OneProcess As Process In Process.GetProcesses
                If OneProcess.ProcessName = "mongod" Then
                    Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongo.exe")
                End If
            Next
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongod.exe")
            wait(2000)
            Process.Start(USerDir & "Program Files\MongoDB\Server\3.2\bin\mongo.exe")

        Catch ex As Exception
            MsgBox("Please reinstall MongoDB.", MsgBoxStyle.Exclamation, "MongoDB Corrupt")
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MessageBox.Show("Are you sure you want to wipe your MongoDB Database?", "Confirm Wipe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                MsgBox(USerDir)
                My.Computer.FileSystem.DeleteDirectory(USerDir & "data\db", FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.CreateDirectory(USerDir & "data\db")
                MsgBox("Database Wipe Success.", MsgBoxStyle.Information, "MongoDB Wipe Success")
            Catch ex As Exception
                MsgBox("Fatal Error Occured. Error Exception Copied to Clipboard.", MsgBoxStyle.Exclamation, "MongoDB Wipe Failure")
                Clipboard.SetText(ex.Message)
            End Try
        End If
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
                Button2.Enabled = False
                listman.Items.Add("mongo")
            End If
            If OneProcess.ProcessName = "node" Then
                Button8.Enabled = False
                listman.Items.Add("node")
            End If
        Next
        If CheckItem("mongod") = False Then
                Button1.Enabled = True
            End If
            If CheckItem("mongo") = False Then
                Button2.Enabled = True
            End If
            If CheckItem("node") = False Then
                Button8.Enabled = True
            End If
    End Sub
    Private Function CheckItem(ByVal Name As String)
        For Each item As String In listman.Items
            If item = Name Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
