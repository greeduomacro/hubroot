using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class CharacterProfileGump : Gump
	{
		private static int DefaultBackgroundHeight = 145;
		private static int MinTextEntries = 1;
		private static int MaxTextEntries = 6;
		private static int DefaultTextEntryHeight = 125;
		private static int MaxTextLength = 224;

		private Mobile _beheld;
		private bool _canEdit;
		private bool _editMode;
		private int _numEntries;

		public CharacterProfileGump( Mobile beholder, Mobile beheld ) : this( beholder, beheld, MinTextEntries, false ) { }

		public CharacterProfileGump( Mobile beholder, Mobile beheld, int numEntries ) : this( beholder, beheld, numEntries, false ) { }

		public CharacterProfileGump( Mobile beholder, Mobile beheld, int numEntries, bool editMode )
			: base( 0, 0 )
		{
			if( beheld.Profile == null )
				beheld.Profile = "";

			if( numEntries < MinTextEntries )
				_numEntries = MinTextEntries;
			else if( numEntries > MaxTextEntries )
				_numEntries = MaxTextEntries;
			else
				_numEntries = numEntries;

			_beheld = beheld;
			_canEdit = (beholder == beheld);
			_editMode = (editMode && _canEdit);

			int height = DefaultBackgroundHeight + ((DefaultTextEntryHeight + 10) * _numEntries);
			string[] profileSlices = SliceString( beheld.Profile, MaxTextLength );

			AddPage( 1 );
			AddBackground( 10, 10, 300, height, 9380 );
			AddImage( 35, 55, 52 );

			AddLabelCropped( 85, 40, 195, 20, 0, Titles.GetNameTitle( beholder, beheld ) );
			AddLabelCropped( 85, 55, 195, 20, 0, Titles.GetSkillTitle( beheld ) );

			if( _canEdit )
			{
				AddLabel( 40, 10, 0, (_editMode ? "Editing Mode" : "Reading Mode") );
				AddButton( 270, 95, 5411, 5411, 5, GumpButtonType.Reply, 0 );
			}

			if( _canEdit && _editMode )
			{
				for( int i = 0, x = 40, y = 120; i < _numEntries; i++, y += (DefaultTextEntryHeight + 10) )
				{
					int slices = profileSlices.Length - 1;
					AddTextEntry( x, y, 235, DefaultTextEntryHeight, 0, (i + 100), (slices >= i ? profileSlices[i] : ""), MaxTextLength );

					if( i != (_numEntries - 1) )
						AddImage( 28, (y + DefaultTextEntryHeight - 3), 57 );
				}
			}
			else
				AddHtml( 40, 120, 235, (DefaultTextEntryHeight * _numEntries), String.Format( "<BASEFONT COLOR=#111111>{0}</BASEFONT>", beheld.Profile ), false, true );

			AddButton( 255, (height - 10), 55, 55, 10, GumpButtonType.Reply, 0 );
			AddButton( 275, (height - 10), 56, 56, 20, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if( _canEdit && _editMode )
			{
				string entryText = "";
				TextRelay relay;

				for( int i = 0; i < _numEntries; i++ )
				{
					relay = info.GetTextEntry( 100 + i );

					if( relay != null )
						entryText += relay.Text;
					else
						break;
				}

				if( entryText != _beheld.Profile )
					_beheld.Profile = entryText.Trim();
			}

			if( info.ButtonID == 5 )
				sender.Mobile.SendGump( new CharacterProfileGump( sender.Mobile, _beheld, _numEntries, !_editMode ) );
			else if( info.ButtonID == 10 )
				sender.Mobile.SendGump( new CharacterProfileGump( sender.Mobile, _beheld, Math.Min( MaxTextEntries, (_numEntries + 1) ), _editMode ) );
			else if( info.ButtonID == 20 )
				sender.Mobile.SendGump( new CharacterProfileGump( sender.Mobile, _beheld, Math.Max( MinTextEntries, (_numEntries - 1) ), _editMode ) );
		}

		private string[] SliceString( string text, int sliceSize )
		{
			if( text == null || text == "" )
				return new string[] { "" };

			List<string> list = new List<string>();
			string currSlice = "";
			int index = 0;

			do
			{
				currSlice = text.Substring( index, Math.Min( sliceSize, (text.Length - index) ) );

				list.Add( currSlice );

				index += currSlice.Length;
				currSlice = "";
			} while( index < text.Length );

			return list.ToArray();
		}
	}
}