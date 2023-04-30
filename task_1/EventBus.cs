using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class EventBus
{
    private readonly Dictionary<string, List<Action<object>>> _eventHandlers = new();
    private readonly TimeSpan _throttleInterval;
    private DateTime _lastEventTime;

    public EventBus(TimeSpan throttleInterval)
    {
        _throttleInterval = throttleInterval;
    }

    public void Subscribe(string eventName, Action<object> eventHandler)
    {
        lock (_eventHandlers)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] = new List<Action<object>>();
            }

            _eventHandlers[eventName].Add(eventHandler);
        }
    }

    public void Unsubscribe(string eventName, Action<object> eventHandler)
    {
        lock (_eventHandlers)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName].Remove(eventHandler);

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

                        foreach (var eventHandler in _eventHandlers[eventName])
                        {
                            eventHandler(eventData);
                        }
                    }
                }
            }
        });
    }
}
