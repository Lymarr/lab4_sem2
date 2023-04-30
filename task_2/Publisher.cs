using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_2
{
    public class Publisher
    {
        private readonly EventBus _eventBus;

        public Publisher(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task SendEventAsync(string eventName, object eventData)
        {
            await _eventBus.PublishAsync(eventName, eventData);
        }
    }

}
