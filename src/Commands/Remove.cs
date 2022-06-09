using CLIPnP.Helper;
using System;

namespace CLIPnP.Commands
{
    internal class Remove : ICommand
    {
        public string Name => "remove";

        public string Description => "Remove (and disable) a port mapping";

        public string Usage => "remove <name of port mapping>";

        public void Execute(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Please include the name of the port mapping!");
                return;
            }

            var portMappings = UPNPManager.PortMappings;

            foreach (var portMapping in portMappings)
            {
                if (portMapping.Name == args[0])
                {
                    portMapping.Disable();
                    UPNPManager.PortMappings.Remove(portMapping);

                    Console.WriteLine($"Removed {portMapping.Name}");
                    return;
                }
            }

            Console.WriteLine("Port mapping not found!");
        }
    }
}
