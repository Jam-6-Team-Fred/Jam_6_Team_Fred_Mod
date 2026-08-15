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

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += Activate;
            SchedulingSocket.DeactivateScheduledEvent += Deactivate;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= Activate;
            SchedulingSocket.DeactivateScheduledEvent -= Deactivate;
        }

        public void Activate(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Activate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "Rain")
            {

            }
        }

        public void Deactivate(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Deactivate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "Rain")
            {

            }
        }
    }
}
