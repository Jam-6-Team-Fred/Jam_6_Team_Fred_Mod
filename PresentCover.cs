using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class PresentCover : MonoBehaviour
    {
        [SerializeField]
        public MeshRenderer meshRenderer;
        [SerializeField]
        public MeshCollider meshCollider;

        [NonSerialized]
        public ModBehaviour mod;

        public void Awake()
        {
            mod = Jam6.Instance;
            PresentSwitch.OpenPresent += Disappear;
            PresentSwitch.ClosePresent += Appear;
        }

        public void Start()
        {
            if (meshCollider == null)
            {
                meshCollider = GetComponent<MeshCollider>();
            }
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void OnDestroy()
        {
            PresentSwitch.OpenPresent -= Disappear;
            PresentSwitch.ClosePresent -= Appear;
        }

        public void Disappear()
        {
            mod.ModHelper.Console.WriteLine("Got Open Present", OWML.Common.MessageType.Success);
            meshRenderer.enabled = false;
            meshCollider.enabled = false;
        }

        public void Appear()
        {
            mod.ModHelper.Console.WriteLine("Got Close Present", OWML.Common.MessageType.Success);
            meshRenderer.enabled = true;
            meshCollider.enabled = true;
        }
    }
}
