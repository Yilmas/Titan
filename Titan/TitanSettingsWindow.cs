using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Titan
{
    public class TitanSettingsWindow : DisplayModule
    {
        public TitanSettingsWindow(TitanCore core)
            : base(core)
        {
            priority = 1000;
            runModuleInModes.Add(ICities.AppMode.AssetEditor);
            runModuleInModes.Add(ICities.AppMode.Game);
            runModuleInModes.Add(ICities.AppMode.MapEditor);

            skinType = GUITitan.SkinType.Titan;
        }

        new public bool windowIsHidden = true;

        internal GUITitan.SkinType skinType;
        internal bool resetAborted = false;

        protected override void WindowGUI(int windowId)
        {
            GUILayout.BeginVertical();

            GUITitan.Title("Skins");
            GUITitan.Label("Current skin: ", skinType.ToString());

            if (GUITitan.skin == null || skinType != GUITitan.SkinType.Titan)
            {
                if (GUILayout.Button("Use Dune GUI skin"))
                {
                    GUITitan.LoadSkin(GUITitan.SkinType.Titan);
                    skinType = GUITitan.SkinType.Titan;
                }
            }

            if (GUITitan.skin == null || skinType != GUITitan.SkinType.Default)
            {
                if (GUILayout.Button("Use default GUI skin"))
                {
                    GUITitan.LoadSkin(GUITitan.SkinType.Default);
                    skinType = GUITitan.SkinType.Default;
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Reset all settings to default: ", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Reset"))
            {
                core.GetControlModule<SettingsDialog>().enabled = true;
                core.GetControlModule<SettingsDialog>().windowIsHidden = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            base.WindowGUI(windowId);
        }

        public override string GetName()
        {
            return "Settings Window";
        }
    }

    internal class SettingsDialog : DisplayModule
    {
        public SettingsDialog(TitanCore core)
            : base(core)
        {
            runModuleInModes.Add(ICities.AppMode.AssetEditor);
            runModuleInModes.Add(ICities.AppMode.Game);
            runModuleInModes.Add(ICities.AppMode.MapEditor);

            hideInToolbar = true;
            windowVector = new Vector4(Screen.width / 2 - 100, Screen.height / 2 - 30, 0, 0);
        }

        protected override void WindowGUI(int windowId)
        {
            GUILayout.BeginVertical();

            GUITitan.Title("Are you sure you want to reset to default settings?");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Do not reset!", GUILayout.ExpandWidth(true)))
            {
                core.GetControlModule<TitanSettingsWindow>().resetAborted = true;
                windowIsHidden = true;
                enabled = false;
            }
            GUILayout.Space(20f);
            if (GUILayout.Button("Reset!", GUILayout.ExpandWidth(true)))
            {
                windowIsHidden = true;
                enabled = false;

                // TODO: Fix messages and saving of chosen skin type

                //KSP.IO.FileInfo.CreateForType<DuneCore>("dune_settings_global.cfg").Delete();

                //if (!FlightGlobals.ActiveVessel.IsNull())
                //{
                //    KSP.IO.FileInfo.CreateForType<DuneCore>("dune_settings_" + FlightGlobals.ActiveVessel.vesselName + ".cfg");
                //}

                //ScreenMessages.PostScreenMessage("Resetting settings and reloading modules!", 5.0f, ScreenMessageStyle.UPPER_CENTER);

                core.ReloadAllControlModules();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            if (GUI.Button(new Rect(windowPosition.width - 18, 2, 16, 16), ""))
            {
                core.GetControlModule<TitanSettingsWindow>().resetAborted = true;
                windowIsHidden = true;
                enabled = false;
            }
            GUI.DragWindow();
        }

        public override GUILayoutOption[] WindowOptions()
        {
            return new GUILayoutOption[] { GUILayout.Width(200), GUILayout.Height(60) };
        }

        public override string GetName()
        {
            return "Confirm Reset!";
        }
    }
}
