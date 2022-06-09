using CLIPnP.Helper;
using CLIPnP.NATUtils;
using System;

namespace CLIPnP.Commands
{
    internal class Add : ICommand
    {
        public string Name => "add";
        public string Description => "Register a port mapping to the table";
        public string Usage => "add <name> <protocol> <internal port> <external port>";

        public void Execute(string[] args)
        {
            if (args == null || args.Length < 4)
            {
                Console.WriteLine("Invalid argument count. <name> <protocol> <internal port> <external port> required!");
                return;
            }

            string name = args[0];

            Protocol protocol;
            string stringProtocol = args[1].ToLower();
            switch (stringProtocol)
            {
                case "tcp":
                    protocol = Protocol.TCP;
                    break;

                case "udp":
                    protocol = Protocol.UDP;
                    break;

                case "both":
                    protocol = Protocol.Both;
                    break;

                default:
                    Console.WriteLine("Invalid protocol!");
                    return;
            }

            if (!int.TryParse(args[2], out int internalPort) || internalPort > 65535)
            {
                Console.WriteLine("Invalid internal port!");
                return;
            }

            if (!int.TryParse(args[3], out int externalPort) || externalPort > 65535)
            {
                Console.WriteLine("Invalid external port!");
                return;
            }


            foreach (PortMapping portMapping in UPNPManager.PortMappings)
            {
                if (portMapping.Name == name)
                {
                    Console.WriteLine("Invalid name! Another mapping with the same name already exists!");
                    return;
                }

                if (portMapping.ExternalPort == externalPort && portMapping.InternalPort == internalPort)
                {
                    Console.WriteLine("Invalid port pair! This mapping pair already exists!");
                    return;
                }
            }

            UPNPManager.PortMappings.Add(new PortMapping(new BaseMapping { 
                Name = name,
                InternalPort = internalPort,
                ExternalPort = externalPort,
                Protocol = protocol,
                DefaultActive = false 
            }, UPNPManager.Device));

            Console.WriteLine("Successfully added a port mapping with the following properties:\n" +
                $"Name: {name}\nProtocol: {protocol}\nInternal Port: {internalPort}\nExternal Port: {externalPort}\nCurrent State: Disabled");
        }
    }
}
