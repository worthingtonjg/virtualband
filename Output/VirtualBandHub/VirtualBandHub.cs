using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace VirtualBandHub
{
    [HubName("VirtualBandHub")]
    public class VirtualBandHub : Hub
    {
        //
        // Broadcast what was passed in out to all the clients.
        // We don't care what the object is.
        //
        public void SetPlayerInfo(object pi)
        {
            if (pi != null)
            {
                //Console.WriteLine(pi.ToString());
                Clients.All.ReceivePlayerInfo(pi);
            }
        }
    }
}