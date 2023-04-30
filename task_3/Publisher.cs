using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_3
{
    public class Publisher
    {
        private readonly EventBus _eventBus;

        public Publisher(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task PublishEventAsync(string eventName, object eventData, RetryPolicy retryPolicy)
        {
            await _eventBus.PublishAsync(eventName, eventData, retryPolicy);
        }
    }
}
