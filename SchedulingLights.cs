using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingLights : MonoBehaviour
    {
        [SerializeField]
        public OWLightController lightController;

        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public float timeStamp;
        [NonSerialized]
        public MeshRenderer lampMesh;
        [NonSerialized]
        public Material lampMaterial;
        [NonSerialized]
        public Color emissiveColor;
        [NonSerialized]
        public bool isOnAnAlwaysActivePedestal;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += LightsOn;
            SchedulingSocket.DeactivateScheduledEvent += LightsOff;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= LightsOn;
            SchedulingSocket.DeactivateScheduledEvent -= LightsOff;
        }

        public void Start()
        {
            if (lightController == null)
            {
                lightController = GetComponent<OWLightController>();
            }
            lightController.SetIntensity(0f);
        }

        public void LightsOn(SchedulingItem item, bool isAlwaysActive)
        {
            if (item.itemID == "Lights")
            {
                isOnAnAlwaysActivePedestal = isAlwaysActive;
                mod.ModHelper.Console.WriteLine("Got Activate Lights", OWML.Common.MessageType.Success);
                lightController.FadeTo(1f, 3f);
                if (!isOnAnAlwaysActivePedestal)
                {
                    timeStamp = TimeLoop.GetSecondsElapsed();
                }
            }
        }

        public void LightsOff(SchedulingItem item, bool isAlwaysActive)
        {
            if (item.itemID == "Lights")
            {
                isOnAnAlwaysActivePedestal = false;
                mod.ModHelper.Console.WriteLine("Got Deactivate Lights", OWML.Common.MessageType.Success);
                lightController.FadeTo(0f, 3f);
            }
        }

        public void Update()
        {
            if (!isOnAnAlwaysActivePedestal && lightController.GetIntensity() == 1f && TimeLoop.GetSecondsElapsed() - timeStamp>=960f)
            {
                mod.ModHelper.Console.WriteLine("8 hours have passed, deactivating lights...", OWML.Common.MessageType.Info);
                lightController.FadeTo(0f, 3f);
            }
        }
    }
}
