using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Timeout;
using shared;

namespace client.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    IHttpClientFactory _httpClientFactory;
    private readonly WaitAndRetryPolicy _retryPolicy;
    private readonly ResponseTimeoutPolicy _timeoutPolicy;
    private readonly CircuitPolicy _circuitPolicy;
    private readonly RateLimitPolicy _ratePolicy;
    private readonly BulkHeadPolicy _bulkHeadPolicy;
    private readonly FallbackPolicy _fallbackPolicy;

    public ClientController(
        IHttpClientFactory httpClientFactory,
        WaitAndRetryPolicy retryPolicy,
        ResponseTimeoutPolicy timeoutPolicy,
        CircuitPolicy circuitPolicy
,
        RateLimitPolicy ratePolicy,
        BulkHeadPolicy bulkHeadPolicy,
        FallbackPolicy fallbackPolicy)
    {
        this._httpClientFactory = httpClientFactory;
        _retryPolicy = retryPolicy;
        _timeoutPolicy = timeoutPolicy;
        this._circuitPolicy = circuitPolicy;
        _ratePolicy = ratePolicy;
        _bulkHeadPolicy = bulkHeadPolicy;
        _fallbackPolicy = fallbackPolicy;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //var client = this._httpClientFactory.CreateClient("PolicyTest");
        var client = this._httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("http://localhost:5174");

        //0- Common Way
        //Instead of this, use policy ExecuteAsync
        //var response = await client.GetAsync("/api/test").ConfigureAwait(false);


        //1- Only Retry Policy
        //HttpResponseMessage response = await _retryPolicy.WaitAndRetry.ExecuteAsync(() => client.GetAsync("/api/retry/1"));


        //2- Timeout Policy
        //var timeout = new Random().Next(50, 1500);
        //HttpResponseMessage response = await _timeoutPolicy.Timeout.ExecuteAsync(() => client.GetAsync("/api/timeout/" + timeout));


        //3- Circuit Breaker Policy

        //HttpResponseMessage response = await _circuitPolicy.Breaker.Execute(() => client.GetAsync("/api/circuit/" + false));


        //4- Fallback 
        //HttpResponseMessage response = await _fallbackPolicy.FallBack.ExecuteAsync(() => client.GetAsync("/api/retry/1"));


        //5- WrapAsync -> Retry and Timeout and Rate Policy

        var policy = _retryPolicy.WaitAndRetry
            .WrapAsync(this._fallbackPolicy.FallBack)
            .WrapAsync(this._timeoutPolicy.Timeout)
            .WrapAsync(this._circuitPolicy.Breaker)
            .WrapAsync(this._ratePolicy.RatePolicy)
            .WrapAsync(this._bulkHeadPolicy.BulkHead)
            ;


        CancellationTokenSource cancellationSource = new CancellationTokenSource();
        var retryId = new Random().Next(0, 3);
        var response = await policy.ExecuteAsync(async cancellationToken => await client.GetAsync("/api/retry/" + retryId, cancellationToken), cancellationSource.Token);


        //Result
        if (response.IsSuccessStatusCode == true)
        {
            var content = response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

