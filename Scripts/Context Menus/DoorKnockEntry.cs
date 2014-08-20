using System;
using Server;
using Server.ContextMenus;
using Server.Items;

namespace Server.ContextMenus
{
	public class DoorKnockEntry : ContextMenuEntry
	{
		private BaseDoor m_Door;

		public DoorKnockEntry( BaseDoor door )
            : base(1063493, 1)
		{
			m_Door = door;
		}

		public override void OnClick()
		{
			int sound = 292;

			if( m_Door != null )
			{
				m_Door.PublicOverheadMessage( Server.Network.MessageType.Regular, 0x38A, false, "*you hear a knock on the door*" );

				sound = GetKnockFor( m_Door );

				new InternalTimer( m_Door, sound ).Start();
				Effects.PlaySound( m_Door.GetWorldLocation(), m_Door.Map, sound );
			}
		}

		public int GetKnockFor( BaseDoor door )
		{
			if( door is MetalDoor || door is IronGate || door is IronGateShort || door is BarredMetalDoor || door is BarredMetalDoor2
			   || door is MetalDoor2 || door is PortcullisNS || door is PortcullisEW )
				return 320;
			else if( door is LightWoodGate || door is DarkWoodGate || door is DarkWoodDoor || door is MediumWoodDoor || door is LightWoodDoor )
				return 938;
			else if( door is StrongWoodDoor )
				return 328;
			else
				return 292;
		}

		private class InternalTimer : Timer
		{
			private BaseDoor m_Door;
			private int m_Sound;
			private int count;

			public InternalTimer( BaseDoor door, int sound )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Door = door;
				m_Sound = sound;

				count = 0;

				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if( m_Door != null )
					Effects.PlaySound( m_Door.GetWorldLocation(), m_Door.Map, m_Sound );

				if( ++count >= 2 )
					Stop();
			}
		}
	}
}
