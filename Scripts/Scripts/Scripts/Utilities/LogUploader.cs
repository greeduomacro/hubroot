using System;
using System.Diagnostics;
using System.IO;
using Server;

namespace Server.Utilities
{
    public class LogUploader
    {
        public const bool Enabled = true;

        [CallPriority(-50)]
        public static void Initialize()
        {
            if( Enabled && (DateTime.Now.Hour >= 2 && DateTime.Now.Hour <= 8) && Util.IsProductionHost(System.Net.Dns.GetHostName()) )
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                try
                {
                    string pkgName = "";

                    Console.Write("Logging: Packaging logs...");
                    pkgName = Util.Compress(new FileInfo("Logs/"), DateTime.Now.ToLongDateString());

                    if( String.IsNullOrEmpty(pkgName) )
                    {
                        throw new Exception();
                    }

                    Console.Write("uploading package...");

                    FTPManager.GetInstance().UploadFile(pkgName, "/logs", false, true);

                    CleanLogDir("Logs");
                    Console.WriteLine("done ({0:F2} second{1})", sw.Elapsed.TotalSeconds, ((int)sw.Elapsed.TotalSeconds == 1 ? "" : "s"));
                }
                catch
                {
                    Console.WriteLine("failed!");
                }
                finally { sw.Stop(); }
            }
        }

        /// <summary>
        /// Recursively deletes log files that have not been written to within the last two days
        /// </summary>
        /// <param name="path">the path to search through</param>
        private static void CleanLogDir( string path )
        {
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            for( int i = 0; i < dirs.Length; i++ )
            {
                CleanLogDir(dirs[i]);
            }

            FileInfo fileInfo = null;
            DateTime lastWrite = DateTime.MinValue;
            DateTime cutoff = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0));

            for( int i = 0; i < files.Length; i++ )
            {
                fileInfo = new FileInfo(files[i]);
                lastWrite = fileInfo.LastWriteTime;

                if( lastWrite < cutoff )
                    File.Delete(files[i]);
            }
        }
    }
}