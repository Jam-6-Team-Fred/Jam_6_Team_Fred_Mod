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
        [NonSerialized]
        public ModBehaviour mod;
        public MeshRenderer meshRenderer;
        public MeshCollider meshCollider;
        [NonSerialized]
        public GameObject signalSource;
        [NonSerialized]
        public float time;

        public void Awake()
        {
            mod = Jam6.Instance;
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
            SchedulingSocket.ActivateScheduledEvent += FireSignal;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().AddListener(FindSignal);
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= FireSignal;
        }

        public void FindSignal(string planetName)
        {
            if (planetName == "Disc_Body")
            {
                signalSource = SearchUtilities.Find("Disc_Body/Sector/AudioSource");
                signalSource?.SetActive(false);
            }
        }

        public void FireSignal(SchedulingItem item)
        {
            signalSource?.SetActive(true);
            meshRenderer?.enabled = false;
            meshCollider?.enabled = false;
            time = TimeLoop.GetSecondsElapsed();
        }

        public void Update()
        {
            if (TimeLoop.GetSecondsElapsed() - time>=20f && signalSource.activeSelf)
            {
                signalSource?.SetActive(false);
                meshRenderer?.enabled = true;
                meshCollider?.enabled = true;
            }
        }
    }
}
