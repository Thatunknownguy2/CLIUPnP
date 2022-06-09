using CLIPnP.Helper;
using CLIPnP.NATUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CLIPnP.Commands
{
    internal class Save : ICommand
    {
        public string Name => "save";

        public string Description => "Save port mappings to a file.";

        public string Usage => "save";

        public void Execute(string[] args)
        {
            if (UPNPManager.PortMappings.Count == 0)
            {
                Console.WriteLine("Can not create a save file when no port mappings have been created!");
                return;
            }

            _ = new SpinnerTask("Saving port mappings", () =>
            {
                List<BaseMapping> baseMappings = new List<BaseMapping>();
                UPNPManager.PortMappings.ForEach(portMapping => baseMappings.Add(portMapping.BaseMapping));

                string textJson = JsonConvert.SerializeObject(baseMappings, Formatting.Indented);
                File.WriteAllText("portmappings.json", textJson);
            });

            Console.WriteLine("Successfully saved all port mappings!");
        }
    }
}
