using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_3
{
    public class Subscriber
    {
        private readonly string _name;

        public Subscriber(string name)
        {
            _name = name;
        }

        public void Subscribe(EventBus eventBus, string eventName, int priority)
        {
            eventBus.Subscribe(eventName, HandleEvent, priority);
        }

        public void Unsubscribe(EventBus eventBus, string eventName)
        {
            eventBus.Unsubscribe(eventName, HandleEvent);
        }

        private void HandleEvent(object eventData)
        {
            Console.WriteLine($"Subscriber {_name} received: {eventData}");
        }
    }
}
