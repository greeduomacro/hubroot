using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;

namespace Server.Utilities
{
    /// <summary>
    /// Group of utility methods for general purpose usage
    /// </summary>
    public sealed class Util
    {
        /// <summary>
        /// Compresses the given file or directory
        /// </summary>
        /// <param name="file">file or directory to compress</param>
        /// <param name="compressedName">name of the compressed file</param>
        /// <returns>path to the compressed file</returns>
        public static string Compress( FileSystemInfo file, string compressedName = null )
        {
            string compressedPath = "";

            using( FileStream fs = new FileStream(file.FullName, FileMode.Open) )
            {
                using( FileStream compressed = File.Create(compressedName ?? file.FullName + ".gz") )
                {
                    using( GZipStream gzip = new GZipStream(compressed, CompressionMode.Compress) )
                    {
                        fs.CopyTo(gzip);
                        gzip.Close();
                    }

                    compressed.Close();
                    compressedPath = compressed.Name;
                }

                fs.Close();
            }

            return compressedPath;
        }

        /// <summary>
        /// Decompresses a gzip file and optionally deletes the original archive afterwards
        /// </summary>
        /// <param name="file">the gzip file to decompress</param>
        /// <param name="deleteOnDecompress">flag that indicates if the archive should be removed after decompression</param>
        public static void Decompress( FileInfo file, bool deleteOnDecompress = true )
        {
            using( FileStream fs = file.OpenRead() )
            {
                using( FileStream decompressed = File.Create(file.FullName.Remove((file.Name.Length - file.Extension.Length))) )
                {
                    using( GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress) )
                    {
                        gzip.CopyTo(decompressed);
                        gzip.Close();
                    }

                    decompressed.Close();
                }

                fs.Close();
            }

            if( deleteOnDecompress )
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="origin">the starting point</param>
        /// <param name="target">the ending point</param>
        /// <returns>an integer of the distance between the origin and target</returns>
        public static int DistanceBetween( IPoint2D origin, IPoint2D target )
        {
            if( origin.X == target.X )
                return Math.Abs((origin.Y - target.Y));

            if( origin.Y == target.Y )
                return Math.Abs((origin.Y - target.Y));

            //pythagorean theorem... dist = sqrt of (x2-x1)^2 + (y2-y1)^2
            return (int)Math.Sqrt(Math.Pow(Math.Abs(target.X - origin.X), 2) + Math.Pow(Math.Abs(target.Y - origin.Y), 2));
        }

        /// <summary>
        /// Formats a System.TimeSpan struct as DAYS:HOURS:MINUTES:SECONDS
        /// </summary>
        /// <param name="ts">the System.TimeSpan to be formatted</param>
        /// <returns>"ts" in the string format DAYS:HOURS:MINUTES:SECONDS</returns>
        public static string FormatLongTimeSpan( TimeSpan ts )
        {
            return String.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", ts.Days, (ts.Hours % 24), (ts.Minutes % 60), (ts.Seconds % 60));
        }

        /// <summary>
        /// Formats a given amount of bytes into a shorter acronym representation (# GB, # MB, etc)
        /// </summary>
        /// <param name="totalBytes">set number of bytes to be formatted</param>
        /// <returns>"totalBytes" represented in gigabytes (GB), megabytes (MB), kilobytes (KB), or bytes (Bytes)</returns>
        public static string FormatByteAmount( long totalBytes )
        {
            if( totalBytes > 1000000000 )
                return String.Format("{0:F1} GB", (double)(totalBytes / 1073741824));

            if( totalBytes > 1000000 )
                return String.Format("{0:F1} MB", (double)(totalBytes / 1048576));

            if( totalBytes > 1000 )
                return String.Format("{0:F1} KB", (double)(totalBytes / 1024));

            return String.Format("{0:F1} Bytes", totalBytes);
        }

        /// <summary>
        /// Finds the deserialization constructor for a given type
        /// </summary>
        public static ConstructorInfo GetDeserializationCtor( string typeName )
        {
            Type type = ScriptCompiler.FindTypeByFullName(typeName);

            if( type == null )
                throw new Exception(String.Format("Error: type '{0}' was not found.", typeName));

            ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(GenericReader) });

            if( ctor == null )
                throw new Exception(String.Format("Error: Deserialization ctor not found on type '{0}'", typeName));

            return ctor;
        }

        /// <summary>
        /// Retrieves the first registered map with the given name (case-insensitive)
        /// </summary>
        /// <param name="name">map name to check for</param>
        /// <returns>instance of the matching <code>Map</code> object, or null if not found</returns>
        public static Map GetMapByName( string name )
        {
            Map foundMap = null;

            for( int i = 0; foundMap == null && i < Map.AllMaps.Count; i++ )
            {
                if( Map.AllMaps[i].Name.ToLower() == name.ToLower() )
                    foundMap = Map.AllMaps[i];
            }

            return foundMap;
        }

        /// <summary>
        /// Determines if the current host machine is identifiable as a production server
        /// </summary>
        /// <param name="hostName">the server hostname to check</param>
        /// <returns>true if the host is a production server</returns>
        public static bool IsProductionHost( string hostName )
        {
            IPHostEntry entry = Dns.GetHostEntry(hostName);
            IPAddress[] ips = entry.AddressList;

            IPAddress[] validHosts = new IPAddress[]
				{
					IPAddress.Parse( "173.1.127.235" )
				};

            for( int i = 0; i < ips.Length; i++ )
            {
                if( Array.IndexOf<IPAddress>(validHosts, ips[i]) > -1 )
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Inserts spaces before each capital letter in a given string (except the first letter)
        /// </summary>
        /// <param name="toSplit">the string containing joined words to split</param>
        /// <returns>"toSplit" as a string containing the separated words and spaces between</returns>
        public static string SplitString( string toSplit )
        {
            string val = String.Copy(toSplit);

            for( int i = 1; i < toSplit.Length; i++ )
            {
                if( Char.IsUpper(toSplit[i]) && !Char.IsWhiteSpace(toSplit[i - 1]) )
                    val = toSplit.Insert(i, " ");
            }

            return val.Trim();
        }
    }
}