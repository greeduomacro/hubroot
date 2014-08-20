using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Commands;
using Server.Network;

namespace Server.Commands
{
	public sealed class GenerateDescriptor
	{
		private static List<Type> _items;
		private static List<Type> _mobiles;
		private static Dictionary<Type, TypeInfo> _types;

		#region -static void generateDescriptor_onCommand( CommandEventArgs )
		[CommandAttribute( "GenerateDescriptor", AccessLevel.Administrator )]
		public static void generateDescriptor_onCommand( CommandEventArgs args )
		{
			World.Broadcast( 0x485, true, "Generating assembly descriptor, please wait..." );

			NetState.FlushAll();
			NetState.Pause();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			Generate();

			sw.Stop();
			NetState.Resume();

			World.Broadcast( 0x485, true, "Assembly descriptor has been compiled. The entire process took {0:F2} seconds.", (sw.ElapsedMilliseconds / 1000) );
		}
		#endregion

		#region -static void ExportItems( string, XmlWriterSettings )
		private static void ExportItems( string filename, XmlWriterSettings settings )
		{
			using( XmlWriter writer = XmlWriter.Create( filename + ".xml", settings ) )
			{
				writer.WriteStartDocument( true );
				writer.WriteStartElement( "descriptor.items" );

				_items.ForEach(
					delegate( Type t )
					{
						if( _types.ContainsKey( t ) )
						{
							TypeInfo info = _types[t];

							writer.WriteStartElement( "item" );

							writer.WriteAttributeString( "base", (info.BaseType == null ? "none" : info.BaseType.FullName) );
							writer.WriteAttributeString( "fullName", info.FullName );
							writer.WriteAttributeString( "name", info.Name );
							writer.WriteAttributeString( "namespace", info.Namespace );

							#region itemID collection
							if( t.IsSubclassOf( typeof( Item ) ) )
							{
								ConstructorInfo ctor;
								Item i = null;

								try
								{
									if( (ctor = t.GetConstructor( Type.EmptyTypes )) != null )
										i = (Item)ctor.Invoke( new object[] { } );
								}
								catch( TargetInvocationException )
								{
									Console.WriteLine( "Bad parameterless ctor for type {0}", t.FullName );
								}

								if( i != null )
								{
									writer.WriteAttributeString( "itemid", XmlConvert.ToString( i.ItemID ) );
									i.Delete();
								}
							}
							#endregion

							#region ctors
							if( info.Constructors != null )
							{
								Array.ForEach<ConstructorInfo>( info.Constructors,
									delegate( ConstructorInfo ctorInfo )
									{
										writer.WriteStartElement( "ctor" );

										Array.ForEach<ParameterInfo>( ctorInfo.GetParameters(),
											delegate( ParameterInfo paramInfo )
											{
												writer.WriteStartElement( "param" );
												writer.WriteAttributeString( "name", paramInfo.Name );
												writer.WriteAttributeString( "type", paramInfo.ParameterType.FullName );
												writer.WriteEndElement();
											} );

										writer.WriteEndElement();
									} );
							}
							#endregion

							#region interfaces
							if( info.Interfaces != null )
							{
								Array.ForEach<Type>( info.Interfaces,
									delegate( Type interfaceType )
									{
										if( !interfaceType.FullName.StartsWith( "System" ) )
										{
											writer.WriteStartElement( "interface" );
											writer.WriteAttributeString( "name", interfaceType.FullName );
											writer.WriteEndElement();
										}
									} );
							}
							#endregion

							writer.WriteEndElement();
						}
					} );

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
		#endregion

		#region -static void ExportMobiles( string, XmlWriterSettings )
		private static void ExportMobiles( string filename, XmlWriterSettings settings )
		{
			using( XmlWriter writer = XmlWriter.Create( filename + ".xml", settings ) )
			{
				writer.WriteStartDocument( true );
				writer.WriteStartElement( "descriptor.mobiles" );

				_mobiles.ForEach(
					delegate( Type t )
					{
						if( _types.ContainsKey( t ) )
						{
							TypeInfo info = _types[t];

							writer.WriteStartElement( "mobile" );
							
							writer.WriteAttributeString( "base", (info.BaseType == null ? "none" : info.BaseType.FullName) );
							writer.WriteAttributeString( "fullName", info.FullName );
							writer.WriteAttributeString( "name", info.Name );
							writer.WriteAttributeString( "namespace", info.Namespace );

							#region body collection
							if( t.IsSubclassOf( typeof( Mobile ) ) )
							{
								ConstructorInfo ctor;
								Mobile m = null;

								try
								{
									if( (ctor = t.GetConstructor( Type.EmptyTypes )) != null )
										m = (Mobile)ctor.Invoke( new object[] { } );
								}
								catch( TargetInvocationException )
								{
									Console.WriteLine( "Bad parameterless ctor for type {0}", t.FullName );
								}

								if( m != null )
								{
									writer.WriteAttributeString( "body", XmlConvert.ToString( m.BodyValue ) );
									m.Delete();
								}
							}
							#endregion

							#region ctors
							if( info.Constructors != null )
							{
								Array.ForEach<ConstructorInfo>( info.Constructors,
									delegate( ConstructorInfo ctorInfo )
									{
										writer.WriteStartElement( "ctor" );

										Array.ForEach<ParameterInfo>( ctorInfo.GetParameters(),
											delegate( ParameterInfo paramInfo )
											{
												writer.WriteStartElement( "param" );
												writer.WriteAttributeString( "name", paramInfo.Name );
												writer.WriteAttributeString( "type", paramInfo.ParameterType.FullName );
												writer.WriteEndElement();
											} );

										writer.WriteEndElement();
									} );
							}
							#endregion

							#region interfaces
							if( info.Interfaces != null )
							{
								Array.ForEach<Type>( info.Interfaces,
									delegate( Type interfaceType )
									{
										if( !interfaceType.FullName.StartsWith( "System" ) )
										{
											writer.WriteStartElement( "interface" );
											writer.WriteAttributeString( "name", interfaceType.FullName );
											writer.WriteEndElement();
										}
									} );
							}
							#endregion

							writer.WriteEndElement();
						}
					} );

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
		#endregion

		#region -static void ExportTypes
		private static void ExportTypes()
		{
			string itemFilename = "items";
			string mobileFilename = "mobiles";

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CloseOutput = true;
			settings.ConformanceLevel = ConformanceLevel.Document;
			settings.Encoding = System.Text.Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";

			if( File.Exists( itemFilename + ".xml" ) )
				File.Delete( itemFilename + ".xml" );
			if( File.Exists( mobileFilename + ".xml" ) )
				File.Delete( mobileFilename + ".xml" );

			ExportItems( itemFilename, settings );
			ExportMobiles( mobileFilename, settings );
		}
		#endregion

		#region -static bool Generate()
		private static bool Generate()
		{
			List<Assembly> assemblies = new List<Assembly>();
			_items = new List<Type>();
			_mobiles = new List<Type>();
			_types = new Dictionary<Type, TypeInfo>();

			assemblies.Add( Core.Assembly );

			for( byte i = 0; i < ScriptCompiler.Assemblies.Length; i++ )
			{
				if( !assemblies.Contains( ScriptCompiler.Assemblies[i] ) )
					assemblies.Add( ScriptCompiler.Assemblies[i] );
			}

			assemblies.ForEach(
				delegate( Assembly a )
				{
					LoadTypes( a );
				} );

			ExportTypes();

			return true;
		}
		#endregion

		#region -static void LoadTypes( Assembly )
		/// <summary>
		/// Loads all constructable types from the given <code>Assembly</code>
		/// </summary>
		private static void LoadTypes( Assembly asm )
		{
			ConstructorInfo[] ctors;
			bool isItem;
			Type type;
			Type[] types = asm.GetTypes();

			for( int i = 0; i < types.Length; i++ )
			{
				type = types[i];

				if( type.IsSpecialName || type.IsAbstract || !IsConstructable( type, out isItem ) )
					continue;

				_types[type] = new TypeInfo( type );

				ctors = type.GetConstructors();
				bool hasCtorAttr = false;

				for( int j = 0; !hasCtorAttr && j < ctors.Length; j++ )
					hasCtorAttr = IsConstructable( ctors[j] );

				if( hasCtorAttr )
				{
					if( isItem )
						_items.Add( type );
					else
						_mobiles.Add( type );
				}
			}

			_items.Sort( new TypeComparer() );
			_mobiles.Sort( new TypeComparer() );
		}
		#endregion

		#region -static bool IsConstructable( ConstructorInfo )
		/// <summary>
		/// Determines if the given ConstructorInfo is constructable in-game through the Constructable ctor attribute
		/// </summary>
		private static bool IsConstructable( ConstructorInfo info )
		{
			return (info.IsDefined( typeof( ConstructableAttribute ), false ));
		}
		#endregion

		#region -static bool IsConstructable( Type, out bool )
		/// <summary>
		/// Determines if the given type is in the inheritance hierarchy of <code>Item</code> or <code>Mobile</code>
		/// </summary>
		private static bool IsConstructable( Type t, out bool isItem )
		{
			if( (isItem = typeof( Item ).IsAssignableFrom( t )) )
				return true;

			return (typeof( Mobile ).IsAssignableFrom( t ));
		}
		#endregion

		#region -sealed class TypeInfo
		private sealed class TypeInfo
		{
			private Assembly _assembly;
			private TypeAttributes _attributes;
			private Type _baseType;
			private ConstructorInfo[] _ctors;
			private object[] _customAttrs;
			private string _fullName;
			private Type[] _interfaces;
			private string _name;
			private string _namespace;
			private Type _type;

			public Assembly Assembly { get { return _assembly; } }
			public TypeAttributes Attributes { get { return _attributes; } }
			public Type BaseType { get { return _baseType; } }
			public ConstructorInfo[] Constructors { get { return _ctors; } }
			public object[] CustomAttributes { get { return _customAttrs; } }
			public string FullName { get { return _fullName; } }
			public Type[] Interfaces { get { return _interfaces; } }
			public string Name { get { return _name; } }
			public string Namespace { get { return _namespace; } }
			public Type Type { get { return _type; } }

			public TypeInfo( Type t )
			{
				_assembly = t.Assembly;
				_attributes = t.Attributes;
				_baseType = t.BaseType;
				_ctors = t.GetConstructors();
				_customAttrs = t.GetCustomAttributes( false );
				_fullName = t.FullName;
				_interfaces = t.GetInterfaces();
				_name = t.Name;
				_namespace = t.Namespace;
				_type = t;
			}
		}
		#endregion

		#region -class TypeComparer : IComparer<Type>
		private class TypeComparer : IComparer<Type>
		{
			public int Compare( Type a, Type b )
			{
				if( a == null && b == null )
					return 0;
				else if( a == null )
					return -1;
				else if( b == null )
					return 1;

				if( b.IsSubclassOf( a ) )
					return 1;
				else if( a.IsSubclassOf( b ) )
					return -1;

				return a.Name.CompareTo( b.Name );
			}
		}
		#endregion
	}
}