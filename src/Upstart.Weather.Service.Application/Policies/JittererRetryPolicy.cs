using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Upstart.Weather.Service.Application.Policies
{
    public static class JittererRetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            var jitterer = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.GatewayTimeout)
                .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp))
                                                     + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
        }
    }
}
