using NewHorizons.Components.Props;
using OWML.ModHelper;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
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
        public bool hasScheduledTimeCome = false;
        [NonSerialized]
        public bool hasThisGoneOffYet = false;
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
            mod.ModHelper.Console.WriteLine($"A schedule pedestal (ah:{activationHour}) is created", OWML.Common.MessageType.Success);
            OnSocketablePlaced += AddSchedulingItem;
            OnSocketableRemoved += RemoveSchedulingItem;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().AddListener(SetHeldItem);
        }

        public override void Start()
        {
            //Base game bullshit fuck you man
            if (_socketedItem != null)
            {
                _socketedItem.MoveAndChildToTransform(_socketTransform);
            }
        }

        public void SetHeldItem(string planetName)
        {
            if (planetName == "Disc")
            {
                if (transform.childCount > 0)
                {
                    heldItem = transform.GetChild(0).gameObject.GetComponent<SchedulingItem>();
                }

                if (isAlwaysActive)
                {
                    activationHour = 0;
                }
            }
        }

        public void OnDestroy()
        {
            OnSocketablePlaced -= AddSchedulingItem;
            OnSocketableRemoved -= RemoveSchedulingItem;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().RemoveListener(SetHeldItem);
        }

        public void AddSchedulingItem(OWItem item)
        {
            if (heldItem == null)
            {
                heldItem = item;
            }
            mod.ModHelper.Console.WriteLine($"I now hold {heldItem.name}", OWML.Common.MessageType.Success);
            if (hasScheduledTimeCome)
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

        public override void Update()
        {
            if (!hasThisGoneOffYet && TimeLoop.GetSecondsElapsed() >= activationHour*120f)
            {   
                if (!hasScheduledTimeCome)
                {
                    mod.ModHelper.Console.WriteLine($"It is {activationHour}:00, Im activating...", OWML.Common.MessageType.Success);
                }
                hasScheduledTimeCome = true;
                if (heldItem != null)
                {
                    mod.ModHelper.Console.WriteLine($"It isnt null, ill try activating...", OWML.Common.MessageType.Success);
                    ActivateScheduledEvent((SchedulingItem)heldItem, isAlwaysActive);
                    hasThisGoneOffYet = true;
                }
            }
        }
    }
}
