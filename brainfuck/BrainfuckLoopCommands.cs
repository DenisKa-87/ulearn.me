using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        static Dictionary<int, int> loops = new Dictionary<int, int>(); // loop number / start - end
        static Stack<int > currenLoopStart = new Stack<int>();

        public static void RegisterTo(IVirtualMachine vm)
		{

            BrainfuckLoopCommands.IndexLoops(vm.Instructions, 0, loops);
			
			vm.RegisterCommand('[', b => 
			{
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    b.InstructionPointer = loops[b.InstructionPointer];
                }
                else
                    currenLoopStart.Push(b.InstructionPointer);
			});
			vm.RegisterCommand(']', b => 
			{
                if (b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = currenLoopStart.Peek();
                }
                else 
                    b.InstructionPointer = loops[currenLoopStart.Pop()];

            });
		}

        public static void IndexLoops(string program, int startIndex, Dictionary<int, int> loops)
        {

            for (int i = startIndex + 1; i < program.Length; i++)
            {
                if (program[i] == ']')
                {
                    loops[startIndex] = i;
                    //currenLoopStart.Push(startIndex);
                    break;
                }
                if (program[i] == '[')
                {
                    loops[i] = -1;
                    IndexLoops(program, i, loops); //"0[2[4]6]8"
                    i = loops[i];
                }
            }
        }
    }
}