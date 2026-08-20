using NewHorizons.Utility;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingArecibo : MonoBehaviour
    {
        [SerializeField]
        public GameObject tunnelCovers;

        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public AudioSignal signalSource;
        [NonSerialized]
        public float timeStamp;

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.ActivateScheduledEvent += FireSignal;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= FireSignal;
        }

        public void Start()
        {
            signalSource = SearchUtilities.Find("Disc_Body/Sector/TelescopeSignal").GetComponent<AudioSignal>();
            signalSource?.SetSignalActivation(false, 0f);
            if (tunnelCovers == null)
            {
                tunnelCovers = transform.Find("Tunnel Covers").gameObject;
            }
        }

        public void FireSignal(SchedulingItem item, bool isAlwaysActive)
        {
            if (!isAlwaysActive)
            {
                signalSource?.SetSignalActivation(true, 2f);
                tunnelCovers.SetActive(false);
                timeStamp = TimeLoop.GetSecondsElapsed();
            }
        }

        public void Update()
        {
            if (signalSource != null)
            {
                if (signalSource._active && TimeLoop.GetSecondsElapsed() - timeStamp >= 20f)
                {
                    signalSource?.SetSignalActivation(false, 2f);
                    tunnelCovers.SetActive(true);
                }
            }
        }
    }
}
