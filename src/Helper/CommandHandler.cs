using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLIPnP.Helper
{ 
    internal class CommandHandler
    {
        private readonly List<ICommand> commands = new List<ICommand>(); // From ICommand, janky af.

        public void Start()
        {
            var commandTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface).ToList();

            foreach (var command in commandTypes)
            {
                commands.Add((ICommand) Activator.CreateInstance(command));
            }

            Console.Write("> ");

            while (true)
            {
                var line = Console.ReadLine();  

                if (line == null || line == "exit")
                {
                    Console.WriteLine("Exiting...");
                    UPNPManager.RemoveAllMappings();
                    break;
                } 
                else if (line == "help")
                {
                    Console.WriteLine("Available Commands:\n");

                    var table = new ConsoleTable("Name", "Description", "Usage");

                    table.AddRow("help", "Show all available commands", "help");
                    table.AddRow("exit", "Safely handle port mappings and exit", "exit");

                    foreach (var command in commands)
                    {
                        table.AddRow(command.Name, command.Description, command.Usage);
                    }

                    table.Write(Format.Minimal);
                }
                else
                    HandleLine(line);

                Console.Write("\n> ");
            }
        }

        private void HandleLine(string line)
        {
            string[] splitLine = line.Split(' ');
            string command = splitLine[0];
            string[] args = null;

            if (splitLine.Length > 1)
            {
                args = new string[splitLine.Length - 1];

                for (int i = 1; i < splitLine.Length; i++)
                {
                    args[i - 1] = splitLine[i];
                }
            }

            foreach (var cmd in commands)
            {
                if (string.Equals(cmd.Name, command, StringComparison.OrdinalIgnoreCase))
                {
                    cmd.Execute(args);
                    return;
                }
            }

            Console.WriteLine("Command not found! Use the \"help\" command to see all available commands.");
        }
    }
}
