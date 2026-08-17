using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class ZeroGravity : MonoBehaviour
    {
        [SerializeField]
        public DirectionalForceVolume gravity;
        [SerializeField]
        public bool disableAligment;
        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public MeshRenderer meshRenderer;

        public void Awake()
        {
            OrbSwitch.ZeroG += TurnOffGravity;
            OrbSwitch.NormalG += TurnOnGravity;
        }
        
        public void Start()
        {
            if (gravity == null)
            {
                gravity = GetComponent<DirectionalForceVolume>();
            }
        }

        public void OnDestroy()
        {
            OrbSwitch.ZeroG -= TurnOffGravity;
            OrbSwitch.NormalG -= TurnOnGravity;
        }

        public void TurnOffGravity()
        {
            gravity.SetFieldMagnitude(0);
            if (disableAligment)
            {
                gravity._affectsAlignment = false;
            }
        }

        public void TurnOnGravity()
        {
            gravity.SetFieldMagnitude(12);
            if (disableAligment)
            {
                gravity._affectsAlignment = true;
            }
        }
    }
}
