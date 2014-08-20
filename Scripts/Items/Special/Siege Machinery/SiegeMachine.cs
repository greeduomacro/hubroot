using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class SiegeMachine : Item
	{
		#region Properties
		private Dictionary<SiegeMachineComponent, Point3D> m_Collection = new Dictionary<SiegeMachineComponent, Point3D>();
		private bool m_Open;
		private int m_MoveSound;
		private int m_StopSound;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Open { get { return m_Open; } set { m_Open = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int MoveSound { get { return m_MoveSound; } set { m_MoveSound = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int StopSound { get { return m_StopSound; } set { m_StopSound = value; } }

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public override int Hue
		{
			get { return base.Hue; }
			set
			{
				base.Hue = value;

				foreach( KeyValuePair<SiegeMachineComponent, Point3D> kvp in m_Collection )
				{
					if( kvp.Key.Hue != value )
						kvp.Key.Hue = value;
				}
			}
		}
		#endregion

		[Constructable]
		public SiegeMachine()
			: base( 0x7CD )
		{
			m_Collection = new Dictionary<SiegeMachineComponent, Point3D>();
			m_Open = (m_Collection != null);
			m_MoveSound = 237;
			m_StopSound = 244;

			Movable = false;
		}

		public SiegeMachine( Serial serial )
			: base( serial )
		{
		}

		#region Collection access
		public bool Contains( SiegeMachineComponent comp )
		{
			return m_Collection.ContainsKey( comp );
		}

		public bool AddTowerPart( SiegeMachineComponent comp, Point3D offset )
		{
			if( m_Collection == null )
				return false;

			if( m_Collection.ContainsKey( comp ) )
				m_Collection.Remove( comp );

			m_Collection.Add( comp, offset );

			return m_Collection.ContainsKey( comp );
		}

		public bool RemoveTowerPart( SiegeMachineComponent comp )
		{
			if( m_Collection == null )
				return false;

			return m_Collection.Remove( comp );
		}

		///
		public ICollection<SiegeMachineComponent> GetKeys()
		{
			return m_Collection.Keys;
		}
		#endregion

		#region Overrides
		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			List<SiegeMachineComponent> toDel = new List<SiegeMachineComponent>( m_Collection.Keys.Count );

			foreach( SiegeMachineComponent c in m_Collection.Keys )
			{
				if( c != null && !c.Deleted )
					toDel.Add( c );
			}

			for( int i = 0; i < toDel.Count; i++ )
			{
				RemoveTowerPart( toDel[i] );
				toDel[i].Tower = null;

				toDel[i].Delete();
			}

			toDel.Clear();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.AccessLevel >= AccessLevel.GameMaster )
			{
				if( m_Open )
				{
					from.SendMessage( "Select the pieces to add to this tower." );
					from.Target = new InternalTarget( this );
				}
				else
				{
					if( from.HasGump( typeof( SiegeMachineControlGump ) ) )
						from.CloseGump( typeof( SiegeMachineControlGump ) );

					from.SendGump( new SiegeMachineControlGump( this ) );
				}
			}
		}

		public override void OnLocationChange( Point3D oldLocation )
		{
			base.OnLocationChange( oldLocation );

			foreach( KeyValuePair<SiegeMachineComponent, Point3D> kvp in m_Collection )
			{
				kvp.Key.MoveToWorld( new Point3D( (this.X + kvp.Value.X), (this.Y + kvp.Value.Y), (this.Z + kvp.Value.Z) ), this.Map );
			}
		}
		#endregion

		#region Serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 );

			#region v0
			if( m_Collection != null )
			{
				writer.Write( true );
				writer.Write( (int)m_Collection.Count );

				foreach( KeyValuePair<SiegeMachineComponent, Point3D> kvp in m_Collection )
				{
					writer.Write( (SiegeMachineComponent)kvp.Key );
					writer.Write( (Point3D)kvp.Value );
				}
			}
			else
			{
				writer.Write( false );
			}

			writer.Write( (bool)m_Open );
			#endregion

			#region v1
			writer.Write( (int)m_MoveSound );
			writer.Write( (int)m_StopSound );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version >= 0 )
			{
				bool notNull = reader.ReadBool();

				if( notNull )
				{
					int count = reader.ReadInt();

					for( int i = 0; i < count; i++ )
					{
						SiegeMachineComponent comp = reader.ReadItem() as SiegeMachineComponent;
						Point3D offset = reader.ReadPoint3D();

						if( comp != null && !comp.Deleted )
							m_Collection.Add( comp, offset );
					}
				}
				else
				{
					m_Collection = new Dictionary<SiegeMachineComponent, Point3D>();
				}

				m_Open = reader.ReadBool();
			}

			if( version >= 1 )
			{
				m_MoveSound = reader.ReadInt();
				m_StopSound = reader.ReadInt();
			}
		}
		#endregion

		private class InternalTarget : Target
		{
			private SiegeMachine m_Tower;

			public InternalTarget( SiegeMachine tower )
				: base( -1, false, TargetFlags.None )
			{
				m_Tower = tower;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if( target is SiegeMachine )
				{
					SiegeMachine st = target as SiegeMachine;

					if( st == m_Tower )
					{
						m_Tower.Open = false;

						from.SendMessage( "This tower will no longer accept design pieces." );
						return;
					}
				}
				else if( target is SiegeMachineComponent )
				{
					SiegeMachineComponent comp = target as SiegeMachineComponent;
					Point3D offset;
					int x = 0, y = 0, z = 0;

					if( m_Tower.Contains( comp ) )
						m_Tower.RemoveTowerPart( comp );

					x = (comp.X - m_Tower.X);
					y = (comp.Y - m_Tower.Y);
					z = (comp.Z - m_Tower.Z);

					offset = new Point3D( x, y, z );

					comp.Tower = m_Tower;
					m_Tower.AddTowerPart( comp, offset );

					from.SendMessage( "The component has been added to the tower design. Select another, or press Esc to cancel." );
					from.Target = new InternalTarget( m_Tower );
				}
				else
				{
					from.SendMessage( "You can only insert items of type \'SiegeMachineComponent\' into the tower design." );
					from.Target = new InternalTarget( m_Tower );
				}
			}
		}
	}
}