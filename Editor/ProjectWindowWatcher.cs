using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/*
 * 
 * Original code by Stephan Hövelbrinks | http://twitter.com/talecrafter
 * Modified by Wappen to include explorer navagation | http://github.com/wappenull/
 * 
 */
namespace CopyCutPaste
{
    [InitializeOnLoad]
    public static partial class ProjectWindowWatcher
    {
        private static AssetBuffer _assetBuffer = new AssetBuffer();

        // ================================================================================
        //  static constructor
        // --------------------------------------------------------------------------------

        static ProjectWindowWatcher( )
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
            EditorApplication.update += EditorUpdate;
            Selection.selectionChanged += ExplorerNavigation.RecordCurrentState;
        }

        // ================================================================================
        //  update methods
        // --------------------------------------------------------------------------------

        private static void EditorUpdate( )
        {
            if( _assetBuffer.hasCommand )
            {
                if( EditorWindow.focusedWindow == null || !EditorWindow.focusedWindow.titleContent.text.Contains( "Project" ) )
                {
                    _assetBuffer.ClearCommand( );
                    EditorApplication.RepaintProjectWindow( );
                }
            }

            // Selection monitor also need regular update
            // In case of script reload or sometime that current project browser folder is changed without user selection
            ExplorerNavigation.RecordCurrentState( );
        }

        // ================================================================================
        //  checking editor events
        // --------------------------------------------------------------------------------

        private static void ProjectWindowItemOnGUI( string guid, Rect selectionRect )
        {
            if( _assetBuffer.isCutCommand && _assetBuffer.ContainsGUID( guid ) )
            {
                DrawRectForCutObject( selectionRect, new Color( 1f, 0, 0, 0.7f ) );
            }

            CheckForCutCopyPasteCommands( );
            ExplorerNavigation.CheckForExplorerNavigationCommand( );
        }

        public static void DrawRectForCutObject( Rect rect, Color color )
        {
            rect.width -= 1f;

            Color prev = Handles.color;
            Handles.color = color;

            Handles.DrawLine( rect.TopLeft( ), rect.TopRight( ) );
            Handles.DrawLine( rect.BottomLeft( ), rect.BottomRight( ) );
            Handles.DrawLine( rect.TopLeft( ), rect.BottomLeft( ) );
            Handles.DrawLine( rect.TopRight( ), rect.BottomRight( ) );

            Handles.color = prev;
        }

        private static void CheckForCutCopyPasteCommands( )
        {
            if( ProjectViewIsRenaming( ) )
                return;

            if( _assetBuffer.hasCommand && GUIEvent.isEscapeKey )
            {
                _assetBuffer.ClearCommand( );
                EditorApplication.RepaintProjectWindow( );
            }

            if( GUIEvent.isValidateCopy || GUIEvent.isValidateCut || GUIEvent.isValidatePaste )
            {
                GUIEvent.Use( );
            }

            if( GUIEvent.isExecuteCopy )
            {
                GUIEvent.Use( );
                _assetBuffer.Copy( );
            }
            if( GUIEvent.isExecuteCut )
            {
                GUIEvent.Use( );
                _assetBuffer.Cut( );
            }
            if( GUIEvent.isExecutePaste )
            {
                GUIEvent.Use( );
                _assetBuffer.Paste( );
            }
        }

        // ================================================================================
        //  check if project view is renaming
        //  added by Olivier Fouques (@PsyKola)
        // --------------------------------------------------------------------------------

        private static MethodInfo _getBrowserMethod = null;
        private static FieldInfo _listAreaField = null;
        private static MethodInfo _renameOverlayMethod = null;
        private static MethodInfo _isRenamingMethod = null;
        private static MethodInfo _tryGetActiveFolderPath = null;

        private static MethodInfo GetBrowserMethod
        {
            get
            {
                if( _getBrowserMethod == null )
                {
                    _getBrowserMethod = typeof( ProjectWindowUtil ).GetMethod( "GetProjectBrowserIfExists", BindingFlags.Static | BindingFlags.NonPublic );
                }
                return _getBrowserMethod;
            }
        }

        private static FieldInfo ListAreaField
        {
            get
            {
                if( _listAreaField == null )
                {
                    Type projectViewType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor.dll");
                    _listAreaField = projectViewType.GetField( "m_ListArea", BindingFlags.Instance | BindingFlags.NonPublic );
                }
                return _listAreaField;
            }
        }

        private static MethodInfo RenameOverlayMethod
        {
            get
            {
                if( _renameOverlayMethod == null )
                {
                    Type listAreaType = Type.GetType("UnityEditor.ObjectListArea,UnityEditor.dll");
                    _renameOverlayMethod = listAreaType.GetMethod( "GetRenameOverlay", BindingFlags.Instance | BindingFlags.NonPublic );
                }
                return _renameOverlayMethod;
            }
        }

        private static MethodInfo IsRenamingMethod
        {
            get
            {
                if( _isRenamingMethod == null )
                {
                    Type renameOverlayType = Type.GetType("UnityEditor.RenameOverlay,UnityEditor.dll");
                    _isRenamingMethod = renameOverlayType.GetMethod( "IsRenaming", BindingFlags.Instance | BindingFlags.Public );
                }
                return _isRenamingMethod;
            }
        }

        private static bool ProjectViewIsRenaming( )
        {
            var browserInstance = GetBrowserMethod.Invoke(null, null);
            if( browserInstance == null )
                return false;

            var field = ListAreaField.GetValue(browserInstance);
            if( field == null )
            {
                return false;
            }

            var renameOverlay = RenameOverlayMethod.Invoke(field, null);
            if( renameOverlay == null )
            {
                return false;
            }

            return (bool)IsRenamingMethod.Invoke( renameOverlay, null );
        }
		
	}
}