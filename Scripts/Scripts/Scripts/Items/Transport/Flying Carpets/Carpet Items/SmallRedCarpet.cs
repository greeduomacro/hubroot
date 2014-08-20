using System;
using Server;

namespace Server.Transport
{
	public class SmallRedCarpet : BaseFlyingCarpet
	{
		public override int NorthId { get { return 0x213; } }
		public override int EastId { get { return 0x214; } }
		public override int SouthId { get { return 0x215; } }
		public override int WestId { get { return 0x216; } }

		public SmallRedCarpet() : base() { Shadow = new Shadow( 0x21D, 0x21E, 0x21F, 0x220 ); }

		public SmallRedCarpet( Serial serial ) : base( serial ) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from != base.Owner || !base.IncludedEntities.Contains(from))
                from.SendMessage("You are not permitted to board this carpet.");

            if (!from.InRange(this.Location, 1))
                from.SendMessage("You must be closer if you wish to board this.");

            if ((from == base.Owner || base.IncludedEntities.Contains(from))
                && from.InRange(this.Location, 1))
            {
                from.MoveToWorld(this.Location, this.Map);
            }
        }

		public override void OnDestroy()
		{
			if( Owner != null )
				Owner.AddToBackpack( new SmallRedCarpetRoll() );
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

	public class SmallRedCarpetRoll : BaseCarpetRoll
	{
		protected override BaseFlyingCarpet Carpet { get { return new SmallRedCarpet(); } }

		[Constructable]
		public SmallRedCarpetRoll() : base( 0x213, Point3D.Zero ) { }

		public SmallRedCarpetRoll( Serial serial ) : base( serial ) { }

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