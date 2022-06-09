using CLIPnP.Helper;
using System;


namespace CLIPnP.Commands
{
    internal class Disable : ICommand
    {
        public string Name => "disable";

        public string Description => "Disable a port mapping";

        public string Usage => "disable <name of port mapping>";

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
                    Console.WriteLine($"Disabled {portMapping.Name}");
                    return;
                }
            }

            Console.WriteLine("Port mapping not found!");
        }
    }
}
