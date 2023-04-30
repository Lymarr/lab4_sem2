using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace task_3
{
  
    class Program
    {
        static async Task Main()
        {
            var eventBus = new EventBus(TimeSpan.FromMilliseconds(500));
            var publisher = new Publisher(eventBus);

            var subscriberA = new Subscriber("A");
            var subscriberB = new Subscriber("B");
            var subscriberC = new Subscriber("C");

            subscriberA.Subscribe(eventBus, "SampleEvent", 3);
            subscriberB.Subscribe(eventBus, "SampleEvent", 1);
            subscriberC.Subscribe(eventBus, "SampleEvent", 2);

            var retryPolicy = new RetryPolicy(maxAttempts: 3, initialDelay: TimeSpan.FromMilliseconds(200), maxDelay: TimeSpan.FromSeconds(1));

            for (int i = 0; i < 10; i++)
            {
                await publisher.PublishEventAsync("SampleEvent", $"Event {i}", retryPolicy);
                await Task.Delay(100);
            }

            subscriberA.Unsubscribe(eventBus, "SampleEvent");
            subscriberB.Unsubscribe(eventBus, "SampleEvent");
            subscriberC.Unsubscribe(eventBus, "SampleEvent");
        }
    }
}
