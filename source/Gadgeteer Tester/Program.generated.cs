﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Gadgeteer Designer.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Gadgeteer.Tester
{
    public partial class Program : Gadgeteer.Program
    {
        // GTM.Module defintions
		Gadgeteer.Modules.GHIElectronics.XBee xbee;

		public static void Main()
        {
			//Important to initialize the Mainboard first
            Mainboard = new GHIElectronics.Gadgeteer.FEZHydra();			

            Program program = new Program();
			program.InitializeModules();
            program.ProgramStarted();
            program.Run(); // Starts Dispatcher
        }

        private void InitializeModules()
        {   
			// Initialize GTM.Modules and event handlers here.		
			xbee = new GTM.GHIElectronics.XBee(7);

        }
    }
}
