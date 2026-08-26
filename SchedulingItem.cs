using NewHorizons.Components.Props;
using NewHorizons.Handlers;
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
        [NonSerialized]
        public string translatedName;
        [NonSerialized]
        public ModBehaviour newHorizons;

        public void OnValidate()
        {
            mod = Jam6.Instance;
            _type = Jam6.SchedulingItemType;
            ItemType = Jam6.SchedulingItemType;
            Droppable = true;
            DropAudio = AudioTypeHandler.GetAudioType("planets/Assets/Audio/Prism_Drop.ogg", mod);
            PickupAudio = AudioTypeHandler.GetAudioType("planets/Assets/Audio/Prism_Grab.ogg", mod);
            SocketAudio = AudioTypeHandler.GetAudioType("planets/Assets/Audio/Prism_Socket.ogg", mod);
            UnsocketAudio = AudioTypeHandler.GetAudioType("planets/Assets/Audio/Prism_Unsocket.ogg", mod);
        }

        public override void Awake()
        {
            OnValidate();
            base.Awake();
            if (string.IsNullOrEmpty(translatedName))
            {
                translatedName = Jam6.Instance.NewHorizons.GetTranslationForUI(DisplayName);
            }
            mod.ModHelper.Console.WriteLine("A scheduling item is created", OWML.Common.MessageType.Success);
        }

        public void Start()
        {
            hologram = transform.GetChild(1).gameObject;
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

        public override string GetDisplayName() => translatedName;
    }
}
