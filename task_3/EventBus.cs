using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_3
{
    public class EventBus
    {
        private readonly Dictionary<string, List<Subscriber>> _eventHandlers = new();
        private readonly TimeSpan _throttleInterval;
        private DateTime _lastEventTime;

        public EventBus(TimeSpan throttleInterval)
        {
            _throttleInterval = throttleInterval;
        }

        public void Subscribe(string eventName, Action<object> eventHandler, int priority)
        {
            lock (_eventHandlers)
            {
                if (!_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName] = new List<Subscriber>();
                }

                _eventHandlers[eventName].Add(new Subscriber(eventHandler, priority));
                _eventHandlers[eventName] = _eventHandlers[eventName].OrderByDescending(s => s.Priority).ToList();
            }
        }

        public void Unsubscribe(string eventName, Action<object> eventHandler)
        {
            lock (_eventHandlers)
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName].RemoveAll(s => s.EventHandler == eventHandler);

                    if (_eventHandlers[eventName].Count == 0)
                    {
                        _eventHandlers.Remove(eventName);
                    }
                }
            }
        }

        public async Task PublishAsync(string eventName, object eventData, RetryPolicy retryPolicy)
        {
            List<Subscriber> subscribersToInvoke = new List<Subscriber>();

            lock (_eventHandlers)
            {
                if (_eventHandlers.ContainsKey(eventName) && DateTime.Now - _lastEventTime > _throttleInterval)
                {
                    _lastEventTime = DateTime.Now;
                    subscribersToInvoke.AddRange(_eventHandlers[eventName]);
                }
            }

            if (subscribersToInvoke.Count > 0)
            {
                foreach (var subscriber in subscribersToInvoke)
                {
                    await InvokeWithRetryAsync(subscriber.EventHandler, eventData, retryPolicy);
                }
            }
        }

        private async Task InvokeWithRetryAsync(Action<object> eventHandler, object eventData, RetryPolicy retryPolicy)
        {
            int attempt = 0;
            bool success = false;
            var random = new Random();

            while (attempt < retryPolicy.MaxAttempts && !success)
            {
                try
                {
                    eventHandler(eventData);
                    success = true;
                }
                catch
                {
                    attempt++;

                    if (attempt < retryPolicy.MaxAttempts)
                    {
                        double exponentialBackoffFactor = Math.Pow(2, attempt);
                        double randomFactor = 1 + random.NextDouble() * 0.1; // Add jitter (randomness) between 1 and 1.1
                        TimeSpan delay = TimeSpan.FromMilliseconds(Math.Min(
                            retryPolicy.InitialDelay.TotalMilliseconds * exponentialBackoffFactor * randomFactor,
                            retryPolicy.MaxDelay.TotalMilliseconds));

                        await Task.Delay(delay);
                    }
                }
            }
        }

        private class Subscriber
        {
            public Action<object> EventHandler { get; }
            public int Priority { get; }

            public Subscriber(Action<object> eventHandler, int priority)
            {
                EventHandler = eventHandler;
                Priority = priority;
            }
        }
    }
}
