using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Mobiles;

namespace Server.Help
{
	public class PageQueue
	{
		private static List<PageEntry> _list = new List<PageEntry>();
		private static Dictionary<Mobile, PageEntry> _handlerDictionary = new Dictionary<Mobile, PageEntry>();
		private static Dictionary<Mobile, PageEntry> _senderDictionary = new Dictionary<Mobile, PageEntry>();

		public static List<PageEntry> List { get { return _list; } }

		public static bool AllowedToPage( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if( pm == null || pm.NetState == null )
				return true;

			if( pm.DesignContext != null )
			{
				m.SendLocalizedMessage( 500182 ); //You cannot request help while customizing a house or transferring a character.
				return false;
			}
			else if( pm.PagingSquelched )
			{
				m.SendMessage( "You are currently prohibited from sending pages." );
				return false;
			}

			return true;
		}

		public static string GetPageTypeName( PageType type )
		{
			if( type == PageType.GeneralQuestion )
				return "General Question";

			return type.ToString();
		}

		public static void OnHandlerChanged( Mobile old, Mobile value, PageEntry entry )
		{
			if( old != null && _handlerDictionary.ContainsKey( old ) )
				_handlerDictionary.Remove( old );

			if( value != null )
			{
				if( IsHandling( value ) )
				{
					value.SendMessage( "You cannot handle more than one page at a time." );
					return;
				}
				else
					_handlerDictionary.Add( value, entry );
			}

			entry.WriteLine( "### ---" );
			entry.WriteLine( String.Format( "Page handler changed from {0} to {1}.", old == null ? "(-null-)" : old.RawName, value == null ? "(-null-)" : value.RawName ) );
			entry.WriteLine( "---###" );
			entry.WriteLine();
		}

		public static bool IsHandling( Mobile m )
		{
			return _handlerDictionary.ContainsKey( m );
		}

		public static bool Contains( Mobile m )
		{
			return _senderDictionary.ContainsKey( m );
		}

		public static int IndexOf( PageEntry entry )
		{
			return _list.IndexOf( entry );
		}

		public static void Cancel( Mobile m )
		{
			PageEntry entry = null;

			_senderDictionary.TryGetValue( m, out entry );

			if( entry != null )
				Remove( entry );
		}

		public static void Remove( Mobile m )
		{
			Remove( GetEntry( m ) );
		}

		public static void Remove( PageEntry entry )
		{
			if( entry == null )
				return;

			_list.Remove( entry );

			if( entry.Sender != null )
				_senderDictionary.Remove( entry.Sender );

			if( entry.Handler != null )
				_handlerDictionary.Remove( entry.Handler );

			if( _list.Count == 0 )
			{
				PageAlertGump.CloseAll();
			}
		}

		public static PageEntry GetEntry( Mobile m )
		{
			PageEntry entry = null;

			_senderDictionary.TryGetValue( m, out entry );

			return entry;
		}

		public static void AddNewPage( PageEntry entry )
		{
			_list.Add( entry );
			_senderDictionary.Add( entry.Sender, entry );

			PageAlertGump.SendToStaff();
		}
	}
}