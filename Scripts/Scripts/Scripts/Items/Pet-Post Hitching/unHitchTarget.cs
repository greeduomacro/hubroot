using System;
using Server.Items;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Targeting
{
    public class unHitchTarget : Target
    {
        private HitchingPost m_Post;

        public unHitchTarget( HitchingPost post )
            : base(2, false, TargetFlags.None)
        {
            m_Post = post;
        }

        protected override void OnTarget( Mobile from, object target )
        {
            if( target is BaseCreature )
            {
                BaseCreature pet = (BaseCreature)target;

                if( !from.InRange(m_Post.GetWorldLocation(), 2) )
                {
                    from.SendMessage("You must be closer to the post.");
                    from.Target = new unHitchTarget(m_Post);
                }
                else if( pet.Body.IsHuman )
                    from.SendMessage("The person looks at you in disgust.");
                else if( pet.ControlMaster != from )
                    from.SendMessage("That is not your pet.");
                else if( pet.Combatant != null && pet.InRange(pet.Combatant, 12) )
                    from.SendMessage("You cannot do that while your pet is fighting.");
                else if( pet.Controlled == true && pet.ControlMaster == from )
                {
                    string untie = String.Format("*unties {0} pet*", from.Female == true ? "her" : "his");

                    if( pet.Blessed == true )
                    {
                        pet.Blessed = false;
                        pet.CantWalk = false;
                        pet.ControlOrder = OrderType.Come;

                        from.LocalOverheadMessage(MessageType.Regular, from.EmoteHue, false, untie);
                        from.SendMessage("You have unhitched your pet.");
                        from.AddToBackpack(new HitchingRope());
                    }
                }
            }
        }
    }
}