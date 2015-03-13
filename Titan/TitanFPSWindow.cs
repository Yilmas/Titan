using UnityEngine;

namespace Titan
{
    public class TitanFPSWindow : DisplayModule
    {
        public TitanFPSWindow(TitanCore core)
            : base(core)
        {
            runModuleInModes.Add(ICities.AppMode.Game);
        }

        private float deltaTime = 0.0f;

        public override void OnUpdate()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            base.OnUpdate();
        }

        protected override void WindowGUI(int windowId)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleRight;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label("FPS: ");
            GUILayout.Label(fps.ToString("0.00"), style);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label("MS: ");
            GUILayout.Label(msec.ToString("0.00"), style);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            base.WindowGUI(windowId);
        }

        public override GUILayoutOption[] WindowOptions()
        {
            return new GUILayoutOption[] { GUILayout.Width(130), GUILayout.Height(50) };
        }

        public override string GetName()
        {
            return "FPS Monitor";
        }
    }
}
