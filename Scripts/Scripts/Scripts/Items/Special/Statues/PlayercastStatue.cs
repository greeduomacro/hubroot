using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.PlayercastStatues
{
	public static class BaseStatue
	{
		private static List<PlayercastStatue> m_List = new List<PlayercastStatue>();

		public static List<PlayercastStatue> AllStatues
		{
			get { return m_List; }
		}

		public static void Initialize()
		{
			Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), new TimerCallback( UpdateStatues ) );
		}

		public static void UpdateStatues()
		{
			if( NetState.Instances.Count > 0 )
			{
				for( int i = 0; i < m_List.Count; i++ )
				{
					m_List[i].RoutineUpdate();
				}
			}

			Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback( UpdateStatues ) );
		}

		public static void AddStatue( PlayercastStatue statue )
		{
			if( m_List.Contains( statue ) )
				m_List.Remove( statue );

			m_List.Add( statue );
		}

		public static void RemoveStatue( PlayercastStatue statue )
		{
			if( m_List.Contains( statue ) )
				m_List.Remove( statue );
		}
	}

	#region Enumerations
	public enum Poses
	{
		AllPraiseMe,
		Casting,
		Fighting,
		HandsOnHips,
		Ready,
		Salute
	}

	public enum MaterialType
	{
		None = 0x0,
		//metal hues
		DullCopper = 0x973,
		ShadowIron = 0x966,
		Copper = 0x96D,
		Bronze = 0x972,
		Gold = 0x8A5,
		Agapite = 0x979,
		Verite = 0x89F,
		Valorite = 0x8AB,
		//OSI standards
		Alabaster1 = 0xB8F,
		Alabaster2 = 0xB90,
		Alabaster3 = 0xB91,
		Alabaster4 = 0xB92,
		Alabaster5 = 0xB89,
		Bloodstone = 0xB85,
		Bronze1 = 0xB98,
		Bronze2 = 0xB99,
		Bronze3 = 0xB9A,
		CorrodedBronze1 = 0xB87,
		CorrodedBronze2 = 0xB88,
		CorrodedBronze3 = 0xB97,
		Granite = 0xB8E,
		GrayMarble = 0xB8B,
		GreenMarble = 0xB8C,
		Jade1 = 0xB93,
		Jade2 = 0xB94,
		Jade3 = 0xB95,
		Jade4 = 0xB96,
		Jade5 = 0xB83
	}
	#endregion

	public class PlayercastStatue : Mobile
	{
		private int m_Animation;
		private int m_FrameCount;
		private bool m_AnimateForward;

		#region Properties
		private Plinth m_Plinth;
		private bool m_HasPlinth;

		private MaterialType m_Material;

		private Mobile m_Owner;

		private Poses m_Pose;

		public string Engraving
		{
			get { return m_Plinth.Engraving; }
			set { m_Plinth.Engraving = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasPlinth
		{
			get { return (m_Plinth != null && !m_Plinth.Deleted); }
			set
			{
				if( value )
				{
					if( m_Plinth == null || m_Plinth.Deleted )
					{
						m_Plinth = new Plinth( this );

						this.Z += 5;
						ValidateLocation();
					}
				}
				else
				{
					if( m_Plinth != null && !m_Plinth.Deleted )
					{
						m_Plinth.Statue = null;
						m_Plinth.Delete();

						this.Z -= 5;
					}

					ValidateLocation();
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MaterialType Material
		{
			get { return m_Material; }
			set
			{
				m_Material = value;

				Rehue( (int)value );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poses Pose
		{
			get { return m_Pose; }
			set
			{
				m_Pose = value;

				Reanimate();
				BeginUpdate();
			}
		}
		#endregion

		#region Property Overrides
		[Hue, CommandProperty( AccessLevel.Counselor )]
		public override int Hue
		{
			get { return base.Hue; }
			set
			{
				base.Hue = value;

				if( m_Plinth != null && m_Plinth.Hue != value )
					m_Plinth.Hue = value;
			}
		}
		#endregion

		public PlayercastStatue( Mobile owner )
		{
			BaseStatue.AddStatue( this );

			Blessed = true;
			Body = 0x190;
			Hue = 0;
			Name = "a statue";

			m_Material = MaterialType.GrayMarble;
			m_Owner = owner;
			m_Pose = Poses.Ready;

			BeginUpdate();
		}

		public PlayercastStatue( Serial serial )
			: base( serial )
		{
		}

		private void BeginUpdate()
		{
			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( RoutineUpdate ) );
		}

		private void Reanimate()
		{
			switch( m_Pose )
			{
				case Poses.AllPraiseMe:
					{
						m_Animation = 17;
						m_FrameCount = 5;
						m_AnimateForward = false;
					} break;
				case Poses.Casting:
					{
						m_Animation = 16;
						m_FrameCount = 5;
						m_AnimateForward = false;
					} break;
				case Poses.Fighting:
					{
						m_Animation = 9;
						m_FrameCount = 5;
						m_AnimateForward = false;
					} break;
				case Poses.HandsOnHips:
					{
						m_Animation = 6;
						m_FrameCount = 7;
						m_AnimateForward = true;
					} break;
				case Poses.Ready:
					{
						m_Animation = 4;
						m_FrameCount = 1;
						m_AnimateForward = true;
					} break;
				case Poses.Salute:
					{
						m_Animation = 33;
						m_FrameCount = 3;
						m_AnimateForward = false;
					} break;
				default: break;
			}

			if( this.Mount != null )
			{
				m_Animation = 25;
				m_FrameCount = 5;
				m_AnimateForward = false;
			}
		}

		private void Rehue( int newHue )
		{
			if( m_Plinth != null )
				m_Plinth.Hue = newHue;

			for( int i = 0; i < this.Items.Count; i++ )
				this.Items[i].Hue = newHue;

			if( this.Backpack != null )
			{
				for( int i = 0; i < this.Backpack.Items.Count; i++ )
					this.Backpack.Items[i].Hue = newHue;
			}

			this.Hue = this.SolidHueOverride = newHue;
		}

		public void RoutineUpdate()
		{
			if( this != null && !this.Deleted )
			{
				Reanimate();

				if( Map.GetSector( this ) != null && Map.GetSector( this ).Active )
				{
					if( this.Mounted )
						Animate( m_Animation, m_FrameCount, 1, m_AnimateForward, false, 5 );
					else
						Animate( m_Animation, m_FrameCount, 1, m_AnimateForward, false, 255 );
				}
			}
		}

		private void ValidateLocation()
		{
			Point3D plinthLoc = new Point3D( this.X, this.Y, this.Z - 5 );

			if( m_Plinth != null )
			{
				if( m_Plinth.Map != this.Map )
					m_Plinth.Map = this.Map;

				if( m_Plinth.Location != plinthLoc )
					m_Plinth.Location = plinthLoc;
			}
		}

		#region Overrides
		public override bool CanBeDamaged()
		{
			return false;
		}

		public override void Delta( MobileDelta flag )
		{
			base.Delta( flag );

			BeginUpdate();
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			RoutineUpdate();
		}

		public override void OnAosSingleClick( Mobile from )
		{
			base.OnAosSingleClick( from );

			RoutineUpdate();
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if( m_Plinth != null )
				m_Plinth.Delete();

			if( this.Mount != null && this.Mount is BaseMount && !((BaseMount)this.Mount).Deleted )
				((BaseMount)this.Mount).Delete();

			BaseStatue.RemoveStatue( this );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if( m.CanSee( this ) )
				RoutineUpdate();
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{
			base.OnLocationChange( oldLocation );

			ValidateLocation();

			BeginUpdate();
		}
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			#region v0
			writer.Write( (Plinth)m_Plinth );

			writer.Write( (int)m_Material );

			writer.Write( (Mobile)m_Owner );

			writer.Write( (int)m_Pose );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			#region v0
			m_Plinth = reader.ReadItem() as Plinth;
			m_HasPlinth = (m_Plinth != null);

			m_Material = (MaterialType)reader.ReadInt();

			m_Owner = reader.ReadMobile();

			m_Pose = (Poses)reader.ReadInt();
			#endregion

			if( !BaseStatue.AllStatues.Contains( this ) )
				BaseStatue.AddStatue( this );

			BeginUpdate();
		}
	}

	[Flipable( 0x14ED, 0x14EE )]
	public class StatueDeed : Item
	{
		#region Properties
		private bool m_HasPlinth;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasPlinth
		{
			get { return m_HasPlinth; }
			set { m_HasPlinth = value; }
		}
		#endregion

		[Constructable]
		public StatueDeed()
			: this( true )
		{
		}

		[Constructable]
		public StatueDeed( bool hasPlinth )
			: base( 0x14ED )
		{
			LootType = LootType.Blessed;
			Name = "rolled up statue design plans";
			Weight = 1.0;

			m_HasPlinth = hasPlinth;
		}

		public StatueDeed( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from ) && IsAccessibleTo( from ) )
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if( ValidateHouse( house, from ) || from.AccessLevel >= AccessLevel.Counselor )
				{
					PlayercastStatue statue = new PlayercastStatue( from );
					statue.Direction = from.Direction;

					if( CloneMobile( from, statue ) )
					{
						statue.MoveToWorld( new Point3D( from.Location ), from.Map );
						statue.HasPlinth = m_HasPlinth;
						statue.Material = MaterialType.GrayMarble;

						this.Delete();
					}
					else
					{
						from.SendMessage( "Statue construction failed. Please contact your administrator." );

						statue.Delete();
					}
				}
				else
					from.SendLocalizedMessage( 502092 ); //You must be in your house to do this.
			}
			else
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
		}

		private bool ValidateHouse( BaseHouse house, Mobile m )
		{
			if( house == null )
				return false;
			else if( house.IsOwner( m ) || house.IsCoOwner( m ) )
				return true;

			return false;
		}

		private bool CloneMobile( Mobile m, PlayercastStatue statue )
		{
			bool success = false;

			try
			{
				statue.Body = m.Body;
				statue.Female = m.Female;
				statue.Hue = m.Hue;
				statue.RawName = String.Format( "a statue of {0}", m.RawName );

				statue.RawDex = m.RawDex;
				statue.RawInt = m.RawInt;
				statue.RawStr = m.RawStr;

				statue.Hits = statue.HitsMax;
				statue.Mana = statue.ManaMax;
				statue.Stam = statue.StamMax;

				statue.HairItemID = m.HairItemID;
				statue.FacialHairItemID = m.FacialHairItemID;

				statue.HairHue = m.HairHue;
				statue.FacialHairHue = m.FacialHairHue;

				for( int i = 0; i < m.Items.Count; i++ )
				{
					Item item = m.Items[i];

					if( item != null && !item.Deleted )
					{
						if( item.RootParent == m && item != m.Backpack && item != m.BankBox && item.Layer != Layer.Mount )
						{
							Item newItem = new Item( item.ItemID );
							newItem.Hue = item.Hue;
							newItem.Layer = item.Layer;
							newItem.Movable = false;
							newItem.Name = item.Name;

							statue.AddItem( newItem );
						}
					}
				}

				for( int i = 0; i < SkillInfo.Table.Length; i++ )
				{
					statue.Skills[i].Cap = statue.Skills[i].Base = m.Skills[i].Cap;
				}

				if( m.Mount != null && m.Mount is BaseMount )
				{
					BaseMount newMount = (BaseMount)Activator.CreateInstance( ((BaseMount)m.Mount).GetType() );

					newMount.Direction = statue.Direction;
					newMount.Hue = (int)statue.Material;
					newMount.Rider = statue;
				}

				success = true;
			}
			catch { success = false; }

			return success;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			#region v0
			writer.Write( (bool)m_HasPlinth );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			#region v0
			m_HasPlinth = reader.ReadBool();
			#endregion
		}
	}
}