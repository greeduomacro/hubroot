using System;

namespace Server.Items
{
	public abstract class Hair : Item
	{
		public const int None			= 0x0;
		public const int Mohawk			= 0x2044;
		public const int PageboyHair	= 0x2045;
		public const int BunsHair		= 0x2046;
		public const int LongHair		= 0x203C;
		public const int ShortHair		= 0x203B;
		public const int PonyTail		= 0x203D;
		public const int Afro			= 0x2047;
		public const int ReceedingHair	= 0x2048;
		public const int TwoPigTails	= 0x2049;
		public const int TopKnot		= 0x204A;
		public const int KrisnaHair		= 0x204A;

		public static void SetHair( Mobile m, int hairID )
		{
			SetHair( m, hairID, 0 );
		}

		public static void SetHair( Mobile m, int hairID, int hairHue )
		{
			m.HairHue = hairHue;
			m.HairItemID = hairID;
		}

		protected Hair( int itemID )
			: this( itemID, 0 )
		{
		}

		protected Hair( int itemID, int hue )
			: base( itemID )
		{
			LootType = LootType.Blessed;
			Layer = Layer.Hair;
			Hue = hue;
		}

		public Hair( Serial serial )
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
			parent.HairItemID = this.ItemID;
			parent.HairHue = this.Hue;

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