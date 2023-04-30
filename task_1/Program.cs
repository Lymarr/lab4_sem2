using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


class Program
{
    static async Task Main()
    {
        var eventBus = new EventBus(TimeSpan.FromMilliseconds(500));

        eventBus.Subscribe("SampleEvent", data => Console.WriteLine($"EventHandler1: {data}"));
        eventBus.Subscribe("SampleEvent", data => Console.WriteLine($"EventHandler2: {data}"));

        for (int i = 0; i < 10; i++)
        {
            await eventBus.PublishAsync("SampleEvent", $"Event {i}");
            await Task.Delay(100);
        }

        Console.ReadLine();
    }
}
