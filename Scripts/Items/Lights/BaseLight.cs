using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public abstract class BaseLight : Item
	{
		public static readonly bool Burnout = true;
		public static readonly int EnchantmentRange = 6;
		public static List<BaseLight> AllLights = new List<BaseLight>();

		private Timer _timer;
		private DateTime _end;
		private bool _burntOut = false;
		private bool _burning = false;
		private bool _protected = false;
		private TimeSpan _duration = TimeSpan.Zero;
		private bool _hasFuel = true;
		private bool _automatic;
		private bool _enchanted;

		public abstract int LitItemID { get; }

		public virtual int UnlitItemID { get { return 0; } }
		public virtual int BurntOutItemID { get { return 0; } }

		public virtual int LitSound { get { return 0x47; } }
		public virtual int UnlitSound { get { return 0x3be; } }
		public virtual int BurntOutSound { get { return 0x4b8; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Automatic { get { return _automatic; } set { _automatic = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Burning
		{
			get { return _burning; }
			set
			{
				if( _burning != value )
				{
					_burning = true;
					DoTimer( _duration );
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BurntOut { get { return _burntOut; } set { _burntOut = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan Duration
		{
			get
			{
				if( _duration != TimeSpan.Zero && _burning )
				{
					return _end - DateTime.Now;
				}
				else
					return _duration;
			}
			set { _duration = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Enchanted { get { return _enchanted; } set { _enchanted = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasFuel { get { return _hasFuel; } set { _hasFuel = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Protected { get { return _protected; } set { _protected = value; } }

		[Constructable]
		public BaseLight( int itemID )
			: base( itemID )
		{
			AllLights.Add( this );
		}

		public BaseLight( Serial serial )
			: base( serial )
		{
		}

		public override bool HandlesOnMovement { get { return _enchanted; } }

		#region +override void OnMovement( Mobile, Point3D )
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );

			if( Enchanted )
			{
				byte mobileCount = 0;

				foreach( Mobile mobile in this.GetMobilesInRange( EnchantmentRange ) )
				{
					if( mobile.Alive && mobile.Body.IsHuman && mobile.InLOS( this.GetWorldLocation() ) && mobile.Player )
					{
						mobileCount++;
						break;
					}
				}

				if( Burning && mobileCount == 0 )
					Douse();
				else if( !Burning && mobileCount > 0 )
					Ignite();
			}
		}
		#endregion

		#region +override void OnDoubleClick( Mobile )
		public override void OnDoubleClick( Mobile from )
		{
			if( !_hasFuel || _burntOut )
				from.SendMessage( "That light is burnt out. It will need more fuel before using it again." );
			else if( _protected && from.AccessLevel < AccessLevel.Counselor )
				from.SendMessage( "This light is protected by an unknown force, and cannot be altered." );
			else if( !from.InRange( this.GetWorldLocation(), 2 ) )
				from.SendLocalizedMessage( CommonLocs.YouTooFar );
			else if( _burning )
			{
				if( UnlitItemID != 0 )
					Douse();
			}
			else
			{
				Ignite();
			}
		}
		#endregion

		#region +virtual void Ignite()
		public virtual void Ignite()
		{
			if( !_burntOut )
			{
				PlayLitSound();

				_burning = true;
				ItemID = LitItemID;
				DoTimer( _duration );
			}
		}
		#endregion

		#region -void DoTimer( TimeSpan )
		private void DoTimer( TimeSpan delay )
		{
			if( _timer != null )
				_timer.Stop();

			if( _protected || delay == TimeSpan.Zero )
				return;

			_end = DateTime.Now + delay;

			_timer = new InternalTimer( this, delay );
			_timer.Start();
		}
		#endregion

		#region +virtual void Burn()
		public virtual void Burn()
		{
			_hasFuel = false;
			_burntOut = true;
			Douse();
		}
		#endregion

		#region +virtual void Douse()
		public virtual void Douse()
		{
			_burning = false;

			if( _burntOut && BurntOutItemID != 0 )
				ItemID = BurntOutItemID;
			else
				ItemID = UnlitItemID;

			if( _burntOut )
				_duration = TimeSpan.Zero;
			else if( _duration != TimeSpan.Zero )
				_duration = _end - DateTime.Now;

			if( _timer != null )
				_timer.Stop();

			PlayUnlitSound();
		}
		#endregion

		#region +virtual void PlayLitSound()
		public virtual void PlayLitSound()
		{
			if( LitSound != 0 )
			{
				Point3D loc = GetWorldLocation();
				Effects.PlaySound( loc, Map, LitSound );
			}
		}
		#endregion

		#region +virtual void PlayUnlitSound()
		public virtual void PlayUnlitSound()
		{
			int sound = UnlitSound;

			if( _burntOut && BurntOutSound != 0 )
				sound = BurntOutSound;

			if( sound != 0 )
			{
				Point3D loc = GetWorldLocation();
				Effects.PlaySound( loc, Map, sound );
			}
		}
		#endregion

		#region +override void Serialize( GenericWriter )
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)3 );

			//version 3
			writer.Write( (bool)_enchanted );
			//version 2
			writer.Write( _automatic );
			//version 1
			writer.Write( _hasFuel );
			//version 0
			writer.Write( _burntOut );
			writer.Write( _burning );
			writer.Write( _duration );
			writer.Write( _protected );

			if( _burning && _duration != TimeSpan.Zero )
				writer.WriteDeltaTime( _end );
		}
		#endregion

		#region +override void Deserialize( GenericReader )
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch( version )
			{
				case 3:
					_enchanted = reader.ReadBool();
					goto case 2;
				case 2:
					_automatic = reader.ReadBool();
					goto case 1;
				case 1:
					_hasFuel = reader.ReadBool();
					goto case 0;
				case 0:
					_burntOut = reader.ReadBool();
					_burning = reader.ReadBool();
					_duration = reader.ReadTimeSpan();
					_protected = reader.ReadBool();

					if( _burning && _duration != TimeSpan.Zero )
						DoTimer( reader.ReadDeltaTime() - DateTime.Now );

					break;
			}

			if( !AllLights.Contains( this ) )
				AllLights.Add( this );
		}
		#endregion

		#region -class InternalTimer : Timer
		private class InternalTimer : Timer
		{
			private BaseLight _light;

			public InternalTimer( BaseLight light, TimeSpan delay )
				: base( delay )
			{
				_light = light;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				if( _light.Protected )
				{
					this.Stop();
					return;
				}

				if( _light != null && !_light.Deleted && !_light.Protected )
					_light.Burn();
			}
		}
		#endregion
	}
}