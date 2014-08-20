using System;
using Server;

namespace Server.Engines.Crops
{
	public class BaseTreeSapling : BaseGrownCrop
	{
		private Type m_FullTreeType;
		private Timer m_Timer;

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual Type FullTreeType { get { return m_FullTreeType; } set { m_FullTreeType = value; } }

		public BaseTreeSapling( Mobile sower )
			: base( sower, 0xCE9 )
		{
			m_Timer = new InternalTimer( this );
			m_Timer.Start();

			Name = "a sapling";
		}

		public BaseTreeSapling( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.Mounted )
			{
				from.SendMessage( "You cannot harvest crops while mounted." );
			}
			else
			{
				Item oneHanded = from.FindItemOnLayer( Layer.OneHanded );
				Item twoHanded = from.FindItemOnLayer( Layer.TwoHanded );

				if( oneHanded != null && oneHanded is Server.Items.BaseAxe )
				{
					Chop( from );
				}
				else if( twoHanded != null && twoHanded is Server.Items.BaseAxe )
				{
					Chop( from );
				}
				else
				{
					from.SendMessage( "You need an axe to cut down this sapling!" );
				}
			}
		}

		public virtual void Chop( Mobile m )
		{
			m.Animate( 13, 7, 1, true, false, 0 );
			m.PlaySound( 0x13E );
			m.SendMessage( "Your axe cuts through the thin trunk with ease." );

			new Weeds().MoveToWorld( this.Location, this.Map );
			new Server.Items.TimedItem( 60.0, 0x1038 ).MoveToWorld( this.Location, this.Map );

			m.AddToBackpack( new Server.Items.Kindling( 4 ) );

			if( m_Timer != null )
				m_Timer.Stop();
			this.Delete();
		}

		public virtual void GrowTree()
		{
			Item tree = null;

			try { tree = Activator.CreateInstance( FullTreeType, this.Sower ) as Item; }
			catch( Exception e ) { Server.Utilities.ExceptionManager.LogException( "BaseTreeSapling.cs", e ); }

			if( tree == null )
				tree = new Weeds();

			tree.MoveToWorld( this.Location, this.Map );

			if( m_Timer != null )
				m_Timer.Stop();
			this.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_FullTreeType == null ? null : m_FullTreeType.FullName );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			string type = reader.ReadString();
			if( type != null )
				m_FullTreeType = ScriptCompiler.FindTypeByFullName( type );

			Timer.DelayCall( TimeSpan.FromMinutes( 15.0 ), new TimerCallback( GrowTree ) );
		}

		private class InternalTimer : Timer
		{
			private BaseTreeSapling m_Sapling;

			public InternalTimer( BaseTreeSapling sapling )
				: base( TimeSpan.FromHours( 8.0 ) )
			{
				m_Sapling = sapling;

				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				if( m_Sapling != null && !m_Sapling.Deleted )
				{
					m_Sapling.GrowTree();
				}

				Stop();
			}
		}
	}
}