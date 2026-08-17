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
        public MeshRenderer meshRenderer;
        [SerializeField]
        public MeshCollider meshCollider;

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
            if (meshCollider == null)
            {
                meshCollider = GetComponent<MeshCollider>();
            }
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void FireSignal(SchedulingItem item, bool isAlwaysActive)
        {
            if (!isAlwaysActive)
            {
                signalSource?.SetSignalActivation(true, 2f);
                meshRenderer?.enabled = false;
                meshCollider?.enabled = false;
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
                    meshRenderer?.enabled = true;
                    meshCollider?.enabled = true;
                }
            }
        }
    }
}
