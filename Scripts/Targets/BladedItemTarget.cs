using System;
using Server.Engines.Crops;
using Server.Engines.Harvest;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Targets
{
	public class BladedItemTarget : Target
	{
		private Item m_Item;

		public BladedItemTarget( Item item )
			: base( 2, false, TargetFlags.None )
		{
			m_Item = item;
		}

		protected override void OnTargetOutOfRange( Mobile from, object targeted )
		{
			if( targeted is UnholyBone && from.InRange( ((UnholyBone)targeted), 12 ) )
				((UnholyBone)targeted).Carve( from, m_Item );
			else
				base.OnTargetOutOfRange( from, targeted );
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if( m_Item.Deleted )
				return;

			if( targeted is ICarvable )
			{
				((ICarvable)targeted).Carve( from, m_Item );
			}
			else if( targeted is ForestWyrm && ((ForestWyrm)targeted).HasBarding )
			{
				ForestWyrm pet = (ForestWyrm)targeted;

				if( !pet.Controlled || pet.ControlMaster != from )
					from.SendLocalizedMessage( 1053022 ); // You cannot remove barding from a swamp dragon you do not own.
				else
					pet.HasBarding = false;
			}
			#region CropSystem
			else if( targeted is Weeds )
			{
				((Weeds)targeted).Delete();

				from.SendMessage( "You slice the roots of the weed and toss them away." );
			}
			else if( targeted is Food && ((Food)targeted).SeedType != null )
			{
				try
				{
					Food food = targeted as Food;
					int amount = (food.Amount * Utility.RandomMinMax( 2, 6 ));
					Item seed = Activator.CreateInstance( food.SeedType, amount ) as Item;

					if( seed != null )
					{
						from.AddToBackpack( seed );
						from.SendMessage( String.Format( "You slice open the plant{0} and remove {1} seeds.",
							(food.Amount > 1 ? "s" : ""), (food.Amount > 1 ? "their" : "its") ) );
					}

					food.Delete();
				}
				catch { }
			}
			else if( targeted is Flax )
			{
				int amount = (((Flax)targeted).Amount * Utility.RandomMinMax( 2, 6 ));

				from.AddToBackpack( new FlaxSeed( amount ) );
				from.SendMessage( "You cut off the stalk of the flax and throw it away, keeping the seeds for later use." );

				((Flax)targeted).Delete();
			}
			else if( targeted is WheatSheaf )
			{
				int amount = (((WheatSheaf)targeted).Amount * Utility.RandomMinMax( 3, 8 ));

				from.AddToBackpack( new WheatSeed( amount ) );
				from.SendMessage( "You split open the wheat and take all the seeds you can find." );

				((WheatSheaf)targeted).Delete();
			}
			else if( targeted is Rose )
			{
				from.AddToBackpack( new RoseBud( Utility.RandomMinMax( 3, 6 ) ) );
				from.SendMessage( "You break away the petals of the flowers and gently remove the buds." );

				((Rose)targeted).Delete();
			}
			#endregion
			else
			{
				HarvestSystem system = Lumberjacking.System;
				HarvestDefinition def = Lumberjacking.System.Definition;

				int tileID;
				Map map;
				Point3D loc;

				if( !system.GetHarvestDetails( from, m_Item, targeted, out tileID, out map, out loc ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else if( !def.Validate( tileID ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else
				{
					HarvestBank bank = def.GetBank( map, loc.X, loc.Y );

					if( bank == null )
						return;

					if( bank.Current < 5 )
					{
						from.SendLocalizedMessage( 500493 ); // There's not enough wood here to harvest.
					}
					else
					{
						bank.Consume( 5, from );

						Item item = new Kindling();

						if( from.PlaceInBackpack( item ) )
						{
							from.SendLocalizedMessage( 500491 ); // You put some kindling into your backpack.
							from.SendLocalizedMessage( 500492 ); // An axe would probably get you more wood.
						}
						else
						{
							from.SendLocalizedMessage( 500490 ); // You can't place any kindling into your backpack!

							item.Delete();
						}
					}
				}
			}
		}
	}
}
