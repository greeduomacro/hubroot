using System;
using Server.Network;

namespace Server
{
	public class CurrentExpansion
	{
		private static readonly Expansion Expansion = Expansion.AOS;

		public static void Configure()
		{
			Core.Expansion = Expansion;

			bool Enabled = Core.AOS;

			Mobile.InsuranceEnabled = false;
			ObjectPropertyList.Enabled = Enabled;
			Mobile.VisibleDamageType = Enabled ? VisibleDamageType.Everyone : VisibleDamageType.None;
			Mobile.GuildClickMessage = !Enabled;
			Mobile.AsciiClickMessage = !Enabled;

			Mobile.MaxPlayerResistance = 50;
            Mobile.GlobalRegenThroughPoison = true;

			if( Enabled )
			{
				//AOS.DisableStatInfluences();

				if( ObjectPropertyList.Enabled )
					PacketHandlers.SingleClickProps = false; // single click for everything is overriden to check object property list
			}
		}
	}
}
