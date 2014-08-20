using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PianoAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new PianoAddonDeed(); } }

		[ Constructable ]
		public PianoAddon()
		{
			AddonComponent ac = null;

			ac = new AddonComponent( 2928 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 2 );

			ac = new AddonComponent( 5981 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 6 );

			ac = new AddonComponent( 5984 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 8);

			ac = new AddonComponent( 5981 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 7 );

			ac = new AddonComponent( 5985 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 9 );

			ac = new AddonComponent( 5431 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 10 );

			ac = new AddonComponent( 7933 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 7 );

			ac = new AddonComponent( 2480 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 1, 11 );

			ac = new AddonComponent( 7883 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, 0, 1 );

			ac = new AddonComponent( 2480 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, -1, -1, 2 );

			ac = new AddonComponent( 2924 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, -1, 0 );

			ac = new AddonComponent( 2925 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 0 );

			AddComponent( new PianoKeys(), 0, 0, 7 );
			AddComponent( new PianoKeys(), 0, 1, 7 );

			ac = new AddonComponent( 5981 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 10 );

			ac = new AddonComponent( 7933 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 9 );

			ac = new AddonComponent( 5991 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 9 );

			ac = new AddonComponent( 5988 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 10 );

			ac = new AddonComponent( 5987 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 8 );

			ac = new AddonComponent( 5988 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 9 );

			ac = new AddonComponent( 2252 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 0, 11 );

			ac = new AddonComponent( 2923 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 0 );

			ac = new AddonComponent( 2845 );
			ac.Light = LightType.Circle225;
			ac.Name = "A Candelabra";
			AddComponent( ac, 0, 1, 17 );

			ac = new AddonComponent( 7031 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 12 );

			ac = new AddonComponent( 7933 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 14 );

			ac = new AddonComponent( 5986 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 14 );

			ac = new AddonComponent( 5986 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 12 );

			ac = new AddonComponent( 5991 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 8 );

			ac = new AddonComponent( 5987 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 9 );

			ac = new AddonComponent( 5985 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 0, 1, 10 );

			ac = new AddonComponent( 3774 );
			ac.Name = "Sheet Music";
			AddComponent( ac, 1, 1, 15 );

			ac = new AddonComponent( 3772 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 1, 1, 12 );

			ac = new AddonComponent( 1114 );
			ac.Hue = 1;
			ac.Name = "Piano";
			AddComponent( ac, 1, 0, 0 );
		}

		public PianoAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class PianoAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new PianoAddon(); } }

		[Constructable]
		public PianoAddonDeed()
		{
			Name = "deed for a piano";
		}

		public PianoAddonDeed( Serial serial ) : base( serial )
		{
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
	
	public class PianoKeys : AddonComponent
	{
		private bool m_Active;
		public bool Active{ get{ return m_Active; } set{ m_Active = value; } }

		[Constructable]
		public PianoKeys() : base( 4006 )
		{
			Name = "piano keys";
			Movable = false;

			Active = false;
		}
		
		public PianoKeys( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !from.InRange( this, 2 ) )
			{
				from.SendLocalizedMessage( 500446 ); //That is too far away.
			}
			else if( !Active )
			{
				Active = true;

				from.PublicOverheadMessage( Network.MessageType.Regular, from.EmoteHue, false, "*begins warming up at the piano*" );
				new WarmUpTimer( from, this ).Start();
			}
			else
			{
				Active = false;
			}
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

	public class WarmUpTimer : Timer
	{
		private Mobile m_From;
		private PianoKeys m_Piano;
		private int keys;

		public WarmUpTimer( Mobile from, PianoKeys piano ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
			m_From = from;
			m_Piano = piano;
			Priority = TimerPriority.TwoFiftyMS;

			keys = 0;
		}

		protected override void OnTick()
		{
			if( m_Piano == null || m_Piano.Deleted )
			{
				Stop();
				return;
			}
			else if( m_From == null || !m_From.Alive || !m_From.InRange( m_Piano, 2 ) )
			{
				Stop();
				return;
			}

			if( keys >= 0 && keys < 3 )
			{
				switch( Utility.Random( 3 ) )
				{
					case 0: m_From.PlaySound( 1024 ); break;
					case 1: m_From.PlaySound( 1026 ); break;
					case 2: m_From.PlaySound( 1028 ); break;
				}

				keys++;
			}
			else if( keys >= 3 && keys <= 6 )
			{
				switch( Utility.Random( 3 ) )
				{
					case 0: m_From.PlaySound( 1045 ); break;
					case 1: m_From.PlaySound( 1044 ); break;
					case 2: m_From.PlaySound( 1040 ); break;
				}

				keys++;
			}
			else
			{
				new PianoTimer( m_From, m_Piano ).Start();
				this.Stop();
			}
		}
	}

	public class PianoTimer : Timer
	{
		private Mobile m_From;
		private PianoKeys m_Piano;

		public PianoTimer( Mobile from, PianoKeys piano ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 3.0 ) )
		{
			m_From = from;
			m_Piano = piano;
			Priority = TimerPriority.TwoFiftyMS;
		}

		protected override void OnTick()
		{
			if( m_Piano == null || m_Piano.Deleted || m_From == null )
			{
				Stop();

				return;
			}
			else if( m_From == null || !m_From.Alive )
			{
				m_Piano.Active = false;
				Stop();

				return;
			}

			switch( Utility.Random( 3 ) )
			{
				case 0: m_From.PlaySound( 1027 ); break;
				case 1: m_From.PlaySound( 1035 ); break;
				case 2: m_From.PlaySound( 1048 ); break;
			}

			if( !m_Piano.Active )
			{
				m_From.SendMessage( "Piano inactive" );

				Stop();
			}
			else if( !m_From.InRange( m_Piano, 2 ) )
			{
				m_Piano.Active = false;
				Stop();
			}
		}
	}
}