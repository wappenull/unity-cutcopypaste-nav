using UnityEngine;
using System.Collections.Generic;

namespace CopyCutPaste
{
    // TODO: This would help history (static var) to not removed everytime script refresh 
    // But it is story for another day
#if false
    // Copied from https://github.com/acoppes/unity-history-window
    internal class EditorTemporaryMemory : MonoBehaviour
    {
        static EditorTemporaryMemory instance;

        static HideFlags instanceHideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;

        static void InitTemporaryMemory( )
        {
            if( instance != null )
                return;

            var editorMemory = GameObject.Find ("~EditorTemporaryMemory");

            if( editorMemory == null )
            {
                editorMemory = new GameObject( "~EditorTemporaryMemory" );
                instance = editorMemory.AddComponent<EditorTemporaryMemory>( );
            }
            else
            {
                instance = editorMemory.GetComponent<EditorTemporaryMemory>( );
                if( instance == null )
                    instance = editorMemory.AddComponent<EditorTemporaryMemory>( );
            }

            editorMemory.hideFlags = instanceHideFlags;
        }

        public static EditorTemporaryMemory Instance
        {
            get
            {
                InitTemporaryMemory( );
                return instance;
            }
        }

        [SerializeField]
        public SelectionHistory selectionHistory = new SelectionHistory();
    }
#endif

}