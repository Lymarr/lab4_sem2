using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public class StartAction : WorkflowAction
    {
        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Starting the workflow...");
            await Task.Delay(1000);
            await OnCompletedAsync();
        }
    }
}
