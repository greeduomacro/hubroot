using System;
using Server;
using Server.Commands;

namespace Server.Items
{
	public class MarkContainer : LockableContainer
	{
		private bool m_AutoLock;
		private InternalTimer m_RelockTimer;
		private Map m_TargetMap;
		private Point3D m_Target;
		private string m_Description;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AutoLock
		{
			get { return m_AutoLock; }
			set
			{
				m_AutoLock = value;

				if( !m_AutoLock )
					StopTimer();
				else if( !Locked && m_RelockTimer == null )
					m_RelockTimer = new InternalTimer( this );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap { get { return m_TargetMap; } set { m_TargetMap = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Target { get { return m_Target; } set { m_Target = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Bone
		{
			get { return ItemID == 0xECA; }
			set
			{
				ItemID = value ? 0xECA : 0xE79;
				Hue = value ? 1102 : 0;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Description { get { return m_Description; } set { m_Description = value; } }

		public override bool IsDecoContainer
		{
			get { return false; }
		}

		[Constructable]
		public MarkContainer()
			: this( false )
		{
		}

		[Constructable]
		public MarkContainer( bool bone )
			: this( bone, false )
		{
		}

		[Constructable]
		public MarkContainer( bool bone, bool locked )
			: base( bone ? 0xECA : 0xE79 )
		{
			Movable = false;

			if( bone )
				Hue = 1102;

			m_AutoLock = locked;
			Locked = locked;

			if( locked )
				LockLevel = -255;
		}

		public MarkContainer( Serial serial )
			: base( serial )
		{
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public override bool Locked
		{
			get { return base.Locked; }
			set
			{
				base.Locked = value;

				if( m_AutoLock )
				{
					StopTimer();

					if( !Locked )
						m_RelockTimer = new InternalTimer( this );
				}
			}
		}

		public void StopTimer()
		{
			if( m_RelockTimer != null )
				m_RelockTimer.Stop();

			m_RelockTimer = null;
		}

		private class InternalTimer : Timer
		{
			private MarkContainer m_Container;
			private DateTime m_RelockTime;

			public MarkContainer Container { get { return m_Container; } }
			public DateTime RelockTime { get { return m_RelockTime; } }

			public InternalTimer( MarkContainer container )
				: this( container, TimeSpan.FromMinutes( 5.0 ) )
			{
			}

			public InternalTimer( MarkContainer container, TimeSpan delay )
				: base( delay )
			{
				m_Container = container;
				m_RelockTime = DateTime.Now + delay;

				Start();
			}

			protected override void OnTick()
			{
				m_Container.Locked = true;
				m_Container.LockLevel = -255;
			}
		}

		public void Mark( RecallRune rune )
		{
			if( TargetMap != null )
			{
				rune.Marked = true;
				rune.TargetMap = m_TargetMap;
				rune.Target = m_Target;
				rune.Description = m_Description;
				rune.House = null;
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			RecallRune rune = dropped as RecallRune;

			if( rune != null && base.OnDragDrop( from, dropped ) )
			{
				Mark( rune );

				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool OnDragDropInto( Mobile from, Item dropped, Point3D p )
		{
			RecallRune rune = dropped as RecallRune;

			if( rune != null && base.OnDragDropInto( from, dropped, p ) )
			{
				Mark( rune );

				return true;
			}
			else
			{
				return false;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( m_AutoLock );

			if( !Locked && m_AutoLock )
				writer.WriteDeltaTime( m_RelockTimer.RelockTime );

			writer.Write( m_TargetMap );
			writer.Write( m_Target );
			writer.Write( m_Description );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_AutoLock = reader.ReadBool();

			if( !Locked && m_AutoLock )
				m_RelockTimer = new InternalTimer( this, reader.ReadDeltaTime() - DateTime.Now );

			m_TargetMap = reader.ReadMap();
			m_Target = reader.ReadPoint3D();
			m_Description = reader.ReadString();
		}
	}
}