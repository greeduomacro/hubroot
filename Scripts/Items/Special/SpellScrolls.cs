using System;
using Server;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Targeting;

namespace Server.Items
{
	[Flipable( 0xE37, 0xEF6 )]
	public class BlessingOfKhopesh : Item
	{
		[Constructable]
		public BlessingOfKhopesh() : base( 0xE37 )
		{
			Name = "Khopesh's Blessing";
		}
		
		public BlessingOfKhopesh( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Skills.Magery.Base < 85 )
				{
					from.SendMessage( "The scroll bursts into flame in your hands!" );
					from.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Waist );
					from.Damage( Utility.RandomMinMax( 40, 55 ) );
					
					this.Delete();
				}
				else
				{
					from.Frozen = true;
					from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "In Kal Quas", false );
					
					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 206, 7, 1, true, false, 0 );
					
					from.BeginTarget( 10, false, TargetFlags.None, new TargetCallback( BlessingOfKhopesh_OnTarget ) );
				}
			}
		}
		
		public virtual void BlessingOfKhopesh_OnTarget( Mobile from, object target )
		{
			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is Mobile )
			{
				Mobile t = (Mobile)target;
				
				if( !from.InRange( t.Location, 10 ) )
					from.SendMessage( "That is too far away." );
				
				t.BoltEffect( 2 );
				Effects.SendLocationParticles( EffectItem.Create( new Point3D( t.X, t.Y, t.Z + 10 ), t.Map, EffectItem.DefaultDuration ), 0x376A, 10, 15, 5045 );
				t.PlaySound( 0x1E1 );
				
				t.Hidden = true;
				t.AllowedStealthSteps = Utility.RandomMinMax( 50, 75 );
				
				from.SendMessage( "You have hidden your target well." );
				t.SendMessage( "You have been hidden well, and can move quietly in the shadows." );
				
				t.AddToBackpack( new KhoMarkGem() );
				
				this.Delete();
			}
			else
			{
				from.SendMessage( "This would not work on that." );
			}

			from.Frozen = false;
		}
	}
	
	public class KhoMarkGem : Item
	{
		[Constructable]
		public KhoMarkGem() : base( 0xE73 )
		{
			Name = "Stealth marking stone";
			Visible = false;
			Movable = false;
		}
		
		public KhoMarkGem( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
	
	[Flipable( 0x14EE, 0x14ED )]
	public class BladeBlessing : Item
	{
		private StatMod m_StatMod0;
		private TimedSkillMod m_TimedSkillMod0;
		private DateTime dateTime = DateTime.Now + TimeSpan.FromMinutes( 5 );
		
		[Constructable]
		public BladeBlessing() : base( 0x14ED )
		{
			Name = "Blade Blessing";
			Hue = 1155;
		}
		
		public BladeBlessing( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Skills.Magery.Base < 50 )
				{
					from.SendMessage( "The scroll bursts into flame in your hands!" );
					from.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Waist );
					from.Damage( Utility.RandomMinMax( 40, 55 ) );
					
					this.Delete();
				}
				else
				{
					from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "Ex Sanct Rel", false );
					
					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 245, 7, 1, true, false, 0 );
					
					from.BeginTarget( 10, false, TargetFlags.None, new TargetCallback( BladeBlessing_OnTarget ) );
				}
			}
		}
		
		public virtual void BladeBlessing_OnTarget( Mobile from, object target )
		{
			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is BaseWeapon )
			{
				BaseWeapon wep = (BaseWeapon)target;
				
				wep.MaxHitPoints += Utility.RandomMinMax( 80, 120 );
				wep.HitPoints = wep.MaxHitPoints;
				wep.LootType = LootType.Blessed;
				
				from.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist );
				from.PlaySound( 0x1EA );
				
				this.Delete();
			}
			else if( target is Mobile )
			{
				Mobile m = (Mobile)target;
				PlayerMobile pm = (PlayerMobile)m;
				
				if( pm.Karma >= 1500 )
				{
					m_StatMod0 = new StatMod( StatType.All, "statbonus", 15, TimeSpan.FromMinutes( 5 ) );
					pm.AddStatMod( m_StatMod0 );
					
					m_TimedSkillMod0 = new TimedSkillMod( SkillName.Tactics, true, 10, dateTime );
					from.AddSkillMod( m_TimedSkillMod0 );
					
					this.Delete();
				}
				else
				{
					from.SendMessage( "You are not worthy of this blessing." );
				}
			}
			else
			{
				from.SendMessage( "This spell would not work on that." );
			}
		}
	}
	
	public class TjnarsScythe : Item
	{
		[Constructable]
		public TjnarsScythe() : base( 0x1869 )
		{
			Name = "Calling of Tjnar's Scythe";
		}
		
		public TjnarsScythe( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Skills.Magery.Base < 85 )
				{
					from.SendMessage( "The scroll bursts into flame in your hands!" );
					from.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Waist );
					from.Damage( Utility.RandomMinMax( 65, 80 ) );
					
					this.Delete();
				}
				else
				{
					from.Frozen = true;
					
					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 206, 7, 1, true, false, 0 );
					
					from.BeginTarget( 10, false, TargetFlags.Harmful, new TargetCallback( TjnarsScythe_OnTarget ) );
				}
			}
		}
		
		public virtual void TjnarsScythe_OnTarget( Mobile from, object target )
		{
			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is Mobile )
			{
				Mobile targetMobile = (Mobile)target;
				
				if( targetMobile.Body.IsHuman )
				{
					if( targetMobile.Hits < (targetMobile.HitsMax / 10) )
					{
						targetMobile.PlaySound( 565 ); //Consecrate Weapon sound
						from.PlaySound( 565 );
						
						from.MovingEffect( targetMobile, 0x26C4, 9, 1, false, false ); //Launches a solid scythe
						Effects.SendMovingParticles( from, targetMobile, 0x26C4, 1, 0, false, false, 1109, 3, 9501, 1, 0, EffectLayer.Waist, 0x100 ); //Launches a rendered scythe
						
						targetMobile.FixedParticles( 0x3779, 1, 30, 9964, 1109, 3, EffectLayer.Waist ); //Small sparklies on the target
						
						targetMobile.Kill();
						
						Head head = new Head( String.Format( "the head of {0}", targetMobile.Name ) );
						from.AddToBackpack( head );
						
						from.BoltEffect( 1 );
						
						this.Delete();
					}
					else
					{
						from.PublicOverheadMessage( MessageType.Emote, from.EmoteHue, false, "*the voice of Tjnar rings out across the region:*", false );
						from.PublicOverheadMessage( MessageType.Yell, from.SpeechHue, false, "\"You fool! That mere mortal is not prepared to die! Crush the body, and the mind shall be yours!\"", false );
						
						from.SendMessage( "Your target must have less than 10% of his health in order for this spell to work." );
					}
				}
				else
				{
					from.SendMessage( "This spell will only work on humans." );
				}
			}

			from.Frozen = false;
		}
	}
	
	[Flipable( 0x14F1, 0x14F2 )]
	public class SummonShip : Item
	{
		private int m_MultiID;
		private Point3D m_Offset;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int MultiID{ get{ return m_MultiID; } set{ m_MultiID = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Offset{ get{ return m_Offset; } set{ m_Offset = value; } }
		
		[Constructable]
		public SummonShip(/* int id, Point3D offset */) : base( 0x14F2 )
		{
			Name = "sea vessel summoning spell";
			
			m_MultiID = 0x4014 & 0x3FFF;//id & 0x3FFF;
			m_Offset = new Point3D( 0, -1, 0 );//offset;
		}
		
		public SummonShip( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
			
			writer.Write( m_MultiID );
			writer.Write( m_Offset );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			m_MultiID = reader.ReadInt();
			m_Offset = reader.ReadPoint3D();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				from.Frozen = true;
				from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "Uus Ylem", false );
				
				if( !from.Mounted && from.Body.IsHuman )
					from.Animate( 230, 7, 1, true, false, 0 );
				
				from.Target = new InternalTarget( this );
				from.SendMessage( "Target the water where you would like to summon the ship." );
			}
		}
		
		public BaseBoat SummonedBoat{ get{ return new MediumDragonBoat(); } }
		
		public void OnSummonBoat( Mobile from, Point3D p )
		{
			Map map = from.Map;
			
			if( map == null )
				return;
			
			BaseBoat boat = SummonedBoat;
			
			if( boat == null )
				return;
			
			p = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
			
			if( BaseBoat.IsValidLocation( p, map ) && boat.CanFit( p, map, boat.ItemID ) )
			{
				Delete();
				
				boat.Owner = from;
				boat.Anchored = true;
				
				uint keyValue = boat.CreateKeys( from );
				
				if( boat.PPlank != null )
					boat.PPlank.KeyValue = keyValue;
				
				if( boat.SPlank != null )
					boat.SPlank.KeyValue = keyValue;
				
				boat.MoveToWorld( p, map );
			}
			else
			{
				boat.Delete();
				
				from.SendMessage( "A ship cannot be summoned here." );
			}

			from.Frozen = false;
		}
		
		private class InternalTarget : MultiTarget
		{
			private SummonShip m_Scroll;
			
			public InternalTarget( SummonShip scroll ) : base( scroll.MultiID, scroll.Offset )
			{
				m_Scroll = scroll;
			}
			
			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D ip = o as IPoint3D;
				
				if( ip != null )
				{
					if( ip is Item )
						ip = ((Item)ip).GetWorldTop();
					
					Point3D p = new Point3D( ip );
					
					Region region = Region.Find( p, from.Map );
					
					if( region is DungeonRegion )
						from.SendLocalizedMessage( 502488 ); //You canont place a ship inside a dungeon.
					else if( region is HouseRegion )
						from.SendLocalizedMessage( 1042549 ); //A boat may not be placed here.
					else
						m_Scroll.OnSummonBoat( from, p );
				}
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				if( cancelType == TargetCancelType.Canceled || cancelType == TargetCancelType.Disconnected || cancelType == TargetCancelType.Timeout || cancelType == TargetCancelType.Overriden )
				{
					from.Frozen = false;
				}

				base.OnTargetCancel( from, cancelType );
			}
		}
	}
	
	public class IntelligenceOfThePatriarch : Item
	{
		[Constructable]
		public IntelligenceOfThePatriarch() :  base( 0xE37 )
		{
			Name = "Intelligence of the Patriarch";
			Hue = 1154;
		}
		
		public IntelligenceOfThePatriarch( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Skills.Magery.Base < 65 )
				{
					from.SendMessage( "The scroll bursts into flame in your hands!" );
					from.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Waist );
					from.Damage( Utility.RandomMinMax( 40, 55 ) );
					
					this.Delete();
				}
				else
				{
					from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "Uus Wis", false );
					
					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 206, 7, 1, true, false, 0 );
					
					from.BeginTarget( 10, false, TargetFlags.Beneficial, new TargetCallback( PatriarchIntelligence_OnTarget ) );
				}
			}
		}
		
		public virtual void PatriarchIntelligence_OnTarget( Mobile from, object target )
		{
			double intBuff = Utility.RandomMinMax( 12, 20 );

			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is Mobile )
			{
				Mobile t = (Mobile)target;
				
				if( !from.InRange( t.Location, 10 ) )
					from.SendLocalizedMessage( 500446 ); //That is too far away.
				
				t.BoltEffect( 5 );
				
				t.RawInt += (int) intBuff;
				
				from.SendMessage( "{0} has been blessed with extra intelligence.", t.Female == true ? "He" : "She" );
				t.SendMessage( "Your intelligence has increased by {0} as a result of the spell blessing.", intBuff );
				
				this.Delete();
			}
		}
	}
	
	public class PigifyOther : Item
	{
		[Constructable]
		public PigifyOther() : base( 0xE37 )
		{
			Name = "Pigify Other";
			Hue = 1157;
		}
		
		public PigifyOther( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Karma > -625 )
					from.SendMessage( "Your past actions are too blessed to call on this spell." );
				else
				{
					from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "Kal Rel Quas", false );
					
					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 209, 7, 1, true, false, 0 );
					
					from.BeginTarget( 10, false, TargetFlags.Harmful, new TargetCallback( Pigify_OnTarget ) );
				}
			}
		}
		
		public virtual void Pigify_OnTarget( Mobile from, object target )
		{
			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is Mobile && ((Mobile)target).Body.IsHuman )
			{
				Mobile t = (Mobile)target;

				if( t.AccessLevel > from.AccessLevel )
					t = from;
				
				t.BoltEffect( 2 );
				t.BoltEffect( 2 );

				t.BodyValue = 0xCB;
				t.RawInt /= 2;
				t.Hits /= 2;
				t.NameMod = "a pigified being";
				
				t.PlaySound( 0xC4 );
				from.PlaySound( 0xC4 );
				
				from.DoHarmful( t );
				from.Karma -= Utility.RandomMinMax( 100, 150 );
				
				new PigifyTimer( t ).Start();
				
				this.Delete();
			}
			else
			{
				from.SendMessage( "This would not work on that." );
			}
		}
		
		private class PigifyTimer : Timer
		{
			private Mobile m_From;
			
			public PigifyTimer( Mobile from ) : base( TimeSpan.FromSeconds( 45 ) )
			{
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			protected override void OnTick()
			{
				if( m_From != null && m_From.Alive )
				{
					if( m_From.Female )
						m_From.BodyValue = 401;
					else
						m_From.BodyValue = 400;

					m_From.RawInt *= 2;
					m_From.Hits = m_From.HitsMax;
					m_From.NameMod = null;
				}
				
				Stop();
			}
		}
	}

	public class WrathOfFray : Item
	{
		[Constructable]
		public WrathOfFray() : base( 0x14EE )
		{
			Name = "The Wrath of Fray";
		}

		public WrathOfFray( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); //That must be in your pack to use it.
			}
			else
			{
				if( from.Skills.Magery.Base < 75 )
					from.SendMessage( "You do not have the skill to understand this spellscroll." );
				else
				{
					from.Frozen = true;

					if( !from.Mounted && from.Body.IsHuman )
						from.Animate( 230, 7, 1, true, false, 0 );

					from.PublicOverheadMessage( MessageType.Spell, from.SpeechHue, true, "In Ort Jux", false );
					from.SendMessage( "Please select your target." );

					from.BeginTarget( 10, false, TargetFlags.Harmful, new TargetCallback( WrathOfFray_OnTarget ) );
				}
			}
		}

		public virtual void WrathOfFray_OnTarget( Mobile from, object target )
		{
			if( !from.CanSee( target ) )
			{
				from.SendLocalizedMessage( 500237 ); //Target cannot be seen.
			}
			else if( target is Mobile )
			{
				Mobile t = (Mobile)target;

				from.MovingParticles( t, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
				from.PlaySound( 0x307 );
				t.PlaySound( 0x307 );

				AOS.Damage( t, from, Utility.RandomMinMax( 25, 45 ), 20, 20, 20, 20, 20, false );

				this.Delete();
			}
			else
				from.SendMessage( "This spell will not work on that." );

			from.Frozen = false;
		}
	}
}