using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Globals.Commands;
using System;
using System.Linq.Expressions;
using System.Collections;

namespace Assets.Scripts.GameEngine
{
    internal class CommandQueue : MonoBehaviour
    {
        private Queue<StoredCommand> commands = new();
        private Command current;
        public int Count => commands.Count;
        public void Start()
        {
            current = GetComponent<Stop>();
            current.Issue(Target.Null);
        }
        public void Update()
        {
            if ((current is not null) && (!current.Issuing))
            {
                current = null;
            }
            if ((current is null) && (Count != 0))
            {
                Issue();
            }
        }
        public void Add(StoredCommand command)
        {
            commands.Enqueue(command);
        }
        public Command Issue()
        {
            if (Count == 0) throw new InvalidOperationException("Sequence contains no elements");
            current = commands.Dequeue().Issue(gameObject);
            return current;
        }
        public Command IssueImmediate(StoredCommand command)
        {
            Clear();
            Add(command);
            return Issue();
        }
        public void Abort()
        {
            if (current is not null && current.CanAbort)
            {
                current.Abort();
                current = null;
            }
        }
        public void Clear()
        {
            Queue<StoredCommand> newCommands = new(commands.Where(x => !x.GetCommand(gameObject).CanAbort));
            commands.Clear();
            commands = newCommands;
            Abort();
        }
    }
}