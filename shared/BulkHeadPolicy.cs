using Polly;
using Polly.Bulkhead;

namespace shared;

public class BulkHeadPolicy
{
    public AsyncBulkheadPolicy BulkHead { get; }

    public BulkHeadPolicy(int maxParallelization)
    {
        this.BulkHead = Policy.BulkheadAsync(maxParallelization);
    }
}

