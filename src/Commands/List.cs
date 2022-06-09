using CLIPnP.Helper;
using ConsoleTables;

namespace CLIPnP.Commands
{
    internal class List : ICommand
    {
        public string Name => "list";

        public string Description => "View / View the current port table";

        public string Usage => "list";

        public void Execute(string[] args)
        {
            var portMappings = UPNPManager.PortMappings;

            var table = new ConsoleTable("Name", "Protocol", "Internal Port", "External Port", "State");
            
            foreach (var portMapping in portMappings)
            {
                table.AddRow(portMapping.Name, portMapping.BaseMapping.Protocol.ToString(), portMapping.InternalPort, portMapping.ExternalPort, portMapping.BaseMapping.DefaultActive ? "Active" : "Disabled");
            }

            table.Write(Format.Minimal);
        }
    }
}
