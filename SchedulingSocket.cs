using NewHorizons.Components.Props;
using OWML.ModHelper;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace Jam6
{
    public class SchedulingSocket : OWItemSocket
    {
        [SerializeField]
        public bool isAlwaysActive;

        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public OWItem heldItem;
        [NonSerialized]
        public bool didScheduledEventHappen;
        public delegate void ScheduledEvent(SchedulingItem item, bool isAlwaysActive);
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

        public override void Start()
        {
            base.Start();
            if (transform.GetChildCount() > 0)
            {
                heldItem = transform.GetChild(0).gameObject.GetComponent<SchedulingItem>();
            }

            if (isAlwaysActive)
            {
                activationHour = 0;
            }
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
            mod.ModHelper.Console.WriteLine($"I now hold {heldItem.name}", OWML.Common.MessageType.Success);
            if (didScheduledEventHappen)
            {
                ActivateScheduledEvent((SchedulingItem)heldItem, isAlwaysActive);
            }
        }

        public void RemoveSchedulingItem(OWItem item)
        {
            mod.ModHelper.Console.WriteLine($"I shouldnt be holding {heldItem.name} anymore", OWML.Common.MessageType.Success);
            DeactivateScheduledEvent((SchedulingItem)heldItem, isAlwaysActive);
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
                    ActivateScheduledEvent((SchedulingItem)heldItem, isAlwaysActive);
                }
            }
        }
    }
}
