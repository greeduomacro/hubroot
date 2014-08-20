using System;
using Server;
using Server.Items;

namespace Server.PlayercastStatues
{
	public class Plinth : Item
	{
		#region Properties
		private PlayercastStatue m_Statue;
		private string m_Engraving;

		public PlayercastStatue Statue
		{
			get { return m_Statue; }
			set { m_Statue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Engraving
		{
			get { return m_Engraving; }
			set { m_Engraving = value; InvalidateProperties(); }
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

				if( m_Statue != null && m_Statue.Hue != value )
					m_Statue.Hue = value;
			}
		}

		public override bool ForceShowProperties
		{
			get { return true; }
		}
		#endregion

		public Plinth( PlayercastStatue statue )
            : base( 0x0788 )
		{
			Hue = (int)statue.Material;
			Movable = false;
			Name = "a statue plinth";

			m_Statue = statue;
		}

		public Plinth( Serial serial )
			: base( serial )
		{
		}

		private void ValidateLocation()
		{
			Point3D statueLoc = new Point3D( this.X, this.Y, this.Z + 5 );

			if( m_Statue != null )
			{
				if( m_Statue.Map != this.Map )
					m_Statue.Map = this.Map;

				if( m_Statue.Location != statueLoc )
					m_Statue.Location = statueLoc;
			}
		}

		#region Overrides
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if( m_Engraving != null )
				list.Add( 1072305, m_Engraving );
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if( m_Statue != null && !m_Statue.Deleted )
				m_Statue.HasPlinth = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.HasGump( typeof( PlayercastStatueGump ) ) )
				from.CloseGump( typeof( PlayercastStatueGump ) );

			if( from.AccessLevel >= AccessLevel.Counselor || from.InRange( this, 3 ) )
			{
				if( from == m_Statue.Owner || from.AccessLevel >= AccessLevel.Counselor )
					from.SendGump( new PlayercastStatueGump( from, m_Statue ) );
				else
					from.SendMessage( "This is not your statue." );
			}
			else
				from.SendMessage( "You are too far away to do that." );
		}

		public override bool OnDragLift( Mobile from )
		{
			return false;
		}

		public override void OnLocationChange( Point3D oldLocation )
		{
			base.OnLocationChange( oldLocation );

			ValidateLocation();
		}
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			#region v0
			writer.Write( (PlayercastStatue)m_Statue );

			writer.Write( (string)m_Engraving );
			#endregion
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			#region v0
			m_Statue = reader.ReadMobile() as PlayercastStatue;

			m_Engraving = reader.ReadString();
			#endregion
		}
	}
}