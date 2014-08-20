using System;
using Server;
using Server.Gumps;
using Server.Accounting;
using Server.Network;
using Server.Commands;

namespace Server.Commands
{
	public class CharacterControlGump : Gump
	{
		private Account _firstAccount;
		private Account _secondAccount;
		private SwapInfo _swapInfo;

		[CommandAttribute( "CharControl", AccessLevel.Administrator )]
		public static void CharControl_OnCommand( CommandEventArgs args )
		{
			args.Mobile.SendGump( new CharacterControlGump() );
		}

		public CharacterControlGump()
			: this( null, null, null, null )
		{
		}

		public CharacterControlGump( Account first, Account second, string ErrorMessage )
			: this( first, second, ErrorMessage, null )
		{
		}

		public CharacterControlGump( Account first, Account second, string ErrorMessage, SwapInfo si )
			: base( 50, 50 )
		{
			Closable = true;
			Disposable = false;
			Dragable = true;
			Resizable = false;

			_firstAccount = first;
			_secondAccount = second;
			_swapInfo = si;

			AddPage( 0 );
			AddBackground( 16, 12, 350, 450, 9270 );
			AddAlphaRegion( 15, 10, 352, 454 );

			AddImage( 190, 22, 9273 );
			AddImage( 128, 385, 9271 );
			AddImage( 180, 22, 9275 );
			AddImage( 190, 100, 9273 );
			AddImage( 180, 100, 9275 );
			AddImage( 233, 385, 9271 );
			AddImage( 26, 385, 9271 );

			if( !InSwapMode )
			{
				AddButton( 176, 49, 4023, 4025, 1, GumpButtonType.Reply, 0 ); //Okay for acct names button

				AddHtml( 30, 395, 325, 56, Color( Center( ErrorMessage ), 0xFF0000 ), false, false );

				AddImageTiled( 33, 50, 140, 20, 0xBBC );
				AddImageTiled( 209, 50, 140, 20, 0xBBC );

				AddTextEntry( 33, 50, 140, 20, 1152, 2, "" );
				AddTextEntry( 209, 50, 140, 20, 1152, 3, "" );
			}

			AddLabel( 58, 28, 1152, (first == null) ? "1st Acct Name" : first.ToString() );
			AddLabel( 232, 28, 1152, (second == null) ? "2nd Acct Name" : second.ToString() );

			#region Create Character Buttons
			int x = 50; //x is 225 for 2nd...

			for( int h = 0; h < 2; h++ )
			{
				if( first != null )
				{
					int y = 87;
					for( int i = 0; i < 6; i++ )	//6 because of 6th char slot and we can handle nulls & out of bounds fine
					{
						Mobile m = first[i];

						if( m == null ) continue;

						if( !(InSwapMode && _swapInfo.AlreadyChose( first )) )
							AddButton( x - 20, y + 3, 5601, 5605, 10 * i + h * 100 + 5, GumpButtonType.Reply, 0 );	//The Swap Select button

						AddLabel( x, y, 1152, String.Format( "{0} (0x{1:X})", m.Name, m.Serial.Value ) );

						int labelY = y + 23;
						int buttonY = y + 27;

						AddLabel( x + 1, labelY, 1152, "Swap" );

						if( second != null && !InSwapMode && HasSpace( second ) != 0 )
							AddButton( x - 15, buttonY, 1210, 1209, 10 * i + h * 100 + 6, GumpButtonType.Reply, 0 );
						else
							AddImage( x - 15, buttonY, 11412 );


						AddLabel( x + 54, labelY, 1152, "Del" );
						if( !InSwapMode )
							AddButton( x + 36, buttonY, 1210, 1209, 10 * i + h * 100 + 7, GumpButtonType.Reply, 0 );
						else
							AddImage( x + 36, buttonY, 11412 );


						AddLabel( x + 95, labelY, 1152, "Move" );

						if( !InSwapMode && second != null && HasSpace( second ) >= 0 )
							AddButton( x + 78, buttonY, 1210, 1209, 10 * i + h * 100 + 8, GumpButtonType.Reply, 0 );
						else
							AddImage( x + 78, buttonY, 11412 );

						y += 48;
					}
				}

				x += 175;

				Account temp = first;
				first = second;
				second = temp;
			}
			#endregion
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			bool SendGumpAgain = true;

			Mobile m = state.Mobile;

			if( m.AccessLevel < AccessLevel.Administrator )
				return;

			#region Sanity
			if( IsDeleted( _firstAccount ) )
				_firstAccount = null;

			if( IsDeleted( _secondAccount ) )
				_secondAccount = null;
			#endregion

			int id = info.ButtonID;

			if( id == 0 )
			{
				if( InSwapMode )
					m.SendGump( new CharacterControlGump( _firstAccount, _secondAccount, "Character _swapInfo canceled" ) );
			}
			if( id == 1 )
			{
				#region Find Acct from Input
				string firstStr = info.GetTextEntry( 2 ).Text;
				string secondStr = info.GetTextEntry( 3 ).Text;

				Account first = Accounts.GetAccount( firstStr ) as Account;
				Account second = Accounts.GetAccount( secondStr ) as Account;

				string ErrorMessage = "";

				if( first == null || second == null )
				{
					if( first == null && firstStr != "" && secondStr == "" )
						ErrorMessage = String.Format( "Account: '{0}' NOT found", firstStr );
					else if( second == null && secondStr != "" && firstStr == "" )
						ErrorMessage = String.Format( "Account: '{0}' NOT found", secondStr );
					else if( firstStr == "" && secondStr == "" )
						ErrorMessage = "Please enter in an Account name";
					else if( second == null && first == null )
						ErrorMessage = String.Format( "Accounts: '{0}' and '{1}' NOT found", firstStr, secondStr );
				}

				if( _firstAccount != null && first == null )
					first = _firstAccount;

				if( _secondAccount != null && second == null )
					second = _secondAccount;

				m.SendGump( new CharacterControlGump( first, second, ErrorMessage ) );
				#endregion
			}
			else if( id > 4 ) //left side
			{
				#region Sanity & Declarations
				int button = id % 10;
				int charIndex = ((id < 100) ? id : (id - 100)) / 10;

				string error = "Invalid Button";

				Account acct;
				Account secondAcct;

				acct = (id >= 100) ? _secondAccount : _firstAccount;
				secondAcct = (id < 100) ? _secondAccount : _firstAccount;

				if( IsDeleted( acct ) )
					error = "Selected Account is null or Deleted";
				else if( acct[charIndex] == null )
					error = "That character is not found";
				#endregion
				else
				{
					Mobile mob = acct[charIndex];
					switch( button )
					{
						#region Swap
						case 5: //Swap Selection And/Or Props
							{
								if( InSwapMode )
								{
									if( !_swapInfo.AlreadyChose( acct ) && !_swapInfo.AlreadyChose( secondAcct ) )
									{
										//Both Empty, even though this should NEVER happen.  Just a sanity check
										_swapInfo.a1 = acct;
										_swapInfo.a1CharIndex = charIndex;
										error = "Please choose a character from the other acct to _swapInfo with";
									}
									else if( (_swapInfo.AlreadyChose( _swapInfo.a1 ) && !_swapInfo.AlreadyChose( _swapInfo._secondAccount )) || (_swapInfo.AlreadyChose( _swapInfo._secondAccount ) && !_swapInfo.AlreadyChose( _swapInfo.a1 )) )
									{
										//First is filled, second is empty
										if( _swapInfo.AlreadyChose( _swapInfo.a1 ) )
										{
											_swapInfo._secondAccount = acct;
											_swapInfo._secondAccountCharIndex = charIndex;
										}
										else
										{
											_swapInfo.a1 = acct;
											_swapInfo.a1CharIndex = charIndex;
										}

										if( _swapInfo.SwapEm() )
										{
											error = String.Format( "Mobile {0} (0x{1:X}) and Mobile {2} (0x{3:X}) sucessfully _swapInfoped between Accounts {4} and {5}", _swapInfo.a1[_swapInfo.a1CharIndex], ((Mobile)_swapInfo.a1[_swapInfo.a1CharIndex]).Serial.Value, _swapInfo._secondAccount[_swapInfo._secondAccountCharIndex], ((Mobile)_swapInfo._secondAccount[_swapInfo._secondAccountCharIndex]).Serial.Value, _swapInfo._secondAccount.ToString(), _swapInfo.a1.ToString() );
											CommandLogging.WriteLine( m, error );
										}
										else
											error = "Swap unsucessful";

										_swapInfo = null;
									}
								}
								else
								{
									m.SendGump( new PropertiesGump( m, mob ) );
									error = "Properties gump sent";
								}

								break;
							}
						case 6: //Swap
							{
								if( IsDeleted( secondAcct ) )
								{
									error = "Both accounts must exist to _swapInfo characters";
								}
								else if( HasSpace( acct ) == 0 || HasSpace( secondAcct ) == 0 )
								{
									error = "Both accounts must have at least one character to _swapInfo.";
								}
								else
								{
									error = "Please Choose the other character to _swapInfo.";
									_swapInfo = new SwapInfo( _firstAccount, _secondAccount );

									if( acct == _firstAccount )
										_swapInfo.a1CharIndex = charIndex;
									else
										_swapInfo._secondAccountCharIndex = charIndex;
								}
								break;
							}
						#endregion
						#region Delete Character
						case 7: //Del
							{
								object[] o = new object[] { acct, mob, this };

								m.SendGump(
									new WarningGump( 1060635, 30720,
									String.Format( "You are about to delete Mobile {0} (0x{1:X}) of Acct {2}.  This can not be reversed without a complete server revert.  Please note that this'll delete any items on that Character, but it'll still leave their house standing.  Do you wish to continue?", mob.Name, mob.Serial.Value, acct.ToString() ),
									0xFFC000, 360, 260, new WarningGumpCallback( CharacterDelete_Callback ), o ) );

								SendGumpAgain = false;

								break;
							}
						#endregion
						#region Move Character
						case 8: //Move
							{
								if( secondAcct == null )
								{
									error = String.Format( "Can't move Mobile {0} (0x{1:X} because the other account is null", mob.Name, mob.Serial.Value );
									break;
								}

								int newCharLocation = HasSpace( secondAcct );

								if( newCharLocation < 0 )
								{
									error = String.Format( "Can't move Mobile {0} (0x{1:X}) to account {2} because that account is full", mob.Name, mob.Serial.Value, secondAcct.ToString() );
									break;
								}

								acct[charIndex] = null;
								secondAcct[newCharLocation] = mob;

								if( mob.NetState != null )
									mob.NetState.Dispose();

								error = String.Format( "Mobile {0} (0x{1:X}) of Account {2} moved to Account {3}.", mob.Name, mob.Serial.Value, acct.ToString(), secondAcct.ToString() );

								CommandLogging.WriteLine( m, error );
								break;
							}
						#endregion
					}
				}
				if( SendGumpAgain )
					m.SendGump( new CharacterControlGump( _firstAccount, _secondAccount, error, _swapInfo ) );
			}
		}

		protected bool InSwapMode
		{
			get { return (_swapInfo != null); }
		}

		public bool IsDeleted( Account a )
		{
			return (a == null || !Accounts.GetAccounts().Contains( a ));
		}

		/// <summary>
		/// Checks to see if an Account used up all of it's character slots.
		/// Only currently supports 5 chars.  will hafta change this when it's changed to 6.
		/// Returns -1 if there's no room, and the location of the first free slot if there is room.
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public int HasSpace( Account a )
		{
			if( IsDeleted( a ) )
				return -1;

			for( int i = 0; i < 6; i++ )
			{
				if( a[i] == null )
				{
					return i;
				}
			}

			return -1;
		}


		protected void CharacterDelete_Callback( Mobile from, bool okay, object state )
		{
			object[] states = (object[])state;
			Account acct = (Account)states[0];
			Mobile mob = (Mobile)states[1];
			CharacterControlGump g = (CharacterControlGump)states[2];

			string error;

			if( mob == null || acct == null )
			{
				error = "Mobile or Acct is null"; //SafeGuard
				return;
			}
			if( okay )
			{
				if( mob.NetState != null )
					mob.NetState.Dispose();

				mob.Delete();
			}

			error = String.Format( "Mobile {0} (0x{1:X}) of Acct {2} {3} Deleted.", mob.Name, mob.Serial.Value, acct.ToString(), okay ? "" : "not" );

			if( okay )
				CommandLogging.WriteLine( from, error );

			from.SendGump( new CharacterControlGump( g._firstAccount, g._secondAccount, error ) );
		}


		public class SwapInfo
		{
			public Account a1;
			public Account _secondAccount;

			public int a1CharIndex;
			public int _secondAccountCharIndex;
			//TODO: MAke getters & setters


			public SwapInfo( Account firstAcct, Account secondAcct )
			{

				a1 = firstAcct;
				_secondAccount = secondAcct;

				a1CharIndex = -1;
				_secondAccountCharIndex = -1;
			}

			public bool AlreadyChose( Account acct )
			{
				if( acct == null )
					return false;

				if( acct == a1 )
					return (a1CharIndex >= 0);

				if( acct == _secondAccount )
					return (_secondAccountCharIndex >= 0);

				return false;
			}

			public bool SwapEm()
			{
				if( a1 == null || _secondAccount == null )
					return false;

				Mobile mob = (Mobile)a1[a1CharIndex];
				Mobile mob2 = (Mobile)_secondAccount[_secondAccountCharIndex];

				a1[a1CharIndex] = mob2;
				_secondAccount[_secondAccountCharIndex] = mob;

				if( mob == null || mob2 == null )
					return false;

				if( mob.NetState != null )
					mob.NetState.Dispose();

				if( mob2.NetState != null )
					mob2.NetState.Dispose();

				return true;
			}
		}
	}
}