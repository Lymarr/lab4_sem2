using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    public class CustomAction : WorkflowAction
    {
        private readonly string _message;

        public CustomAction(string message)
        {
            _message = message;
        }

        public override async Task ExecuteAsync()
        {
            Console.WriteLine($"Performing custom action: {_message}");
            await Task.Delay(1000);
            await OnCompletedAsync();
        }
    }
}
