using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace VirtualBand
{
    public class UnityHelper
    {
        public delegate void UnityEvent(object arg);

        public UnityHelper()
        {
        }

        public void SendMessage(string message, object obj)
        {
            if(UnityPlayer.AppCallbacks.Instance.IsInitialized())
            {
                UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(new UnityPlayer.AppCallbackItem(() => 
                {
                    UnityEngine.GameObject bandManager = UnityEngine.GameObject.Find("BandManager");

                    if (bandManager != null)
                    {
                        bandManager.GetComponent<XAMLConnection>().ProcessMessage(message, obj);
                    }
                    else
                    {
                        throw new Exception("BandManager not found - make sure it is defined in your scene");
                    }

                }), false);
            }
        }



        public void SetEvent(UnityEvent e)
        {
            UnityPlayer.AppCallbacks.Instance.InvokeOnAppThread(new UnityPlayer.AppCallbackItem(() => {
                UnityEngine.GameObject bandManager = UnityEngine.GameObject.Find("BandManager");

                if (bandManager != null)
                {
                    bandManager.GetComponent<XAMLConnection>().onEvent = new XAMLConnection.OnEvent(e);
                }
                else
                {
                    throw new Exception("BandManager not found - make sure it is defined in your scene");
                }
            }), false);
        }
    }
}
