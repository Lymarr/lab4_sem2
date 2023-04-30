using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public class IntermediateAction : WorkflowAction
    {
        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Performing intermediate action...");
            await Task.Delay(1000);
            await OnCompletedAsync();
        }
    }
}
