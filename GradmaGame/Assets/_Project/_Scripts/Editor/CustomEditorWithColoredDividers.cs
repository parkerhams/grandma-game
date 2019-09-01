using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using System.IO;

namespace CustomPrettyEditor {
	[InitializeOnLoad]
	internal static class GUIDrawer 
	{
		
		private static GameObject editorUtilityGameObject;
		private static Rect lastRect;
		
		static GUIDrawer() 
		{
			EditorApplication.hierarchyWindowItemOnGUI += ItemGUI;
			EditorApplication.projectWindowItemOnGUI += ItemProjectWidowItem;
		}

        public static Color ColorForHex(string hex)  // dont forget the #
        {
            if (hex.Length == 7) // add alpha if needed
                hex += "FF";
            Color color = new Color();
            ColorUtility.TryParseHtmlString(hex, out color);
            return color;
        }

        private static void ItemProjectWidowItem(string pGUID, Rect selectionRect)
		{
			string assetpath = AssetDatabase.GUIDToAssetPath(pGUID);
			string name = Path.GetFileName(assetpath);
			if (name == "_Project")
				ColorBg(selectionRect, ColorForHex("#00991A28"));
				
			if (name.Contains("______"))
			{
 #if UNITY_PRO_LICENSE
				if((selectionRect.y / selectionRect.height) % 2 < 1f)
					EditorGUI.DrawRect(selectionRect, LBUtility.ColorForHex("#323232ff"));
				else
					EditorGUI.DrawRect(selectionRect, LBUtility.ColorForHex("#383838FF"));
				//line
				EditorGUI.DrawRect(new Rect(selectionRect.x,selectionRect.y,selectionRect.width,1), LBUtility.ColorForHex("#2C2C2Cff"));
			#else
				ColorBg(selectionRect, ColorForHex("#c2c2c2FF"));
# endif 
			}

            ColorSort(selectionRect);
			
			lastRect = selectionRect;
			lastRect.y--;
		}

		private static void ItemGUI(int instanceID, Rect rect) 
		{
			try {
				
				editorUtilityGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
				
				if(!editorUtilityGameObject)
					return;
				
                if (editorUtilityGameObject.tag == "Scene")
					ColorBg(rect, ColorForHex("#00991A28"));
				else if (editorUtilityGameObject.tag == "Manager")
					ColorBg(rect, ColorForHex("#8D991728"));
				else if (editorUtilityGameObject.tag == "Canvas")
					ColorBg(rect, ColorForHex("#99006228"));
				else if (editorUtilityGameObject.tag == "Visual Break")
				{
					if((rect.y / rect.height) % 2 < 1f)
						ColorBg(rect, ColorForHex("#323232ff"));
					else
						ColorBg(rect, ColorForHex("#383838FF"));
				}
			
				else if (editorUtilityGameObject.tag == "EditorOnly")
					ColorBg(rect, ColorForHex("#38383898"));
				else if (editorUtilityGameObject.name == "UniversePageGOView" || editorUtilityGameObject.name == "UniversePageView" )
					ColorBg(rect, ColorForHex("#D4701633"));							
			}
			catch {
				
			}
		}
		
		private static void ColorBg(Rect rect, Color color, float height = 0) {
			rect.xMin = 0f;
			if (height != 0)
				rect.yMax = rect.yMin + height;
			EditorGUI.DrawRect(rect, color);
		}
		
		private static void ColorSort(Rect rect, string alpha="14") {
			rect.xMin = 0f;
			
			var count = lastRect.y / lastRect.height;
			for(int i = 0; i < count; i++) {
				if((rect.y / rect.height) % 2 < 1f)
					EditorGUI.DrawRect(rect, ColorForHex("#2C2C2C"+alpha));
				rect.y += rect.height;
				
				//line
				EditorGUI.DrawRect(new Rect(rect.x,rect.y,rect.width,1), ColorForHex("#2C2C2Cff"));
				
			}
		}
			
	}
}
