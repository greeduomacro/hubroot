using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class MageCorpse : Corpse
	{
		private static bool MobileSex;

		private static Mobile GetOwner()
		{
			Mobile m_Corpse = new Mobile();

			MobileSex = Utility.RandomBool(); // true for female, false for male (no reason)
			m_Corpse.Hue = Utility.RandomSkinHue();

			Utility.AssignRandomHair( m_Corpse, true );

			if( MobileSex )
			{
				m_Corpse.Female = true;
				m_Corpse.Body = 0x191;
				m_Corpse.Name = NameList.RandomName( "female" ); // don't need a name, but it can't hurt
			}
			else
			{
				m_Corpse.Female = false;
				m_Corpse.Body = 0x190;
				m_Corpse.Name = NameList.RandomName( "male" ); // don't need a name, but it can't hurt

				Utility.AssignRandomFacialHair( m_Corpse, m_Corpse.HairHue );
			}

			m_Corpse.Delete();

			return m_Corpse;
		}

		private static System.Collections.Generic.List<Item> GetEquipment()
		{
			System.Collections.Generic.List<Item> list = new System.Collections.Generic.List<Item>();

			list.Add( new Robe( Utility.RandomYellowHue() ) );
			list.Add( new WizardsHat( Utility.RandomYellowHue() ) );
			list.Add( new Sandals() );

			list.Add( new Spellbook() );
			return list;
		}

		[Constructable]
		public MageCorpse()
			: base( GetOwner(), GetEquipment() )
		{
			switch( Utility.Random( 4 ) )
			{
				case 0:
					{
						Direction = Direction.North;
						break;
					}
				case 1:
					{
						Direction = Direction.East;
						break;
					}
				case 2:
					{
						Direction = Direction.South;
						break;
					}
				case 3:
					{
						Direction = Direction.West;
						break;
					}
			}


			Carved = true;
			Channeled = true;

			foreach( Item item in EquipItems )
			{
				DropItem( item );
			}

		}

		public MageCorpse( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if( ItemID == 0x2006 ) // Corpse form
			{
				//				list.Add( "a mage's corpse" );
				list.Add( "The corpse of " + this.Name );
			}
		}

		public override void Open( Mobile from, bool checkSelfLoot )
		{
			if( !from.InRange( this.GetWorldLocation(), 2 ) )
				return;

			PlayerMobile player = from as PlayerMobile;
			player.SendMessage( "For some reason disturbing the dead doesn't sit too well with you." );
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}