using System;
using Server;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2B02, 0x2B03 )]
	public class Quiver : BaseContainer, ICraftable
	{
		private Mobile m_Crafter;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get { return m_Crafter; }
			set { m_Crafter = value; InvalidateProperties(); }
		}

		public override int DefaultGumpID { get { return 0x41; } }
		public override int DefaultDropSound { get { return 0x48; } }
		public override int DefaultMaxItems { get { return 50; } }
		public override int DefaultMaxWeight { get { return 150; } }

		[Constructable]
		public Quiver()
			: base( 0x2B02 )
		{
			Layer = Layer.Cloak;
			Name = "a quiver";
			Weight = 2.0;
		}

		public Quiver( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			int arrowCount = this.GetAmount( typeof( Arrow ), true );
			int boltCount = this.GetAmount( typeof( Bolt ), true );

			if( arrowCount > 0 )
				list.Add( 1060658, "Arrows\t" + arrowCount.ToString() );

			if( boltCount > 0 )
				list.Add( 1060659, "Bolts\t" + boltCount.ToString() );

			if( m_Crafter != null )
				list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if( !(item is Arrow || item is Bolt) )
				return false;

			return base.OnDragDropInto( from, item, p );
		}

		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if( (dropped is Arrow || dropped is Bolt) && base.CheckHold( from, dropped, sendFullMessage, true ) )
			{
				from.PlaySound( GetDroppedSound( dropped ) );
				UpdateTotal( dropped, TotalType.Weight, 0 );

				for( int i = 0; i < this.Items.Count; i++ )
				{
					Item item = this.Items[i];

					if( !(item is Container) && item.StackWith( from, dropped, false ) )
					{
						UpdateTotals();
						InvalidateProperties();

						return true;
					}
				}

				DropItem( dropped );

				return true;
			}

			return false;
		}

		public override void OnItemAdded( Item item )
		{
			base.OnItemAdded( item );

			UpdateTotals();
			InvalidateProperties();
		}

		public override void OnItemRemoved( Item item )
		{
			base.OnItemRemoved( item );

			UpdateTotals();
			InvalidateProperties();
		}

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			if( makersMark )
				this.Crafter = from;

			return quality;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( (Mobile)m_Crafter );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Crafter = reader.ReadMobile();
		}
	}
}