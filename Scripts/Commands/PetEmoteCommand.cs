using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    public class PetEmoteCommand
    {
        [CommandAttribute("PetEmote", AccessLevel.GameMaster)]
        public static void PetEmoteCommand_OnCommand( CommandEventArgs args )
        {
            Mobile m = args.Mobile;

            string text = args.ArgString.Trim();

            if( text != null && text.Length > 0 )
            {
                if( text.StartsWith("*") && text.EndsWith("*") )
                {
                    m.SendMessage("Select the pet to emote for.");
                    m.Target = new InternalTarget(text);
                }
                else
                    m.SendMessage("This command is only for emoting your pet\'s actions (use \"*\" around the text).");
            }
            else
            {
                m.SendMessage("Format: PetEmote <*text*>");
            }
        }

        private class InternalTarget : Target
        {
            private string m_Text;

            public InternalTarget( string text )
                : base(10, false, TargetFlags.None)
            {
                m_Text = text;
            }

            protected override void OnTarget( Mobile from, object target )
            {
                if( target is BaseCreature )
                {
                    BaseCreature bc = target as BaseCreature;

                    if( bc.Alive && !bc.IsDeadBondedPet )
                    {
                        if( (bc.Controlled && (bc.ControlMaster != null && bc.ControlMaster == from)) || from.AccessLevel >= AccessLevel.Counselor )
                        {
                            bc.Say(true, m_Text);
                        }
                        else
                        {
                            from.SendMessage("This is not your pet!");
                        }
                    }
                    else
                    {
                        from.SendMessage("This command only works on living pets.");
                    }
                }
                else
                {
                    from.SendMessage("This only works on pet creatures.");
                    from.Target = new InternalTarget(m_Text);
                }
            }
        }
    }
}