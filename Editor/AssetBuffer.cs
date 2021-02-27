using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace CopyCutPaste
{
	public class AssetBuffer
	{
		enum CutOrCopyCommand
		{
			None,
			Cut,
			Copy
		}

		private List<UnityEngine.Object> _buffer = new List<UnityEngine.Object>();
		private List<string> _bufferGuids = new List<string>();

		private CutOrCopyCommand _cutOrCopyCommand = CutOrCopyCommand.None;

		public bool hasCommand { get { return _cutOrCopyCommand != CutOrCopyCommand.None; } }
		public bool isCopyCommand { get { return _cutOrCopyCommand == CutOrCopyCommand.Copy; } }
		public bool isCutCommand { get { return _cutOrCopyCommand == CutOrCopyCommand.Cut; } }

		// ================================================================================
		//  public methods
		// --------------------------------------------------------------------------------

		public void Copy()
		{
			_cutOrCopyCommand = CutOrCopyCommand.Copy;

			FillBufferWithSelection();
		}

		public void Cut()
		{
			_cutOrCopyCommand = CutOrCopyCommand.Cut;

			FillBufferWithSelection();
		}

		public void Paste()
		{
			string targetPath = GetTargetPathFromSelection();

			if (targetPath != null)
			{
				ClearListOfAssetsInSubFolders(_buffer);
				ClearListOfAssetWithTargetPath(_buffer, targetPath);

				List<string> newPaths = new List<string>();

				foreach (var obj in _buffer)
				{
					obj.hideFlags = HideFlags.None;

					string fromPath = AssetDatabase.GetAssetPath(obj);
					string fileName = Path.GetFileName(fromPath);
					string toPath = Path.Combine(targetPath, fileName);

					toPath = AssetDatabase.GenerateUniqueAssetPath(toPath);

					if (isCopyCommand)
					{
						if (AssetDatabase.CopyAsset(fromPath, toPath))
						{
							newPaths.Add(toPath);
						}
					}
					else
					{
						string movedPath = AssetDatabase.MoveAsset(fromPath, toPath);
						newPaths.Add(movedPath);
					}
				}

				ClearCommand();

				List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>();
				foreach (var path in newPaths)
				{
					newSelection.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
				}
				Selection.objects = newSelection.ToArray();
			}
		}

		public void ClearCommand()
		{
			Clear();
			_cutOrCopyCommand = CutOrCopyCommand.None;
		}

		public bool ContainsGUID(string guid)
		{
			return _bufferGuids.Contains(guid);
		}

		// ================================================================================
		//  private methods
		// --------------------------------------------------------------------------------

		private void Clear()
		{
			_buffer.Clear();
			_bufferGuids.Clear();
		}

		private static string GetTargetPathFromSelection()
		{
			if (isTwoColumnMode)
			{
				return GetSelectedFolderPathForTwoColumnMode();
			}
			else
			{
				var targetObject = GetTargetObject();

				if (IsFolder(targetObject))
				{
					return AssetDatabase.GetAssetPath(targetObject);
				}
				else if (targetObject != null && AssetDatabase.IsMainAsset(targetObject))
				{
					return Path.GetDirectoryName(AssetDatabase.GetAssetPath(targetObject));
				}

				return null;
			}
		}

		/// <summary>
		/// returns the Object currently selected in the ProjectWindow, can be Asset or Folder
		/// apparently we cannot use Selection.activeObject here as something different is reported from Unity in TwoColumn View
		/// </summary>
		private static UnityEngine.Object GetTargetObject()
		{
			var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
			if (selectedAssets.Length == 1)
			{
				return selectedAssets[0];
			}

			foreach (var obj in selectedAssets)
			{
				if (obj == Selection.activeObject)
				{
					return obj;
				}
			}

			return null;
		}

		private static bool isTwoColumnMode
		{
			get
			{
				// we assume this is the ProjectWindow, otherwise we wouldn't be here
				var focusedWindow = EditorWindow.focusedWindow;
				var focusedWindowType = focusedWindow.GetType();

				var modeFieldInfo = focusedWindowType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic);

				int mode = (int)modeFieldInfo.GetValue(focusedWindow);

				// 0 == ViewMode.OneColumn
				// 1 == ViewMode.TwoColum

				return mode == 1;
			}
		}

		private static string GetSelectedFolderPathForTwoColumnMode()
		{
			var focusedWindow = EditorWindow.focusedWindow;
			var focusedWindowType = focusedWindow.GetType();

			var methodInfo = focusedWindowType.GetMethod("GetActiveFolderPath", BindingFlags.Instance | BindingFlags.NonPublic);
			return methodInfo.Invoke(focusedWindow, new object[0]) as string;
		}

		private static bool IsFolder(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			return obj is DefaultAsset && !AssetDatabase.IsForeignAsset(obj);
		}

		private static void ClearListOfAssetsInSubFolders(List<UnityEngine.Object> objects)
		{
			List<UnityEngine.Object> objectsInSubFolders = new List<UnityEngine.Object>();

			for (int i = 0; i < objects.Count; i++)
			{
				UnityEngine.Object obj = objects[i];
				string objPath = AssetDatabase.GetAssetPath(obj);

				for (int k = 0; k < objects.Count; k++)
				{
					// don't check against itself
					if (i == k)
					{
						continue;
					}

					UnityEngine.Object otherObject = objects[k];
					string otherObjPath = AssetDatabase.GetAssetPath(otherObject);

					if (objPath.StartsWith(otherObjPath))
					{
						objectsInSubFolders.Add(obj);
						break;
					}
				}
			}

			foreach (var item in objectsInSubFolders)
			{
				objects.Remove(item);
			}
		}

		private static void ClearListOfAssetWithTargetPath(List<UnityEngine.Object> objects, string targetPath)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				if (AssetDatabase.GetAssetPath(objects[i]) == targetPath)
				{
					objects.RemoveAt(i);
					break;
				}
			}
		}

		private void FillBufferWithSelection()
		{
			Clear();

			var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
			foreach (var obj in selectedAssets)
			{
				if (AssetDatabase.IsMainAsset(obj))
				{
					_buffer.Add(obj);
					_bufferGuids.Add(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
				}
			}
		}		
	}
}