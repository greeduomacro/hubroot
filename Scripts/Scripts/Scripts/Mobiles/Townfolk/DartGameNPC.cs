using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class DartGameNPC : BaseCreature
	{
		private DartBoard m_Board;
		private Mobile m_Gamer;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DartBoard DartBoard
		{
			get{ return m_Board; }
			set{ m_Board = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Gamer
		{
			get{ return m_Gamer; }
			set
			{
				m_Gamer = value;
			}
		}
		
		public bool hasTalked;
		
		[Constructable]
		public DartGameNPC() : base( AIType.AI_Vendor, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "male" );
			BodyValue = 0x190;
			Hue = Utility.RandomSkinHue();
			
			Utility.AssignRandomHair( this, true );
			
			AddItem( new FloppyHat( Utility.RandomGreenHue() ) );
			AddItem( new FancyShirt( Utility.RandomGreenHue() ) );
			AddItem( new LeatherGloves() );
			AddItem( new LongPants( Utility.RandomBlueHue() ) );
			AddItem( new Sandals() );
			
			hasTalked = false;
		}
		
		public DartGameNPC( Serial serial ) : base( serial )
		{
		}
		
		public override bool DisallowAllMoves{ get{ return true; } }
		
		public override void OnMovement( Mobile m, Point3D oldLoc )
		{
			if( m.InRange( this.Location, 2 ) && !m.Hidden && m.Alive && !hasTalked )
			{
				Say( "Hear ye, hear ye! Come and try your unsteady hands at a game of darts!" );
				Say( "Throw a pair, hit the center, and you win!" );
				Say( "All ya gotta say is \"play the game\"!" );
				
				Direction = GetDirectionTo( m );
				
				hasTalked = true;
				new InternalSpamTimer( this ).Start();
			}
			
			base.OnMovement( m, oldLoc );
		}
		
		public override bool HandlesOnSpeech( Mobile m )
		{
			if( m.InRange( this.Location, 2 ) && m.Alive && !m.Hidden && m_Board != null )
				return true;
			
			return base.HandlesOnSpeech( m );
		}
		
		public override void OnSpeech( SpeechEventArgs args )
		{
			if( !args.Handled && args.Mobile.InRange( this.Location, 2 ) )
			{
				Mobile m = args.Mobile;
				
				if( args.Speech.IndexOf( "play" ) > -1 && args.Speech.IndexOf( "game" ) > -1 )
				{
					if( m_Gamer != null )
					{
						Say( String.Format( "Hiyo! {0} is already taking {1} turn. Wait till {2} done!", m_Gamer.RawName, m_Gamer.Female ? "her" : "his", m_Gamer.Female ? "she\'s" : "he\'s" ) );
					}
					else if( m_Board == null || m_Board.Deleted )
					{
						Say( "Uh... nevermind! Someone stole my dartboard!" );
					}
					else
					{
						Direction = GetDirectionTo( m );
						Say( "Aha! We have a taker! Here\'s a pair of knives, see what you can do!" );
						
						m_Gamer = m;
						
						new InternalTimer( m, this ).Start();
					}
				}
			}
			
			base.OnSpeech( args );
		}
		
		public void CheckThrow( Mobile thrower )
		{
			double rand = Utility.RandomDouble();
			
			if( rand < 0.05 )
			{
				Say( "Perfect throw! That\'s a bullseye!" );
				
				Reward( thrower );
			}
			else if( rand < 0.20 )
				Say( "Ooh, just outside the center! SO close, but no beans on that throw!" );
			else if( rand < 0.45 )
				Say( "That was okay, but I\'m not gonna reward it!" );
			else if( rand < 0.70 )
				Say( "What was THAT?" );
			else
				Say( "Wow! You didn\'t even manage to hit the dartboard!" );
		}
		
		public void Reward( Mobile m )
		{
			if( m.Alive && m.Backpack != null )
			{
				Item knife = new Item( 0xF52 );
				knife.LootType = LootType.Blessed;
				knife.Hue = 2213;
				knife.Name = "a gold-plated knife";
				
				m.AddToBackpack( knife );
				
				Say( "All right! Well done, well done! Here\'s some coin and a knife for your wall! Don\'t spend it all in one place, eh?" );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );
			
			writer.Write( m_Board );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_Board = (DartBoard)reader.ReadItem();
		}
		
		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private DartGameNPC m_NPC;
			private int count;
			
			public InternalTimer( Mobile from, DartGameNPC npc ) : base( TimeSpan.FromSeconds( 3.0 ), TimeSpan.FromSeconds( 3.0 ) )
			{
				m_From = from;
				m_NPC = npc;
				count = 0;
				
				Priority = TimerPriority.OneSecond;
			}
			
			protected override void OnTick()
			{
				if( m_From == null || !m_From.Alive || (m_NPC.DartBoard == null || m_NPC.DartBoard.Deleted) )
				{
					m_NPC.Say( "Hmm...minor detail..." );
					
					Stop();
				}
				else if( ++count > 2 )
				{
					m_NPC.Gamer = null;
					m_NPC.Say( "Game over! NEXT!" );
					
					Stop();
				}
				else
				{
					m_From.Direction = m_From.GetDirectionTo( m_NPC.DartBoard.GetWorldLocation() );
					
					m_From.Animate( m_From.Mounted ? 26 : 9, 7, 1, true, false, 0 );
					m_From.MovingEffect( m_NPC.DartBoard, 0xF51, 7, 1, false, false );
					Effects.PlaySound( m_From.Location, m_From.Map, 0x238 );
					
					m_NPC.CheckThrow( m_From );
				}
			}
		}
		
		private class InternalSpamTimer : Timer
		{
			private DartGameNPC m_NPC;
			
			public InternalSpamTimer( DartGameNPC npc ) : base( TimeSpan.FromSeconds( 10.0 ) )
			{
				m_NPC = npc;
				
				Priority = TimerPriority.OneSecond;
			}
			
			protected override void OnTick()
			{
				if( m_NPC != null )
					m_NPC.hasTalked = false;
				
				Stop();
			}
		}
	}
}
