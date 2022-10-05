using Polly;
using Polly.Retry;

namespace shared;

public class WaitAndRetryPolicy
{
    public AsyncRetryPolicy<HttpResponseMessage> WaitAndRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> OnlyRetry { get; }

    public WaitAndRetryPolicy(int retryCount = 2, int sleepDuration = 5)
    {
        this.WaitAndRetry = Policy
                              .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                              .WaitAndRetryAsync(retryCount, sleepDurationProvider => TimeSpan.FromSeconds(sleepDuration));

        this.OnlyRetry = Policy
                             .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                             .RetryAsync(retryCount, (exception, retryCount, context) =>
                             {
                                 Console.WriteLine("OnlyRetry Logs...");
                                 var methodName = context["methodName"];
                                 Console.WriteLine(exception.Exception.Message);
                                 Console.WriteLine(methodName);
                             });
    }
}


