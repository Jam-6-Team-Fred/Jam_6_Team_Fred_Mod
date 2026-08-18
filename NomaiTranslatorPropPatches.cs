// Code originally designed by Hawkbar/Hawkbat
//   Original Source:
//   https://github.com/Hawkbat/OuterWildsModJam5/blob/main/Terrarium/Patches/NomaiTranslatorPropPatches.cs

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jam6
{
    [HarmonyPatch(typeof(NomaiTranslatorProp))]
    public static class NomaiTranslatorPropPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(NomaiTranslatorProp.SetNomaiText), typeof(NomaiText), typeof(int))]
        public static void SetNomaiText(NomaiTranslatorProp __instance)
        {
            var text = __instance._textNodeToDisplay;
            if (text.Contains("$TEAMFRED_EXPO_"))
            {
                text = text.Replace("$TEAMFRED_EXPO_TIME", "Expo Hall Scheduled to open in <color=red>ERROR</color> hours");
                __instance._textNodeToDisplay = text;
            }
        }
    }
}