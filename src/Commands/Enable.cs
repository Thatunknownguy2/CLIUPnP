using CLIPnP.Helper;
using System;

namespace CLIPnP.Commands
{
    internal class Enable : ICommand
    {
        public string Name => "enable";

        public string Description => "Enable a port mapping";

        public string Usage => "enable <name of port mapping>";

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
                    portMapping.Enable();
                    Console.WriteLine($"Successfully enabled {portMapping.Name}");
                    return;
                }
            }

            Console.WriteLine("Port mapping not found!");
        }
    }
}
