using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public class EndAction : WorkflowAction
    {
        public override async Task ExecuteAsync()
        {
            Console.WriteLine("Ending the workflow...");
            await Task.Delay(1000);
            await OnCompletedAsync();
        }
    }
}
