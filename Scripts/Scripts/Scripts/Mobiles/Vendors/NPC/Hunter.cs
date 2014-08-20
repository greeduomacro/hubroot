using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Hunter : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get{ return m_SBInfos; } }
		
		[Constructable]
		public Hunter() : base( "the hunter" )
		{
			Fame = 6000;
			Karma = 2000;
			
			VirtualArmor = 100;
		}
		
		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBHunter() );
		}
		
		public override void InitBody()
		{
			InitStats( 150, 135, 100 );
			
			Hue = Utility.RandomSkinHue();
			SpeechHue = 906;
			
			BodyValue = 400;
			Name = NameList.RandomName( "BacktrolMale" ) + ",";
		}
		
		public override void InitOutfit()
		{
			AddItem( new PlateGorget() );
			AddItem( new ChainChest() );
			AddItem( new PlateLegs() );
			AddItem( new PlateArms() );
			AddItem( new PlateGloves() );
			AddItem( new Cloak( Utility.RandomNeutralHue() ) );

			HairItemID = Hair.LongHair;
			HairHue = Utility.RandomHairHue();
		}
		
		public Hunter( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}
