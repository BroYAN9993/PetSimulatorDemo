using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace PetSimulatorDemo.StateMachineBase
{
    public delegate void CallBack();

    public delegate void MessageHandler(Message message);

    public delegate bool MessageFilter(Message message);

    public interface IRunable
    {
        public void Run();
        public uint Time { get; set; }
    }
    public abstract class Machine<T> : IRunable where T : struct, Enum
    {
        public T State { get; set; }
        public HashSet<IRunable> Components { get; set; }
        public Dictionary<T, List<CallBack>> AutoCallHandlers { get; set; }
        public IIo StandardIn { get; set; }
        public IIo StandardOut { get; set; }
        public uint Time { get; set; }
        public string Name { get; set; }

        public Machine(T state, string name, IIo standardIn = null, IIo standardOut = null)
        {
            State = state;
            Components = new HashSet<IRunable>();
            AutoCallHandlers = GetAllStates().Aggregate(new Dictionary<T, List<CallBack>>(), (dic, s) =>
            {
                dic.Add(s, new List<CallBack>());
                return dic;
            });
            StandardIn = standardIn;
            StandardOut = standardOut;
            Name = name;
            Time = World.Time;
        }

        public T[] GetAllStates()
        {
            return Enum.GetNames(typeof(T)).Select(e =>
            {
                T t;
                Enum.TryParse(e, out t);
                return t;
            }).ToArray();
        }

        public void Install(IRunable module)
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
        
        public void Listen(Type messageType, CallBack callBack)
        {
            StandardIn.Filter(m => m.GetType() == messageType)
                .Subscribe(m => callBack());
        }

        public void ListenWithMessage(Type messageType, MessageHandler messageHandler)
        {
            StandardIn.Filter(m => m.GetType() == messageType)
                .Subscribe(m => messageHandler(m));
        }

        public void AutoCallByState(T state, CallBack callBack)
        {
            AutoCallHandlers[state].Add(callBack);
        }
    }

    public interface IIo
    {
        public void Push(Message message);
        public void Subscribe(MessageHandler onNext);
        public IObservable<Message> Filter(MessageFilter filter);
    }

    public abstract class Message
    {
        public abstract uint Code { get; }
    }
}