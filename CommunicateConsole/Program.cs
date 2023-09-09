using System;
using System.IO;
using System.IO.Pipes;

namespace CommunicateConsole;

class Program
{
    static void Main()
    {
        try
        {
            // Set up a named pipe client to communicate with Revit
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "RevitPipe", PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                Console.WriteLine("Connecting to Revit add-in...");
                pipeClient.Connect();

                // Send the "Hello" command to the add-in
                using (StreamWriter writer = new StreamWriter(pipeClient))
                {
                    writer.WriteLine("Hello Revit API, I love you!");
                    writer.Flush();
                    writer.Close();
                }
                // Read and print the response from the add-in
                using (StreamReader reader = new StreamReader(pipeClient))
                {
                    string? response = reader.ReadLine();
                    Console.WriteLine("Response from Revit: " + response);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}