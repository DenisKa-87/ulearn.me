using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private List<NewClone> clones = new List<NewClone>() { new NewClone() };
        public string Execute(string query)
        {
            var queryArgs = query.Split(' ');
            int cloneToWorkWith = Int32.Parse(queryArgs[1]) - 1;
            string programToWorkWith;
            var command = queryArgs[0].ToLower();
            
            if (queryArgs.Length == 3 && command.Equals("learn"))
            {
                programToWorkWith = queryArgs[2];
                clones[cloneToWorkWith].ProgramsLearned.Push(programToWorkWith);
            }
            else if (queryArgs.Count() == 2)
            {
                if (command.Equals("clone"))
                {
                    clones.Add(clones[cloneToWorkWith].CopyClone());
                }
                else if (command.Equals("rollback"))
                {
                    if (!clones[cloneToWorkWith].ProgramsLearned.Head.NameOfCommand.Equals("basic"))
                        clones[cloneToWorkWith].ProgramsUnLearned.Push(clones[cloneToWorkWith].ProgramsLearned.Pop());
                }
                else if (command.Equals("relearn"))
                {
                    if (clones[cloneToWorkWith].ProgramsUnLearned.Head != null)
                    {
                        clones[cloneToWorkWith].ProgramsLearned.Push(clones[cloneToWorkWith].ProgramsUnLearned.Pop());
                    }
                }
                else if (command.Equals("check"))
                {
                    return clones[cloneToWorkWith].ProgramsLearned.Head.NameOfCommand;
                }
            }
            return null;
        }
    }

    public class NewClone
    {
        private CommandStack programsLearned;
        private CommandStack programsUnLearned;
        public NewClone()
        {
            this.programsLearned = new CommandStack();
            this.ProgramsLearned.Push("basic");
            this.programsUnLearned = new CommandStack();
        }

        public CommandStack ProgramsLearned { get => programsLearned; set => programsLearned = value; }
        public CommandStack ProgramsUnLearned { get => programsUnLearned; set => programsUnLearned = value; }

        public NewClone CopyClone()
        {
            return new NewClone()
            {
                programsLearned = this.programsLearned.Clone(),
                programsUnLearned = this.programsUnLearned.Clone()
            };
        }
    }

    public class CommandStack
    {
        public class Command
        {
            private string nameOfCommand;
            private Command? next;

            public Command(string commmand)
            {
                nameOfCommand = commmand;
                next = null;
            }

            public string NameOfCommand { get => nameOfCommand; set => nameOfCommand = value; }
            public Command? Next { get => next; set => next = value; }
        }

        private Command? head;
        private int count;

        public CommandStack()
        {
            this.head = null;
            this.count = 0;
        }

        public Command? Head { get => head; set => head = value; }
        public int Count { get => count; set => count = value; }

        public void Push(string commandName)
        {
            Command command = new Command(commandName);
            if (head != null)
            {
                command.Next = head;
                head = command;
                count++;
                return;
            }
            else if (head == null)
            {
                head = command;
            }
        }

        public string Pop()
        {
            if (this.head != null && !this.Head.NameOfCommand.Equals("basic"))
            {
                var tmp = this.head.NameOfCommand;
                head = this.head.Next;
                this.count--;
                return tmp;
            }
            return null;
        }

        public CommandStack Clone()
        {
            CommandStack clonedStack = new CommandStack();
            clonedStack.head = this.head;
            clonedStack.count = this.count;
            return clonedStack;
        }
    }
}
