using Polly;
using Polly.Fallback;

namespace shared;

public class FallbackPolicy
{
    public AsyncFallbackPolicy<HttpResponseMessage> FallBack { get; }

    public FallbackPolicy()
    {
        this.FallBack = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .FallbackAsync(FallbackAction, OnFallbackAsync);
    }

    private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
    {
        Console.WriteLine("Fallback object created.");
        return Task.CompletedTask;
    }

    private Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Fallback object will be creating");

        HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
        {
            Content = new StringContent($"Fallback response : {responseToFailedRequest.Result.ReasonPhrase}")
        };
        return Task.FromResult(httpResponseMessage);
    }

}
