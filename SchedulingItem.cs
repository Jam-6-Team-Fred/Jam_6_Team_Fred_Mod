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
        public ModBehaviour mod;

        public void OnValidate()
        {
            _type = Jam6.SchedulingItemType;
            DisplayName = "SchedulingItemDisplayName";
            Droppable = true;
        }

        public override void Awake()
        {
            OnValidate();
            base.Awake();
            mod = Jam6.Instance;
            mod.ModHelper.Console.WriteLine("A scheduling item is created", OWML.Common.MessageType.Success);
        }
    }
}
