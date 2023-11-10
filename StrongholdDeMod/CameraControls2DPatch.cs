using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using HarmonyLib.Tools;
using MelonLoader;
using static MelonLoader.MelonLogger;

namespace StrongholdDeMod
{
    [HarmonyPatch(typeof(CameraControls2D), "Update")]
    public static class UpdatePatch
    {
        private static void Prefix(ref CameraControls2D __instance)
        {
            Type CameraControls2DType = typeof(CameraControls2D);
            FieldInfo zoomDelayField = CameraControls2DType.GetField("zoomDelay", BindingFlags.NonPublic | BindingFlags.Instance);
            zoomDelayField.SetValue(__instance, DateTime.MinValue);
        }

        private static void Postfix(ref CameraControls2D __instance)
        {
            Type CameraControls2DType = typeof(CameraControls2D);
            FieldInfo zoomDelayField = CameraControls2DType.GetField("zoomDelay", BindingFlags.NonPublic | BindingFlags.Instance);
            zoomDelayField.SetValue(__instance, DateTime.MinValue);
        }
    }

    [HarmonyPatch(typeof(CameraControls2D), "isMapLocked")]
    public static class isMapLockedPatch
    {
        private static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(CameraControls2D), "debarZoom")]
    public static class debarZoomPatch
    {

        private static void Postfix(ref CameraControls2D __instance)
        {
            __instance.AllowZoom = true;
        }
    }
}
