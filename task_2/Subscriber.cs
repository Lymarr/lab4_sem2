using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_2
{
    public class Subscriber
    {
        public Subscriber(EventBus eventBus, string eventName, Action<object> eventHandler, int priority)
        {
            eventBus.Subscribe(eventName, eventHandler, priority);
        }
    }
}
