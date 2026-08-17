using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam6
{
    [HarmonyPatch(typeof(JetpackThrusterModel))]
    public static class JetpackThrusterModelPatches
    {
        [HarmonyPrefix, HarmonyPatch(nameof(JetpackThrusterModel.OnBreakAlignment))]
        public static bool OnBreakAlignment(JetpackThrusterModel __instance)
        {
            __instance._manualAngularVelocity = Vector3.zero;
            __instance._boostActivated = false;
            RumbleManager.StopJetpackBoost();
            return false;
        }
    }
}