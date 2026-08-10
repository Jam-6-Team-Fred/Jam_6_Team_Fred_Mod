using NewHorizons.Components.Props;
using OWML.ModHelper;
using UnityEngine;

namespace Jam6
{
    public class SchedulingSocket : OWItemSocket
    {
        public ModBehaviour mod;
        public OWItem heldItem;
        public bool didScheduledEventHappen;
        public delegate void ScheduledEvent(OWItem item);
        public ScheduledEvent ActivateScheduledEvent;

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
        }

        public void AddSchedulingItem(OWItem item)
        {
            heldItem = item;
            mod.ModHelper.Console.WriteLine($"I hold {heldItem.name}", OWML.Common.MessageType.Success);
            if (didScheduledEventHappen)
            {
                ActivateScheduledEvent(heldItem);
            }
        }

        public void Update()
        {
            if (!didScheduledEventHappen && TimeLoop.GetSecondsElapsed() >= activationHour*120f)
            {   
                didScheduledEventHappen = true;
                mod.ModHelper.Console.WriteLine($"It is {activationHour}:00, Im activating...", OWML.Common.MessageType.Success);
                if (heldItem != null)
                {
                    ActivateScheduledEvent(heldItem);
                }
            }
        }
    }
}
