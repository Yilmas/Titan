using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Titan
{
    public class TitanCore : MonoBehaviour
    {
        // Reference point for all ControlModules.
        internal TitanDisplayControl displayControl;

        // Internal list of ControlModules.
        private List<ControlModule> controlModules = new List<ControlModule>();
        private List<ControlModule> controlModulesToLoad = new List<ControlModule>();
        private bool controlModulesUpdated = false;
        private static List<Type> moduleRegistry;

        // ConfigNode Global and Local
        // TODO: Figure out how to save/load data.
        
        internal bool showGUI = true;

        // 3rd Party plugin check.
        // internal bool modnameExists = Utilities.TryParse(AssemblyLoader.loadedAssemblies.Any(a => a.Name == "modname").ToString(), false);
        
        public void Start()
        {
            Log.Message("[Titan] Core Start()");

            if(controlModules.Count == 0)
            {
                OnLoad(null);
            }

            foreach(ControlModule module in controlModules)
            {
                try
                {
                    module.OnStart();
                }
                catch (Exception ex)
                {
                    Log.Error("[Titan] Core Start() Module: " + module.GetType().Name + " Exception: " + ex);
                }
            }
                
        }

        public void Awake()
        {
            DontDestroyOnLoad(this);

            foreach(ControlModule module in controlModules)
            {
                try
                {
                    module.OnAwake();
                }
                catch (Exception ex)
                {
                    Log.Error("[Titan] Core OnAwake() Module: " + module.GetType().Name + " Exception: " + ex);
                }
            }
        }

        public void FixedUpdate()
        {
            LoadDelayedModules();

            foreach (ControlModule module in controlModules)
            {
                try
                {
                    if (module.enabled) module.OnFixedUpdate();
                }
                catch (Exception ex)
                {
                    Log.Error("[Titan] Core FixedUpdate() Module: " + module.GetType().Name + " Exception: " + ex);
                }
            }
        }

        public void Update()
        {
            if(controlModulesUpdated)
            {
                controlModules.Sort();
                controlModulesUpdated = false;
            }

            foreach(ControlModule module in controlModules)
            {
                try
                {
                    if(!(module is DisplayModule))
                    {
                        if (module.runModuleInModes.Contains(Utilities.currentMode))
                        {
                            module.enabled = true;
                        }
                        else
                        {
                            module.enabled = false;
                        }

                        module.enabled = true;

                        if (module.enabled) module.OnUpdate();
                    }
                }
                catch(Exception ex)
                {
                    Log.Error("[Titan] Core OnUpdate() Module: " + module.GetType().Name + " Exception: " + ex);
                }
            }
        }

        private void LoadControlModules()
        {
            if(moduleRegistry == null)
            {
                moduleRegistry = new List<Type>();
                foreach(var ass in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (var module in ass.GetTypes().Where(p => p.IsSubclassOf(typeof(ControlModule))).ToList())
                        {
                            Log.Warning("[Titan] Core assembly loaded: " + module.FullName);
                            moduleRegistry.Add(module);
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.Error("[Titan] Core LoadControlModules() moduleRegistry loading: " + ass.FullName + " Exception: " + ex);
                    }
                }
            }

            try
            {
                foreach(Type t in moduleRegistry)
                {
                    if(t != typeof(ControlModule) && (t != typeof(DisplayModule)) && (GetControlModule(t.Name) == null))
                        AddControlModule((ControlModule)(t.GetConstructor(new Type[] {typeof(TitanCore)}).Invoke(new object[] { this } )));
                }
            }
            catch (Exception ex)
            {
                Log.Error("[Titan] Core LoadControlModules() moduleRegistry: " + ex);
            }

            displayControl = GetControlModule<TitanDisplayControl>();
        }

        public void LoadDelayedModules()
        {
            if(controlModulesToLoad.Count > 0)
            {
                controlModules.AddRange(controlModulesToLoad);
                controlModulesUpdated = true;
                controlModulesToLoad.Clear();
            }
        }

        public void OnLoad(object node)
        {
            Log.Message("[Titan] Core OnLoad()");

            if(GUITitan.skin == null)
            {
                GameObject guiLoader = new GameObject("GUILoader", typeof(GUILoader));
            }

            try
            {
                LoadControlModules();
                // TODO: Insert load logic here

                foreach(ControlModule module in controlModules)
                {
                    try
                    {
                        module.OnLoad();
                    }
                    catch(Exception ex)
                    {
                        Log.Error("[Titan] module " + module.GetType().Name + " threw an exception in OnLoad: " + ex);
                    }
                }

                LoadDelayedModules();
            }
            catch(Exception ex)
            {
                Log.Error("[Titan] Core OnLoad(): " + ex);
            }
        }

        public void OnSave(object node)
        {
            Log.Message("[Titan] Core OnSave()");

            try
            {
                LoadDelayedModules();

                // TODO: Insert save logic here

                foreach(ControlModule module in controlModules)
                {
                    try
                    {
                        string name = module.GetType().Name;
                        module.OnSave();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("[Titan] Core OnSave() module " + module.GetType().Name + " Exception: " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[Titan] Core OnSave() Exception: " + ex);
            }
        }

        private void OnGUI()
        {
            if (!showGUI) return;

            GUITitan.LoadSkin(GetControlModule<TitanSettingsWindow>().skinType);

            GUI.skin = GUITitan.skin;

            foreach(DisplayModule module in GetControlModules<DisplayModule>())
            {
                try
                {
                    if (module.enabled) module.DrawGUI();
                }
                catch (Exception ex)
                {
                    Log.Error("[Titan] Core OnGUI() module " + module.GetName() + " Exception: " + ex);
                }
            }
        }

        public void OnDestroy()
        {
            Log.Message("[Titan] Core Destroy()");

            OnSave(null);

            foreach(ControlModule module in controlModules)
            {
                try
                {
                    module.OnDestroy();
                }
                catch(Exception ex)
                {
                    Log.Error("[Titan] Core OnDestroy() module " + module.GetType().Name + " Exception: " + ex);
                }
            }
        }

        public T GetControlModule<T>() where T : ControlModule
        {
            return (T)controlModules.FirstOrDefault(p => p is T); // Null if no matches
        }

        public List<T> GetControlModules<T>() where T : ControlModule
        {
            return controlModules.FindAll(p => p is T).Cast<T>().ToList();
        }

        public ControlModule GetControlModule(string type)
        {
            return controlModules.FirstOrDefault(p => p.GetType().Name.ToLowerInvariant() == type.ToLowerInvariant()); // Null if no matches
        }

        public void AddControlModule(ControlModule control)
        {
            controlModules.Add(control);
            controlModulesUpdated = true;
        }

        public void AddControlModuleListener(ControlModule module)
        {
            controlModulesToLoad.Add(module);
        }

        public void RemoveControlModule(ControlModule module)
        {
            controlModules.Remove(module);
            controlModulesUpdated = true;
        }

        public void ReloadAllControlModules()
        {
            foreach (ControlModule module in controlModules) module.OnDestroy();
            controlModules.Clear();

            OnLoad(null);
            Start();
        }

        public int priority = 0;
    }
}
