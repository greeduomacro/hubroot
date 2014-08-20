using System;
using Server;
using Server.Items;

namespace Server.Engines.Crops
{
	public class BaseGrownCrop : BaseCrop
	{
		private Mobile m_Sower;
		private Type m_CropType;
		private int m_Yield;
		private int m_AnimationFrame;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower { get { return m_Sower; } }

		public virtual Type CropType { get { return m_CropType; } set { m_CropType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual int StandardYield { get { return m_Yield; } }

		public virtual int AnimationFrame { get { return m_AnimationFrame; } }

		public BaseGrownCrop( Mobile sower, int itemID )
			: base( itemID )
		{
			m_Sower = sower;
			m_Yield = 1;
			m_AnimationFrame = 32;

			Movable = false;
		}

		public BaseGrownCrop( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			OnDoubleClick( from, true );
		}

		public virtual void OnDoubleClick( Mobile from, bool createWeedsAndDelete )
		{
			if( !from.CanSee( this ) )
			{
				from.SendMessage( "You cannot see that." );
			}
			else if( !(this is HarvestableTree) && !from.InRange( this, 1 ) )
			{
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
			}
			else if( this is HarvestableTree && !from.InRange( this, 2 ) )
			{
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
			}
			else if( (Sower != null && from != m_Sower) && !(this is HarvestableTree) )
			{
				from.SendMessage( "You cannot harvest any of this crop." );
			}
			else if( from.Mounted )
			{
				from.SendMessage( "You cannot do this while riding a mount." );
			}
			else
			{
				int calcYield = CalculateYield( from );
				Item crop = null;

				try { crop = CreateCrop( calcYield ); }
				catch( Exception e ) { Server.Utilities.ExceptionManager.LogException( "BaseGrownCrop.cs", e ); }

				if( crop == null )
				{
					from.SendMessage( "You are unable to harvest any of this crop!" );
				}
				else
				{
					from.AddToBackpack( crop );

					from.Direction = from.GetDirectionTo( this );
					from.Animate( AnimationFrame, 5, 1, true, false, 0 );
					from.PlaySound( 0x133 );
					from.SendMessage( "You successfully harvest the crop!" );
				}

				if( createWeedsAndDelete )
				{
					new Weeds().MoveToWorld( this.Location, this.Map );
					this.Delete();
				}
				else
				{
					OnHarvest( from );
				}
			}
		}

		public virtual int CalculateYield( Mobile m )
		{
			int toRet = StandardYield;

			toRet += (int)((m.Skills[SkillName.Cooking].Base + m.Skills[SkillName.Herding].Base) / 20);

			if( toRet <= 0 )
				toRet = 1;
			else if( toRet > 15 )
				toRet = 15;

			return toRet;
		}

		public virtual Item CreateCrop( int amount )
		{
			if( CropType == null )
				return null;

			if( CropType == typeof( Rose ) )
				return (Activator.CreateInstance( CropType ) as Item);
			else
				return (Activator.CreateInstance( CropType, amount ) as Item);
		}

		public virtual void OnHarvest( Mobile from )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Sower );
			writer.Write( m_CropType == null ? null : m_CropType.FullName );
			writer.Write( m_Yield );
			writer.Write( m_AnimationFrame );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Sower = reader.ReadMobile();
			string type = reader.ReadString();
			if( type != null )
				m_CropType = ScriptCompiler.FindTypeByFullName( type );
			m_Yield = reader.ReadInt();
			m_AnimationFrame = reader.ReadInt();
		}
	}
}