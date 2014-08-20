using System;
using System.Collections;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class TentFlap : Item
	{
		private Mobile m_Owner;
		private ArrayList m_Friends;

		private TravelTent m_Tent;

		public Mobile Owner { get { return m_Owner; } }
		public ArrayList Friends { get { return m_Friends; } set { m_Friends = value; } }

		[Constructable]
		public TentFlap( Mobile owner, TravelTent tent )
			: base( 0x1F7 )
		{
			Name = "tent flap";
			Movable = false;

			m_Owner = owner;
			m_Friends = new ArrayList();
			m_Tent = tent;
		}

		public TentFlap( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( this, 2 ) && m_Tent != null )
			{
				if( from == m_Owner || IsFriend( from ) || m_Tent.Bounds.Contains( from ) || from.AccessLevel >= AccessLevel.GameMaster )
				{
					if( ItemID == 0x1F7 )
					{
						ItemID = 0x1F6;
						X += 1;
						Y -= 1;

						FlapTimer timer = new FlapTimer( this );
						timer.Start();

						from.PublicOverheadMessage( MessageType.Regular, from.EmoteHue, false, "*unclasps the tent flap*" );
					}
					else if( ItemID == 0x1F6 )
					{
						ItemID = 0x1F7;
						X -= 1;
						Y += 1;

						from.PublicOverheadMessage( MessageType.Regular, from.EmoteHue, false, "*closes the flap*" );
					}
				}
				else
				{
					from.SendMessage( "This is not your tent." );
				}
			}
			else
			{
				from.SendMessage( "That is too far away." );
			}
		}

		public void AddFriend( Mobile from, Mobile targ )
		{
			if( m_Friends == null || m_Owner == targ )
				return;

			if( !targ.Player )
			{
				from.SendMessage( "That can't be a friend of the tent." );
			}
			else if( m_Friends.Contains( targ ) )
			{
				from.SendLocalizedMessage( 501376 ); //This person is already on your friends list.
			}
			else
			{
				m_Friends.Add( targ );

				targ.Delta( MobileDelta.Noto );
				targ.SendMessage( "You have been granted access to this tent." );
			}
		}

		public void RemoveFriend( Mobile from, Mobile targ )
		{
			if( m_Friends == null )
				return;

			if( m_Friends.Contains( targ ) )
			{
				m_Friends.Remove( targ );

				targ.Delta( MobileDelta.Noto );

				from.SendMessage( "{0} has been removed from your friends list.", targ.RawName );
				targ.SendMessage( "Your access to this tent has been removed." );
			}
		}

		public bool IsFriend( Mobile m )
		{
			if( m == null || m_Friends == null )
				return false;

			return m_Friends.Contains( m );
		}

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			Mobile from = e.Mobile;

			if( !e.Handled && from == m_Owner )
			{
				if( e.Speech.ToLower().IndexOf( "add" ) >= 0 && e.Speech.ToLower().IndexOf( "friend" ) >= 0 )
				{
					from.SendMessage( "Target the friend you wish to give access to." );
					from.Target = new TentFlapFriendTarget( true, this );
				}
				else if( e.Speech.ToLower().IndexOf( "remove" ) >= 0 && e.Speech.ToLower().IndexOf( "friend" ) >= 0 )
				{
					from.SendMessage( "Who would you like to refuse access to?" );
					from.Target = new TentFlapFriendTarget( false, this );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)2 );

			writer.Write( (Mobile)m_Owner );
			writer.WriteMobileList( m_Friends, true );

			#region v2
			writer.Write( m_Tent );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();
			m_Friends = reader.ReadMobileList();

			if( version >= 2 )
				m_Tent = reader.ReadItem() as TravelTent;
		}

		private class FlapTimer : Timer
		{
			private TentFlap m_Flap;

			public FlapTimer( TentFlap flap )
				: base( TimeSpan.FromSeconds( 5 ) )
			{
				m_Flap = flap;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if( m_Flap.ItemID == 0x1F6 )
				{
					m_Flap.ItemID = 0x1F7;
					m_Flap.X -= 1;
					m_Flap.Y += 1;

					m_Flap.PublicOverheadMessage( MessageType.Regular, 0x38A, false, "*the tent flap falls back into place*" );
				}

				Stop();
			}
		}
	}

	public class TentFlapFriendTarget : Target
	{
		private TentFlap m_Flap;
		private bool m_toAdd;

		public TentFlapFriendTarget( bool add, TentFlap flap )
			: base( 12, false, TargetFlags.None )
		{
			CheckLOS = false;

			m_Flap = flap;
			m_toAdd = add;
		}

		protected override void OnTarget( Mobile from, object target )
		{
			if( !from.Alive || m_Flap.Deleted )
				return;

			if( target is Mobile )
			{
				if( m_toAdd )
					m_Flap.AddFriend( from, (Mobile)target );
				else
					m_Flap.RemoveFriend( from, (Mobile)target );
			}
			else
			{
				from.SendMessage( "You cannot grant access to that." );
			}
		}
	}
}