using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan
{
    public class TitanDisplayControl : ControlModule
    {
        public TitanDisplayControl(TitanCore core)
            : base(core)
        {
            priority = -1000;
            //runModuleInModes.Add(ICities.AppMode.AssetEditor);
            runModuleInModes.Add(ICities.AppMode.Game);
            //runModuleInModes.Add(ICities.AppMode.MapEditor);
        }

        public override void OnUpdate()
        {
            foreach(DisplayModule module in core.GetControlModules<DisplayModule>())
            {
                if(module.runModuleInModes.Contains(ICities.AppMode.Game))
                {
                    module.enabled = true;
                }
                else
                {
                    module.enabled = false;
                }
            }
        }
    }
}
