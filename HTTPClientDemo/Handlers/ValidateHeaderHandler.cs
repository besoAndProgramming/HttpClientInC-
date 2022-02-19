using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPClientDemo.Handlers
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        public ValidateHeaderHandler(): base()
        {

        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Accept"))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("The API Key Accept is reqiured")
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
