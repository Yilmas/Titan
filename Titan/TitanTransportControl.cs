using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ICities;

namespace Titan
{
    public class TitanTransportControl : ControlModule
    {
        public TitanTransportControl(TitanCore core)
            : base(core)
        {
            runModuleInModes.Add(AppMode.Game);
        }
    }
}
