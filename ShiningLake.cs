using NewHorizons.Utility;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class ShiningLake : MonoBehaviour
    {
        [SerializeField]
        public GameObject spotLight;
        [SerializeField]
        public GameObject lakeSurface;
        [Space]
        [SerializeField]
        public Color normalColor = new Color(61, 84, 81);
        [SerializeField]
        public Color shiningColor = new Color(66, 183, 167);
        [SerializeField]
        public float durationToBlue = 10f;
        [Space]
        [SerializeField]
        public float durationToShine = 5f;
        [SerializeField]
        public float endSpotLightIntensity = 5;

        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public Material lakeMaterial;
        [NonSerialized]
        public Light spotLightLight;
        [NonSerialized]
        public bool didItBlue;
        [NonSerialized]
        public bool didItUnBlue;
        [NonSerialized]
        public bool didItShine;
        [NonSerialized]
        public bool didItUnShine;
        [NonSerialized]
        public float currentTime;
        [NonSerialized]
        public float startTime;
        [NonSerialized]
        public Color currentColor;

        //float num = Mathf.InverseLerp(_fadeStartTime, _fadeStartTime + _fadeDuration, Time.time);
        //_intensity = Mathf.Lerp(_fadeStartIntensity, _fadeTargetIntensity, Mathf.SmoothStep(0f, 1f, num));

        public void Awake()
        {
            mod = Jam6.Instance;
            //spotLight = SearchUtilities.Find("Disc_Body/Sector/Disc/Past/Lake/Lake Spotlight");
        }

        public void Start()
        {
            lakeMaterial = lakeSurface.GetComponent<MeshRenderer>().material;
            spotLightLight = spotLight.GetComponent<Light>();
            lakeMaterial.SetColor("_FogColor", normalColor);
            spotLightLight.intensity = 0;
        }

        public void Update()
        {
            currentTime = TimeLoop.GetSecondsElapsed();
            if (!didItBlue && currentTime >= 660f)
            {
                didItBlue = true;
                startTime = currentTime;
                mod.ModHelper.Console.WriteLine($"Am i blue {didItBlue}", OWML.Common.MessageType.Success);
            }
            if (didItBlue && currentTime <= 660f + durationToBlue)
            {
                UpdateColor(normalColor, shiningColor);
            }
            if (!didItShine && currentTime >= 720f)
            {
                didItShine = true;
                startTime = currentTime;
                mod.ModHelper.Console.WriteLine($"Am i shining {didItShine}", OWML.Common.MessageType.Success);
            }
            if (didItShine && currentTime <= 720f + durationToShine)
            {
                UpdateSpotLightIntensity(0, endSpotLightIntensity);
            }
            if (didItShine && currentTime >= 780f)
            {
                didItUnShine = true;
                startTime = currentTime;
                mod.ModHelper.Console.WriteLine($"Am i blue {didItBlue}", OWML.Common.MessageType.Success);
            }
            if (didItUnShine && currentTime <= 780f+durationToShine)
            {
                UpdateSpotLightIntensity(endSpotLightIntensity, 0);
            }
            if (didItBlue && currentTime >= 840f)
            {
                didItUnBlue = true;
                startTime = currentTime;
                mod.ModHelper.Console.WriteLine($"Am i shining {didItShine}", OWML.Common.MessageType.Success);
            }
            if (didItUnBlue && currentTime <= 840f + durationToBlue)
            {
                UpdateColor(shiningColor, normalColor);

            }
        }

        public void UpdateColor(Color fromColor, Color toColor)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToBlue, Time.time);

            //R
            currentColor.r = Mathf.Lerp(fromColor.r, toColor.r, Mathf.SmoothStep(0f, 1f, num));

            //G
            currentColor.g = Mathf.Lerp(fromColor.g, toColor.g, Mathf.SmoothStep(0f, 1f, num));

            //B
            currentColor.b = Mathf.Lerp(fromColor.b, toColor.b, Mathf.SmoothStep(0f, 1f, num));

            //Applying the whole color
            lakeMaterial.SetColor("_FogColor", currentColor);
        }

        public void UpdateSpotLightIntensity(float fromIntensity, float toIntensity)
        {
            float num2 = Mathf.InverseLerp(startTime, startTime + durationToShine, Time.time);
            spotLightLight.intensity = Mathf.Lerp(fromIntensity, toIntensity, Mathf.SmoothStep(0f, 1f, num2));
        }
    }
}
