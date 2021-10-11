using System;
using System.IO;    // Used for streamreader
using System.Net;
using Renci.SshNet;

namespace IpUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("IpUploader");
            string publicIP = MyIP();
            string outputPath = "public-ip.txt";
            Console.WriteLine("Public IP: " + publicIP);

            string strToWrite = "Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n" + "IPv4: " + publicIP + "\n";
            WriteToFile(outputPath, strToWrite);
            Console.WriteLine("Uploading IP to server...");
            SendFileToServer.Send(outputPath);
        }

        public static string MyIP()
        {
            string externalip = new WebClient().DownloadString("https://soltveit.org/pages/myip/");

            return externalip;
        }

        public static void WriteToFile(string path, string toWrite)
        {
            Console.WriteLine("Writing ip to file");
            File.WriteAllText(path, toWrite);
        }

        public static string[] GetConfig(string path)
        {
            string[] config = File.ReadAllLines(path);  // Check README for more info

            return config;
        }

        public static class SendFileToServer
        {
            public static int Send(string fileName)
            {
                string[] config = GetConfig("config.txt");
                var connectionInfo = new ConnectionInfo(config[0], config[1], new PasswordAuthenticationMethod(config[1], config[2]));
                using (var sftp = new SftpClient(connectionInfo))
                {
                    try
                    {
                        sftp.Connect();
                        sftp.ChangeDirectory(config[3]);
                        using (var uplfileStream = File.OpenRead(fileName))
                        {
                            sftp.UploadFile(uplfileStream, fileName, true);
                        }
                        sftp.Disconnect();
                        Console.WriteLine("Done....\nDisconnected...\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return 0;
            }
        }

    }
}
