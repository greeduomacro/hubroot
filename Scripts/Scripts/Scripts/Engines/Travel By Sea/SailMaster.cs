using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using System.Collections.Generic;

namespace Khazman.TravelBySea
{
	public class SailMaster : BaseVendor
	{
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
		private static bool m_Talked;
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public SailMaster() : base( "the sailboat captain" )
		{
			EquipItem( new SmithHammer() );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBSailMaster() );
		}

		public override void InitOutfit()
		{
			base.InitOutfit();
		}

		public SailMaster( Serial serial ) : base( serial )
		{
		}

		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.InRange( this.Location, 5 ) )
				return true;

			return base.HandlesOnSpeech( from );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
		  	if( m_Talked == false && m is PlayerMobile )
			{
				if ( m.InRange( this, 1 ) )
				{
					m_Talked = true;
					this.Say( "Hail and well met." );
					this.Say( "When thee are wishing to sail, just say so and we can make arrangements." );
					this.Direction = GetDirectionTo( m.Location );
					SpamTimer t = new SpamTimer();
					t.Start();
				}
			}
		}
 
		private class SpamTimer : Timer
		{
			public SpamTimer() : base( TimeSpan.FromSeconds( 15 ) )
			{
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
					m_Talked = false;
			}
		}

		public override void OnSpeech(SpeechEventArgs e )
		{
			if ( !e.Handled && e.Mobile.InRange( this.Location, 5 ) )
			{
				if (e.Speech.ToLower().IndexOf( "sail" ) >= 0 )
				{
					e.Mobile.SendGump( new SailDestinationGump( e.Mobile, this.Location ) );
					this.Direction = GetDirectionTo( e.Mobile.Location );
				}
			}

			base.OnSpeech( e );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}