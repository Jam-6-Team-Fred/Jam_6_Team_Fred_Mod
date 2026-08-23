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
        public Material lakeSurfaceMaterial;
        [SerializeField]
        public Material lakeFogMaterial;
        [SerializeField]
        public Material[] volumetricLightsMaterials;
        [Space]
        [SerializeField]
        public Color normalSurfaceColor = new Color(61, 84, 81);
        [SerializeField]
        public Color shiningSurfaceColor = new Color(66, 183, 167);
        [Space]
        [SerializeField]
        public Color normalFogColor = new Color(24, 33, 32);
        [SerializeField]
        public Color shiningFogColor = new Color(48, 133, 121);
        [Space]
        [SerializeField]
        public float durationToBlue = 10f;
        [SerializeField]
        public float durationToShine = 5f;
        [SerializeField]
        public float endSpotLightIntensity = 5;
        [SerializeField]
        public int endVolumetricLightMaterialAlpha = 40;

        [NonSerialized]
        public ModBehaviour mod;
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
        public Color currentSurfaceColor;
        [NonSerialized]
        public Color currentFogColor;
        [NonSerialized]
        public float currentAlpha;
        [NonSerialized]
        public float materialAlpha;


        public float hourAmount = 5.5f;

        public void Awake()
        {
            mod = Jam6.Instance;
        }

        public void Start()
        {
            spotLightLight = spotLight.GetComponent<Light>();
            lakeSurfaceMaterial.SetColor("_FogColor", normalSurfaceColor);
            spotLightLight.intensity = 0;
            SetVolMaterialsAlpha(0);
            materialAlpha = (float)endVolumetricLightMaterialAlpha/255;
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
                UpdateSurfaceColor(normalSurfaceColor, shiningSurfaceColor);
            }
            if (!didItShine && currentTime >= (hourAmount+1)*120f)
            {
                didItShine = true;
                startTime = currentTime;
            }
            if (didItShine && currentTime <= (hourAmount + 1) * 120f + durationToShine)
            {
                UpdateLightShine(0, endSpotLightIntensity, 0, materialAlpha);
            }
            if (!didItUnShine && currentTime >= (hourAmount + 2) * 120f)
            {
                didItUnShine = true;
                startTime = currentTime;
            }
            if (didItUnShine && currentTime <= (hourAmount + 2) * 120f + durationToShine)
            {
                UpdateLightShine(endSpotLightIntensity, 0, materialAlpha, 0);
            }
            if (!didItUnBlue && currentTime >= (hourAmount + 3) * 120f)
            {
                didItUnBlue = true;
                startTime = currentTime;
            }
            if (didItUnBlue && currentTime <= (hourAmount + 3) * 120f + durationToBlue)
            {
                UpdateSurfaceColor(shiningSurfaceColor, normalSurfaceColor);

            }
        }

        public void UpdateSurfaceColor(Color fromColor, Color toColor)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToBlue, currentTime);

            //I can apparently Lerp the whole color??? Hello???
            currentSurfaceColor = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, num));

            //Applying the color
            lakeSurfaceMaterial.SetColor("_FogColor", currentSurfaceColor);
        }

        public void UpdateFogColor(Color fromColor, Color toColor)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToBlue, currentTime);

            //I can apparently Lerp the whole color??? Hello???
            currentFogColor = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, num));

            //Applying the color
            lakeSurfaceMaterial.SetColor("_FogColor", currentFogColor);
        }

        public void UpdateLightShine(float fromIntensity, float toIntensity, float fromAlpha, float toAlpha)
        {
            //Funny smooooth curve thing
            float num = Mathf.InverseLerp(startTime, startTime + durationToShine, currentTime);
            float smoothStep = Mathf.SmoothStep(0f, 1f, num);

            //The Lerps
            spotLightLight.intensity = Mathf.Lerp(fromIntensity, toIntensity, smoothStep);
            currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, smoothStep);

            SetVolMaterialsAlpha(currentAlpha);
        }

        public void SetVolMaterialsAlpha(float alpha)
        {
            foreach (Material volLightMat in volumetricLightsMaterials)
            {
                volLightMat.SetAlpha(alpha);
            }
        }
    }
}
