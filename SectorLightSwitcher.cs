using NewHorizons.Utility;
using OWML.ModHelper;
using System;
using UnityEngine;

namespace Jam6
{
    public class SectorLightSwitcher : Sector
    {
        [SerializeField]
        public Sector sectorDome;
        [SerializeField]
        public Light[] domeLights;
        [NonSerialized]
        public Light sunLight;
        [NonSerialized]
        public ModBehaviour mod;

        public override void Awake()
        {
            base.Awake();
            mod = Jam6.Instance;
            foreach (Light l in domeLights)
            {
                l.enabled = false;
            }
            sunLight = SearchUtilities.Find("Sun_Body/Sector_SUN/Effects_SUN/SunLight").GetComponent<Light>();
            _owTriggerVolume.OnEntry += EnterLightSwitch;
            _owTriggerVolume.OnExit += ExitLightSwitch;
        }

        public void OnDestroy()
        {
            _owTriggerVolume.OnEntry -= EnterLightSwitch;
            _owTriggerVolume.OnExit -= ExitLightSwitch;
        }

        public void EnterLightSwitch(GameObject hitObj)
        {
            SectorDetector component = hitObj.GetComponent<SectorDetector>();
            mod.ModHelper.Console.WriteLine("Entered Dome", OWML.Common.MessageType.Success);
            if (component.GetOccupantType() == DynamicOccupant.Player)
            {
                mod.ModHelper.Console.WriteLine("Its the player", OWML.Common.MessageType.Success);
                sunLight.enabled = false;
                foreach (Light l in domeLights)
                {
                    l.enabled = true;
                }
            }
        }

        public void ExitLightSwitch(GameObject hitObj)
        {
            SectorDetector component = hitObj.GetComponent<SectorDetector>();
            mod.ModHelper.Console.WriteLine("Exited Dome", OWML.Common.MessageType.Success);
            if (component.GetOccupantType() == DynamicOccupant.Player)
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
