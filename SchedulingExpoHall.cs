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
        [SerializeField]
        public MeshRenderer meshRenderer;
        [SerializeField]
        public MeshCollider meshCollider;

        [NonSerialized]
        public ModBehaviour mod;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += Open;
            SchedulingSocket.DeactivateScheduledEvent += Close;
        }

        public void Start()
        {
            if (meshCollider == null)
            {
                meshCollider = GetComponent<MeshCollider>();
            }
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= Open;
            SchedulingSocket.DeactivateScheduledEvent -= Close;
        }

        public void Open(SchedulingItem item, bool doesntMatter)
        {
            if (item.itemID == "ExpoHall")
            {
                mod.ModHelper.Console.WriteLine("Got Activate ExpoHall", OWML.Common.MessageType.Success);
                meshRenderer.enabled = false;
                meshCollider.enabled = false;
            }
        }

        public void Close(SchedulingItem item, bool doesntMatter)
        {
            if (item.itemID == "ExpoHall")
            {
                mod.ModHelper.Console.WriteLine("Got Deactivate ExpoHall", OWML.Common.MessageType.Success);
                meshRenderer.enabled = true;
                meshCollider.enabled = true;
            }
        }
    }
}
