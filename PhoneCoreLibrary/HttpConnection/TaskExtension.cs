using System;
using System.Net;
using System.Threading.Tasks;

namespace PhoneCoreLibrary.HttpConnection
{
    public static class TaskExtension
    {
        public static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            return Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var b = task.Wait(duration);
                        return b ? task.Result : default(T);
                    }
                    catch (AggregateException aex)
                    {
                        aex.Flatten().Handle(ex => ex is WebException);
                    }
                    return default(T);
                });
        }
    }
}
