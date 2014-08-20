using System;
using Server.Items;

namespace Server.Items
{
	public class ArmorDyeTub : DyeTub
	{
		public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.SpecialDyeTub; } }
		public override bool AllowRunebooks { get { return false; } }
		public override bool AllowFurniture { get { return false; } }
		public override bool AllowStatuettes { get { return false; } }
		public override bool AllowDyables { get { return false; } }
		public override bool AllowLeather { get { return true; } }
		public override bool AllowArmor { get { return true; } }
		public override bool AllowWeapons { get { return false; } }

		[Constructable]
		public ArmorDyeTub()
		{
			Hue = DyedHue = 0x0;
			Redyable = true;
			Name = "An Armor Dye Tub";
		}

		public ArmorDyeTub( Serial serial )
			: base( serial )
		{
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

	public class WeaponDyeTub : DyeTub
	{
		public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.SpecialDyeTub; } }
		public override bool AllowRunebooks { get { return false; } }
		public override bool AllowFurniture { get { return false; } }
		public override bool AllowStatuettes { get { return false; } }
		public override bool AllowDyables { get { return false; } }
		public override bool AllowLeather { get { return false; } }
		public override bool AllowArmor { get { return false; } }
		public override bool AllowWeapons { get { return true; } }

		[Constructable]
		public WeaponDyeTub()
		{
			Hue = DyedHue = 0x0;
			Redyable = true;
			Name = "A Weapon Dye Tub";
		}

		public WeaponDyeTub( Serial serial )
			: base( serial )
		{
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

	public class BookDyeTub : DyeTub
	{
		public override bool AllowRunebooks { get { return true; } }
		public override bool AllowFurniture { get { return false; } }
		public override bool AllowStatuettes { get { return false; } }
		public override bool AllowDyables { get { return false; } }
		public override bool AllowLeather { get { return false; } }
		public override bool AllowArmor { get { return false; } }
		public override bool AllowWeapons { get { return false; } }
		public override bool AllowBooks { get { return true; } }

		[Constructable]
		public BookDyeTub()
		{
			Hue = DyedHue = 0x0;
			Redyable = true;
			Name = "A Book Dye Tub";
		}

		public BookDyeTub( Serial serial )
			: base( serial )
		{
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

	public class TrueWhiteDyeTub : DyeTub
	{
		public override bool AllowRunebooks { get { return true; } }
		public override bool AllowFurniture { get { return true; } }
		public override bool AllowStatuettes { get { return true; } }
		public override bool AllowDyables { get { return true; } }
		public override bool AllowLeather { get { return true; } }
		public override bool AllowArmor { get { return false; } }
		public override bool AllowWeapons { get { return false; } }

		[Constructable]
		public TrueWhiteDyeTub()
		{
			Hue = DyedHue = 1150;
			Redyable = false;
			Name = "True White Dye Tub";
		}

		public TrueWhiteDyeTub( Serial serial )
			: base( serial )
		{
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

	public class RangerGreenDyeTub : DyeTub
	{
		public override bool AllowRunebooks { get { return true; } }
		public override bool AllowFurniture { get { return true; } }
		public override bool AllowStatuettes { get { return true; } }
		public override bool AllowDyables { get { return true; } }
		public override bool AllowLeather { get { return true; } }
		public override bool AllowArmor { get { return false; } }
		public override bool AllowWeapons { get { return false; } }

		[Constructable]
		public RangerGreenDyeTub()
		{
			Hue = DyedHue = 1155;
			Redyable = false;
			Name = "Ranger Green Dye Tub";
		}

		public RangerGreenDyeTub( Serial serial )
			: base( serial )
		{
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
