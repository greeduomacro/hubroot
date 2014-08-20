using System;
using Server;

namespace Server.Items
{
	public class BagOfHolding : Bag
	{
		private double m_Redux;

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int ReductionPercent
		{
			get { return (int)(m_Redux * 100); }
			set
			{
				if( value < 0 )
					value = 0;
				if( value > 100 )
					value = 100;
				m_Redux = ((double)value) / 100;
				if( Parent is Item )
				{
					(Parent as Item).UpdateTotals();
					(Parent as Item).InvalidateProperties();
				}
				else if( Parent is Mobile )
					(Parent as Mobile).UpdateTotals();
				else
					UpdateTotals();
				InvalidateProperties();
			}
		}

		public override string DefaultName { get { return "Bag of Holding"; } }
		public override double DefaultWeight { get { return 0.0; } }

		[Constructable]
		public BagOfHolding()
			: this( 10 )
		{
		}

		[Constructable]
		public BagOfHolding( int maxItems )
			: base()
		{
			LootType = LootType.Blessed;

			MaxItems = maxItems;
		}

		public BagOfHolding( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060658, "Item capacity\t{0}", MaxItems ); // ~1_val~: ~2_val~

			if( TotalWeight != 0 )
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( InvalidateProperties ) );
		}

		public override void UpdateTotal( Item sender, TotalType type, int delta )
		{
			base.UpdateTotal( sender, type, delta );

			if( type == TotalType.Weight )
			{
				if( Parent is Item )
					(Parent as Item).UpdateTotal( sender, type, (int)(delta * m_Redux) * -1 );
				else if( Parent is Mobile )
					(Parent as Mobile).UpdateTotal( sender, type, (int)(delta * m_Redux) * -1 );
			}
		}

		public override int GetTotal( TotalType type )
		{
			if( type == TotalType.Weight )
				return (int)(base.GetTotal( type ) * (1.0 - m_Redux));

			return base.GetTotal( type );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if( dropped is BaseContainer )
				return false;
			else
				return base.OnDragDrop( from, dropped );
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if( item is BaseContainer )
				return false;
			else
				return base.OnDragDropInto( from, item, p );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			//version 1
			writer.Write( (double)m_Redux );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version >= 1 )
				m_Redux = reader.ReadDouble();
		}
	}
}
