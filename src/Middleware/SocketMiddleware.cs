using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace hello.restaurant.apiServices
{
    public class SocketMiddleware : Hub
    {

        public SocketMiddleware()
        {
        }

        /*public async Task PullAsync()
        {
            var entity = await _jobService.ListParamsAsync();
            await Clients.Caller.SendAsync("PullAsync", entity);
        }*/

    }
}