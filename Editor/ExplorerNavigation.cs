using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System.Reflection;
using System;

/*
 * 
 * Original code by Stephan Hövelbrinks | http://twitter.com/talecrafter
 * Modified by Wappen to include explorer navagation | http://github.com/wappenull/
 * 
 */
namespace CopyCutPaste
{
    public static partial class ProjectWindowWatcher
    {
        static class ExplorerNavigation
        {
            internal static bool DebugLog = false;

            /* Explorer navigation extension ///////////////////////////////*/

            // Most reference are taken from https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/ProjectBrowser.cs

            // This will be removed when refresh script, but I will ok with it for now
            static SelectionHistory historyBuffer = new SelectionHistory( );

            public static void RecordCurrentState( )
            {
                SelectionHistory.Item state = GetCurrentProjectWindowState( );
                historyBuffer.PushIfUniqueOrUpdateLastSelectionContent( state, Selection.activeInstanceID ); // If we are on same state, send current selection item in this state

#if false
			    // Do not record empty or multiple selection (probably from search result)
			    if( Selection.activeObject == null || Selection.instanceIDs.Length > 1 )
				    return;

                // Only check folder if it changed
                string path = AssetDatabase.GetAssetPath( Selection.activeInstanceID );
			    if( string.IsNullOrEmpty( path ) )
				    return;

			    if( ProjectWindowUtil.IsFolder( Selection.activeInstanceID ) )
			    {
				    historyBuffer.PushIfUnique( path );
			    }
			    else
			    {
				    // If it is file within folder, such as Assets/Folder1/Folder2/file.xxx
				    // Record Assets/Folder1/Folder2
				    string parentFolder = ProjectWindowUtil.GetContainingFolder( path );
				    historyBuffer.PushIfUnique( parentFolder );
			    }
#endif
            }

            static string m_LastSearch;
            static double m_NextSearch = double.MaxValue;
            static SelectionHistory.Item GetCurrentProjectWindowState( )
            {
                SelectionHistory.Item state = default;
                string searching = GetCurrentSearchFilter( );
                if( !string.IsNullOrEmpty( searching ) ) // Check if user is using search feature
                {
                    // Unity editor will delay search commit time, we will also use that to delay
                    // Unity uses around 0.25 sec, we will top it a bit to 0.5
                    const double SearchTimeDelay = 0.5;
                    if( m_LastSearch != searching )
                    {
                        m_LastSearch = searching;
                        m_NextSearch = EditorApplication.timeSinceStartup + SearchTimeDelay;
                    }

                    if( EditorApplication.timeSinceStartup > m_NextSearch )
                    {
                        // Finally output current search result
                        m_NextSearch = double.MaxValue;
                        state.isSearch = true;
                        state.s = searching;
                    }
                    else
                    {
                        // Return empty state
                    }
                }
                else if( TryGetActiveFolderPath( out string path ) )
                {
                    state.isSearch = false;
                    state.s = path;
                }
                return state;
            }

            public static void CheckForExplorerNavigationCommand( )
            {
                if( GUIEvent.alt ) // ALT key is holding
                {
                    var arrow = GUIEvent.usesArrowKey;
                    if( arrow == GUIEvent.ArrowKey.Up )
                    {
                        // Pressing Up while in search state
                        // normally search would have to actual parent to go up to
                        // (Windows explorer will just return to Desktop)
                        // We will make this action become "Back" instead
                        string searching = GetCurrentSearchFilter( );
                        if( !string.IsNullOrEmpty( searching ) )
                        {
                            _BackOneLevel( );
                        }
                        else
                        {
                            _UpOneLevel( );
                        }
                    }
                    else if( arrow == GUIEvent.ArrowKey.Left )
                    {
                        // Back
                        _BackOneLevel( );
                    }
                    else if( arrow == GUIEvent.ArrowKey.Right )
                    {
                        // Forward
                        _ForwardOneLevel( );
                    }
                }
            }

            private static void _BackOneLevel( )
            {
                var previousState = historyBuffer.PeekBack( );
                if( previousState.IsValid )
                {
                    historyBuffer.StepBack( );
                    _ApplyState( previousState );
                }
            }

            private static void _ForwardOneLevel( )
            {
                var forwardState = historyBuffer.PeekForward( );
                if( forwardState.IsValid )
                {
                    historyBuffer.StepForward( );
                    _ApplyState( forwardState );
                }
            }

            static void _UpOneLevel( )
            {
                if( TryGetActiveFolderPath( out string currentPath ) )
                {
                    if( DebugLog ) Debug.Log( "Currrent is " + currentPath );

                    string up1 = ProjectWindowUtil.GetContainingFolder( currentPath );
                    if( AssetDatabase.IsValidFolder( up1 ) )
                    {
                        var upObject = AssetDatabase.LoadAssetAtPath<Object>( currentPath ); // Highlight last folder that we came from
                        Selection.activeObject = upObject;
                        EditorUtility.FocusProjectWindow( );
                        //EditorGUIUtility.PingObject( upObject ); // Also ping it for annoyance
                        if( DebugLog ) Debug.Log( "Up parent level" );

                        // Also manually record this action
                        RecordCurrentState( );
                    }
                }
            }

            static void _ApplyState( SelectionHistory.Item state )
            {
                if( state.isSearch )
                {
                    // Restore a search result
                    if( DebugLog ) Debug.Log( "Back to search " + state.s );
                    ShowSearchQuery( state.s );
                    historyBuffer.StepBack( );
                }
                else
                {
                    if( DebugLog ) Debug.Log( "Back to folder " + state.s );
                    ShowSearchQuery( null );
                    ShowFolderContents( state.s, true );

                    // If backing
                    // Also make selection on last object in that state
                    Selection.activeInstanceID = state.lastSelectedInstancId;
                }
            }

            /* Reflection accessor ////////////////////////////*/

            private static bool TryGetActiveFolderPath( out string path )
            {
                if( _tryGetActiveFolderPath == null )
                    _tryGetActiveFolderPath = typeof( ProjectWindowUtil ).GetMethod( "TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic );

                object[] args = new object[] { null };
                bool found = (bool)_tryGetActiveFolderPath.Invoke( null, args );
                path = (string)args[0];

                return found;
            }

            static Type _projectBrowserType;
            static Type ProjectBrowserType
            {
                get
                {
                    if( _projectBrowserType == null )
                        _projectBrowserType = Type.GetType( "UnityEditor.ProjectBrowser,UnityEditor.dll" );
                    return _projectBrowserType;
                }
            }

            static FieldInfo _projectBrowserSearchField;
            static MethodInfo _projectBrowserShowFolderContents;
            static MethodInfo _projectBrowserGetFolderInstanceId;

            private static string GetCurrentSearchFilter( )
            {
                object objProjectBrowser = GetBrowserMethod.Invoke( null, null );
                if( objProjectBrowser == null )
                    return null;

                if( _projectBrowserSearchField == null )
                    _projectBrowserSearchField = ProjectBrowserType.GetField( "m_SearchFieldText", BindingFlags.Instance | BindingFlags.NonPublic );

                string searchText = (string)_projectBrowserSearchField.GetValue( objProjectBrowser );
                return searchText;
            }

            private static void ShowFolderContents( string path, bool revealAndFrameInFolderTree )
            {
                int instanceId = GetFolderInstanceId( path );
                if( instanceId == 0 )
                    return;

                ShowFolderContents( instanceId, revealAndFrameInFolderTree );
            }
            private static void ShowFolderContents( int folderInstanceId, bool revealAndFrameInFolderTree )
            {
                object objProjectBrowser = GetBrowserMethod.Invoke( null, null );
                if( objProjectBrowser == null )
                    return;

                if( _projectBrowserShowFolderContents == null )
                    _projectBrowserShowFolderContents = ProjectBrowserType.GetMethod( "ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic );

                object[] args = new object[] { folderInstanceId, revealAndFrameInFolderTree };
                _projectBrowserShowFolderContents.Invoke( objProjectBrowser, args );
            }

            private static int GetFolderInstanceId( string path )
            {
                object objProjectBrowser = GetBrowserMethod.Invoke( null, null );
                if( objProjectBrowser == null )
                    return 0;

                if( !AssetDatabase.IsValidFolder( path ) )
                    return 0;

                Object f = AssetDatabase.LoadAssetAtPath<Object>( path );
                if( f == null )
                    return 0;

                return f.GetInstanceID( );

#if false
            if( _projectBrowserGetFolderInstanceId == null )
                _projectBrowserGetFolderInstanceId = ProjectBrowserType.GetMethod( "GetFolderInstanceId", BindingFlags.Static | BindingFlags.NonPublic );

            object[] args = new object[] { path };
            int id = (int)_projectBrowserGetFolderInstanceId.Invoke( objProjectBrowser, args );
            return id;
#endif
            }

            static MethodInfo _projectBrowserSetSearch;
            static MethodInfo _projectBrowserClearSearch;
            private static void ShowSearchQuery( string q )
            {
                object objProjectBrowser = GetBrowserMethod.Invoke( null, null );
                if( objProjectBrowser == null )
                    return;

                if( string.IsNullOrEmpty( q ) )
                {
                    if( _projectBrowserClearSearch == null )
                        _projectBrowserClearSearch = ProjectBrowserType.GetMethod( "ClearSearch", BindingFlags.Instance | BindingFlags.NonPublic );
                    _projectBrowserClearSearch.Invoke( objProjectBrowser, null );
                }
                else
                {
                    if( _projectBrowserSetSearch == null )
                    {
                        // Specifically as for SetSearch( string )
                        Type[] argList = new Type[] { typeof(string) };
                        _projectBrowserSetSearch = ProjectBrowserType.GetMethod( "SetSearch", BindingFlags.Instance | BindingFlags.Public, Type.DefaultBinder, argList, null );
                    }
                    object[] args = new object[] { q };
                    _projectBrowserSetSearch.Invoke( objProjectBrowser, args );
                }
            }
        }

    }
}