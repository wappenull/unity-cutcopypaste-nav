using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CopyCutPaste
{
	public static class RectExtensions
	{
		public static Vector2 BottomLeft(this Rect rect)
		{
			return new Vector2(rect.xMin, rect.yMin);
		}

		public static Vector2 BottomRight(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.yMin);
		}

		public static Vector2 TopLeft(this Rect rect)
		{
			return new Vector2(rect.xMin, rect.yMax);
		}

		public static Vector2 TopRight(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.yMax);
		}

		public static Vector2 TopCenter(this Rect rect)
		{
			return new Vector2(rect.center.x, rect.yMax);
		}

		public static Vector2 BottomCenter(this Rect rect)
		{
			return new Vector2(rect.center.x, rect.yMin);
		}

		public static Vector2 MiddleRight(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.center.y);
		}

		public static Vector2 MiddleLeft(this Rect rect)
		{
			return new Vector2(rect.xMin, rect.center.y);
		}		
	}
}