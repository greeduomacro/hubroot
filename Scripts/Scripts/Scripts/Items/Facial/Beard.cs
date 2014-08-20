using System;

namespace Server.Items
{
	public abstract class Beard : Item
	{
		public const int None = 0x0;
		public const int LongBeard = 0x203E;
		public const int ShortBeard = 0x203F;
		public const int Goatee = 0x2040;
		public const int Mustache = 0x2041;
		public const int MediumShortBeard = 0x204B;
		public const int MediumLongBeard = 0x204C;
		public const int Vandyke = 0x204D;

		public static void SetFacial( Mobile m, int facialID )
		{
			SetFacial( m, facialID, 0 );
		}

		public static void SetFacial( Mobile m, int facialID, int facialHue )
		{
			m.FacialHairHue = facialHue;
			m.FacialHairItemID = facialID;
		}

		protected Beard( int itemID )
			: this( itemID, 0 )
		{
		}

		protected Beard( int itemID, int hue )
			: base( itemID )
		{
			LootType = LootType.Blessed;
			Layer = Layer.FacialHair;
			Hue = hue;
		}

		public Beard( Serial serial )
			: base( serial )
		{
		}

		public override bool DisplayLootType { get { return false; } }

		public override bool VerifyMove( Mobile from )
		{
			return (from.AccessLevel >= AccessLevel.GameMaster);
		}

		public override DeathMoveResult OnParentDeath( Mobile parent )
		{
			//Dupe( Amount );

			parent.FacialHairItemID = this.ItemID;
			parent.FacialHairHue = this.Hue;

			return DeathMoveResult.MoveToCorpse;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}
	}
}