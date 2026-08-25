using NewHorizons.Utility;
using OWML.ModHelper;
using System;
using UnityEngine;

namespace Jam6
{
    public class SectorLightSwitcher : MonoBehaviour
    {
        [SerializeField]
        public Collider colliderDome;
        [SerializeField]
        public Light[] domeLights;
        [NonSerialized]
        public Light sunLight;
        [NonSerialized]
        public ModBehaviour mod;

        public void Awake()
        {
            mod = Jam6.Instance;
            foreach (Light l in domeLights)
            {
                l.enabled = false;
            }
            sunLight = SearchUtilities.Find("Sun_Body/Sector_SUN/Effects_SUN/SunLight").GetComponent<Light>();
        }

        public void OnTriggerEnter(Collider other)
        {
            mod.ModHelper.Console.WriteLine("Entered Dome", OWML.Common.MessageType.Success);
            if (other.gameObject.name == "PlayerDetector")
            {
                mod.ModHelper.Console.WriteLine("Its the player", OWML.Common.MessageType.Success);
                sunLight.enabled = false;
                foreach (Light l in domeLights)
                {
                    l.enabled = true;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            mod.ModHelper.Console.WriteLine("Exited Dome", OWML.Common.MessageType.Success);
            if (other.gameObject.name == "PlayerDetector")
            {
                mod.ModHelper.Console.WriteLine("Its the player", OWML.Common.MessageType.Success);
                sunLight.enabled = true;
                foreach (Light l in domeLights)
                {
                    l.enabled = false;
                }
            }
        }
    }
}
