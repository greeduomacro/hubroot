using System;
using Server;

namespace Server.Items
{
	public class RingOfResurrection : SilverRing
	{
		[Constructable]
		public RingOfResurrection()
			: base()
		{
			LootType = LootType.Cursed;
			Name = "a ring of resurrection";
		}

		public RingOfResurrection( Serial serial ) : base( serial ) { }

		public override bool OnEquip( Mobile from )
		{
			Movable = false;
			Effects.SendBoltEffect( from );
			Effects.SendTargetParticles( from, 0x3789, 1, 30, 0, EffectLayer.CenterFeet );

			from.SendMessage( "The ring tightens around your finger as you slip it on!" );

			return base.OnEquip( from );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}