using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace CopyCutPaste
{
	public static class GUIEvent
	{
		public static void Use()
		{
			if (Event.current != null)
			{
				Event.current.Use();
			}
		}

		public static bool CheckAndUse(bool isValid)
		{
			if (isValid)
			{
				Event.current.Use();
			}
			return isValid;
		}

		#region mouse queries

		// ================================================================================
		//  mouse queries
		// --------------------------------------------------------------------------------

		public static Vector2 mousePosition { get { return Event.current.mousePosition; } }

		public static bool isMouse { get { return Event.current.isMouse; } }
		public static int mouseButton { get { return Event.current.button; } }
		public static int clickCount { get { return Event.current.clickCount; } }

		public static bool isMouseUp { get { return Event.current.type == EventType.MouseUp; } }

		/// <summary>
		/// left mouse button click as event
		/// </summary>
		public static bool isLeftMouseClick { get { return Event.current.type == EventType.MouseDown && Event.current.button == 0; } }
		public static bool useLeftMouseClick { get { return CheckAndUse(isLeftMouseClick); } }

		/// <summary>
		/// left mouse button is used (e.g. in GUI.Button)
		/// </summary>
		public static bool isLeftMouseButton { get { return Event.current.button == 0; } }

		public static bool isLeftMouseUp { get { return Event.current.type == EventType.MouseUp && Event.current.button == 0; } }
		public static bool isLeftMouseDrag { get { return Event.current.type == EventType.MouseDrag && Event.current.button == 0; } }
		public static bool isLeftMouseClickOrDrag { get { return isLeftMouseClick || isLeftMouseDrag; } }

		/// <summary>
		/// right mouse button click as event
		/// </summary>
		public static bool isRightMouseClick { get { return Event.current.type == EventType.MouseDown && Event.current.button == 1; } }
		public static bool isRightMouseUp { get { return Event.current.type == EventType.MouseUp && Event.current.button == 1; } }
		public static bool isRightMouseDrag { get { return Event.current.type == EventType.MouseDrag && Event.current.button == 1; } }
		public static bool isRightMouseClickOrDrag { get { return isRightMouseClick || isRightMouseDrag; } }

		/// <summary>
		/// right mouse button is used (e.g. in GUI.Button)
		/// </summary>
		public static bool isRightMouseButton { get { return Event.current.button == 1; } }

		public static bool isMiddleMouseClick { get { return Event.current.type == EventType.MouseDown && Event.current.button == 1; } }

		#endregion

		#region standard keys

		// ================================================================================
		//  standard keys
		// --------------------------------------------------------------------------------

		public static bool isSpacebar { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space; } }
		public static bool useSpacebar { get { return CheckAndUse(isSpacebar); } }

		#endregion

		#region F keys

		// ================================================================================
		//  F keys
		// --------------------------------------------------------------------------------

		public enum FKey { F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, None }

		public static bool isF1 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F1; } }
		public static bool isF2 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F2; } }
		public static bool isF3 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F3; } }
		public static bool isF4 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F4; } }
		public static bool isF5 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F5; } }
		public static bool isF6 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F6; } }
		public static bool isF7 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F7; } }
		public static bool isF8 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F8; } }
		public static bool isF9 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F9; } }
		public static bool isF10 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F10; } }
		public static bool isF11 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F11; } }
		public static bool isF12 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F12; } }

		public static bool useF1 { get { return CheckAndUse(isF1); } }
		public static bool useF2 { get { return CheckAndUse(isF2); } }
		public static bool useF3 { get { return CheckAndUse(isF3); } }
		public static bool useF4 { get { return CheckAndUse(isF4); } }
		public static bool useF5 { get { return CheckAndUse(isF5); } }
		public static bool useF6 { get { return CheckAndUse(isF6); } }
		public static bool useF7 { get { return CheckAndUse(isF7); } }
		public static bool useF8 { get { return CheckAndUse(isF8); } }
		public static bool useF9 { get { return CheckAndUse(isF9); } }
		public static bool useF10 { get { return CheckAndUse(isF10); } }
		public static bool useF11 { get { return CheckAndUse(isF11); } }
		public static bool useF12 { get { return CheckAndUse(isF12); } }

		public static FKey useFKey
		{
			get
			{
				if (useF1) return FKey.F1;
				if (useF2) return FKey.F2;
				if (useF3) return FKey.F3;
				if (useF4) return FKey.F4;
				if (useF5) return FKey.F5;
				if (useF6) return FKey.F6;
				if (useF7) return FKey.F7;
				if (useF8) return FKey.F8;
				if (useF9) return FKey.F9;
				if (useF10) return FKey.F10;
				if (useF11) return FKey.F11;
				if (useF12) return FKey.F12;

				return FKey.None;
			}
		}

		#endregion

		#region number keys

		public static bool isKey0 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha0; } }
		public static bool isKey1 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha1; } }
		public static bool isKey2 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha2; } }
		public static bool isKey3 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha3; } }
		public static bool isKey4 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha4; } }
		public static bool isKey5 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha5; } }
		public static bool isKey6 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha6; } }
		public static bool isKey7 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha7; } }
		public static bool isKey8 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha8; } }
		public static bool isKey9 { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Alpha9; } }

		public static bool useKey0 { get { return CheckAndUse(isKey0); } }
		public static bool useKey1 { get { return CheckAndUse(isKey1); } }
		public static bool useKey2 { get { return CheckAndUse(isKey2); } }
		public static bool useKey3 { get { return CheckAndUse(isKey3); } }
		public static bool useKey4 { get { return CheckAndUse(isKey4); } }
		public static bool useKey5 { get { return CheckAndUse(isKey5); } }
		public static bool useKey6 { get { return CheckAndUse(isKey6); } }
		public static bool useKey7 { get { return CheckAndUse(isKey7); } }
		public static bool useKey8 { get { return CheckAndUse(isKey8); } }
		public static bool useKey9 { get { return CheckAndUse(isKey9); } }

		#endregion

		#region standard keys

		// ================================================================================
		//  standard keys
		// --------------------------------------------------------------------------------

		public static bool isKeyA { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A; } }
		public static bool isKeyB { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.B; } }
		public static bool isKeyC { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C; } }
		public static bool isKeyD { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D; } }
		public static bool isKeyE { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E; } }
		public static bool isKeyF { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F; } }
		public static bool isKeyG { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G; } }
		public static bool isKeyH { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.H; } }
		public static bool isKeyI { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.I; } }
		public static bool isKeyJ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.J; } }
		public static bool isKeyK { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.K; } }
		public static bool isKeyL { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.L; } }
		public static bool isKeyM { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.M; } }
		public static bool isKeyN { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N; } }
		public static bool isKeyO { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.O; } }
		public static bool isKeyP { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P; } }
		public static bool isKeyQ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q; } }
		public static bool isKeyR { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R; } }
		public static bool isKeyS { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S; } }
		public static bool isKeyT { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.T; } }
		public static bool isKeyU { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.U; } }
		public static bool isKeyV { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.V; } }
		public static bool isKeyW { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.W; } }
		public static bool isKeyX { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X; } }
		public static bool isKeyY { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Y; } }
		public static bool isKeyZ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Z; } }

		public static bool useKeyA { get { return CheckAndUse(isKeyA); } }
		public static bool useKeyB { get { return CheckAndUse(isKeyB); } }
		public static bool useKeyC { get { return CheckAndUse(isKeyC); } }
		public static bool useKeyD { get { return CheckAndUse(isKeyD); } }
		public static bool useKeyE { get { return CheckAndUse(isKeyE); } }
		public static bool useKeyF { get { return CheckAndUse(isKeyF); } }
		public static bool useKeyG { get { return CheckAndUse(isKeyG); } }
		public static bool useKeyH { get { return CheckAndUse(isKeyH); } }
		public static bool useKeyI { get { return CheckAndUse(isKeyI); } }
		public static bool useKeyJ { get { return CheckAndUse(isKeyJ); } }
		public static bool useKeyK { get { return CheckAndUse(isKeyK); } }
		public static bool useKeyL { get { return CheckAndUse(isKeyL); } }
		public static bool useKeyM { get { return CheckAndUse(isKeyM); } }
		public static bool useKeyN { get { return CheckAndUse(isKeyN); } }
		public static bool useKeyO { get { return CheckAndUse(isKeyO); } }
		public static bool useKeyP { get { return CheckAndUse(isKeyP); } }
		public static bool useKeyQ { get { return CheckAndUse(isKeyQ); } }
		public static bool useKeyR { get { return CheckAndUse(isKeyR); } }
		public static bool useKeyS { get { return CheckAndUse(isKeyS); } }
		public static bool useKeyT { get { return CheckAndUse(isKeyT); } }
		public static bool useKeyU { get { return CheckAndUse(isKeyU); } }
		public static bool useKeyV { get { return CheckAndUse(isKeyV); } }
		public static bool useKeyW { get { return CheckAndUse(isKeyW); } }
		public static bool useKeyX { get { return CheckAndUse(isKeyX); } }
		public static bool useKeyY { get { return CheckAndUse(isKeyY); } }
		public static bool useKeyZ { get { return CheckAndUse(isKeyZ); } }

		public static bool isKeyShiftA { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A && Event.current.shift; } }
		public static bool isKeyShiftB { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.B && Event.current.shift; } }
		public static bool isKeyShiftC { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C && Event.current.shift; } }
		public static bool isKeyShiftD { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && Event.current.shift; } }
		public static bool isKeyShiftE { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E && Event.current.shift; } }
		public static bool isKeyShiftF { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F && Event.current.shift; } }
		public static bool isKeyShiftG { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G && Event.current.shift; } }
		public static bool isKeyShiftH { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.H && Event.current.shift; } }
		public static bool isKeyShiftI { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.I && Event.current.shift; } }
		public static bool isKeyShiftJ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.J && Event.current.shift; } }
		public static bool isKeyShiftK { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.K && Event.current.shift; } }
		public static bool isKeyShiftL { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.L && Event.current.shift; } }
		public static bool isKeyShiftM { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.M && Event.current.shift; } }
		public static bool isKeyShiftN { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.N && Event.current.shift; } }
		public static bool isKeyShiftO { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.O && Event.current.shift; } }
		public static bool isKeyShiftP { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.P && Event.current.shift; } }
		public static bool isKeyShiftQ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q && Event.current.shift; } }
		public static bool isKeyShiftR { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R && Event.current.shift; } }
		public static bool isKeyShiftS { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S && Event.current.shift; } }
		public static bool isKeyShiftT { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.T && Event.current.shift; } }
		public static bool isKeyShiftU { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.U && Event.current.shift; } }
		public static bool isKeyShiftV { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.V && Event.current.shift; } }
		public static bool isKeyShiftW { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.W && Event.current.shift; } }
		public static bool isKeyShiftX { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X && Event.current.shift; } }
		public static bool isKeyShiftY { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Y && Event.current.shift; } }
		public static bool isKeyShiftZ { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Z && Event.current.shift; } }

		public static bool useKeyShiftA { get { return CheckAndUse(isKeyShiftA); } }
		public static bool useKeyShiftB { get { return CheckAndUse(isKeyShiftB); } }
		public static bool useKeyShiftC { get { return CheckAndUse(isKeyShiftC); } }
		public static bool useKeyShiftD { get { return CheckAndUse(isKeyShiftD); } }
		public static bool useKeyShiftE { get { return CheckAndUse(isKeyShiftE); } }
		public static bool useKeyShiftF { get { return CheckAndUse(isKeyShiftF); } }
		public static bool useKeyShiftG { get { return CheckAndUse(isKeyShiftG); } }
		public static bool useKeyShiftH { get { return CheckAndUse(isKeyShiftH); } }
		public static bool useKeyShiftI { get { return CheckAndUse(isKeyShiftI); } }
		public static bool useKeyShiftJ { get { return CheckAndUse(isKeyShiftJ); } }
		public static bool useKeyShiftK { get { return CheckAndUse(isKeyShiftK); } }
		public static bool useKeyShiftL { get { return CheckAndUse(isKeyShiftL); } }
		public static bool useKeyShiftM { get { return CheckAndUse(isKeyShiftM); } }
		public static bool useKeyShiftN { get { return CheckAndUse(isKeyShiftN); } }
		public static bool useKeyShiftO { get { return CheckAndUse(isKeyShiftO); } }
		public static bool useKeyShiftP { get { return CheckAndUse(isKeyShiftP); } }
		public static bool useKeyShiftQ { get { return CheckAndUse(isKeyShiftQ); } }
		public static bool useKeyShiftR { get { return CheckAndUse(isKeyShiftR); } }
		public static bool useKeyShiftS { get { return CheckAndUse(isKeyShiftS); } }
		public static bool useKeyShiftT { get { return CheckAndUse(isKeyShiftT); } }
		public static bool useKeyShiftU { get { return CheckAndUse(isKeyShiftU); } }
		public static bool useKeyShiftV { get { return CheckAndUse(isKeyShiftV); } }
		public static bool useKeyShiftW { get { return CheckAndUse(isKeyShiftW); } }
		public static bool useKeyShiftX { get { return CheckAndUse(isKeyShiftX); } }
		public static bool useKeyShiftY { get { return CheckAndUse(isKeyShiftY); } }
		public static bool useKeyShiftZ { get { return CheckAndUse(isKeyShiftZ); } }

		#endregion

		#region arrow keys

		// ================================================================================
		//  arrow keys
		// --------------------------------------------------------------------------------

		public enum ArrowKey
		{
			Up, Right, Down, Left, None
		}

		public static ArrowKey usesArrowKey
		{
			get
			{
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
				{
					Event.current.Use();
					return ArrowKey.Up;
				}

				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
				{
					Event.current.Use();
					return ArrowKey.Right;
				}

				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
				{
					Event.current.Use();
					return ArrowKey.Down;
				}

				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
				{
					Event.current.Use();
					return ArrowKey.Left;
				}

				return ArrowKey.None;
			}
		}

		#endregion

		#region queries

		// ================================================================================
		//  general queries
		// --------------------------------------------------------------------------------

		public static bool isRepaint { get { return Event.current.type == EventType.Repaint; } }
		public static bool isLayout { get { return Event.current.type == EventType.Layout; } }

		public static bool isDeleteKey { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete; } }
		public static bool useDeleteKey { get { return CheckAndUse(isDeleteKey); } }

		public static bool isTab { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab; } }
		public static bool useTab { get { return CheckAndUse(isTab); } }

		public static bool isReturn { get { return Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter); } }
		public static bool useReturn { get { return CheckAndUse(isReturn); } }

		public static bool isEscapeKey { get { return Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape; } }
		public static bool useEscapeKey { get { return CheckAndUse(isEscapeKey); } }

		public static bool alt { get { return Event.current.alt; } }
		public static bool shift { get { return Event.current.shift; } }
		public static bool control { get { return Event.current.control; } }

		#endregion

		#region drag and drop

		public static bool isDragUpdated { get { return Event.current.type == EventType.DragUpdated; } }
		public static bool isDragPerform { get { return Event.current.type == EventType.DragPerform; } }
		public static bool isDragExited { get { return Event.current.type == EventType.DragExited; } }
		public static bool isDragAndDrop { get { return isDragUpdated || isDragPerform || isDragExited; } }

		#endregion

		#region commands

		// ================================================================================
		//  commands
		// --------------------------------------------------------------------------------

		public enum Command
		{
			SelectAll, Cut, Copy, Paste, Duplicate, None
		}

		public static bool IsCommand(this Command command)
		{
			return command != Command.None;
		}

		public static bool isExecuteSelectAll
		{
			get
			{
				return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll";
			}
		}

		public static bool isExecuteCut
		{
			get
			{
				return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Cut";
			}
		}

		public static bool isExecuteCopy
		{
			get
			{
				return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Copy";
			}
		}

		public static bool isExecutePaste
		{
			get
			{
				return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Paste";
			}
		}

		public static bool isExecuteDuplicate
		{
			get
			{
				return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Duplicate";
			}
		}

		public static bool isValidateSelectAll
		{
			get
			{
				return Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll";
			}
		}

		public static bool isValidateCut
		{
			get
			{
				return Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Cut";
			}
		}

		public static bool isValidateCopy
		{
			get
			{
				return Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Copy";
			}
		}

		public static bool isValidatePaste
		{
			get
			{
				return Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Paste";
			}
		}

		public static bool isValidateDuplicate
		{
			get
			{
				return Event.current.type == EventType.ValidateCommand && Event.current.commandName == "Duplicate";
			}
		}

		public static Command useCommand
		{
			get
			{
				ValidateAllCommands();

				if (isExecuteSelectAll)
				{
					Use();
					return Command.SelectAll;
				}

				if (isExecuteCut)
				{
					Use();
					return Command.Cut;
				}

				if (isExecuteCopy)
				{
					Use();
					return Command.Copy;
				}

				if (isExecutePaste)
				{
					Use();
					return Command.Paste;
				}

				if (isExecuteDuplicate)
				{
					Use();
					return Command.Duplicate;
				}

				return Command.None;
			}
		}

		// only react to a specific set of commands, don't touch the others
		public static Command UseCommand(params Command[] commands)
		{
			foreach (var command in commands)
			{
				switch (command)
				{
					case Command.SelectAll:
						if (isValidateSelectAll) Use();
						if (isExecuteSelectAll)
						{
							Use();
							return Command.SelectAll;
						}
						break;
					case Command.Cut:
						if (isValidateCut) Use();
						if (isExecuteCut)
						{
							Use();
							return Command.Cut;
						}
						break;
					case Command.Copy:
						if (isValidateCopy) Use();
						if (isExecuteCopy)
						{
							Use();
							return Command.Copy;
						}
						break;
					case Command.Paste:
						if (isValidatePaste) Use();
						if (isExecutePaste)
						{
							Use();
							return Command.Paste;
						}
						break;
					case Command.Duplicate:
						if (isValidateDuplicate) Use();
						if (isExecuteDuplicate)
						{
							Use();
							return Command.Duplicate;
						}
						break;
				}
			}

			return Command.None;
		}

		private static void ValidateAllCommands()
		{
			if (isValidateCopy || isValidateCut || isValidateDuplicate || isValidatePaste || isValidateSelectAll)
			{
				Use();
			}			
		}

		public static void UseAllCommands()
		{
			ValidateAllCommands();

			if (isExecuteSelectAll || isExecuteCut || isExecuteCopy || isExecutePaste || isExecuteDuplicate)
			{
				Use();
			}
		}

		#endregion
	}
}