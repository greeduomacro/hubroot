namespace Server.Items
{
	public abstract class BaseScabbard : BaseContainer
	{
		public override int DefaultMaxItems { get { return 1; } }
		public override int DefaultMaxWeight { get { return 25; } }

		public BaseScabbard( int itemId ) : this( itemId, 0 ) { }

		public BaseScabbard( int itemId, int hue )
			: base( itemId )
		{
			Hue = hue;
			Name = "a scabbard";
			Weight = 1.0;
		}

		public BaseScabbard( Serial serial ) : base( serial ) { }

		public override bool IsAccessibleTo( Mobile m )
		{
			if( IsChildOf( m ) || m.AccessLevel >= AccessLevel.Counselor )
				return true;

			return false;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if( !(item is BaseSword) )
				return false;

			return base.OnDragDropInto( from, item, p );
		}

		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if( !(dropped is BaseSword) )
				return false;

			return base.TryDropItem( from, dropped, sendFullMessage );
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

	public class SideScabbard : BaseScabbard
	{
		[Constructable]
		public SideScabbard() : this( 0 ) { }

		[Constructable]
		public SideScabbard( int hue )
			: base( 0x3DC2, hue )
		{
			Layer = Layer.MiddleTorso;
			Name = "a side scabbard";
		}

		public SideScabbard( Serial serial ) : base( serial ) { }

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

	public class RearScabbard : BaseScabbard
	{
		[Constructable]
		public RearScabbard() : this( 0 ) { }

		[Constructable]
		public RearScabbard( int hue )
			: base( 0x3DC8, hue )
		{
			Layer = Layer.Waist;
			Name = "a rear scabbard";
		}

		public RearScabbard( Serial serial ) : base( serial ) { }

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
}