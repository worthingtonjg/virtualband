using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace VirtualBand.Hub
{
    public delegate void PlayerInfoResultsDelegate(PlayerInfo playerInfo);

    public class HubActions
    {
        private IHubProxy hubProxy;
        private PlayerInfoResultsDelegate playerInfoResultsDelegate;



        public HubActions(PlayerInfoResultsDelegate resultsDelegate)
        {
            playerInfoResultsDelegate = resultsDelegate;

            string url = @"http://virtualbandhub.azurewebsites.net/";

            var connection = new HubConnection(url);
            hubProxy = connection.CreateHubProxy("VirtualBandHub");
            connection.Start().Wait();

            // Listen for calls from the Hub.
            hubProxy.On<object>("ReceivePlayerInfo", pi => HandlePlayerInfo(pi));
        }

        private void HandlePlayerInfo(object pi)
        {
            if (pi != null)
            {
                PlayerInfo player = JsonConvert.DeserializeObject<PlayerInfo>(pi.ToString());
                if (player != null)
                {
                    Debug.WriteLine("Received from Hub: " + player.ToString());
                    if (playerInfoResultsDelegate != null)
                    {
                        playerInfoResultsDelegate(player);
                    }
                }
            }
        }

        public void SendDataToHub(PlayerInfo playerInfo)
        {
            if (playerInfo != null)
            {
                Debug.WriteLine("Sending to Hub: " + playerInfo.ToString());
                hubProxy.Invoke("SetPlayerInfo", playerInfo).Wait();
            }
        }

    }
}
