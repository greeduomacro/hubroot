using System;
using Server;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	public class ControlTarget : Target
	{
		private Item m_Item;

		public ControlTarget( Item item )
			: base( 20, true, TargetFlags.None )
		{
			m_Item = item;
		}

		protected override void OnTarget( Mobile from, Object to )
		{
			if( to is BaseCreature )
			{
				BaseCreature Controlled = to as BaseCreature;

				from.SendMessage( "Now controlling " + Controlled.Name );
				Controlled.CantWalk = true;
				from.SendGump( new CreatureControl( Controlled, from ) );
			}
			else
				from.SendMessage( "You can only control creatures!" );
		}
	}

	public class CombatTarget : Target
	{
		private BaseAI m_AI;
		private BaseCreature creature;

		public CombatTarget( BaseAI ai, BaseCreature bc )
			: base( 10, false, TargetFlags.None )
		{
			m_AI = ai;
			creature = bc;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( targeted is Mobile )
			{
				Mobile m = ((Mobile)targeted);

				if( m == creature )
				{
					from.SendMessage( "It cannot attack itself!" );
					from.SendGump( new CreatureControl( creature, from ) );
				}

				else
				{

					if( creature.Alive )
					{
						m_AI.Action = ActionType.Combat;
						m_AI.NextMove = DateTime.Now;
						creature.Combatant = m;
						from.SendGump( new CreatureControl( creature, from ) );
					}
				}
			}

			else
			{
				from.SendMessage( "You cannot attack that!" );
				from.SendGump( new CreatureControl( creature, from ) );
			}
		}

	}

	public class CreatureController : Item
	{
		[Constructable]
		public CreatureController()
			: base( 0x1F19 )
		{
			Name = "a Creature Controller";
			Hue = 2129;
			Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.GameMaster )
			{
				this.Delete();
			}
			else if( IsChildOf( from.Backpack ) )
			{
				from.Target = new ControlTarget( this );
				from.SendMessage( "Select a creature to control." );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}


		public CreatureController( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}

	public class CreatureControl : Gump
	{
		private BaseCreature m_to;
		private BaseAI m_ai;

		public CreatureControl( BaseCreature to, Mobile from )
			: base( 200, 150 )
		{
			m_to = to;
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage( 0 );
			this.AddImage( 167, 119, 9007 );
			this.AddImage( 227, 180, 1417 );
			this.AddButton( 236, 189, 5582, 5582, 9, GumpButtonType.Reply, 0 );
			this.AddButton( 276, 161, 4501, 4501, 1, GumpButtonType.Reply, 0 );
			this.AddButton( 272, 227, 4503, 4503, 2, GumpButtonType.Reply, 0 );
			this.AddButton( 210, 227, 4505, 4505, 3, GumpButtonType.Reply, 0 );
			this.AddButton( 210, 161, 4507, 4507, 4, GumpButtonType.Reply, 0 );
			this.AddButton( 243, 148, 4500, 4500, 5, GumpButtonType.Reply, 0 );
			this.AddButton( 288, 193, 4502, 4502, 6, GumpButtonType.Reply, 0 );
			this.AddButton( 243, 239, 4504, 4504, 7, GumpButtonType.Reply, 0 );
			this.AddButton( 197, 193, 4506, 4506, 8, GumpButtonType.Reply, 0 );
			this.AddButton( 236, 189, 5582, 5582, 9, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			BaseCreature toMobile = m_to;

			if( toMobile.Alive )
			{
				m_ai = toMobile.AIObject;

				if( m_ai == null )
				{
					from.SendMessage( "Mobile AI is null or unavailable. Unable to process with this item." );
					return;
				}

				toMobile.CantWalk = false;

				m_ai.NextMove = DateTime.Now + TimeSpan.FromSeconds( 60 );
				DateTime delay = DateTime.Now + TimeSpan.FromSeconds( 60 );

				if( info.ButtonID == 0 )
				{
					from.SendMessage( "You finish controlling " + toMobile.Name );
					m_ai.NextMove = DateTime.Now;
				}

				if( info.ButtonID == 1 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.North );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 2 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.East );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 3 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.South );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 4 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.West );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 5 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.Up );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 6 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.Right );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 7 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.Down );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 8 )
				{
					m_ai.NextMove = DateTime.Now;
					m_ai.DoMove( Direction.Left );
					m_ai.NextMove = delay;
					from.SendGump( new CreatureControl( toMobile, from ) );
				}

				if( info.ButtonID == 9 )
				{
					from.SendMessage( "Who would you like this creature to attack?" );
					from.Target = new CombatTarget( m_ai, toMobile );
				}
			}

		}

	}
}