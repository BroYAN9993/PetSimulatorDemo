using System.Reactive;
using System.Reactive.Subjects;

namespace PetSimulatorDemo.StateMachineBase
{
    public class Bus : IIo
    {
        public Subject<IMessage> Subject { get; set; }

        public Bus()
        {
            Subject = new Subject<IMessage>();
        }
        public void Push(IMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(MessageHandle onNext)
        {
            throw new System.NotImplementedException();
        }
    }
}