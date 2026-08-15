using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingLights : MonoBehaviour
    {
        [NonSerialized]
        public ModBehaviour mod;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += LightsOn;
            SchedulingSocket.DeactivateScheduledEvent += LightsOff;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= LightsOn;
            SchedulingSocket.DeactivateScheduledEvent -= LightsOff;
        }

        public void LightsOn(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Activate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "Lights")
            {

            }
        }

        public void LightsOff(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Deactivate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "Lights")
            {

            }
        }
    }
}
