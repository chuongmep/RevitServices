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
        // Initialize the pipe server in the constructor
        // Initialize the pipe server in the constructor with asynchronous mode
        pipeServer = new NamedPipeServerStream("RevitPipe", PipeDirection.InOut, 100, PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);

        // set event track opening to close the form 
        this.Loaded += Communicate_Loaded;
    }
    // Rest of your code...

    public void Communicate_Loaded(object sender, RoutedEventArgs routedEventArgs)
    {
        try
        {
            // Begin waiting for a connection asynchronously
            pipeServer.BeginWaitForConnection(PipeConnectedCallback, pipeServer);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }

    private void PipeConnectedCallback(IAsyncResult result)
    {
        // Complete the asynchronous operation
        pipeServer.EndWaitForConnection(result);
        // Check if the pipe is connected before reading or writing
        if (pipeServer.IsConnected)
        {
            // Handle the command
            HandleCommand(pipeServer);
        }
        else
        {
            MessageBox.Show("Pipe is not connected!");
            // Handle the case where the pipe is not connected yet
        }
        // Continue listening for the next connection asynchronously)
        // pipeServer.BeginWaitForConnection(PipeConnectedCallback, pipeServer);
    }

    private void HandleCommand(NamedPipeServerStream pipe)
    {
        try
        {
            // Read the command from the console application
            using (StreamReader reader = new StreamReader(pipe))
            {
                string command = reader.ReadLine() ?? string.Empty;

                if (command == "Hello Revit API")
                {
                    // Send a response back to the console
                    using (StreamWriter writer = new StreamWriter(pipe))
                    {
                        writer.WriteLine("Hello Server, I am Revit API");
                        //writer.Flush(); // Important: Flush before responding
                        this.Dispatcher.Invoke(() =>
                        {
                            Debug.WriteLine("Server Said: Hello Revit API");
                            MessageBox.Show(this,"Server Said: Hello Revit API");
                            btnSend.Content = "Server Said: Hello Revit API";
                            writer.Flush();
                        });

                        // writer.Dispose();
                    }
                }
            }
        }
        catch (System.IO.IOException e)
        {
            // ignore
            MessageBox.Show(pipeServer.IsConnected.ToString());
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }

    // private void HandleConnection(NamedPipeServerStream pipe)
    // {
    //     // Read and process data from the client here
    //     // ...
    // }
}