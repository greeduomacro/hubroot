using System;
using Server;
using Server.Prompts;

namespace Server.PlayercastStatues
{
	public class EngraveStatuePrompt : Prompt
	{
		private PlayercastStatue m_Statue;

		public EngraveStatuePrompt( PlayercastStatue s )
		{
			m_Statue = s;
		}

		public override void OnResponse( Mobile from, string text )
		{
			if( m_Statue != null && !m_Statue.Deleted )
			{
				if( m_Statue.HasPlinth )
				{
					m_Statue.Engraving = text;

					from.SendMessage( "The statue\'s plinth has been engraved." );
				}
				else
				{
					from.SendMessage( "That statue lacks a plinth to engrave." );
				}
			}
		}
	}

	public class StatueRenamePrompt : Prompt
	{
		private PlayercastStatue m_Statue;

		public StatueRenamePrompt( PlayercastStatue s )
		{
			m_Statue = s;
		}

		public override void OnResponse( Mobile from, string text )
		{
			if( m_Statue != null && !m_Statue.Deleted )
			{
				m_Statue.RawName = text;

				from.SendMessage( "The statue\'s name has been changed." );
			}
		}
	}
}