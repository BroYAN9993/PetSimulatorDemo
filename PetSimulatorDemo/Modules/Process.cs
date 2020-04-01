using System;
using PetSimulatorDemo.StateMachineBase;

namespace PetSimulatorDemo.Modules
{
    public enum ProcessState
    {
        Init,
        Processing,
        Finished
    }

    public class ProcessStartMessage : Message
    {
        public override uint Code { get => 8848; }
    }
    
    public class Process : Machine<ProcessState>
    {
        public uint[] Runner { get; set; }
        public uint Progress { get; set; }
        public Process(ProcessState state, string name, uint stop, uint step, IIo standardIn = null, IIo standardOut = null)
            : base(state, name, standardIn, standardOut)
        {
            Runner = new uint[] {step, stop};
            Progress = 0;
            Listen(typeof(ProcessStartMessage), () => { State = ProcessState.Processing; });
            AutoCallByState(ProcessState.Processing, () => Console.WriteLine($"[PROCESS INFO] {State}, Progress is {Progress}"));
        }

        public void Reset()
        {
            State = ProcessState.Init;
            Progress = 0;
        }
        public override void Execute()
        {
            if (State != ProcessState.Processing) return;
            if (Progress >= Runner[1])
            {
                State = ProcessState.Finished;
                Progress = Runner[1];
            }
            else
            {
                Progress += Runner[0];
            }
        }
    }
}