using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_3
{
    public class RetryPolicy
    {
        public int MaxAttempts { get; }
        public TimeSpan InitialDelay { get; }
        public TimeSpan MaxDelay { get; }

        public RetryPolicy(int maxAttempts, TimeSpan initialDelay, TimeSpan maxDelay)
        {
            MaxAttempts = maxAttempts;
            InitialDelay = initialDelay;
            MaxDelay = maxDelay;
        }
    }
}
