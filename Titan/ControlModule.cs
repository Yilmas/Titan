using System;
using System.Collections.Generic;
using UnityEngine;

namespace Titan
{
    public class ControlModule : IComparable<ControlModule>
    {
        public TitanCore core = null;
        public int priority = 0;

        //public List<GameScenes> runModuleInScenes = new List<GameScenes>();
        public List<ICities.AppMode> runModuleInModes = new List<ICities.AppMode>();

        public int CompareTo(ControlModule other)
        {
            if (other == null) return 1;
            return priority.CompareTo(other.priority);
        }

        protected bool _enabled = false;

        public bool enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    if (_enabled)
                        OnControllerEnabled();
                    else
                        OnControllerDisabled();
                }
            }
        }

        public ControlModule(TitanCore core)
        {
            this.core = core;
        }

        public virtual void OnControllerEnabled()
        {
            Log.Message("[Titan] " + this.GetType().Name + " Enabled: " + enabled);
        }

        public virtual void OnControllerDisabled()
        {
            Log.Message("[Titan] " + this.GetType().Name + " Enabled: " + enabled);
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnActive()
        {
        }

        public virtual void OnInactive()
        {
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnSave()
        {
        }

        public virtual void OnDestroy()
        {
        }
    }

    [Flags]
    public enum Pass
    {
        configGlobal = 1,
        configLocal = 4
    }
}
