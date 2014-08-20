using System;
using Server;
using Server.Items;

namespace Server.Engines.Crops
{
	public class CabbageCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Cabbage ); } }

		public CabbageCrop( Mobile sower )
			: base( sower, 0xC7C )
		{
		}

		public CabbageCrop( Serial serial )
			: base( serial )
		{
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

	public class CantaloupeCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Cantaloupe ); } }

		public CantaloupeCrop( Mobile sower )
			: base( sower, 0xC60 )
		{
		}

		public CantaloupeCrop( Serial serial )
			: base( serial )
		{
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

	public class CarrotCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Carrot ); } }
		public override int StandardYield { get { return 3; } }

		public CarrotCrop( Mobile sower )
			: base( sower, 0xC76 )
		{
		}

		public CarrotCrop( Serial serial )
			: base( serial )
		{
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

	public class CornCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.EarOfCorn ); } }

		public CornCrop( Mobile sower )
			: base( sower, 0xC7D )
		{
		}

		public CornCrop( Serial serial )
			: base( serial )
		{
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

	public class CottonCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Cotton ); } }

		public CottonCrop( Mobile sower )
			: base( sower, 0xC50 )
		{
		}

		public CottonCrop( Serial serial )
			: base( serial )
		{
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

	public class FlaxCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Flax ); } }
		public override int StandardYield { get { return 2; } }

		public FlaxCrop( Mobile sower )
			: base( sower, 0x1A9B )
		{
		}

		public FlaxCrop( Serial serial )
			: base( serial )
		{
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

	public class GourdCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Gourd ); } }
		public override int StandardYield { get { return 2; } }

		public GourdCrop( Mobile sower )
			: base( sower, 0xC60 )
		{
		}

		public GourdCrop( Serial serial )
			: base( serial )
		{
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

	public class HoneydewMelonCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.HoneydewMelon ); } }

		public HoneydewMelonCrop( Mobile sower )
			: base( sower, 0xC60 )
		{
		}

		public HoneydewMelonCrop( Serial serial )
			: base( serial )
		{
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

	public class LettuceCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Lettuce ); } }

		public LettuceCrop( Mobile sower )
			: base( sower, 0xC71 )
		{
		}

		public LettuceCrop( Serial serial )
			: base( serial )
		{
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

	public class OnionCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Onion ); } }
		public override int StandardYield { get { return 3; } }

		public OnionCrop( Mobile sower )
			: base( sower, 0xC6F )
		{
		}

		public OnionCrop( Serial serial )
			: base( serial )
		{
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

	public class PotatoCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Potato ); } }
		public override int StandardYield { get { return 3; } }

		public PotatoCrop( Mobile sower )
			: base( sower, 0xC69 )
		{
		}

		public PotatoCrop( Serial serial )
			: base( serial )
		{
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

	public class PumpkinCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Pumpkin ); } }

		public PumpkinCrop( Mobile sower )
			: base( sower, 0xC6B )
		{
		}

		public PumpkinCrop( Serial serial )
			: base( serial )
		{
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

	public class TomatoCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Tomato ); } }
		public override int StandardYield { get { return 4; } }

		public TomatoCrop( Mobile sower )
			: base( sower, 0xC60 )
		{
		}

		public TomatoCrop( Serial serial )
			: base( serial )
		{
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

	public class RosePlant : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Rose ); } }

		public RosePlant( Mobile sower )
			: base( sower, 0x18EA )
		{
			Name = "a full grown rose";
		}

		public RosePlant( Serial serial )
			: base( serial )
		{
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

	public class SquashCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Squash ); } }
		public override int StandardYield { get { return 2; } }

		public SquashCrop( Mobile sower )
			: base( sower, 0xC60 )
		{
		}

		public SquashCrop( Serial serial )
			: base( serial )
		{
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

	public class TurnipCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Turnip ); } }

		public TurnipCrop( Mobile sower )
			: base( sower, 0xC63 )
		{
		}

		public TurnipCrop( Serial serial )
			: base( serial )
		{
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

	public class WatermelonCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.Watermelon ); } }

		public WatermelonCrop( Mobile sower )
			: base( sower, 0xC5C )
		{
		}

		public WatermelonCrop( Serial serial )
			: base( serial )
		{
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

	public class WheatCrop : BaseGrownCrop
	{
		public override Type CropType { get { return typeof( Server.Items.WheatSheaf ); } }

		public WheatCrop( Mobile sower )
			: base( sower, 0xC5A )
		{
		}

		public WheatCrop( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !from.InRange( this, 2 ) )
			{
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
				return;
			}

			WheatSheaf sheaf = null;

			try { sheaf = CreateCrop( this.StandardYield ) as WheatSheaf; }
			catch { }

			if( sheaf == null )
			{
				from.SendMessage( "You are unable to harvest any of this crop!" );

				new Weeds().MoveToWorld( this.Location, this.Map );
				this.Delete();
			}
			else
			{
				Scythe scythe = from.FindItemOnLayer( Layer.TwoHanded ) as Scythe;

				if( scythe != null )
				{
					from.AddToBackpack( sheaf );

					from.Animate( 13, 7, 1, true, false, 0 );
					from.PlaySound( 0x133 );
					from.SendMessage( "You successfully harvest the crop!" );

					this.Delete();
				}
				else
				{
					from.SendMessage( "You need a scythe to harvest this crop!" );

					sheaf.Delete();
				}
			}
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