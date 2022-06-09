using CLIPnP.Helper;
using CLIPnP.NATUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CLIPnP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to CLIUPNP\n");

            Console.CancelKeyPress += HandleSIGINT;

            var foundDevice = false;
            _ = new SpinnerTask("Looking for a valid NAT Device", () =>
              {
                  foundDevice = UPNPManager.FindNATDevice();
              });

            if (foundDevice)
            {
                Console.WriteLine("Found a valid NAT device!\n");

                LoadSavedJson();

                Console.WriteLine("Now ready for UPNP Mapping!\n");

                new CommandHandler().Start();
            }
            else
            {
                Console.WriteLine("Error!, could not find a valid NAT device. Your router may not support UPNP or it may not be enabled.");
            }
        }

        public static void HandleSIGINT(object sender, EventArgs args)
        {
            UPNPManager.RemoveAllMappings();
            Environment.Exit(0);
        }

        private static void LoadSavedJson()
        {
            if (File.Exists("portmappings.json"))
            {
                Console.WriteLine("Found a port mappings save file...");

                _ = new SpinnerTask("Now loading (and enabling) port mappings", () =>
                  {
                      string textJson = File.ReadAllText("portmappings.json");
                      List<BaseMapping> baseMappings = JsonConvert.DeserializeObject<List<BaseMapping>>(textJson);

                      foreach (var baseMapping in baseMappings)
                      {
                          var portMapping = new PortMapping(baseMapping, UPNPManager.Device);
                          UPNPManager.PortMappings.Add(portMapping);

                          if (baseMapping.DefaultActive)
                              portMapping.Enable();
                      }
                  });

                Console.WriteLine("Successfully loaded all saved port mappings!\n");
            }
        }
    }
}
