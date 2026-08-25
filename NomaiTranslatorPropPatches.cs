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
                string amountOfHoursFormatted = "";
                float currentSeconds = TimeLoop.GetSecondsElapsed();
                int currentHour = (int)currentSeconds / 120;
                int amountOfHours = 0;
                if (Jam6.Instance.expoHallOpeningHour != null)
                {
                    amountOfHours = (int)Jam6.Instance.expoHallOpeningHour - currentHour;
                    if (amountOfHours < 0)
                    {
                        amountOfHoursFormatted = $"<color=cyan>{-amountOfHours}</color>";
                        text = text.Replace("$TEAMFRED_EXPO_TIME", Jam6.Instance.NewHorizons.GetTranslationForOtherText("$TEAMFRED.ExpoHall_Past").Replace("{amountOfHoursHormatted}", amountOfHoursFormatted));
                    }
                    else if (amountOfHours > 0)
                    {
                        amountOfHoursFormatted = $"<color=cyan>{amountOfHours}</color>";
                        text = text.Replace("$TEAMFRED_EXPO_TIME", Jam6.Instance.NewHorizons.GetTranslationForOtherText("$TEAMFRED.ExpoHall_Future").Replace("{amountOfHoursHormatted}", amountOfHoursFormatted));
                    }
                    else
                    {
                        text = text.Replace("$TEAMFRED_EXPO_TIME", Jam6.Instance.NewHorizons.GetTranslationForOtherText("$TEAMFRED.ExpoHall_Present").Replace("{amountOfHoursHormatted}", amountOfHoursFormatted));
                    }
                }
                else
                {
                    amountOfHoursFormatted = Jam6.Instance.NewHorizons.GetTranslationForOtherText("$TEAMFRED.ExpoHall_Error");
                }
                __instance._textNodeToDisplay = text;
            }
        }
    }
}