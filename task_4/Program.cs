using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace task_4
{
 
    class Program
    {
        static async Task Main()
        {
            var workflow = new Workflow();
            workflow.AddAction(new StartAction());
            workflow.AddAction(new IntermediateAction());
            workflow.AddAction(new CustomAction("This is a custom action."));
            workflow.AddAction(new EndAction());

            await workflow.ExecuteAsync();
        }
    }
}
