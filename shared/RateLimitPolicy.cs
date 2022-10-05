using Polly;
using Polly.RateLimit;

namespace shared;

public class RateLimitPolicy
{
    public AsyncRateLimitPolicy RatePolicy { get; }

    public RateLimitPolicy(int numberOfExecution, TimeSpan perTime, int maxBurst)
    {
        this.RatePolicy = Policy.RateLimitAsync(numberOfExecution, perTime, maxBurst);
    }
}

