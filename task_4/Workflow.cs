using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public class Workflow
    {
        private readonly Queue<WorkflowAction> _actions = new();

        public void AddAction(WorkflowAction action)
        {
            _actions.Enqueue(action);
        }

        public async Task ExecuteAsync()
        {
            while (_actions.Count > 0)
            {
                var action = _actions.Dequeue();
                action.Completed += async () =>
                {
                    if (_actions.Count > 0)
                    {
                        var nextAction = _actions.Peek();
                        await nextAction.ExecuteAsync();
                    }
                };

                await action.ExecuteAsync();
            }
        }
    }
}
