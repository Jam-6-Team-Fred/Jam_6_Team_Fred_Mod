using NewHorizons.Components.Props;
using OWML.ModHelper;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace Jam6
{
    public class SchedulingSocket : OWItemSocket
    {
        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public OWItem heldItem;
        [NonSerialized]
        public bool didScheduledEventHappen;
        public delegate void ScheduledEvent(SchedulingItem item);
        public static event ScheduledEvent ActivateScheduledEvent;
        public static event ScheduledEvent DeactivateScheduledEvent;

        [SerializeField]
        public int activationHour;

        public void OnValidate()
        {
            _acceptableType = Jam6.SchedulingItemType;
        }

        public override void Awake()
        {
            OnValidate();
            base.Awake();
            mod = Jam6.Instance;
            mod.ModHelper.Console.WriteLine("A schedule board is created", OWML.Common.MessageType.Success);
            OnSocketableDonePlacing += AddSchedulingItem;
            OnSocketablePlaced += AddSchedulingItem;
            OnSocketableRemoved += RemoveSchedulingItem;
            OnSocketableDoneRemoving += RemoveSchedulingItem;
        }

        public void OnDestroy()
        {
            OnSocketableDonePlacing -= AddSchedulingItem;
            OnSocketablePlaced -= AddSchedulingItem;
            OnSocketableRemoved -= RemoveSchedulingItem;
            OnSocketableDoneRemoving -= RemoveSchedulingItem;
        }

        public void AddSchedulingItem(OWItem item)
        {
            if (heldItem == null)
            {
                heldItem = item;
            }
            mod.ModHelper.Console.WriteLine($"I hold {heldItem.name}", OWML.Common.MessageType.Success);
            if (didScheduledEventHappen)
            {
                ActivateScheduledEvent((SchedulingItem)heldItem);
            }
        }

        public void RemoveSchedulingItem(OWItem item)
        {
            mod.ModHelper.Console.WriteLine($"I shouldnt be holding {heldItem.name} anymore", OWML.Common.MessageType.Success);
            DeactivateScheduledEvent((SchedulingItem)heldItem);
            heldItem = null;
        }

        public void Update()
        {
            if (!didScheduledEventHappen && TimeLoop.GetSecondsElapsed() >= activationHour*120f)
            {   
                didScheduledEventHappen = true;
                mod.ModHelper.Console.WriteLine($"It is {activationHour}:00, Im activating...", OWML.Common.MessageType.Success);
                if (heldItem != null)
                {
                    ActivateScheduledEvent((SchedulingItem)heldItem);
                }
            }
        }
    }
}
