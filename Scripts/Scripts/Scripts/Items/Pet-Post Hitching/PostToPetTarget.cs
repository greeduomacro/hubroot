using System;
using Server.Items;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Targeting
{
    public class PostToPetTarget : Target
    {
        private BaseAI m_ai;

        public PostToPetTarget()
            : base(2, false, TargetFlags.None)
        {
        }

        protected override void OnTarget( Mobile from, object target )
        {
            if( target is BaseCreature )
            {
                BaseCreature pet = (BaseCreature)target;
                m_ai = pet.AIObject;

                if( pet.Body.IsHuman )
                    from.SendMessage("The person looks at you in disgust.");
                else if( pet.ControlMaster != from )
                    from.SendMessage("That is not your pet.");
                else if( pet.Combatant != null && pet.InRange(pet.Combatant, 12) )
                    from.SendMessage("You cannot do that while your pet is fighting.");
                else if( pet.Controlled == true && pet.ControlMaster == from )
                {
                    if( Utility.Random(15) == 1 )
                    {
                        m_ai.DoMove(Direction.South);
                        m_ai.DoMove(Direction.South);
                        m_ai.DoMove(Direction.East);
                        m_ai.DoMove(Direction.South);
                        m_ai.NextMove = DateTime.Now;

                        from.SendMessage("Your pet shies away from the rope.");
                        pet.PlaySound(pet.GetAngerSound());
                    }
                    else
                    {
                        if( pet.Blessed == false )
                        {
                            pet.Blessed = true;
                            pet.CantWalk = true;
                            pet.ControlOrder = OrderType.Stay;
                            pet.Combatant = null;
                            pet.Loyalty = 85;

                            from.LocalOverheadMessage(MessageType.Regular, from.EmoteHue, false, "*attaches the rope to the animal*");
                            from.Backpack.ConsumeTotal(typeof(HitchingRope), 1);
                            from.SendMessage("Your pet has been hitched to the post.");
                        }
                    }
                }
            }
        }
    }
}