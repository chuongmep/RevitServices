using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Windows;

namespace RevitConsoleCommunication;

public partial class Communicate : Window
{
    private NamedPipeServerStream pipeServer;

    public Communicate()
    {
        InitializeComponent();
        // Initialize the pipe server in the constructor with asynchronous mode
        pipeServer = new NamedPipeServerStream("RevitPipe", PipeDirection.InOut, 100, PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);
        Action<NamedPipeServerStream> a = callBack;
        a.BeginInvoke(pipeServer, ar => { }, null);
    }
    private void callBack(NamedPipeServerStream pipe)
    {
        while (true)
        {
            pipe.WaitForConnection();
            StreamReader sr = new StreamReader(pipe);
            btnSend.Content = sr.ReadLine();
            MessageBox.Show(Owner,sr.ReadLine());
            pipe.Disconnect();
        }
    }
}