using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingTest : MonoBehaviour
    {
        [NonSerialized]
        public ModBehaviour mod;
        public MeshRenderer meshRenderer;

        public void Awake()
        {
            mod = Jam6.Instance;
            meshRenderer = GetComponent<MeshRenderer>();
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
            if (item.itemID == "test")
            {
                meshRenderer.enabled = false;
                mod.ModHelper.Console.WriteLine($"Its test, mesh: {meshRenderer.enabled}", OWML.Common.MessageType.Success);
            }
        }

        public void Appear(SchedulingItem item)
        {
            mod.ModHelper.Console.WriteLine("Got Deactivate Event", OWML.Common.MessageType.Success);
            if (item.itemID == "test")
            {
                meshRenderer.enabled = true;
                mod.ModHelper.Console.WriteLine($"Its test, mesh: {meshRenderer.enabled}", OWML.Common.MessageType.Success);
            }
        }
    }
}
