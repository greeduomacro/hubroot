using System;
using System.IO;
using Server;
using Server.Commands;

namespace Server.Scripts.Commands
{
	public class ProjectGen
	{
		public static void Initialize()
		{
			CommandSystem.Register( "ProjectGen", AccessLevel.Administrator, new CommandEventHandler( ProjectGen_OnCommand ) );
		}

		[Usage( "ProjectGen" )]
		[Description( "Generate project file with all scripts added to it." )]
		public static void ProjectGen_OnCommand( CommandEventArgs e )
		{
			DateTime startTime = DateTime.Now;

			e.Mobile.SendMessage( "Project generation in process. Please wait..." );
			Console.Write( "Project generation in process. Please wait..." );

			using( StreamWriter file = new StreamWriter( ".\\Scripts\\Scripts.csproj" ) )
			{
				#region Project File
				file.WriteLine( "" );
				file.WriteLine( "<VisualStudioProject>" );
				file.WriteLine( "    <CSHARP" );
				file.WriteLine( "        ProjectType = \"Local\"" );
				file.WriteLine( "        ProductVersion = \"7.10.3077\"" );
				file.WriteLine( "        SchemaVersion = \"2.0\"" );
				file.WriteLine( "        ProjectGuid = \"{CAC02510-74B0-412C-BEF8-25610A2B5F67}\"" );
				file.WriteLine( "    >" );
				file.WriteLine( "        <Build>" );
				file.WriteLine( "            <Settings" );
				file.WriteLine( "                ApplicationIcon = \"\"" );
				file.WriteLine( "                AssemblyKeyContainerName = \"\"" );
				file.WriteLine( "                AssemblyName = \"scripts\"" );
				file.WriteLine( "                AssemblyOriginatorKeyFile = \"\"" );
				file.WriteLine( "                DefaultClientScript = \"JScript\"" );
				file.WriteLine( "                DefaultHTMLPageLayout = \"Grid\"" );
				file.WriteLine( "                DefaultTargetSchema = \"IE50\"" );
				file.WriteLine( "                DelaySign = \"false\"" );
				file.WriteLine( "                OutputType = \"Exe\"" );
				file.WriteLine( "                PreBuildEvent = \"\"" );
				file.WriteLine( "                PostBuildEvent = \"\"" );
				file.WriteLine( "                RootNamespace = \"scripts\"" );
				file.WriteLine( "                RunPostBuildEvent = \"OnBuildSuccess\"" );
				file.WriteLine( "                StartupObject = \"\"" );
				file.WriteLine( "            >" );
				file.WriteLine( "                <Config" );
				file.WriteLine( "                    Name = \"Debug\"" );
				file.WriteLine( "                    AllowUnsafeBlocks = \"false\"" );
				file.WriteLine( "                    BaseAddress = \"285212672\"" );
				file.WriteLine( "                    CheckForOverflowUnderflow = \"false\"" );
				file.WriteLine( "                    ConfigurationOverrideFile = \"\"" );
				file.WriteLine( "                    DefineConstants = \"DEBUG;TRACE\"" );
				file.WriteLine( "                    DocumentationFile = \"\"" );
				file.WriteLine( "                    DebugSymbols = \"true\"" );
				file.WriteLine( "                    FileAlignment = \"4096\"" );
				file.WriteLine( "                    IncrementalBuild = \"false\"" );
				file.WriteLine( "                    NoStdLib = \"false\"" );
				file.WriteLine( "                    NoWarn = \"\"" );
				file.WriteLine( "                    Optimize = \"false\"" );
				file.WriteLine( "                    OutputPath = \"bin\\Debug\"" );
				file.WriteLine( "                    RegisterForComInterop = \"false\"" );
				file.WriteLine( "                    RemoveIntegerChecks = \"false\"" );
				file.WriteLine( "                    TreatWarningsAsErrors = \"false\"" );
				file.WriteLine( "                    WarningLevel = \"4\"" );
				file.WriteLine( "                />" );
				file.WriteLine( "                <Config" );
				file.WriteLine( "                    Name = \"Release\"" );
				file.WriteLine( "                    AllowUnsafeBlocks = \"false\"" );
				file.WriteLine( "                    BaseAddress = \"285212672\"" );
				file.WriteLine( "                    CheckForOverflowUnderflow = \"false\"" );
				file.WriteLine( "                    ConfigurationOverrideFile = \"\"" );
				file.WriteLine( "                    DefineConstants = \"TRACE\"" );
				file.WriteLine( "                    DocumentationFile = \"\"" );
				file.WriteLine( "                    DebugSymbols = \"false\"" );
				file.WriteLine( "                    FileAlignment = \"4096\"" );
				file.WriteLine( "                    IncrementalBuild = \"false\"" );
				file.WriteLine( "                    NoStdLib = \"false\"" );
				file.WriteLine( "                    NoWarn = \"\"" );
				file.WriteLine( "                    Optimize = \"true\"" );
				file.WriteLine( "                    OutputPath = \"bin\\Release\"" );
				file.WriteLine( "                    RegisterForComInterop = \"false\"" );
				file.WriteLine( "                    RemoveIntegerChecks = \"false\"" );
				file.WriteLine( "                    TreatWarningsAsErrors = \"false\"" );
				file.WriteLine( "                    WarningLevel = \"4\"" );
				file.WriteLine( "                />" );
				file.WriteLine( "            </Settings>" );
				file.WriteLine( "            <References/>" );
				file.WriteLine( "        </Build>" );
				file.WriteLine( "        <Files>" );
				file.WriteLine( "            <Include>" );

				WriteFilesToProject( file, ".\\Scripts\\" );

				file.WriteLine( "           </Include>" );
				file.WriteLine( "        </Files>" );
				file.WriteLine( "    </CSHARP>" );
				file.WriteLine( "</VisualStudioProject>" );


				#endregion
			}

			e.Mobile.SendMessage( "Process complete in {0:F1} seconds", (DateTime.Now - startTime).TotalSeconds );
			Console.WriteLine( "complete ({0:F1} seconds)", (DateTime.Now - startTime).TotalSeconds );
		}

		public static void WriteFilesToProject( StreamWriter file, string dir )
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo( dir );
				FileSystemInfo[] dirs = di.GetDirectories();
				foreach( DirectoryInfo diNext in dirs )
				{
					WriteFilesToProject( file, dir + diNext + "\\" );

					FileInfo[] fi = diNext.GetFiles( "*.cs" );

					foreach( FileInfo fiTemp in fi )
					{
						string fn = dir + diNext + "\\" + fiTemp.Name;
						file.WriteLine( "                <File" );
						file.WriteLine( "                    RelPath = \"" + fn.Replace( ".\\Scripts\\", "" ) + "\"" );
						file.WriteLine( "                    SubType = \"Code\"" );
						file.WriteLine( "                    BuildAction = \"Compile\"" );
						file.WriteLine( "                />" );
					}
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( "The process failed: {0}", e.ToString() );
			}
		}
	}
}