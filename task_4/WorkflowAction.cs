using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public abstract class WorkflowAction
    {
        public event Func<Task> Completed;

        public abstract Task ExecuteAsync();

        protected async Task OnCompletedAsync()
        {
            if (Completed != null)
            {
                await Completed();
            }
        }
    }
}
