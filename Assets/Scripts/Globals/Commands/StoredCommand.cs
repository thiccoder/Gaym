using System;
using UnityEngine;

namespace Assets.Scripts.Globals.Commands
{
    internal struct StoredCommand
    {
        public readonly Target Target;
        public readonly Type CommandType;
        public Command GetCommand(GameObject obj)
        {
            return (Command)obj.GetComponent(CommandType.Name);
        }
        public StoredCommand(Type commandType, Target target)
        {
            CommandType = commandType;
            Target = target;
        }
        public Command Issue(GameObject obj)
        {
            var commands = obj.GetComponents<Command>();
            foreach (var command in commands)
            {
                if (command.GetType() == CommandType)
                {
                    command.Issue(Target);
                    return command;
                }
            }
            return null;
        }
    }
}
