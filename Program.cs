//Written by Jason Bernier
//https://github.com/jasonbernier/7D2DServerAnnouncer
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Program
{
    private static string configFilePath = "config.txt"; // Configuration file path
    private static string messageFilePath; // Path to the message file
    private static TcpClient client;
    private static StreamWriter writer;
    private static bool running = true;

    // Main entry point of the program
    public static void Main()
    {
        Console.WriteLine("7DTD Server Announcer");

        // Load configuration from config.txt
        Config config = LoadConfiguration();

        if (config == null)
        {
            Console.WriteLine("Error loading configuration. Application will now exit.");
            return;
        }

        messageFilePath = config.MessageFileName;

        // Connect to the server
        try
        {
            client = new TcpClient(config.ServerIP, config.ServerPort);
            NetworkStream stream = client.GetStream();
            writer = new StreamWriter(stream);
            writer.WriteLine(config.Password);
            writer.Flush();

            // Handle Ctrl+C to exit gracefully
            Console.CancelKeyPress += HandleCancelKeyPress;

            // Start sending messages based on frequency
            while (running)
            {
                string message = GetRandomMessage();
                bool success = SendSayCommand(writer, message);

                Console.WriteLine($"Message: '{message}' - Status: {(success ? "Sent successfully" : "Failed to send")}");

                // Wait for the specified frequency interval
                Thread.Sleep(config.Frequency * 1000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            if (client != null)
                client.Close();
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    // Function to handle Ctrl+C
    private static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        Console.WriteLine("Ctrl+C detected. Exiting gracefully...");
        running = false;

        // Clean up resources
        if (writer != null)
        {
            writer.Dispose();
        }

        if (client != null)
        {
            client.Close();
        }

        Environment.Exit(0);
    }

    // Function to load configuration from config.txt
    private static Config LoadConfiguration()
    {
        try
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine($"Configuration file '{configFilePath}' not found in current directory.");
                return null;
            }

            string[] lines = File.ReadAllLines(configFilePath);
            Config config = new Config();

            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    switch (key.ToLower())
                    {
                        case "serverip":
                            config.ServerIP = value;
                            break;
                        case "port":
                            if (int.TryParse(value, out int port))
                                config.ServerPort = port;
                            break;
                        case "password":
                            config.Password = value;
                            break;
                        case "messagefilename":
                            config.MessageFileName = value;
                            break;
                        case "frequency":
                            if (int.TryParse(value, out int frequency))
                                config.Frequency = frequency;
                            break;
                    }
                }
            }

            return config;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading configuration from {configFilePath}: {ex.Message}");
            return null;
        }
    }

    // Function to read a random message from the specified file
    private static string GetRandomMessage()
    {
        try
        {
            string[] messages = File.ReadAllLines(messageFilePath);
            Random random = new Random();
            int index = random.Next(0, messages.Length);
            return messages[index].Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading message file '{messageFilePath}': {ex.Message}");
            return string.Empty;
        }
    }

    // Function to send a "say" command with the specified message
    private static bool SendSayCommand(StreamWriter writer, string message)
    {
        try
        {
            writer.WriteLine($"say {message}");
            writer.Flush();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
            return false;
        }
    }

    // Configuration class representing the structure of config.txt
    public class Config
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string Password { get; set; }
        public string MessageFileName { get; set; }
        public int Frequency { get; set; }
    }
}
