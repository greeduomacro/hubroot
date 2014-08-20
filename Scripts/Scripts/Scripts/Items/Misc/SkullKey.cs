using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class SkullKey : Item
	{
		[Constructable]
		public SkullKey()
			: base( 0x1010 )
		{
			Name = "a skull key";
			LootType = LootType.Cursed;
		}

		public SkullKey( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 );
			}
			else
			{
				from.SendLocalizedMessage( 501662 ); //What shall I use this key on?
				from.Target = new InternalTarget( this );
			}
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

		private class InternalTarget : Target
		{
			private SkullKey m_Key;

			public InternalTarget( SkullKey key )
				: base( 2, false, TargetFlags.None )
			{
				m_Key = key;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is ILockable )
				{
					ILockable o = (ILockable)target;

					o.Locked = !o.Locked; //o.Locked is equal to the opposite of the original o.Locked boolean?

					if( o.Locked )
						from.SendLocalizedMessage( 1048000 );
					else
						from.SendLocalizedMessage( 1048001 );
				}
				else
				{
					from.SendLocalizedMessage( 501666 ); //You can't unlock that!
				}
			}
		}
	}
}
