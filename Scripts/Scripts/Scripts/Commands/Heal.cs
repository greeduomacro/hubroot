using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Commands
{
	public class Heal
	{
		[CommandAttribute( "Heal", AccessLevel.Counselor )]
		public static void Heal_OnCommand( Server.Commands.CommandEventArgs e )
		{
			e.Mobile.Target = new HealTarget( e.Mobile );
		}

		private class HealTarget : Target
		{
			public HealTarget( Mobile m )
				: base( -1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if( !Server.Commands.Generic.BaseCommand.IsAccessible( from, o ) )
				{
					from.SendMessage( "That is not accessible." );
				}
				else if( o is Mobile )
				{
					Mobile m = (Mobile)o;

					if( !m.Alive )
						m.Resurrect();

					m.Hits = m.HitsMax;
					m.Mana = m.ManaMax;
					m.Stam = m.StamMax;

					m.Hunger = 20;
					m.Thirst = 20;

					m.Poison = null;
				}
				else if( o is Item )
				{
					Item item = (Item)o;

					if( item is BaseArmor )
						((BaseArmor)item).HitPoints = ((BaseArmor)item).MaxHitPoints;

					if( item is BaseWeapon )
						((BaseWeapon)item).HitPoints = ((BaseWeapon)item).MaxHitPoints;
				}
			}
		}
	}
}