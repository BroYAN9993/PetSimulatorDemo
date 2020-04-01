using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace PetSimulatorDemo.StateMachineBase
{
    public class Bus : IIo
    {
        public Subject<Message> Subject { get; set; }
        public ThreadPoolScheduler Scheduler { get; set; }

        public Bus()
        {
            Subject = new Subject<Message>();
            Scheduler = ThreadPoolScheduler.Instance;
        }
        public void Push(Message message)
        {
            Subject.OnNext(message);
        }

        public void Subscribe(MessageHandler onNext)
        {
            Subject.SubscribeOn(Scheduler).Subscribe(m=> onNext(m));
        }

        public IObservable<Message> Filter(MessageFilter filter)
        {
            return Subject.SubscribeOn(Scheduler).Where(m => filter(m));
        }
    }
}