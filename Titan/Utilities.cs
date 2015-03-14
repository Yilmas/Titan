using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;

namespace Titan
{
    internal static class Utilities
    {
        public static float TryParse(string s, float defaultValue)
        {
            float value;
            if (!float.TryParse(s, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        public static bool TryParse(string s, bool defaultValue)
        {
            bool value;
            if (!bool.TryParse(s, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        public static bool IsNull<T>(this T @object)
        {
            return Equals(@object, null);
        }

        public static AppMode currentMode
        {
            get
            {
                switch(ColossalFramework.Singleton<ToolManager>.instance.m_properties.m_mode)
                {
                    case ItemClass.Availability.Game:
                        return AppMode.Game;
                    case ItemClass.Availability.MapEditor:
                        return AppMode.MapEditor;
                    case ItemClass.Availability.AssetEditor:
                        return AppMode.AssetEditor;
                    default:
                        return AppMode.Game;
                }
            }
        }
    }
}
