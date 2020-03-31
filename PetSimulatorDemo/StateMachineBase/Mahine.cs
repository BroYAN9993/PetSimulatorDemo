using System;
using System.Collections.Generic;
using System.Linq;

namespace PetSimulatorDemo.StateMachineBase
{
    public delegate void CallBack();

    public delegate void MessageHandle(IMessage message);
    public class Machine
    {
        public IState State { get; set; }
        public HashSet<Machine> Components { get; set; }
        public Dictionary<IState, List<CallBack>> AutoCallHandles { get; set; }
        public IIo StandardIn { get; set; }
        public IIo StandardOut { get; set; }
        public uint Time { get; set; }
        public string Name { get; set; }

        public Machine(IState state, string name, IIo standardIn = null, IIo standardOut = null)
        {
            State = state;
            Components = new HashSet<Machine>();
            StandardIn = standardIn;
            StandardOut = standardOut;
        }

        public void Run()
        {
            foreach (var module in Components.Where(m => m.Time <= Time))
            {
                module.Run();
            }

            foreach (var handle in AutoCallHandles[State])
            {
                handle();
            }

            Time++;
        }
    }

    public interface IState
    {
        public Enum State { get; set; }
    }

    public interface IIo
    {
        public void Push(IMessage message);
        public void Subscribe(MessageHandle onNext);
    }

    public interface IMessage
    {
        public string MessageType { get; set; }
    }
}