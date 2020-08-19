
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System;

using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

//using LoggerService;
using hello.restaurant.api.Models;
using hello.restaurant.api.Extensions;

namespace hello.restaurant.api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger Logg;

        private readonly IConfiguration _configuration;

        public LoggingMiddleware(RequestDelegate next, IConfiguration configuration)//, ILogger _logger)
        {
            _next = next;
            _configuration = configuration;

            var path = $"assets/logs";

            //write file
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var IsEnabled = _configuration["AppSettings:LoggingMiddleware:IsEnabled"];
            if (Convert.ToBoolean(IsEnabled))
            {
                //logg
                Logg = new LoggerConfiguration()
                     .MinimumLevel.Information()
                     // .WriteTo.Sink(new EntityLogEventSink())
                     .WriteTo.RollingFile($@"{path}/middleware.txt", retainedFileCountLimit: 7)  //to file
                     .CreateLogger();
            }
            else
            {
                Logg = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();
            }
        }

        public async Task Invoke(HttpContext context)
        {
            //First, get the incoming request
            //var request = await FormatRequest(context.Request);

            context.Request.EnableRewind();

            using (var reader = new StreamReader(context.Request.Body))
            {
                var body = ""; //reader.ReadToEnd();

                // Do something
                //TODO: Save log to chosen datastore
                var elapsedMs = 77;
                var user = "anonymous"; //await this.GetHttpLoggingUserAsync(context);
                var requestId = Guid.NewGuid();

                //set context dor exception milldeware
                context.Items.Add("body", body);
                context.Items.Add("unique", user);
                context.Items.Add("id", requestId);
                //TODO: Save log to chosen datastore

                var requestPath = context.Request.Path.ToString();
                var requestScheme = $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {body}";

                //write when not this path
                //var EXCEPT_PATH = new string[] { "api/1.0/job/noti/get", "api/1.0/job/noti/list" };
                if (!context.Request.Path.Value.Contains("hub"))
                {
                    Logg.Information("Processed {@requestId} {@requestPath} {@requestScheme} {@trace} in {Elapsed:000} ms. by {unique} {type}",
                                 requestId, requestPath, requestScheme, "", elapsedMs, user, EFLogType.REQUEST.GetDescription());
                }

                context.Request.Body.Seek(0, SeekOrigin.Begin);
                await _next(context);
            }


            //Copy a pointer to the original response body stream
            /* var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response.DeepClone());

                //TODO: Save log to chosen datastore

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }*/
        }


        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableRewind();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}