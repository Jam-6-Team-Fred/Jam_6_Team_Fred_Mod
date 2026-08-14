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
        [NonSerialized]
        public MeshRenderer meshRenderer;
        [NonSerialized]
        public MeshCollider meshCollider;

        public void Awake()
        {
            mod = Jam6.Instance;
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
            SchedulingSocket.ActivateScheduledEvent += FireSignal;
        }

        public void OnDestroy()
        {
            SchedulingSocket.ActivateScheduledEvent -= FireSignal;
        }

        public void FireSignal(SchedulingItem item)
        {
            return; 
        }
    }
}
