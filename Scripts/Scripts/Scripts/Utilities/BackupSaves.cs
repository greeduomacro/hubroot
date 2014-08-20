using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Server;
using Server.Commands;

namespace Server.Utilities
{
	public class SavesBackup
	{
		public static void Initialize()
		{
			CommandSystem.Register( "BackupSaves", AccessLevel.Administrator, new CommandEventHandler( BackupSaves_OnCommand ) );
			EventSink.WorldSave += new WorldSaveEventHandler( EventSink_WorldSave );
		}

		[Usage( "BackupSaves" )]
		[Description( "Creates a backup of your current Saves folder contents." )]
		public static void BackupSaves_OnCommand( CommandEventArgs e )
		{
			BackupSaves();
		}

		public static void EventSink_WorldSave( WorldSaveEventArgs e )
		{
			BackupSaves();
		}

		public static void BackupSaves()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Console.Write( "Backing up save files..." );

			try
			{
				string folderTimeStamp = GetBackupTime();

				string saves = Combine( Core.BaseDirectory, String.Format( "Saves/" ) );
				string backupPath = Combine( Core.BaseDirectory, String.Format( "Backups/{0}/", GetBackupTime() ) );

				DirectoryInfo dir = new DirectoryInfo( backupPath );

				if( dir.Exists )
					dir.Delete( true );
				if( !dir.Exists )
					dir.Create();

				DirectoryInfo acct = dir.CreateSubdirectory( "Accounts" );
				DirectoryInfo items = dir.CreateSubdirectory( "Items" );
				DirectoryInfo guilds = dir.CreateSubdirectory( "Guilds" );
				DirectoryInfo mobs = dir.CreateSubdirectory( "Mobiles" );
				DirectoryInfo regions = dir.CreateSubdirectory( "Regions" );
				DirectoryInfo piety = dir.CreateSubdirectory( "Piety" );

				//Begin copying files
				CopyFile( saves, backupPath, "Accounts/accounts.xml" );

				CopyFile( saves, backupPath, "Items/Items.bin" );
				CopyFile( saves, backupPath, "Items/Items.idx" );
				CopyFile( saves, backupPath, "Items/Items.tdb" );

				CopyFile( saves, backupPath, "Guilds/Guilds.bin" );
				CopyFile( saves, backupPath, "Guilds/Guilds.idx" );

				CopyFile( saves, backupPath, "Mobiles/Mobiles.bin" );
				CopyFile( saves, backupPath, "Mobiles/Mobiles.idx" );
				CopyFile( saves, backupPath, "Mobiles/Mobiles.tdb" );

				CopyFile( saves, backupPath, "Regions/Regions.bin" );
				CopyFile( saves, backupPath, "Regions/Regions.idx" );

				CopyFile( saves, backupPath, "Piety/piety.bin" );

				Console.WriteLine( "done ({0:F2} seconds)", (sw.ElapsedMilliseconds / 1000) );
			}
			catch( Exception e )
			{
				Console.WriteLine( "failed" );
				Console.WriteLine( "Exception caught: {0}", e );
			}
			finally { sw.Stop(); }
		}

		public static string Combine( string firstPath, string secondPath )
		{
			if( firstPath == "" )
				return secondPath;

			return Path.Combine( firstPath, secondPath );
		}

		public static string GetBackupTime()
		{
			DateTime check = DateTime.Now;

			return String.Format( "{0}-{1}-{2} - {3}.{4}.{5}", check.Month, check.Day, check.Year, check.Hour, check.Minute, check.Second );
		}

		public static void CreateDirectory( string toCreate )
		{
			if( !Directory.Exists( toCreate ) )
				Directory.CreateDirectory( toCreate );
		}

		public static void CreateDirectory( string firstToCreate, string secondToCreate )
		{
			CreateDirectory( Combine( firstToCreate, secondToCreate ) );
		}

		public static void CopyFile( string fileOrigin, string fileEnd, string directoryPath )
		{
			string originPath = Combine( fileOrigin, directoryPath );
			string backupPath = Combine( fileEnd, directoryPath );

			try
			{
				if( File.Exists( originPath ) )
					File.Copy( originPath, backupPath );
			}
			catch
			{
			}
		}
	}
}
