using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ICities;

namespace Titan
{
    public class TitanTransportWindow : DisplayModule
    {
        public TitanTransportWindow(TitanCore core)
            : base(core)
        {
            runModuleInModes.Add(AppMode.Game);
        }

        public override string GetName()
        {
            return "Transport Window";
        } 
    }
}
