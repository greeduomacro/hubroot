using System;
using Server;

namespace Server.Engines.Crops
{
	public class CropSeedling : BaseCrop
	{
		private Mobile m_Sower;
		private Type m_FullCrop;
		private Timer m_Timer;

		[CommandProperty( AccessLevel.Counselor )]
		public Type FullCropType { get { return m_FullCrop; } }

		public CropSeedling( Type fullCropType, int itemID )
			: base( itemID )
		{
			m_FullCrop = fullCropType;

			Movable = false;

			if( itemID == 0x913 )
				Name = "a dirt patch";
			else
				Name = "a seedling";
		}

		public CropSeedling( Serial serial )
			: base( serial )
		{
		}

		public virtual void Plant( Mobile from, BaseCropSeed tempSeed )
		{
			m_Sower = from;

			Timer.DelayCall( TimeSpan.FromMinutes( 30.0 ), new TimerStateCallback( FinishGrowth ),
				new object[] { from.Location, from.Map, tempSeed } );
		}

		public virtual void FinishGrowth( object state )
		{
			object[] args = state as object[];

			Point3D targetLoc = (Point3D)args[0];
			Map targetMap = args[1] as Map;
			BaseCropSeed seed = args[2] as BaseCropSeed;

			MoveToWorld( targetLoc, targetMap );

			m_Timer = new InternalTimer( m_Sower, this );
			m_Timer.Start();

			if( seed != null )
				seed.Delete();
		}

		public virtual void Grow( object state )
		{
			Mobile sower = state as Mobile;

			if( sower == null || this == null || this.Deleted )
				return;

			Item item = Activator.CreateInstance( m_FullCrop, sower ) as Item;

			if( item == null )
				item = new Weeds();
			else
			{
				if( item is BananaSapling )
					item.ItemID = 0xCA8;
				else if( item is CoconutPalmSapling )
					item.ItemID = 0xC9A;
				else if( item is DatePalmSapling )
					item.ItemID = 0xC99;
			}

			item.MoveToWorld( this.Location, this.Map );

			this.Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !from.InRange( this, 2 ) )
			{
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
				return;
			}

			if( m_Timer != null && m_Timer.Running )
				m_Timer.Stop();

			from.Direction = from.GetDirectionTo( this );
			from.Animate( 32, 5, 1, true, false, 0 );
			from.SendMessage( "You uproot the seedling with ease." );

			this.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Sower );
			writer.Write( m_FullCrop == null ? null : m_FullCrop.FullName );

            if( m_Timer != null && m_Timer.Running )
            {
                writer.Write( true );
                writer.Write( m_Timer.Next );
            }
            else
            {
                writer.Write( false );
            }
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Sower = reader.ReadMobile();
			string type = reader.ReadString();
			if( type != null )
				m_FullCrop = ScriptCompiler.FindTypeByFullName( type );

            if( reader.ReadBool() )
            {
                m_Timer = new InternalTimer( m_Sower, this );

                DateTime next = reader.ReadDateTime();

                if( next < DateTime.Now )
                    next = DateTime.Now;

                m_Timer.Delay = (next - DateTime.Now);
                m_Timer.Start();
            }
            else
            {
                Timer.DelayCall( TimeSpan.FromMinutes( 15.0 ), new TimerStateCallback( Grow ), m_Sower );
            }
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Sower;
			private CropSeedling m_Seedling;

			public InternalTimer( Mobile sower, CropSeedling seedling )
				: base( TimeSpan.FromHours( 4.0 ) )
			{
				m_Sower = sower;
				m_Seedling = seedling;

				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				if( m_Seedling != null && !m_Seedling.Deleted )
				{
					m_Seedling.Grow( m_Sower );
				}

				Stop();
			}
		}
	}
}