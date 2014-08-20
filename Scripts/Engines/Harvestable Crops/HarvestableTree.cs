using System;
using Server;
using Server.Items;

namespace Server.Engines.Crops
{
	public class HarvestableTree : BaseGrownCrop, IChopable
	{
		private int m_TrunkID;
		private Static m_Trunk;
		private bool m_YieldingFruit;

		public virtual int TrunkID { get { return m_TrunkID; } set { m_TrunkID = value; } }
		public virtual Static Trunk { get { return m_Trunk; } set { m_Trunk = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool YieldingFruit { get { return m_YieldingFruit; } set { m_YieldingFruit = value; } }

		public override int StandardYield { get { return 4; } }
		public override int AnimationFrame { get { return 269; } }

		public HarvestableTree( Mobile sower, int itemID )
			: base( sower, itemID )
		{
			m_YieldingFruit = true;

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerCallback( AddTrunk ) );
		}

		public HarvestableTree( Serial serial )
			: base( serial )
		{
		}

		public virtual void AddTrunk()
		{
			if( m_Trunk != null )
				m_Trunk.Delete();

			if( TrunkID > 0 )
			{
				m_TrunkID = TrunkID;
				m_Trunk = new Static( TrunkID );
				m_Trunk.Name = "a tree trunk";
				m_Trunk.MoveToWorld( this.Location, this.Map );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( YieldingFruit )
				OnDoubleClick( from, false );
			else
				from.SendMessage( "This tree is not currently bearing fruit." );
		}

		public override void OnHarvest( Mobile from )
		{
			m_YieldingFruit = false;

			Timer.DelayCall( TimeSpan.FromHours( 1.0 ), new TimerCallback( ResetYieldingFruit ) );
		}

		public virtual void ResetYieldingFruit()
		{
			m_YieldingFruit = true;
		}

		public virtual void OnChop( Mobile from )
		{
			double chance = (from.Skills[SkillName.Lumberjacking].Value * 0.20);

			if( !from.InRange( this, 2 ) )
			{
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
			}
			else if( Sower != null && from != Sower )
			{
				from.SendMessage( "You cannot chop down this tree." );
			}
			else if( chance > Utility.RandomDouble() )
			{
				from.Animate( 13, 7, 1, true, false, 0 );
				from.PlaySound( 0x13E );
				from.SendMessage( "You manage to cut down the tree." );

				new Weeds().MoveToWorld( this.Location, this.Map );
				new Server.Items.TimedItem( 60.0, 0x1038 ).MoveToWorld( this.Location, this.Map );

				from.AddToBackpack( new Log( 10 ) );

				this.Delete();
			}
			else
			{
				from.SendMessage( "You find yourself unable to cut down this tree right now." );
			}
		}

		public override void OnAfterDelete()
		{
			if( m_Trunk != null )
				m_Trunk.Delete();

			base.OnAfterDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			writer.Write( (int)m_TrunkID );
			writer.Write( (Static)m_Trunk );

			writer.Write( (bool)m_YieldingFruit );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_TrunkID = reader.ReadInt();
			m_Trunk = reader.ReadItem() as Static;

			if( m_Trunk != null && (m_Trunk.ItemID > 0 && m_Trunk.ItemID != m_TrunkID) )
				m_Trunk.ItemID = m_TrunkID;

			if( version >= 1 )
				m_YieldingFruit = reader.ReadBool();

			Timer.DelayCall( TimeSpan.FromMinutes( 15.0 ), new TimerCallback( ResetYieldingFruit ) );
		}
	}
}