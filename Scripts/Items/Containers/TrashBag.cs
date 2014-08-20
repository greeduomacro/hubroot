using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class TrashBag : Bag
    {
        private Timer m_Timer;

        [Constructable]
        public TrashBag()
        {
            Hue = 1154;
            LootType = LootType.Blessed;
            Name = "a trash bag";

            m_Timer = new InternalTimer(this);
        }

        public TrashBag( Serial serial )
            : base(serial)
        {
        }

        private void StartTimer( Mobile from )
        {
            from.SendMessage("Emptying in one minute...");

            if( m_Timer == null )
                m_Timer = new InternalTimer(this);

            if( m_Timer.Running )
                m_Timer.Stop();

            m_Timer.Start();
        }

        private void Empty()
        {
            if( this.Items.Count <= 0 )
                return;

            Mobile holder = GetHolder();

            if( holder != null )
                holder.SendMessage("Emptying the trash bag!");
            else
                PublicOverheadMessage(Server.Network.MessageType.Regular, 906, false, "Emptying the trash bag!");

            List<Item> toDelete = new List<Item>();

            for( int i = 0; i < this.Items.Count; i++ )
            {
                if( this.Items[i] != null && !this.Items[i].Deleted )
                    toDelete.Add(this.Items[i]);
            }

            for( int i = 0; i < toDelete.Count; i++ )
                toDelete[i].Delete();
        }

        private Mobile GetHolder()
        {
            Mobile m = null;

            if( RootParent != null && RootParent is Mobile )
                m = RootParent as Mobile;
            else if( Parent != null && Parent is Mobile )
                m = Parent as Mobile;

            return m;
        }

        public override bool OnDragDrop( Mobile from, Item dropped )
        {
            if( TryDropItem(from, dropped, false) )
            {
                StartTimer(from);
                return true;
            }

            return false;
        }

        public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
        {
            if( TryDropItem(from, item, false) )
            {
                StartTimer(from);
                return true;
            }

            return false;
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private class InternalTimer : Timer
        {
            private TrashBag m_Bag;

            public InternalTimer( TrashBag bag )
                : base(TimeSpan.FromSeconds(60.0))
            {
                m_Bag = bag;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if( m_Bag != null )
                    m_Bag.Empty();
            }
        }
    }
}