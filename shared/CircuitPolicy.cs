using Polly;
using Polly.CircuitBreaker;

namespace shared;

public class CircuitPolicy
{
    public AsyncCircuitBreakerPolicy<HttpResponseMessage> Breaker { get; }

    public CircuitPolicy(int exceptionCount, TimeSpan durationOfBreak)
    {

        this.Breaker = Policy
                        .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                        .CircuitBreakerAsync(exceptionCount, durationOfBreak, OnBreak, OnReset, OnHalfOpen)
                        ;

    }

    private void OnHalfOpen()
    {
        Console.WriteLine("OnHalfOpen");
    }

    private void OnReset()
    {
        Console.WriteLine("OnReset");
    }

    private void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
    {
        Console.WriteLine("OnBreak");
    }
}

