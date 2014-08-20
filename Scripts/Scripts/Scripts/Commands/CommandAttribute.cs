using System;
using System.Diagnostics;
using System.Reflection;
using Server;
using Server.Commands;

namespace Server.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        [CallPriority(-100)]
        public static void Initialize()
        {
            Console.Write("CommandAttribute.Initialize(): Loading commands...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int count = 0;

            for( int i = 0; i < ScriptCompiler.Assemblies.Length; i++ )
            {
                Type[] types = ScriptCompiler.Assemblies[i].GetTypes();

                for( int j = 0; j < types.Length; j++ )
                {
                    MethodInfo[] methods = types[j].GetMethods();

                    for( int k = 0; k < methods.Length; k++ )
                    {
                        ParameterInfo[] paramInfo = methods[k].GetParameters();

                        if( paramInfo.Length != 1 || paramInfo[0].ParameterType != typeof(CommandEventArgs) )
                            continue;

                        object[] attrb = methods[k].GetCustomAttributes(typeof(CommandAttribute), false);

                        if( attrb.Length == 1 )
                        {
                            CommandAttribute cmdAttrib = attrb[0] as CommandAttribute;
                            CommandSystem.Register(cmdAttrib.Command, cmdAttrib.AccessLevel, (CommandEventHandler)Delegate.CreateDelegate(typeof(CommandEventHandler), methods[k]));

                            count++;
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine("done ({0} commands loaded in {1:F2} second{2})", count, sw.Elapsed.TotalSeconds, ((int)sw.Elapsed.TotalSeconds == 1 ? "" : "s"));
        }

        private string m_Command;
        private AccessLevel m_AccessLevel;

        public string Command { get { return m_Command; } }
        public AccessLevel AccessLevel { get { return m_AccessLevel; } }

        public CommandAttribute( string command, AccessLevel accessLevel )
        {
            m_Command = command;
            m_AccessLevel = accessLevel;
        }
    }
}