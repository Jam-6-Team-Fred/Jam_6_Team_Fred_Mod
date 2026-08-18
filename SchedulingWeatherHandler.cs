using NewHorizons.Utility;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingWeatherHandler : MonoBehaviour
    {
        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public GameObject weatherHandler;
        [NonSerialized]
        public GameObject rainZeroG;
        [NonSerialized]
        public GameObject rainNormalG;
        [NonSerialized]
        public GameObject snowZeroG;
        [NonSerialized]
        public GameObject snowNormalG;
        [NonSerialized]
        public float timeStamp;
        [NonSerialized]
        public bool isOnAnAlwaysActivePedestal;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += Activate;
            SchedulingSocket.DeactivateScheduledEvent += Deactivate;
            OrbSwitch.ZeroG += ZeroGWeatherHandler;
            OrbSwitch.NormalG += NormalGWeatherHandler;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().AddListener(FindWeather);
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= Activate;
            SchedulingSocket.DeactivateScheduledEvent -= Deactivate;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().RemoveListener(FindWeather);
        }

        public void Start()
        {
            rainZeroG = weatherHandler.transform.Find("Atmosphere_Rain_0g").gameObject;
            rainNormalG = weatherHandler.transform.Find("Atmosphere_Rain").gameObject;
            snowZeroG = weatherHandler.transform.Find("Atmosphere_Snow_0g").gameObject;
            snowNormalG = weatherHandler.transform.Find("Atmosphere_Snow").gameObject;
        }

        public void FindWeather(string planetName)
        {
            if (planetName == "Disc_Body")
            {
                weatherHandler = SearchUtilities.Find("Disc_Body/Sector/Atmosphere_Holder");
                weatherHandler?.SetActive(false);
            }
        }

        public void Activate(SchedulingItem item, bool isAlwaysActive)
        {
            if (item.itemID == "WeatherHandler")
            {
                isOnAnAlwaysActivePedestal = isAlwaysActive;
                mod.ModHelper.Console.WriteLine("Got Activate WeatherHandler", OWML.Common.MessageType.Success);
                weatherHandler?.SetActive(true);
                if (!isOnAnAlwaysActivePedestal)
                {
                    timeStamp = TimeLoop.GetSecondsElapsed();
                }
            }
        }

        public void Deactivate(SchedulingItem item, bool isAlwaysActive)
        {
            if (item.itemID == "WeatherHandler")
            {
                isOnAnAlwaysActivePedestal = false;
                mod.ModHelper.Console.WriteLine("Got Deactivate WeatherHandler", OWML.Common.MessageType.Success);
                weatherHandler?.SetActive(false);
            }
        }

        public void ZeroGWeatherHandler()
        {
            rainNormalG?.SetActive(false);
            rainZeroG?.SetActive(true);
            snowNormalG?.SetActive(false);
            snowZeroG?.SetActive(true);
        }

        public void NormalGWeatherHandler()
        {
            rainNormalG?.SetActive(true);
            rainZeroG?.SetActive(false);
            snowNormalG?.SetActive(true);
            snowZeroG?.SetActive(false);
        }

        public void Update()
        {
            if (!isOnAnAlwaysActivePedestal && weatherHandler.activeSelf && TimeLoop.GetSecondsElapsed() - timeStamp >= 360f)
            {
                weatherHandler?.SetActive(false);
            }
        }
    }
}
