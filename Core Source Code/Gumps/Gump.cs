/***************************************************************************
 *                                 Gump.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Gump.cs 1065 2013-06-02 13:12:09Z eos@runuo.com $
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;

namespace Server.Gumps
{
    public class Gump
    {
        private List<GumpEntry> m_Entries;
        private List<string> m_Strings;

        internal int m_TextEntries, m_Switches;

        private static int m_NextSerial = 1;

        private int m_Serial;
        private int m_TypeID;
        private int m_X, m_Y;

        private bool m_Dragable = true;
        private bool m_Closable = true;
        private bool m_Resizable = true;
        private bool m_Disposable = true;

        public static int GetTypeID( Type type )
        {
            return type.FullName.GetHashCode();
        }

        public Gump( int x, int y )
        {
            do
            {
                m_Serial = m_NextSerial++;
            } while( m_Serial == 0 ); // standard client apparently doesn't send a gump response packet if serial == 0

            m_X = x;
            m_Y = y;

            m_TypeID = GetTypeID(this.GetType());

            m_Entries = new List<GumpEntry>();
            m_Strings = new List<string>();
        }

        public void Invalidate()
        {
            //if ( m_Strings.Count > 0 )
            //	m_Strings.Clear();
        }

        public int TypeID
        {
            get
            {
                return m_TypeID;
            }
        }

        public List<GumpEntry> Entries
        {
            get { return m_Entries; }
        }

        public int Serial
        {
            get
            {
                return m_Serial;
            }
            set
            {
                if( m_Serial != value )
                {
                    m_Serial = value;
                    Invalidate();
                }
            }
        }

        public int X
        {
            get
            {
                return m_X;
            }
            set
            {
                if( m_X != value )
                {
                    m_X = value;
                    Invalidate();
                }
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                if( m_Y != value )
                {
                    m_Y = value;
                    Invalidate();
                }
            }
        }

        public bool Disposable
        {
            get
            {
                return m_Disposable;
            }
            set
            {
                if( m_Disposable != value )
                {
                    m_Disposable = value;
                    Invalidate();
                }
            }
        }

        public bool Resizable
        {
            get
            {
                return m_Resizable;
            }
            set
            {
                if( m_Resizable != value )
                {
                    m_Resizable = value;
                    Invalidate();
                }
            }
        }

        public bool Dragable
        {
            get
            {
                return m_Dragable;
            }
            set
            {
                if( m_Dragable != value )
                {
                    m_Dragable = value;
                    Invalidate();
                }
            }
        }

        public bool Closable
        {
            get
            {
                return m_Closable;
            }
            set
            {
                if( m_Closable != value )
                {
                    m_Closable = value;
                    Invalidate();
                }
            }
        }

        public void AddPage( int page )
        {
            Add(new GumpPage(page));
        }

        public void AddAlphaRegion( int x, int y, int width, int height )
        {
            Add(new GumpAlphaRegion(x, y, width, height));
        }

        /// <summary>
        /// Adds a dark alpha region
        /// </summary>
        public void AddBlackAlpha( int x, int y, int width, int height )
        {
            AddImageTiled(x, y, width, height, 2624);
            AddAlphaRegion(x, y, width, height);
        }

        public void AddBackground( int x, int y, int width, int height, int gumpID )
        {
            Add(new GumpBackground(x, y, width, height, gumpID));
        }

        public void AddButton( int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param )
        {
            Add(new GumpButton(x, y, normalID, pressedID, buttonID, type, param));
        }

        public void AddCheck( int x, int y, int inactiveID, int activeID, bool initialState, int switchID )
        {
            Add(new GumpCheck(x, y, inactiveID, activeID, initialState, switchID));
        }

        public void AddGroup( int group )
        {
            Add(new GumpGroup(group));
        }

        public void AddTooltip( int number )
        {
            Add(new GumpTooltip(number));
        }

        public void AddHtml( int x, int y, int width, int height, string text, bool background, bool scrollbar )
        {
            Add(new GumpHtml(x, y, width, height, text, background, scrollbar));
        }

        /// <summary>
        /// Formats a string to colorize in a HTML control
        /// </summary>
        /// <param name="text">the text to colorize</param>
        /// <param name="color">the hex code of the color to apply</param>
        public string Color( string text, int color )
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }

        /// <summary>
        /// Center-aligns a string in a HTML control
        /// </summary>
        /// <param name="text">the text to align</param>
        public string Center( string text )
        {
            return String.Format("<CENTER>{0}</CENTER>", text);
        }

        public void AddHtmlLocalized( int x, int y, int width, int height, int number, bool background, bool scrollbar )
        {
            Add(new GumpHtmlLocalized(x, y, width, height, number, background, scrollbar));
        }

        public void AddHtmlLocalized( int x, int y, int width, int height, int number, int color, bool background, bool scrollbar )
        {
            Add(new GumpHtmlLocalized(x, y, width, height, number, color, background, scrollbar));
        }

        public void AddHtmlLocalized( int x, int y, int width, int height, int number, string args, int color, bool background, bool scrollbar )
        {
            Add(new GumpHtmlLocalized(x, y, width, height, number, args, color, background, scrollbar));
        }

        public void AddImage( int x, int y, int gumpID )
        {
            Add(new GumpImage(x, y, gumpID));
        }

        public void AddImage( int x, int y, int gumpID, int hue )
        {
            Add(new GumpImage(x, y, gumpID, hue));
        }

        public void AddImageTiled( int x, int y, int width, int height, int gumpID )
        {
            Add(new GumpImageTiled(x, y, width, height, gumpID));
        }

        public void AddImageTiledButton( int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param, int itemID, int hue, int width, int height )
        {
            Add(new GumpImageTileButton(x, y, normalID, pressedID, buttonID, type, param, itemID, hue, width, height));
        }
        public void AddImageTiledButton( int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param, int itemID, int hue, int width, int height, int localizedTooltip )
        {
            Add(new GumpImageTileButton(x, y, normalID, pressedID, buttonID, type, param, itemID, hue, width, height, localizedTooltip));
        }

        public void AddItem( int x, int y, int itemID )
        {
            Add(new GumpItem(x, y, itemID));
        }

        public void AddItem( int x, int y, int itemID, int hue )
        {
            Add(new GumpItem(x, y, itemID, hue));
        }

        public void AddLabel( int x, int y, int hue, string text )
        {
            Add(new GumpLabel(x, y, hue, text));
        }

        public void AddLabelCropped( int x, int y, int width, int height, int hue, string text )
        {
            Add(new GumpLabelCropped(x, y, width, height, hue, text));
        }

        public void AddRadio( int x, int y, int inactiveID, int activeID, bool initialState, int switchID )
        {
            Add(new GumpRadio(x, y, inactiveID, activeID, initialState, switchID));
        }

        public void AddTextEntry( int x, int y, int width, int height, int hue, int entryID, string initialText )
        {
            Add(new GumpTextEntry(x, y, width, height, hue, entryID, initialText));
        }

        public void AddTextEntry( int x, int y, int width, int height, int hue, int entryID, string initialText, int size )
        {
            Add(new GumpTextEntryLimited(x, y, width, height, hue, entryID, initialText, size));
        }

		public void AddItemProperty( int serial )
		{
			Add( new GumpItemProperty( serial ) );
		}

        public void Add( GumpEntry g )
        {
            if( g.Parent != this )
            {
                g.Parent = this;
            }
            else if( !m_Entries.Contains(g) )
            {
                Invalidate();
                m_Entries.Add(g);
            }
        }

        public void Remove( GumpEntry g )
        {
            if( g == null || !m_Entries.Contains(g) )
                return;

            Invalidate();
            m_Entries.Remove(g);
            g.Parent = null;
        }

        public int Intern( string value )
        {
            int indexOf = m_Strings.IndexOf(value);

            if( indexOf >= 0 )
            {
                return indexOf;
            }
            else
            {
                Invalidate();
                m_Strings.Add(value);
                return m_Strings.Count - 1;
            }
        }

        public void SendTo( NetState state )
        {
            state.AddGump(this);
            state.Send(Compile(state));
        }

        public static byte[] StringToBuffer( string str )
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static byte[] m_BeginLayout = StringToBuffer("{ ");
        private static byte[] m_EndLayout = StringToBuffer(" }");

        private static byte[] m_NoMove = StringToBuffer("{ nomove }");
        private static byte[] m_NoClose = StringToBuffer("{ noclose }");
        private static byte[] m_NoDispose = StringToBuffer("{ nodispose }");
        private static byte[] m_NoResize = StringToBuffer("{ noresize }");

        private Packet Compile()
        {
            return Compile(null);
        }

        private Packet Compile( NetState ns )
        {
            IGumpWriter disp;

            if( ns != null && ns.Unpack )
                disp = new DisplayGumpPacked(this);
            else
                disp = new DisplayGumpFast(this);

            if( !m_Dragable )
                disp.AppendLayout(m_NoMove);

            if( !m_Closable )
                disp.AppendLayout(m_NoClose);

            if( !m_Disposable )
                disp.AppendLayout(m_NoDispose);

            if( !m_Resizable )
                disp.AppendLayout(m_NoResize);

            int count = m_Entries.Count;
            GumpEntry e;

            for( int i = 0; i < count; ++i )
            {
                e = m_Entries[i];

                disp.AppendLayout(m_BeginLayout);
                e.AppendTo(disp);
                disp.AppendLayout(m_EndLayout);
            }

            disp.WriteStrings(m_Strings);

            disp.Flush();

            m_TextEntries = disp.TextEntries;
            m_Switches = disp.Switches;

            return disp as Packet;
        }

        public virtual void OnResponse( NetState sender, RelayInfo info )
        {
        }

        public virtual void OnServerClose( NetState owner )
        {
        }

        protected virtual int GetButtonId( int type, int index )
        {
            return (1 + (index * 10) + type);
        }

        protected virtual void DecodeButtonId( int buttonId, out int val, out int type, out int index )
        {
            val = buttonId - 1;

            type = -1;
            index = -1;

            if( val > 0 )
            {
                type = (val % 10);
                index = (val / 10);
            }
        }
    }
}