using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_2
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

        public async Task PublishAsync(string eventName, object eventData)
        {
            await Task.Run(() =>
            {
                lock (_eventHandlers)
                {
                    if (_eventHandlers.ContainsKey(eventName))
                    {
                        if (DateTime.Now - _lastEventTime > _throttleInterval)
                        {
                            _lastEventTime = DateTime.Now;

                            foreach (var subscriber in _eventHandlers[eventName])
                            {
                                subscriber.EventHandler(eventData);
                            }
                        }
                    }
                }
            });
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
