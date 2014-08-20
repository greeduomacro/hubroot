using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace Server.Utilities
{
    public class FTPManager
    {
        #region singleton implementation
        private static FTPManager _instance;

        public static FTPManager GetInstance()
        {
            if( _instance == null )
                _instance = new FTPManager();

            return _instance;
        }
        #endregion

        private string _ftpHost, _ftpUsername, _ftpPassword;
        private string _instructionFile;
        private Queue<UploadRequest> _requestQueue;
        private UploadRequest _currentRequest;
        private int _timeoutDelay;
        private Process _ftpProc;
        private bool _processLocked;

        public FTPManager()
        {
            _requestQueue = new Queue<UploadRequest>();
        }

        /// <summary>
        /// Uploads the specified file to the shard website
        /// </summary>
        /// <param name="fileName">the absolute or relative path of the file to be uploaded</param>
        /// <param name="uploadPath">the relative path on the web server to upload the file to</param>
        /// <param name="asciiFormat">if true, upload in ASCII format; if false, upload in binary format</param>
        /// <param name="deleteAfterUpload">if true, deletes the given file after uploading</param>
        public void UploadFile( string fileName, string uploadPath, bool asciiFormat, bool deleteAfterUpload )
        {
            if( !CollectRegistrySettings(out _ftpHost, out _ftpUsername, out _ftpPassword) )
            {
                ExceptionManager.LogException("FTPManager", new Exception("Unable to collect the necessary registry settings to upload " + fileName));
            }
            else if( !File.Exists(fileName) )
            {
                ExceptionManager.LogException("FTPManager", new FileNotFoundException("The specified file was not found.", fileName));
            }
            else
            {
                _currentRequest = new UploadRequest(fileName, uploadPath, asciiFormat, deleteAfterUpload);
                _instructionFile = DateTime.Now.Ticks.ToString() + ".ftp";

                if( _processLocked || File.Exists(_instructionFile) )
                    _requestQueue.Enqueue(_currentRequest);
                else
                {
                    try
                    {
                        using( StreamWriter writer = new StreamWriter(_instructionFile) )
                        {
                            writer.AutoFlush = true;

                            writer.WriteLine("open {0}", _ftpHost);
                            writer.WriteLine(_ftpUsername);
                            writer.WriteLine(_ftpPassword);
                            writer.WriteLine("cd {0}", _currentRequest.UploadPath);
                            writer.WriteLine(_currentRequest.ASCIIFormat ? "ascii" : "binary");
                            writer.WriteLine("put \"{0}\"", _currentRequest.FileName);
                            writer.WriteLine("close");
                            writer.Write("quit");

                            writer.Close();
                        }

                        _timeoutDelay = (IsArchivePackage(fileName) ? 180 : 60);

                        _ftpProc = new Process();
                        _ftpProc.EnableRaisingEvents = true;
                        _ftpProc.Exited += new EventHandler(delegate( object sender, EventArgs args ) { CleanUp(); });
                        _ftpProc.StartInfo = new ProcessStartInfo("ftp", String.Format("-v -i -s:\"{0}\"", _instructionFile));
                        _ftpProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                        _processLocked = true;
                        _ftpProc.Start();

                        int elapsedTime = 0;
                        while( _processLocked )
                        {
                            elapsedTime += 100;

                            if( elapsedTime >= (_timeoutDelay * 1000) )
                            {
                                CleanUp();
                                break;
                            }

                            Thread.Sleep(100);
                        }
                    }
                    catch( IOException ioe )
                    {
                        ExceptionManager.LogException("FTPManager.UploadFile(): Unable to upload file \"" + fileName + "\":\n", ioe);
                        CleanUp();
                    }
                    catch( Exception e )
                    {
                        ExceptionManager.LogException("FTPManager.UploadFile(): Unable to upload file \"" + fileName + "\":\n", e);
                        CleanUp();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the server name and credentials from the system registry
        /// </summary>
        /// <param name="hostname">hostname of the web server</param>
        /// <param name="username">username credential to login with</param>
        /// <param name="password">password credential to login with (unencrypted)</param>
        /// <returns>true if the information was found in the registry, false if not</returns>
        private bool CollectRegistrySettings( out string hostname, out string username, out string password )
        {
            RegistryKey HKCU = Registry.CurrentUser.OpenSubKey("SOFTWARE", false);
            RegistryKey parent = HKCU.OpenSubKey("UOServer", false);

            hostname = username = password = "";

            if( parent != null )
            {
                RegistryKey key = parent.OpenSubKey("FTPManager", false);

                if( key != null )
                {
                    hostname = (string)key.GetValue("Hostname");
                    username = (string)key.GetValue("Username");
                    password = (string)key.GetValue("Password");
                }
            }

            if( hostname != "" && username != "" )
                return true;

            return false;
        }

        #region -void CleanUp()
        /// <summary>
        /// Cleans up the instruction file and, optionally, the local copy of the uploaded file.
        /// </summary>
        private void CleanUp()
        {
            try
            {
                if( _ftpProc != null && !_ftpProc.HasExited )
                    _ftpProc.Kill();

                if( File.Exists(_instructionFile) )
                    File.Delete(_instructionFile);

                _processLocked = false;

                if( _currentRequest.DeleteAfterUpload && File.Exists(_currentRequest.FileName) )
                    File.Delete(_currentRequest.FileName);
            }
            catch( Exception e )
            {
                ExceptionManager.LogException("FTPManager.CleanUp(): Unable to upload file \"" + _currentRequest.FileName + "\":\n", e);
            }

            ProcessQueue();
        }
        #endregion

        #region -void ProcessQueue()
        /// <summary>
        /// Processes the next item in the <code>_requestQueue</code>
        /// </summary>
        private void ProcessQueue()
        {
            if( _requestQueue == null || _requestQueue.Count <= 0 )
                return;

            UploadRequest request = _requestQueue.Dequeue();
            UploadFile(request.FileName, request.UploadPath, request.ASCIIFormat, request.DeleteAfterUpload);
        }
        #endregion

        /// <summary>
        /// Determines if the given file is an archive of some kind
        /// </summary>
        private bool IsArchivePackage( string file )
        {
            if( !File.Exists(file) )
                return false;

            if( (File.GetAttributes(file) & FileAttributes.Archive) != 0 )
                return true;

            string[] archiveExtensions = new string[] { ".zip", ".rar", ".gzip", ".gz", ".tar", ".7z", ".iso", ".img", ".cab", ".tar" };
            bool hasExt = false;

            for( int i = 0; !hasExt && i < archiveExtensions.Length; i++ )
                if( file.EndsWith(archiveExtensions[i], StringComparison.OrdinalIgnoreCase) )
                    hasExt = true;

            return hasExt;
        }

        private class UploadRequest
        {
            public string FileName;
            public string UploadPath;
            public bool ASCIIFormat;
            public bool DeleteAfterUpload;

            public UploadRequest( string filename, string uploadPath, bool ascii, bool deleteAfterUpload )
            {
                FileName = filename;
                UploadPath = uploadPath;
                ASCIIFormat = ascii;
                DeleteAfterUpload = deleteAfterUpload;
            }
        }
    }
}