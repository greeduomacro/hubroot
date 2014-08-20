using System;
using Server.Items;

namespace Server.ContextMenus
{
	public class DescriptiveItemEntry : ContextMenuEntry
	{
		private Mobile _from;
		private IDescriptiveItem _item;

		public DescriptiveItemEntry( Mobile from, IDescriptiveItem item )
			: base( 6504, 1 )
		{
			_from = from;
			_item = item;
		}

		public override void OnClick()
		{
			if( !String.IsNullOrEmpty( _item.Description ) )
				_item.PrivateOverheadMessage( Server.Network.MessageType.Regular, 0x38A, true, _item.Description, _from.NetState );
		}
	}
}