using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_2
{
    class Program
    {
        static async Task Main()
        {
            var eventBus = new EventBus(TimeSpan.FromMilliseconds(500));

            var publisher = new Publisher(eventBus);

            var subscriber1 = new Subscriber(eventBus, "SampleEvent", data => Console.WriteLine($"Subscriber1: {data}"), priority: 2);
            var subscriber2 = new Subscriber(eventBus, "SampleEvent", data => Console.WriteLine($"Subscriber2: {data}"), priority: 1);

            for (int i = 0; i < 10; i++)
            {
                await publisher.SendEventAsync("SampleEvent", $"Event {i}");
                await Task.Delay(100);
            }

            Console.ReadLine();
        }
    }
}
