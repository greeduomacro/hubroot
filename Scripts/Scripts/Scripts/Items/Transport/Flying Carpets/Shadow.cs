using System;
using Server;
using Server.Items;

namespace Server.Transport
{
	public class Shadow : BaseMulti
	{
		private int _northId;
		private int _eastId;
		private int _southId;
		private int _westId;

		public int NorthId { get { return _northId; } }
		public int EastId { get { return _eastId; } }
		public int SouthId { get { return _southId; } }
		public int WestId { get { return _westId; } }

		public Shadow( int northId, int eastId, int southId, int westId )
			: base( northId )
		{
			_northId = northId;
			_eastId = eastId;
			_southId = southId;
			_westId = westId;

			Hue = 1;
			Movable = false;
		}

		public Shadow( Serial serial ) : base( serial ) { }

		public virtual void OnRotate( Direction oldDir, Direction newDir )
		{
			switch( newDir )
			{
				case Direction.North: ItemID = NorthId; break;
				case Direction.East: ItemID = EastId; break;
				case Direction.South: ItemID = SouthId; break;
				case Direction.West: ItemID = WestId; break;
			}
		}

		#region serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );

			writer.Write( (int)NorthId );
			writer.Write( (int)EastId );
			writer.Write( (int)SouthId );
			writer.Write( (int)WestId );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch( version )
			{
				case 0:
					{
						_northId = reader.ReadInt();
						_eastId = reader.ReadInt();
						_southId = reader.ReadInt();
						_westId = reader.ReadInt();
						break;
					}
			}
		} 
		#endregion
	}
}