using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
            vm.RegisterCommand('+', b => { unchecked { b.Memory[b.MemoryPointer]++; } });
            vm.RegisterCommand('-', b => { unchecked { b.Memory[b.MemoryPointer]--; } });
            vm.RegisterCommand(',', b =>
            {
                var symbol = (byte)read();
                b.Memory[b.MemoryPointer] = symbol;
            }
            );

            vm.RegisterCommand('>', b =>
            {
                b.MemoryPointer++;
                if (b.MemoryPointer > b.Memory.Length - 1)
                    b.MemoryPointer = 0;
            }
            );

            vm.RegisterCommand('<', b =>
            {
                b.MemoryPointer--;
                if (b.MemoryPointer < 0)
                    b.MemoryPointer = b.Memory.Length - 1;
            }
            );
            for (char a = 'A'; a <= 'Z'; a++)
            {
                RegisterLetteeorNumber(a);
            }
            for (char a = 'a'; a <= 'z'; a++)
            {
                RegisterLetteeorNumber(a);
            }
            for (char a = '0'; a <= '9'; a++)
            {
                RegisterLetteeorNumber(a);
            }

            void RegisterLetteeorNumber(char symbol)
            {
                byte info = (byte)symbol;
                vm.RegisterCommand(symbol, b => { b.Memory[b.MemoryPointer] = info; });
            }
        }
    }
}