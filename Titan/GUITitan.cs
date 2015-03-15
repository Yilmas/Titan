using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Titan
{
    public class GUILoader : MonoBehaviour
    {
        private void OnGUI()
        {
            GUITitan.SetupTitanSkin();
            if (GUITitan.skin.IsNull()) GUITitan.skin = GUITitan.titanSkin;
            GameObject.Destroy(gameObject);
        }
    }

    public static class GUITitan
    {
        static GUITitan()
        {
            SetProperties();
        }

        public enum SkinType { Default, Titan };
        public static GUISkin skin;
        public static GUISkin defaultSkin;
        public static GUISkin titanSkin;

        private static void CopyDefaultSkin()
        {
            GUI.skin = null;
            defaultSkin = (GUISkin)GameObject.Instantiate(GUI.skin);
        }

        public static void LoadSkin(SkinType skinType)
        {
            switch(skinType)
            {
                case SkinType.Default:
                    if (defaultSkin == null) CopyDefaultSkin();
                    skin = defaultSkin;
                    break;
                case SkinType.Titan:
                    if (titanSkin == null) SetupTitanSkin();
                    skin = titanSkin;
                    break;
            }
        }

        public static void SetupTitanSkin()
        {
            GUI.skin = null;
            titanSkin = (GUISkin)GameObject.Instantiate(GUI.skin);

            GUITitan.skin.name = "Titan";

            // TODO: Setup new skin styles.
        }

        public static bool MouseIsOverWindow(TitanCore core)
        {
            foreach(DisplayModule module in core.GetControlModules<DisplayModule>())
            {
                if(module.enabled && module.runModuleInModes.Contains(Utilities.currentMode) && 
                    module.windowPosition.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
                {
                    return true;
                }
            }
            return false;
        }

        private static void SetProperties()
        {
            // TexturePaths
            IconTexturePath = "";
            ImageTexturePath = "";

            // Images
            //TitanLogo = GameDatabase.Instance.GetTexture(ImageTexturePath + "pageMainWide", false);
        }

        // Textures...

        #region Texture Paths
        public static string IconTexturePath { get; private set; }
        public static string ImageTexturePath { get; private set; }
        #endregion

        #region Textures
        public static Texture2D TitanLogo { get; private set; }
        #endregion

        // Styles...

        #region Text Styles

        public static GUIStyle SetTitleAlignCenter()
        {
            return new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        }

        public static GUIStyle SetWarningStyle()
        {
            return new GUIStyle(GUI.skin.box) { fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.MiddleCenter, fontSize = 18, margin = new RectOffset(10, 10, 5, 20) };
        }

        public static GUIStyle SetFooterAlignCenter()
        {
            return new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic, alignment = TextAnchor.MiddleCenter, fontSize = 10 };
        }

        public static GUIStyle SetAlignCenter()
        {
            return new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        }

        public static GUIStyle SetAlignRight()
        {
            return new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
        }

        #endregion

        #region List Styles
        public static GUIStyle listStyle()
        {
            GUIStyle listStyle = new GUIStyle();
            listStyle.normal.textColor = Color.white;
            listStyle.onHover.background = listStyle.hover.background = new Texture2D(2, 2);
            listStyle.padding = new RectOffset(4, 4, 4, 4);

            return listStyle;
        }
        #endregion

        // Controls...

        #region Image Controls
        public static void Image(Texture2D img)
        {
            GUILayout.Box(img, new GUIStyle(GUI.skin.box) { imagePosition = ImagePosition.ImageOnly });
        }
        #endregion

        #region Label Controls
        public static void Warning(string label)
        {
            GUILayout.BeginHorizontal();

            Color prevColor = GUI.color;
            GUI.color = Color.red;
            //GUILayout.Label(label, SetWarningStyle(), GUILayout.ExpandWidth(true));
            GUILayout.Box(label, SetWarningStyle());
            GUI.color = prevColor;

            GUILayout.EndHorizontal();
        }

        public static void Title(string label)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, SetTitleAlignCenter());

            GUILayout.EndHorizontal();
        }

        public static void Footer(string label)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, SetFooterAlignCenter());

            GUILayout.EndHorizontal();
        }

        public static void TextBox(string leftLabel, string textBox, string rightLabel = "", float width = 100)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(leftLabel, GUILayout.ExpandWidth(true));
            textBox = GUILayout.TextField(textBox, GUILayout.ExpandWidth(true), GUILayout.Width(width));
            GUILayout.Label(rightLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        public static void Label(string leftLabel, int rightValue)
        {
            Label(leftLabel, rightValue.ToString());
        }

        public static void Label(string leftLabel, string rightLabel = "")
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(leftLabel, GUILayout.ExpandWidth(true));
            GUILayout.Label(rightLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }
        #endregion

        #region ComboBox
        //from http://wiki.unity3d.com/index.php?title=PopupList
        public static bool List(Rect position, ref bool showList, ref int selectedItemIndex, string buttonContent,
            GUIContent[] listContent)
        {
            return List(position, ref showList, ref selectedItemIndex, buttonContent, listContent, "button", "box", "list");
        }

        public static bool List(Rect position, ref bool showList, ref int selectedItemIndex, string buttonContent,
            GUIContent[] listContent, GUIStyle listStyle)
        {
            return List(position, ref showList, ref selectedItemIndex, buttonContent, listContent, "button", "box", listStyle);
        }

        public static bool List(Rect position, ref bool showList, ref int selectedItemIndex, string buttonContent,
            GUIContent[] listContent, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle)
        {
            int controlID = GUIUtility.GetControlID(351331, FocusType.Passive);
            bool done = false;

            // Double check selection.
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.mouseDown:
                    if (position.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        showList = true;
                    }
                    break;
                case EventType.mouseUp:
                    if (showList)
                    {
                        done = true;
                    }
                    break;
            }

            // Populate GUI.Box and set selected listEntry
            GUI.Label(position, buttonContent, buttonStyle);
            if (showList)
            {
                Rect listRect = new Rect(position.x, position.y, position.width, listContent.Length * 20);
                GUI.Box(listRect, "", boxStyle);
                selectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
            }
            if (done)
            {
                showList = false;
            }

            return done;
        }
        #endregion

        #region Arrow Selector
        public static int ArrowSelector(int index, int modulo, string label)
        {
            Action drawLabel = () => GUILayout.Label(label, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, stretchWidth = true });
            return ArrowSelector(index, modulo, drawLabel);
        }

        public static int ArrowSelector(int index, int numIndices, Action centerGuiAction)
        {
            if (numIndices == 0) return index;

            GUILayout.BeginHorizontal();

            if (numIndices > 1 && GUILayout.Button("◄", GUILayout.ExpandWidth(false))) index = (index - 1 + numIndices) % numIndices;
            centerGuiAction();
            if (numIndices > 1 && GUILayout.Button("►", GUILayout.ExpandWidth(false))) index = (index + 1) % numIndices;

            GUILayout.EndHorizontal();

            return index;
        }
        #endregion
    }
}
