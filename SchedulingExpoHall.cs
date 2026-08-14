using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingExpoHall : MonoBehaviour
    {
        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public MeshRenderer meshRenderer;
        [NonSerialized]
        public MeshCollider meshCollider;

        public void Awake()
        {
            mod = Jam6.Instance;
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
            SchedulingSocket.ActivateScheduledEvent += Disappear;
            SchedulingSocket.DeactivateScheduledEvent += Appear;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= Disappear;
            SchedulingSocket.DeactivateScheduledEvent -= Appear;
        }

        public void Disappear(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Activate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "ExpoHall")
            {
                meshRenderer.enabled = false;
                mod.ModHelper.Console.WriteLine($"Its ExpoHall, mesh: {meshRenderer.enabled}", OWML.Common.MessageType.Success);
            }
        }

        public void Appear(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Deactivate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "ExpoHall")
            {
                meshRenderer.enabled = true;
                mod.ModHelper.Console.WriteLine($"Its ExpoHall, mesh: {meshRenderer.enabled}", OWML.Common.MessageType.Success);
            }
        }
    }
}
