using NewHorizons.Components.Props;
using OWML.ModHelper;
using System;
using UnityEngine;

namespace Jam6
{
    public class SchedulingItem : NHItem
    {
        [SerializeField]
        public string itemID;

        [NonSerialized]
        public GameObject hologram;
        [NonSerialized]
        public ModBehaviour mod;

        public void OnValidate()
        {
            _type = Jam6.SchedulingItemType;
            Droppable = true;
            hologram = transform.GetChild(1).gameObject;
        }

        public override void Awake()
        {
            OnValidate();
            base.Awake();
            mod = Jam6.Instance;
            mod.ModHelper.Console.WriteLine("A scheduling item is created", OWML.Common.MessageType.Success);
        }

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            mod.ModHelper.Console.WriteLine("I got picked up", OWML.Common.MessageType.Success);
            hologram.SetActive(false);
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            mod.ModHelper.Console.WriteLine("I got picked down", OWML.Common.MessageType.Success);
            hologram.SetActive(true);
        }

        public override void SocketItem(Transform socketTransform, Sector sector)
        {
            base.SocketItem(socketTransform, sector);
            mod.ModHelper.Console.WriteLine("I got socketed", OWML.Common.MessageType.Success);
            hologram.SetActive(true);
        }
    }
}
