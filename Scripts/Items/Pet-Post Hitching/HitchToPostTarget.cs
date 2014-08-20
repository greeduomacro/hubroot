using Server.Items;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Targeting
{
    public class HitchToPostTarget : Target
    {
        private HitchingRope m_Rope;

        public HitchToPostTarget( HitchingRope rope )
            : base(4, false, TargetFlags.None)
        {
            m_Rope = rope;
        }

        protected override void OnTarget( Mobile from, object target )
        {
            if( target is HitchingPost )
            {
                HitchingPost post = (HitchingPost)target;

                if( !from.InRange(post.GetWorldLocation(), 2) )
                    from.SendLocalizedMessage(500295);

                else
                {
                    from.LocalOverheadMessage(MessageType.Regular, from.EmoteHue, false, "*ties the rope to the hitching post ring*");
                    from.SendMessage("Target the pet you wish to hitch.");

                    from.Target = new PostToPetTarget();
                }
            }
            else if( target is BaseCreature )
                from.SendMessage("You must tie the rope to a hitching post before being able to hitch your pet.");

            else if( target is PlayerMobile )
                from.SendMessage("The person looks at you in disgust.");

            else if( target == from )
                from.SendMessage("That would be a stupid idea.");
        }
    }
}