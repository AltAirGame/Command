using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameSystems.Core
{
    

    public static class CommandFactory
    {
        private static Dictionary<string, Type> CommandByName;
        private static bool isInitlized = false;

        public static void InitilizeFctory()
        {
            if (isInitlized)
            {
                return;
            }

            var CommandTypes = Assembly.GetAssembly(typeof(ICommand)).GetTypes().Where(comandType =>
                comandType.GetInterfaces().Contains(typeof(ICommand)));
            CommandByName = new Dictionary<string, Type>();
            foreach (var item in CommandTypes)
            {
                var tempCommand = Activator.CreateInstance(item
                ) as ICommand;
                CommandByName.Add(tempCommand.Name,item);
          
            }

            isInitlized = true;
        }

        public static ICommand GetCommand(string commandType)
        {
            InitilizeFctory();
            if (CommandByName.ContainsKey(commandType))
            {
                Type type = CommandByName[commandType];
                var command = Activator.CreateInstance(type) as ICommand;
                return command;
            }

            return null;
        }

        public static List<ICommand> GetAllCommandInFactory()
        {  InitilizeFctory();
            List<ICommand> commands=new List<ICommand>();

            foreach (var item in CommandByName)
            {
                var tempCommand = Activator.CreateInstance(item.Value) as ICommand;
            
                commands.Add(tempCommand);
            }

            return commands;
        }

    }}