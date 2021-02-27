using System;
using System.Collections.Generic;
using UnityEngine;

namespace CopyCutPaste
{
    // Original code from https://github.com/acoppes/unity-history-window
    // Modified by Wappen to use with this project
    [Serializable]
    internal class SelectionHistory
    {
        [SerializeField]
        List<Item> _history = new List<Item>(100);

        internal struct Item : IEquatable<Item>
        {
            public string s;
            public bool isSearch;
            internal int lastSelectedInstancId;

            public bool IsValid => !string.IsNullOrEmpty( s );

            public override string ToString( )
            {
                return $"search={isSearch} {s}";
            }
            public override bool Equals( object obj )
            {
                if( !(obj is Item) )
                    return false;
                return Equals( (Item)obj );
            }

            public bool Equals( Item other )
            {
                return isSearch == other.isSearch && s == other.s;
            }

            public static bool operator ==( Item x, Item y )
            {
                return x.Equals( y );
            }

            public static bool operator !=( Item x, Item y )
            {
                return !x.Equals( y );
            }

            public override int GetHashCode( )
            {
                int hash = isSearch.GetHashCode( );
                if( s != null )
                    hash += s.GetHashCode( );
                return hash;
            }
        }

        int currentPointer;

        int historySize = 10;

        public List<Item> History
        {
            get { return _history; }
        }

        public int HistorySize
        {
            get { return historySize; }
            set { historySize = value; }
        }

        public void Clear( )
        {
            _history.Clear( );
        }

        public int GetHistoryCount( )
        {
            return _history.Count;
        }

        public void PushIfUniqueOrUpdateLastSelectionContent( Item i, int lastSelectionInstanceId )
        {
            if( !i.IsValid )
                return;

            var currentItem = _history.Count > 0 ? _history[currentPointer] : default;

            if( currentItem != i )
            {
                // Pushing new one will discard all next history
                // See 'New item insert' case below
                while( _history.Count-1 > currentPointer ) // If stack top index is > currentPointer, remove them
                    _history.RemoveAt( _history.Count - 1 );

                // Finally add
                _history.Add( i );
                currentPointer = _history.Count - 1;

                //Debug.Log( $"Push {i}\nHistory now have {_history.Count} item" );
            }
            else if( i.isSearch == false )
            {
                // If not a search type, update inner instance id selection in that state
                i.lastSelectedInstancId = lastSelectionInstanceId;
                _history[currentPointer] = i;
            }

            // Trim max history size
            if( _history.Count > historySize )
                _history.RemoveRange( 0, _history.Count - historySize );
            if( currentPointer > _history.Count - 1 )
                currentPointer = _history.Count - 1;
        }

        public Item PeekBack( )
        {
            if( _history.Count > 0 && currentPointer > 0 )
                return _history[currentPointer-1];
            return default;
        }

        public Item PeekForward( )
        {
            if( currentPointer+1 < _history.Count )
                return _history[currentPointer+1];
            return default;
        }

        // In normal operation, currentPointer will always point to last index

        // Initial
        // [] [] [] []
        // ^ currentPointer = 0, _history.Count = 0

        // After first update, (user cannot back at this point)
        // [0] [] [] []
        //  ^ currentPointer = 0, _history.Count = 1

        // After 3 adds
        // [0] [1] [2] [3] []
        //              ^ currentPointer = 3, _history.Count = 4

        // After previous
        // [0] [1] [2] [3] []
        //          ^ currentPointer = 2, _history.Count = 4

        // New item insert ([3] is killed)
        // [0] [1] [2] [x] Remove all index higher than currentPointer
        //          ^
        // [0] [1] [2] [3a] Then add the new 3
        //              ^

        public void StepBack( )
        {
            if( _history.Count == 0 || currentPointer == 0 )
                return;

            currentPointer--;
        }

        public void StepForward( )
        {
            // Has future to go to, see 'After previous' case
            if( currentPointer+1 < _history.Count )
                currentPointer++;
        }

    }
}