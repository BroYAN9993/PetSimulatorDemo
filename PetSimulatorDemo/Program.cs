using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using PetSimulatorDemo.Modules;
using PetSimulatorDemo.StateMachineBase;

namespace PetSimulatorDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var BUS = new Bus();
            var process = new Process(ProcessState.Init, "process", 10, 1, BUS);
            for (var i = 0; i < 100; i++)
            {
                Console.WriteLine($"Frame is {i}");
                if (i == 20) BUS.Push(new ProcessStartMessage());
                process.Run();
                Console.WriteLine($"{process.Name} is running, state is {process.State}");
                Thread.Sleep(1000);
            }
        }
    }
}