using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }
        public Dictionary<char, Action<IVirtualMachine>> CommandList = new Dictionary<char, Action<IVirtualMachine>>();
        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            if (!CommandList.ContainsKey(symbol))
                CommandList.Add(symbol, execute);
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                if (CommandList.ContainsKey(Instructions[InstructionPointer]))
                {
                    CommandList[Instructions[InstructionPointer]](this);
                }
                InstructionPointer++;
            }
        }
    }
}