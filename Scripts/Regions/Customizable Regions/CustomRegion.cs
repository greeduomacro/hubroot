using Server;
using System;
using System.Collections;
using Server.Items;
using Server.Spells;
using Server.Mobiles;

namespace Server.Regions
{
	public class CustomRegion : GuardedRegion
	{
		private RegionControl m_Controller;

		public RegionControl Controller
		{
			get { return m_Controller; }
		}

		public CustomRegion( RegionControl control )
			: base( control.RegionName, control.Map, control.RegionPriority, control.RegionArea )
		{
			Disabled = !control.IsGuarded;
			Music = control.Music;
			m_Controller = control;
		}

		public override bool IsDisabled()
		{
			if( !m_Controller.IsGuarded != Disabled )
				m_Controller.IsGuarded = !Disabled;

			return Disabled;
		}

		public override bool AllowBeneficial( Mobile from, Mobile target )
		{
			if( (!m_Controller.AllowBenefitPlayer && target is PlayerMobile) || (!m_Controller.AllowBenefitNPC && target is BaseCreature) )
			{
				from.SendMessage( "You cannot perform benificial acts on your target." );
				return false;
			}

			return base.AllowBeneficial( from, target );
		}

		public override bool AllowHarmful( Mobile from, Mobile target )
		{
			if( (!m_Controller.AllowHarmPlayer && target is PlayerMobile) || (!m_Controller.AllowHarmNPC && target is BaseCreature) )
			{
				from.SendMessage( "You cannot perform harmful acts on your target." );
				return false;
			}

			return base.AllowHarmful( from, target );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return m_Controller.AllowHousing;
		}

		public override bool AllowSpawn()
		{
			return m_Controller.AllowSpawn;
		}

		public override bool CanUseStuckMenu( Mobile m )
		{
			if( !m_Controller.CanUseStuckMenu )
				m.SendMessage( "You cannot use that option here." );
			return m_Controller.CanUseStuckMenu;
		}

		public override bool OnDamage( Mobile m, ref int Damage )
		{
			if( !m_Controller.CanBeDamaged )
			{
				m.SendMessage( "You cannot be damaged here." );
			}

			return m_Controller.CanBeDamaged;
		}
		public override bool OnResurrect( Mobile m )
		{
			if( !m_Controller.CanRessurect && m.AccessLevel == AccessLevel.Player )
				m.SendMessage( "You cannot ressurect here." );
			return m_Controller.CanRessurect;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if( from.AccessLevel == AccessLevel.Player )
			{
				bool restricted = m_Controller.IsRestrictedSpell( s );
				if( restricted )
				{
					from.SendMessage( "You cannot cast that spell here." );
					return false;
				}

				//if ( s is EtherealSpell && !CanMountEthereal ) Grr, EthereealSpell is private :<
				if( !m_Controller.CanMountEthereal && ((Spell)s).Info.Name == "Ethereal Mount" ) //Hafta check with a name compare of the string to see if ethy
				{
					from.SendMessage( "You cannot mount your ethereal here." );
					return false;
				}
			}

			//return base.OnBeginSpellCast( from, s );
			return true;	//Let users customize spells, not rely on weather it's guarded or not.
		}

		public override bool OnDecay( Item item )
		{
			return m_Controller.ItemDecay;
		}

		public override bool OnHeal( Mobile m, ref int Heal )
		{
			if( !m_Controller.CanHeal )
			{
				m.SendMessage( "You cannot be healed here." );
			}

			return m_Controller.CanHeal;
		}

		public override bool OnSkillUse( Mobile m, int skill )
		{
			bool restricted = m_Controller.IsRestrictedSkill( skill );
			if( restricted && m.AccessLevel == AccessLevel.Player )
			{
				m.SendMessage( "You cannot use that skill here." );
				return false;
			}

			return base.OnSkillUse( m, skill );
		}

		public override void OnExit( Mobile m )
		{
			if( m_Controller.ShowExitMessage )
				m.SendMessage( "You have left {0}", this.Name );

			base.OnExit( m );

		}

		public override void OnEnter( Mobile m )
		{
			if( m_Controller.ShowEnterMessage )
				m.SendMessage( "You have entered {0}", this.Name );

			base.OnEnter( m );
		}

		public override bool OnMoveInto( Mobile m, Direction d, Point3D newLocation, Point3D oldLocation )
		{
			if( !m_Controller.CanEnter && !this.Contains( oldLocation ) )
			{
				m.SendMessage( "You cannot enter this area." );
				return false;
			}

			return true;
		}

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			if( m.AccessLevel == AccessLevel.Player )
				return m_Controller.PlayerLogoutDelay;

			return base.GetLogoutDelay( m );
		}

		public override bool OnDoubleClick( Mobile m, object o )
		{
			if( o is BasePotion && !m_Controller.CanUsePotions )
			{
				m.SendMessage( "You cannot drink potions here." );
				return false;
			}

			if( o is Corpse )
			{
				Corpse c = (Corpse)o;

				bool canLoot;

				if( c.Owner == m )
					canLoot = m_Controller.CanLootOwnCorpse;
				else if( c.Owner is PlayerMobile )
					canLoot = m_Controller.CanLootPlayerCorpse;
				else
					canLoot = m_Controller.CanLootNPCCorpse;

				if( !canLoot )
					m.SendMessage( "You cannot loot that corpse here." );

				if( m.AccessLevel >= AccessLevel.GameMaster && !canLoot )
				{
					m.SendMessage( "This is unlootable, but you are able to open that with your godly powers." );
					return true;
				}

				return canLoot;
			}

			return base.OnDoubleClick( m, o );
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			if( m_Controller.LightLevel >= 0 )
				global = m_Controller.LightLevel;
			else
				base.AlterLightLevel( m, ref global, ref personal );
		}
	}
}
