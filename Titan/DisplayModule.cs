using System;
using System.Linq;
using UnityEngine;

namespace Titan
{
    public class DisplayModule : ControlModule
    {
        public bool windowIsHidden = true;
        public Vector4 windowVector = new Vector4(10, 40, 0, 0);
        public bool hideInToolbar = false;

        public int Id;
        public static int nextId = 6451535;

        public Rect windowPosition
        {
            get
            {
                return new Rect(windowVector.x, windowVector.y, windowVector.z, windowVector.x);
            }
            set
            {
                windowVector = new Vector4
                    (
                        Math.Min(Math.Max(value.x, 0), Screen.width - value.width),
                        Math.Min(Math.Max(value.y, 0), Screen.height - value.height),
                        value.width, value.height
                    );
                windowVector.x = Mathf.Clamp(windowVector.x, 10 - value.width, Screen.width - 10);
                windowVector.y = Mathf.Clamp(windowVector.y, 10 - value.height, Screen.height - 10);
            }
        }

        public DisplayModule(TitanCore core)
            : base(core)
        {
            Id = nextId;
            nextId++;
        }

        public virtual GUILayoutOption[] WindowOptions()
        {
            return new GUILayoutOption[] { GUILayout.Width(250), GUILayout.Height(50) };
        }

        protected virtual void WindowGUI(int windowId)
        {
            if (GUI.Button(new Rect(windowPosition.width - 18, 2, 16, 16), ""))
            {
                windowIsHidden = true;
            }
            GUI.DragWindow();
        }

        public virtual void DrawGUI()
        {
            if (runModuleInModes.Contains(Utilities.currentMode) && !windowIsHidden)
            {
                windowPosition = GUILayout.Window(Id, windowPosition, WindowGUI, GetName(), WindowOptions());
            }
        }

        public virtual string GetName()
        {
            return "Display Module";
        }
    }
}
