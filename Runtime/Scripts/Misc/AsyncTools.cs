using System;
using System.Threading.Tasks;

namespace NP
{
    public static class AsyncTools
    {
        public static void FireAndForget(
            this Task task,
            Action<Exception> errorHandler = null)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && errorHandler != null)
                    errorHandler(t.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}