using NewHorizons.Utility;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingRain : MonoBehaviour
    {
        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public GameObject rain;
        [NonSerialized]
        public float timeStamp;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += Activate;
            SchedulingSocket.DeactivateScheduledEvent += Deactivate;
            rain = SearchUtilities.Find("Disc_Body/Sector/Atmosphere_Rain");
            rain?.SetActive(false);
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= Activate;
            SchedulingSocket.DeactivateScheduledEvent -= Deactivate;
        }

        public void Activate(SchedulingItem item)
        {
            if (item.itemID == "Rain")
            {
                mod.ModHelper.Console.WriteLine("Got Activate Rain", OWML.Common.MessageType.Success);
                rain?.SetActive(true);
                timeStamp = TimeLoop.GetSecondsElapsed();
            }
        }

        public void Deactivate(SchedulingItem item)
        {
            if (item.itemID == "Rain")
            {
                mod.ModHelper.Console.WriteLine("Got Deactivate Rain", OWML.Common.MessageType.Success);
                rain?.SetActive(false);
            }
        }

        public void Update()
        {
            if (rain.activeSelf && TimeLoop.GetSecondsElapsed() - timeStamp >= 360f)
            {
                rain?.SetActive(false);
            }
        }
    }
}
