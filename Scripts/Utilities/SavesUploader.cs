using System;
using System.Diagnostics;
using System.IO;
using Server;

namespace Server.Utilities
{
    public class SavesUploader
    {
        public const bool Enabled = true;
        public const DayOfWeek UploadDay = DayOfWeek.Sunday;

        [CallPriority(-50)]
        public static void Initialize()
        {
            if( Enabled && DateTime.Now.DayOfWeek == UploadDay && (DateTime.Now.Hour >= 2 && DateTime.Now.Hour <= 8) && Util.IsProductionHost(System.Net.Dns.GetHostName()) )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                try
                {
                    string pkgName = "";

                    Console.Write("Saves: Packaging save files...");
                    pkgName = Util.Compress(new FileInfo("Saves/"), DateTime.Now.ToLongDateString());

                    if( String.IsNullOrEmpty(pkgName) )
                    {
                        throw new Exception();
                    }

                    Console.Write("uploading package...");

                    FTPManager.GetInstance().UploadFile(pkgName, "/saves", false, true);

                    Console.WriteLine("done ({0:F2} second{1})", sw.Elapsed.TotalSeconds, ((int)sw.Elapsed.TotalSeconds == 1 ? "" : "s"));
                }
                catch
                {
                    Console.WriteLine("failed!");
                }
                finally { sw.Stop(); }
            }
        }
    }
}