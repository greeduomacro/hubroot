//--------------------------------------------------------------------------------
// Copyright Joe Kraska, 2006. This file is restricted according to the GPL.
// Terms and conditions can be found in COPYING.txt.
//--------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
//--------------------------------------------------------------------------------
namespace Server.Collections {
//--------------------------------------------------------------------------------
//  Implements a basic MRU/LRU cache. Least recently used elements are expired from
//  the cache after it reaches capacity. Hit() and Store() methods offer O(1)
//  performance
//--------------------------------------------------------------------------------
public class Cache <T,K>
{
    int                             m_nentries;
    int                             m_size;
    Dictionary<K,Dlist.Entry>       m_dict;
    Dlist                           m_dlist;

    long                            m_hits;
    long                            m_misses;
    long                            m_stores;
    long                            m_ejections;

    public int                      Nentries { get { return m_nentries; } }
    public int                      Size { get { return m_size; } }

    public long                     Hits { get { return m_hits; } }
    public long                     Misses { get { return m_misses; } }
    public long                     Ejections { get { return m_ejections; } }
    public long                     Stores { get { return m_stores; } }

    public Cache( int size )
    {
        m_nentries = 0;
        m_size = size;
        m_dict = new Dictionary<K,Dlist.Entry>(size);
        m_dlist = new Dlist();
    }
    //----------------------------------------------------------------------------
    //  Hit() -- search for a 'cache hit'
    //----------------------------------------------------------------------------
    public T Hit( K key )
    {
//        if( !m_dict.ContainsKey( key ) )
//        {
//            m_misses++;
//            return default(T);
//        }
//
//        m_hits++;
//
//        Dlist.Entry hit = m_dict[key];
//
//        hit.Snip();
//
//        m_dlist.PushHead( hit );
//
//        return hit.m_data;

        Dlist.Entry hit;

        if( m_dict.TryGetValue( key, out hit ) )
        {
            m_hits++;
            hit.Snip();
            m_dlist.PushHead( hit );
            return hit.m_data;
        }

        m_misses++;
        return default( T );
    }
    //----------------------------------------------------------------------------
    //  Store() -- store an item in the cache; expires old items
    //----------------------------------------------------------------------------
    public void Store( K key, T val )
    {
        m_stores++;

        if( m_nentries + 1 > m_size )
        {
            m_ejections++;

            Dlist.Entry  toRemove = m_dlist.PopTail();
            //Console.WriteLine( "removing " + toRemove.m_key );
            m_dict.Remove( toRemove.m_key );
        }
        else m_nentries++;

        Dlist.Entry entry = new Dlist.Entry( key, val );
        m_dlist.PushHead( entry );
        m_dict.Add( key, entry );
    }
    //----------------------------------------------------------------------------
    //  Minimal implementation of a basic doubly-linked list that exposes its
    //  internals in a fashion amenable to the LRU/MRU cache functionality
    //----------------------------------------------------------------------------
    internal class Dlist
    {
        Entry     m_sentinel;

        public Dlist()
        {
            m_sentinel = new Entry();
            m_sentinel.m_next = m_sentinel.m_previous = m_sentinel;
        }

        public void PushHead( Entry entry )
        {
            entry.m_next                  = m_sentinel.m_next;
            entry.m_previous              = m_sentinel;

            m_sentinel.m_next.m_previous  = entry;
            m_sentinel.m_next             = entry;
        }

        public Entry PopTail( )
        {
            Entry tail              = m_sentinel.m_previous;

            tail.m_previous.m_next  = m_sentinel;

            m_sentinel.m_previous   = tail.m_previous;

            return tail;
        }

        internal class Entry
        {
            public  K           m_key;
            public  T           m_data;
            public  Entry       m_previous;
            public  Entry       m_next;

            public Entry()
            {
            //    m_key = -1;
            }
        
            public Entry(K key, T data)
            {
                m_key = key;
                m_data = data;
                m_next = m_previous = null;
            }

            public void Snip()
            {
                m_previous.m_next = m_next;
                m_next.m_previous = m_previous;
            }
        }
    }
}
//--------------------------------------------------------------------------------
} // namespace Custom.Collections 
//--------------------------------------------------------------------------------

