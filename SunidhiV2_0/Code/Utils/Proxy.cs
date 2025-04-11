using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace SunidhiV2_0.Code.Utils
{
    public class Proxy
    {
        public async Task ReportProxy(HttpContext context,string host,int port)
        {
            var request = context.Request;
            // CookieContainer cookies = new CookieContainer();
            // HttpClientHandler handler = new HttpClientHandler();

            // handler.CookieContainer = cookies;
            var client = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false, UseCookies = false });
            var uri = new Uri(UriHelper.BuildAbsolute("http", new HostString(host, port), "", request.Path, request.QueryString));

            // foreach (var rcookie in request.Cookies)
            // {
            //     cookies.Add(new Cookie(rcookie.Key, rcookie.Value) { Domain = uri.Host });
            // }
            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            // Copy the request headers
            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            var ipaddress = request.Headers["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ipaddress))
                ipaddress = request.Headers["REMOTE_ADDR"];
            // requestMessage.Headers.Add("X-Forwarded-For", "127.0.0.1");
            // requestMessage.Headers.Add("X-Forwarded-Host", "localhost:5000");
            //requestMessage.Headers.Add("host", request.Host.Host.ToString()+":"+request.Host.Port.ToString());

            // requestMessage.Headers.Host =request.Host.Host.ToString()+":"+request.Host.Port.ToString();
            //requestMessage.Headers.Host=uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(request.Method);

            var responseMessage = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);

            var response = context.Response;
            response.StatusCode = (int)responseMessage.StatusCode;
            foreach (var header in responseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {

                await responseStream.CopyToAsync(response.Body, 81920, context.RequestAborted);
            }
        }
    }
}

