using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace PetSimulatorDemo.StateMachineBase
{
    public delegate void CallBack();

    public delegate void MessageHandler(Message message);

    public delegate bool MessageFilter(Message message);
    public abstract class Machine
    {
        public IState State { get; set; }
        public HashSet<Machine> Components { get; set; }
        public Dictionary<IState, List<CallBack>> AutoCallHandlers { get; set; }
        public IIo StandardIn { get; set; }
        public IIo StandardOut { get; set; }
        public uint Time { get; set; }
        public string Name { get; set; }

        public Machine(IState state, string name, IIo standardIn = null, IIo standardOut = null)
        {
            State = state;
            Components = new HashSet<Machine>();
            AutoCallHandlers = new Dictionary<IState, List<CallBack>>();
            StandardIn = standardIn;
            StandardOut = standardOut;
            Time = World.Time;
        }

        public void Install(Machine module)
        {
            Components.Add(module);
        }

        public abstract void Execute();
        public void Run()
        {
            foreach (var module in Components.Where(m => m.Time <= Time))
            {
                module.Run();
            }

            foreach (var handler in AutoCallHandlers[State])
            {
                handler();
            }
            Execute();
            Time++;
        }
        
        private void Listen(Type messageType, CallBack callBack)
        {
            StandardIn.Filter(m => m.GetType() == messageType);
        }
    }

    public interface IState
    {
        public Enum State { get; set; }
    }

    public interface IIo
    {
        public void Push(Message message);
        public void Subscribe(MessageHandler onNext);
        public void Filter(MessageFilter filter);
    }

    public abstract class Message
    {
        public uint Code { get; set; }
    }
}