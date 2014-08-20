using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Regions;
using System.Collections;
using Server.SkillHandlers;
using Server.Gumps;

namespace Server.Items
{
	public enum RegionFlag
	{
		None = 0x00000000,
		AllowBenefitPlayer = 0x00000001,
		AllowHarmPlayer = 0x00000002,
		AllowHousing = 0x00000004,
		AllowSpawn = 0x00000008,
		CanBeDamaged = 0x00000010,
		CanHeal = 0x00000020,
		CanRessurect = 0x00000040,
		CanUseStuckMenu = 0x00000080,
		ItemDecay = 0x00000100,
		ShowEnterMessage = 0x00000200,
		ShowExitMessage = 0x00000400,
		AllowBenefitNPC = 0x00000800,
		AllowHarmNPC = 0x00001000,
		CanMountEthereal = 0x00002000,
		CanEnter = 0x00004000,
		CanLootPlayerCorpse = 0x00008000,
		CanLootNPCCorpse = 0x00010000,
		CanLootOwnCorpse = 0x00020000,
		CanUsePotions = 0x00040000,
		IsGuarded = 0x00080000
	}

	public class RegionControl : Item
	{
		private static List<RegionControl> m_AllControls = new List<RegionControl>();

		public static List<RegionControl> AllControls {
			get { return m_AllControls; }
		}

		#region Region Flags
		private RegionFlag m_Flags;

		public RegionFlag Flags {
			get { return m_Flags; }
			set { m_Flags = value; }
		}

		public bool GetFlag( RegionFlag flag ) {
			return ((m_Flags & flag) != 0);
		}

		public void SetFlag( RegionFlag flag, bool value ) {
			if( value )
				m_Flags |= flag;
			else {
				m_Flags &= ~flag;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowBenefitPlayer {
			get { return GetFlag( RegionFlag.AllowBenefitPlayer ); }
			set { SetFlag( RegionFlag.AllowBenefitPlayer, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHarmPlayer {
			get { return GetFlag( RegionFlag.AllowHarmPlayer ); }
			set { SetFlag( RegionFlag.AllowHarmPlayer, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHousing {
			get { return GetFlag( RegionFlag.AllowHousing ); }
			set { SetFlag( RegionFlag.AllowHousing, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowSpawn {
			get { return GetFlag( RegionFlag.AllowSpawn ); }
			set { SetFlag( RegionFlag.AllowSpawn, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanBeDamaged {
			get { return GetFlag( RegionFlag.CanBeDamaged ); }
			set { SetFlag( RegionFlag.CanBeDamaged, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanMountEthereal {
			get { return GetFlag( RegionFlag.CanMountEthereal ); }
			set { SetFlag( RegionFlag.CanMountEthereal, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanEnter {
			get { return GetFlag( RegionFlag.CanEnter ); }
			set { SetFlag( RegionFlag.CanEnter, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanHeal {
			get { return GetFlag( RegionFlag.CanHeal ); }
			set { SetFlag( RegionFlag.CanHeal, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanRessurect {
			get { return GetFlag( RegionFlag.CanRessurect ); }
			set { SetFlag( RegionFlag.CanRessurect, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUseStuckMenu {
			get { return GetFlag( RegionFlag.CanUseStuckMenu ); }
			set { SetFlag( RegionFlag.CanUseStuckMenu, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ItemDecay {
			get { return GetFlag( RegionFlag.ItemDecay ); }
			set { SetFlag( RegionFlag.ItemDecay, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowBenefitNPC {
			get { return GetFlag( RegionFlag.AllowBenefitNPC ); }
			set { SetFlag( RegionFlag.AllowBenefitNPC, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowHarmNPC {
			get { return GetFlag( RegionFlag.AllowHarmNPC ); }
			set { SetFlag( RegionFlag.AllowHarmNPC, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowEnterMessage {
			get { return GetFlag( RegionFlag.ShowEnterMessage ); }
			set { SetFlag( RegionFlag.ShowEnterMessage, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowExitMessage {
			get { return GetFlag( RegionFlag.ShowExitMessage ); }
			set { SetFlag( RegionFlag.ShowExitMessage, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanLootPlayerCorpse {
			get { return GetFlag( RegionFlag.CanLootPlayerCorpse ); }
			set { SetFlag( RegionFlag.CanLootPlayerCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanLootNPCCorpse {
			get { return GetFlag( RegionFlag.CanLootNPCCorpse ); }
			set { SetFlag( RegionFlag.CanLootNPCCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanLootOwnCorpse {
			get { return GetFlag( RegionFlag.CanLootOwnCorpse ); }
			set { SetFlag( RegionFlag.CanLootOwnCorpse, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool CanUsePotions {
			get { return GetFlag( RegionFlag.CanUsePotions ); }
			set { SetFlag( RegionFlag.CanUsePotions, value ); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsGuarded {
			get { return GetFlag( RegionFlag.IsGuarded ); }
			set {
				SetFlag( RegionFlag.IsGuarded, value );
				if( m_Region != null )
					m_Region.Disabled = !value;

				Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerCallback( UpdateRegion ) );
			}
		}
		#endregion

		#region Region Restrictions
		private BitArray m_RestrictedSpells;
		private BitArray m_RestrictedSkills;

		public BitArray RestrictedSpells {
			get { return m_RestrictedSpells; }
		}

		public BitArray RestrictedSkills {
			get { return m_RestrictedSkills; }
		}
		#endregion

		#region Region Related Objects
		private CustomRegion m_Region;
		private Rectangle3D[] m_RegionArea;

		public CustomRegion Region {
			get { return m_Region; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Rectangle3D[] RegionArea {
			get { return m_RegionArea; }
			set { m_RegionArea = value; }
		}
		#endregion

		#region Control Properties
		private bool m_Active = true;
		private AccessLevel m_EditAccessLevel = AccessLevel.GameMaster;
		private int _minZ;
		private int _maxZ;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Active {
			get { return m_Active; }
			set {
				if( m_Active != value ) {
					m_Active = value;
					UpdateRegion();
				}
			}

		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AccessLevel EditAccessLevel {
			get { return m_EditAccessLevel; }
			set { m_EditAccessLevel = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MinZ {
			get { return _minZ; }
			set {
				_minZ = value;

				for( int i = 0; i < m_RegionArea.Length; i++ )
					m_RegionArea[i] = new Rectangle3D( new Point3D( m_RegionArea[i].Start, value ), m_RegionArea[i].End );

				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxZ {
			get { return _maxZ; }
			set {
				_maxZ = value;

				for( int i = 0; i < m_RegionArea.Length; i++ )
					m_RegionArea[i] = new Rectangle3D( m_RegionArea[i].Start, new Point3D( m_RegionArea[i].End, value ) );

				UpdateRegion();
			}
		}
		#endregion

		#region Region Properties
		private string m_RegionName;
		private int m_RegionPriority;
		private MusicName m_Music;
		private TimeSpan m_PlayerLogoutDelay;
		private int m_LightLevel;

		[CommandProperty( AccessLevel.GameMaster )]
		public string RegionName {
			get { return m_RegionName; }
			set {
				if( Map != null && !RegionNameTaken( value ) )
					m_RegionName = value;
				else if( Map != null )
					Console.WriteLine( "RegionName not changed for {0}, {1} already has a Region with the name of {2}", this, Map, value );
				else if( Map == null )
					Console.WriteLine( "RegionName not changed for {0} to {1}, it's Map value was null", this, value );

				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RegionPriority {
			get { return m_RegionPriority; }
			set {
				m_RegionPriority = value;
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MusicName Music {
			get { return m_Music; }
			set {
				m_Music = value;
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan PlayerLogoutDelay {
			get { return m_PlayerLogoutDelay; }
			set {
				m_PlayerLogoutDelay = value;
				UpdateRegion();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int LightLevel {
			get { return m_LightLevel; }
			set {
				m_LightLevel = value;
				UpdateRegion();
			}
		}
		#endregion

		#region Constructors
		[Constructable]
		public RegionControl()
			: base( 5609 ) {
			Visible = false;
			Movable = false;
			Name = "Region Controller";

			if( m_AllControls == null )
				m_AllControls = new List<RegionControl>();
			m_AllControls.Add( this );

			m_RegionName = FindNewName( "Custom Region" );
			m_RegionPriority = CustomRegion.DefaultPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );
		}

		[Constructable]
		public RegionControl( Rectangle2D rect )
			: base( 5609 ) {
			Visible = false;
			Movable = false;
			Name = "Region Controller";

			if( m_AllControls == null )
				m_AllControls = new List<RegionControl>();
			m_AllControls.Add( this );

			m_RegionName = FindNewName( "Custom Region" );
			m_RegionPriority = CustomRegion.DefaultPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			Rectangle3D newrect = Server.Region.ConvertTo3D( rect );
			DoChooseArea( null, this.Map, newrect.Start, newrect.End, this );

			UpdateRegion();
		}

		[Constructable]
		public RegionControl( Rectangle3D rect )
			: base( 5609 ) {
			Visible = false;
			Movable = false;
			Name = "Region Controller";

			if( m_AllControls == null )
				m_AllControls = new List<RegionControl>();
			m_AllControls.Add( this );

			m_RegionName = FindNewName( "Custom Region" );
			m_RegionPriority = CustomRegion.DefaultPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			DoChooseArea( null, this.Map, rect.Start, rect.End, this );

			UpdateRegion();
		}

		[Constructable]
		public RegionControl( Rectangle2D[] rects )
			: base( 5609 ) {
			Visible = false;
			Movable = false;
			Name = "Region Controller";

			if( m_AllControls == null )
				m_AllControls = new List<RegionControl>();
			m_AllControls.Add( this );

			m_RegionName = FindNewName( "Custom Region" );
			m_RegionPriority = CustomRegion.DefaultPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			foreach( Rectangle2D rect2d in rects ) {
				Rectangle3D newrect = Server.Region.ConvertTo3D( rect2d );
				DoChooseArea( null, this.Map, newrect.Start, newrect.End, this );
			}

			UpdateRegion();
		}

		[Constructable]
		public RegionControl( Rectangle3D[] rects )
			: base( 5609 ) {
			Visible = false;
			Movable = false;
			Name = "Region Controller";

			if( m_AllControls == null )
				m_AllControls = new List<RegionControl>();
			m_AllControls.Add( this );

			m_RegionName = FindNewName( "Custom Region" );
			m_RegionPriority = CustomRegion.DefaultPriority;

			m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );
			m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

			foreach( Rectangle3D rect3d in rects ) {
				DoChooseArea( null, this.Map, rect3d.Start, rect3d.End, this );
			}

			UpdateRegion();
		}

		public RegionControl( Serial serial )
			: base( serial ) {
		}
		#endregion

		#region Control Special Voids
		public bool RegionNameTaken( string testName ) {

			if( m_AllControls != null ) {
				for( int i = 0; i < m_AllControls.Count; i++ ) {
					if( m_AllControls[i] != null && m_AllControls[i].RegionName == testName && m_AllControls[i] != this )
						return true;
				}
			}

			return false;
		}

		public string FindNewName( string oldName ) {
			int i = 1;

			string newName = oldName;
			while( RegionNameTaken( newName ) ) {
				newName = oldName;
				newName += String.Format( " {0}", i );
				i++;
			}

			return newName;
		}

		public void UpdateRegion() {
			if( m_Region != null )
				m_Region.Unregister();

			if( this.Map != null && this.Active ) {
				if( this != null && this.RegionArea != null && this.RegionArea.Length > 0 ) {
					m_Region = new CustomRegion( this );
					m_Region.Register();
				} else
					m_Region = null;
			} else
				m_Region = null;
		}

		public void RemoveArea( int index, Mobile from ) {
			try {
				List<Rectangle3D> rects = new List<Rectangle3D>();
				foreach( Rectangle3D rect in m_RegionArea )
					rects.Add( rect );

				rects.RemoveAt( index );
				m_RegionArea = rects.ToArray();

				UpdateRegion();
				from.SendMessage( "Area Removed!" );
			} catch {
				from.SendMessage( "Removing of Area Failed!" );
			}
		}
		public static int GetRegistryNumber( ISpell s ) {
			Type[] t = SpellRegistry.Types;

			for( int i = 0; i < t.Length; i++ ) {
				if( s.GetType() == t[i] )
					return i;
			}

			return -1;
		}


		public bool IsRestrictedSpell( ISpell s ) {
			if( m_RestrictedSpells.Length != SpellRegistry.Types.Length ) {

				m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );

				for( int i = 0; i < m_RestrictedSpells.Length; i++ )
					m_RestrictedSpells[i] = false;

			}

			int regNum = GetRegistryNumber( s );

			if( regNum < 0 )	//Happens with unregistered Spells
				return false;

			return m_RestrictedSpells[regNum];
		}

		public bool IsRestrictedSkill( int skill ) {
			if( m_RestrictedSkills.Length != SkillInfo.Table.Length ) {

				m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

				for( int i = 0; i < m_RestrictedSkills.Length; i++ )
					m_RestrictedSkills[i] = false;

			}

			if( skill < 0 )
				return false;

			return m_RestrictedSkills[skill];
		}

		public void ChooseArea( Mobile m ) {
			BoundingBoxPicker.Begin( m, new BoundingBoxCallback( CustomRegion_Callback ), this );
		}

		public void CustomRegion_Callback( Mobile from, Map map, Point3D start, Point3D end, object state ) {
			DoChooseArea( from, map, start, end, state );
		}

		public void DoChooseArea( Mobile from, Map map, Point3D start, Point3D end, object control ) {
			if( this != null ) {
				List<Rectangle3D> areas = new List<Rectangle3D>();

				if( this.m_RegionArea != null ) {
					foreach( Rectangle3D rect in this.m_RegionArea )
						areas.Add( rect );
				}

				Rectangle3D newrect = new Rectangle3D( new Point3D( start, _minZ ), new Point3D( end, _maxZ ) );
				areas.Add( newrect );

				this.m_RegionArea = areas.ToArray();

				this.UpdateRegion();
			}
		}
		#endregion

		#region Control Overrides
		public override void OnDoubleClick( Mobile m ) {
			if( m.AccessLevel >= m_EditAccessLevel ) {
				if( m_RestrictedSpells.Length != SpellRegistry.Types.Length ) {
					m_RestrictedSpells = new BitArray( SpellRegistry.Types.Length );

					for( int i = 0; i < m_RestrictedSpells.Length; i++ )
						m_RestrictedSpells[i] = false;

					m.SendMessage( "Resetting all restricted Spells due to Spell change" );
				}

				if( m_RestrictedSkills.Length != SkillInfo.Table.Length ) {

					m_RestrictedSkills = new BitArray( SkillInfo.Table.Length );

					for( int i = 0; i < m_RestrictedSkills.Length; i++ )
						m_RestrictedSkills[i] = false;

					m.SendMessage( "Resetting all restricted Skills due to Skill change" );

				}

				m.CloseGump( typeof( RegionControlGump ) );
				m.SendGump( new RegionControlGump( this ) );
				m.SendMessage( "Don't forget to [Props this object for more options!" );
				m.CloseGump( typeof( RemoveAreaGump ) );
				m.SendGump( new RemoveAreaGump( this ) );
			}
		}

		public override void OnMapChange() {
			UpdateRegion();
			base.OnMapChange();
		}

		public override void OnDelete() {
			if( m_Region != null )
				m_Region.Unregister();

			if( m_AllControls != null )
				m_AllControls.Remove( this );

			base.OnDelete();
		}
		#endregion

		#region Ser/Deser Helpers
		public static void WriteBitArray( GenericWriter writer, BitArray ba ) {
			writer.Write( ba.Length );

			for( int i = 0; i < ba.Length; i++ ) {
				writer.Write( ba[i] );
			}
			return;
		}

		public static BitArray ReadBitArray( GenericReader reader ) {
			int size = reader.ReadInt();

			BitArray newBA = new BitArray( size );

			for( int i = 0; i < size; i++ ) {
				newBA[i] = reader.ReadBool();
			}

			return newBA;
		}


		public static void WriteRect3DArray( GenericWriter writer, Rectangle3D[] ary ) {
			if( ary == null ) {
				writer.Write( 0 );
				return;
			}

			writer.Write( ary.Length );

			for( int i = 0; i < ary.Length; i++ ) {
				Rectangle3D rect = ((Rectangle3D)ary[i]);
				writer.Write( (Point3D)rect.Start );
				writer.Write( (Point3D)rect.End );
			}
			return;
		}

		public static List<Rectangle2D> ReadRect2DArray( GenericReader reader ) {
			int size = reader.ReadInt();
			List<Rectangle2D> newAry = new List<Rectangle2D>();

			for( int i = 0; i < size; i++ ) {
				newAry.Add( reader.ReadRect2D() );
			}

			return newAry;
		}

		public static Rectangle3D[] ReadRect3DArray( GenericReader reader ) {
			int size = reader.ReadInt();
			List<Rectangle3D> newAry = new List<Rectangle3D>();

			for( int i = 0; i < size; i++ ) {
				Point3D start = reader.ReadPoint3D();
				Point3D end = reader.ReadPoint3D();
				newAry.Add( new Rectangle3D( start, end ) );
			}

			return newAry.ToArray();
		}
		#endregion

		public override void Serialize( GenericWriter writer ) {
			base.Serialize( writer );

			writer.Write( (int)6 ); // version

			writer.Write( (int)_minZ );
			writer.Write( (int)_maxZ );

			writer.Write( (int)m_EditAccessLevel );

			WriteRect3DArray( writer, m_RegionArea );

			writer.Write( (int)m_Flags );

			WriteBitArray( writer, m_RestrictedSpells );
			WriteBitArray( writer, m_RestrictedSkills );

			writer.Write( (bool)m_Active );

			writer.Write( (string)m_RegionName );
			writer.Write( (int)m_RegionPriority );
			writer.Write( (int)m_Music );
			writer.Write( (TimeSpan)m_PlayerLogoutDelay );
			writer.Write( (int)m_LightLevel );
		}

		public override void Deserialize( GenericReader reader ) {
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version ) {
				case 6: {
						_minZ = reader.ReadInt();
						_maxZ = reader.ReadInt();

						goto case 5;
					}
				case 5: {
						m_EditAccessLevel = (AccessLevel)reader.ReadInt();

						goto case 4;
					}
				case 4: {
						m_RegionArea = ReadRect3DArray( reader );

						m_Flags = (RegionFlag)reader.ReadInt();

						m_RestrictedSpells = ReadBitArray( reader );
						m_RestrictedSkills = ReadBitArray( reader );

						m_Active = reader.ReadBool();

						m_RegionName = reader.ReadString();
						m_RegionPriority = reader.ReadInt();
						m_Music = (MusicName)reader.ReadInt();
						m_PlayerLogoutDelay = reader.ReadTimeSpan();
						m_LightLevel = reader.ReadInt();

						if( version <= 4 ) {
							reader.ReadMap();
							reader.ReadPoint3D();
							reader.ReadMap();
							reader.ReadPoint3D();
						}

						break;
					}
				case 3: //pre-RunUO 2.0
					{
						m_LightLevel = reader.ReadInt();
						goto case 2;
					}
				case 2: {
						m_Music = (MusicName)reader.ReadInt();
						goto case 1;
					}
				case 1: {
						List<Rectangle2D> rects2d = ReadRect2DArray( reader );
						foreach( Rectangle2D rect in rects2d ) {
							Rectangle3D newrect = Server.Region.ConvertTo3D( rect );
							DoChooseArea( null, this.Map, newrect.Start, newrect.End, this );
						}

						m_RegionPriority = reader.ReadInt();
						m_PlayerLogoutDelay = reader.ReadTimeSpan();

						m_RestrictedSpells = ReadBitArray( reader );
						m_RestrictedSkills = ReadBitArray( reader );

						m_Flags = (RegionFlag)reader.ReadInt();

						m_RegionName = reader.ReadString();
						break;
					}
				case 0: {
						List<Rectangle2D> rects2d = ReadRect2DArray( reader );
						foreach( Rectangle2D rect in rects2d ) {
							Rectangle3D newrect = Server.Region.ConvertTo3D( rect );
							DoChooseArea( null, this.Map, newrect.Start, newrect.End, this );
						}

						m_RestrictedSpells = ReadBitArray( reader );
						m_RestrictedSkills = ReadBitArray( reader );

						m_Flags = (RegionFlag)reader.ReadInt();

						m_RegionName = reader.ReadString();
						break;
					}
			}

			m_AllControls.Add( this );

			if( RegionNameTaken( m_RegionName ) )
				m_RegionName = FindNewName( m_RegionName );

			UpdateRegion();
		}
	}
}
