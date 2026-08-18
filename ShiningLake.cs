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


        public float hourAmount = 1;

        public void Awake()
        {
            mod = Jam6.Instance;
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
            if (!didItBlue && currentTime >= hourAmount * 120f)
            {
                didItBlue = true;
                startTime = currentTime;
            }
            if (didItBlue && currentTime <= hourAmount * 120f + durationToBlue)
            {
                //mod.ModHelper.Console.WriteLine("Trying to change color", OWML.Common.MessageType.Info);
                UpdateColor(normalColor, shiningColor);
            }
            if (!didItShine && currentTime >= (hourAmount+1)*120f)
            {
                didItShine = true;
                startTime = currentTime;
            }
            if (didItShine && currentTime <= (hourAmount + 1) * 120f + durationToShine)
            {
                UpdateSpotLightIntensity(0, endSpotLightIntensity);
            }
            if (!didItUnShine && currentTime >= (hourAmount + 2) * 120f)
            {
                didItUnShine = true;
                startTime = currentTime;
            }
            if (didItUnShine && currentTime <= (hourAmount + 2) * 120f + durationToShine)
            {
                UpdateSpotLightIntensity(endSpotLightIntensity, 0);
            }
            if (!didItUnBlue && currentTime >= (hourAmount + 3) * 120f)
            {
                didItUnBlue = true;
                startTime = currentTime;
            }
            if (didItUnBlue && currentTime <= (hourAmount + 3) * 120f + durationToBlue)
            {
                UpdateColor(shiningColor, normalColor);

            }
        }

        public void UpdateColor(Color fromColor, Color toColor)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToBlue, currentTime);

            //I can apparently Lerp the whole color??? Hello???
            currentColor = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, num));

            mod.ModHelper.Console.WriteLine($"Current color: {currentColor}");

            //Applying the whole color
            lakeMaterial.SetColor("_FogColor", currentColor);
        }

        public void UpdateSpotLightIntensity(float fromIntensity, float toIntensity)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToShine, currentTime);

            //The Lerp
            spotLightLight.intensity = Mathf.Lerp(fromIntensity, toIntensity, Mathf.SmoothStep(0f, 1f, num));
            mod.ModHelper.Console.WriteLine($"Current intensity: {spotLightLight.intensity}");
        }
    }
}
