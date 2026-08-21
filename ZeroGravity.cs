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
        [Space]
        [SerializeField]
        public DirectionalForceVolume gravity;
        [SerializeField]
        public bool disableAligment;
        [Space]
        [SerializeField]
        public float normalMagnitude = 18f;
        [SerializeField]
        public float lowMagnitude = 3.6f;

        [NonSerialized]
        public ModBehaviour mod;

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
            gravity.SetFieldMagnitude(normalMagnitude);
        }

        public void OnDestroy()
        {
            OrbSwitch.ZeroG -= TurnOffGravity;
            OrbSwitch.NormalG -= TurnOnGravity;
        }

        public void TurnOffGravity()
        {
            gravity.SetFieldMagnitude(lowMagnitude);
        }

        public void TurnOnGravity()
        {
            gravity.SetFieldMagnitude(normalMagnitude);
        }
    }
}
