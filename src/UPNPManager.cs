using CLIPnP.NATUtils;
using Mono.Nat;
using System.Collections.Generic;
using System.Threading;

namespace CLIPnP 
{
    internal class UPNPManager
    {
        public static List<PortMapping> PortMappings { get => portMappings; }
        public static INatDevice Device { get => device; }

        private static INatDevice device = null;
        private static readonly List<PortMapping> portMappings = new List<PortMapping>();

        private static void FoundNatDevice(object sender, DeviceEventArgs e)
        {
            device = e.Device;
            NatUtility.StopDiscovery();
            NatUtility.DeviceFound -= FoundNatDevice;
        }

        public static bool FindNATDevice()
        {
            NatUtility.DeviceFound += FoundNatDevice;
            NatUtility.StartDiscovery();

            var ellapsed = 0;
            var timeout = 5000;

            while (device == null)
            {
                Thread.Sleep(250);
                ellapsed += 250;

                if (ellapsed >= timeout)
                {
                    return false;
                }
            }

            return true;
        }

        public static void RemoveAllMappings()
        {
            if (device != null && portMappings.Count != 0)
            { 
                foreach (var portMapping in portMappings)
                {
                    portMapping.Disable();
                }

                portMappings.Clear();
            }
        }
    }
}
