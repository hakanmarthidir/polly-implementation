using Polly;
using Polly.Timeout;

namespace shared;

public class ResponseTimeoutPolicy
{
    public AsyncTimeoutPolicy<HttpResponseMessage> Timeout { get; }

    public ResponseTimeoutPolicy(int timeOutSeconds = 150)
    {
        this.Timeout = Policy.TimeoutAsync<HttpResponseMessage>(timeOutSeconds, Polly.Timeout.TimeoutStrategy.Pessimistic);
    }
}
