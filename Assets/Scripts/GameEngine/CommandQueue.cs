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
        private StoredCommand currentStored;
        public int Count => commands.Count;
        public void Start()
        {
            current = GetComponent<Stop>();
            current.Issue(Target.Null);
        }
        public void Update()
        {
            if ((current != null) && (!current.Issuing))
            {
                if (!current.Completed)
                {
                    Target target=currentStored.Target;
                    Vector3 targetpos;
                    if (target.GetType() == typeof(LocationTarget))
                    {
                        targetpos = (target as LocationTarget).Value;
                        print($"Reissuing (automatic) {transform.name} to \"Move\" to {targetpos}");
                        print($"Reissuing {transform.name} to \"{currentStored.CommandType.Name}\" to {targetpos}");
                    }
                    else
                    {
                        targetpos = (target as UnitTarget).Value.Transform.position;
                        print($"Reissuing (automatic) {transform.name} to \"Move\" to {(target as UnitTarget).Value.Transform.name}'s position");
                        print($"Reissuing {transform.name} to \"{currentStored.CommandType.Name}\" {(target as UnitTarget).Value.Transform.name}");
                    }
                    print($"Reissuing {transform.name} to \"{currentStored.CommandType.Name}\"");
                    StoredCommand automove = AutoMove(transform.position, targetpos, current.Range);
                    commands.Enqueue(automove);
                    commands.Enqueue(currentStored);
                }
                current = null;                
            }
            if ((current == null) && (Count != 0))
            {
                Issue();
            }
            //print(string.Join(", ",commands));
        }
        public void Add(StoredCommand command)
        {
            commands.Enqueue(command);
        }
        public Command Issue()
        {
            if (Count == 0) throw new InvalidOperationException("Sequence contains no elements");
            currentStored = commands.Dequeue();
            current = currentStored.Issue(gameObject);
            print($"{transform.name} is doing \"{current.GetType().Name}\"");
            return current;
        }
        public void Abort()
        {
            if (current != null && current.CanAbort)
            {
                current.Abort();
                current = null;
            }
        }
        public void Clear()
        {
            Queue<StoredCommand> newCommands = new(commands.Where(x => !x.Of(gameObject).CanAbort));
            commands.Clear();
            commands = newCommands;
            Abort();
        }
        static private StoredCommand AutoMove(Vector3 start, Vector3 end, Vector2 range)
        {
            Vector3 movepos;
            start.y = 0;
            end.y = 0;
            if ((end - start).sqrMagnitude < range.x * range.x)
            {
                movepos = Vector3.Normalize(end - start) * -range.x;
            }
            else if ((end - start).sqrMagnitude > range.y * range.y)
            {
                movepos = Vector3.Normalize(end - start) * range.y;
            }
            else
            {
                movepos = Vector3.Normalize(end - start) * float.Epsilon;
            }
            movepos.y = 0;
            return new StoredCommand(typeof(Move), new LocationTarget(start + movepos));
        }
    }
}