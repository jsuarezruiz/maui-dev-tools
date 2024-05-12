#nullable enable
namespace MauiDevTools.Extensions
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task, Action<Exception> errorHandler = null)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && errorHandler != null)
                    errorHandler(t.Exception);

            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}