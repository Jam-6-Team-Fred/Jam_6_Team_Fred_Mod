using NewHorizons.Utility;
using System;
using UnityEngine;

namespace Jam6
{
    public class SectorLightSwitcher : MonoBehaviour
    {
        [SerializeField]
        public Sector sectorDome;
        [SerializeField]
        public Light[] domeLights;
        [NonSerialized]
        public Light sunLight;

        public void Awake()
        {
            foreach (Light l in domeLights)
            {
                l.enabled = false;
            }
            sunLight = SearchUtilities.Find("Sun_Body/Sector_SUN/Effects_SUN/SunLight").GetComponent<Light>();
            sectorDome.OnOccupantEnterSector.AddListener(EnterLightSwitch);
            sectorDome.OnOccupantEnterSector.AddListener(ExitLightSwitch);
        }

        public void OnDestroy()
        {
            sectorDome.OnOccupantEnterSector.RemoveListener(EnterLightSwitch);
            sectorDome.OnOccupantEnterSector.RemoveListener(ExitLightSwitch);
        }

        public void EnterLightSwitch(SectorDetector sectorDetector)
        {
            if (sectorDetector.GetOccupantType() == DynamicOccupant.Player)
            {
                sunLight.enabled = false;
                foreach (Light l in domeLights)
                {
                    l.enabled = true;
                }
            }
        }

        public void ExitLightSwitch(SectorDetector sectorDetector)
        {
            if (sectorDetector.GetOccupantType() == DynamicOccupant.Player)
            {
                sunLight.enabled = true;
                foreach (Light l in domeLights)
                {
                    l.enabled = false;
                }
            }
        }
    }
}
