using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using HarmonyLib.Tools;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace StrongholdDeMod
{
    [HarmonyPatch(typeof(PerfectPixelWithZoom), "Start")]
    public static class StartPatch
    {
        static bool Prefix(ref PerfectPixelWithZoom __instance)
        {
            Type PerfectPixelWithZoomType = typeof(PerfectPixelWithZoom);
            FieldInfo zoomScaleMinField = PerfectPixelWithZoomType.GetField("zoomScaleMin", BindingFlags.NonPublic | BindingFlags.Instance);
            zoomScaleMinField.SetValue(__instance, float.MinValue);

            return true;
        }

        static void Postfix(ref PerfectPixelWithZoom __instance)
        {
            MelonLogger.Msg("Postfix Start");

            __instance.SetPixelsPerUnit(64);
            __instance.setCamaraToStartSize();
            __instance.returnCamaraToCurrentSize();
        }
    }


    [HarmonyPatch(typeof(PerfectPixelWithZoom), "CanUserExtraZoom")]
    public static class CanUserExtraZoomPatch
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(PerfectPixelWithZoom), "capZoom")]
    public static class capZoomPatch
    {
        static bool Prefix()
        {
            return false;
        }
    }


    [HarmonyPatch(typeof(PerfectPixelWithZoom), "adjustZoom", new Type[] { typeof(int), typeof(bool) })]
    public static class adjustZoomPatch
    {

        static bool Prefix(ref PerfectPixelWithZoom __instance, ref int adjustment, ref bool loop)
        {

            Type PerfectPixelWithZoomType = typeof(PerfectPixelWithZoom);

            // Get zoomPos and increment by adjustement
            FieldInfo zoomPosField = PerfectPixelWithZoomType.GetField("zoomPos", BindingFlags.NonPublic | BindingFlags.Instance);
            int currentZoomPos = (int)zoomPosField.GetValue(__instance);
            int newZoomPos = currentZoomPos + adjustment;

            float sensibility = StrongholdDeModMain.GetZoomSensibility();
            int minZoom = StrongholdDeModMain.GetMinZoom();
            int maxZoom = StrongholdDeModMain.GetMaxZoom();
            float pixelsPerUnitScale = Mathf.Max(Mathf.Min(newZoomPos * sensibility, Int32.MaxValue), Int32.MinValue);
            if (pixelsPerUnitScale >= maxZoom || pixelsPerUnitScale <= minZoom)
            {
                MelonLogger.Msg($"[adjustZoom] pixelsPerUnitScale will be unplayable, do not zoom.");
                return false;
            }

            MelonLogger.Msg($"[adjustZoom] Setting zoomPos to {newZoomPos}");
            zoomPosField.SetValue(__instance, newZoomPos);

            MelonLogger.Msg($"[adjustZoom] Calling Zoom with argument {newZoomPos}");
            MethodInfo ZoomMethod = PerfectPixelWithZoomType.GetMethod("Zoom", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(int) }, null);
            ZoomMethod.Invoke(__instance, new object[] { newZoomPos });

            return false;
        }
    }

    
    [HarmonyPatch(typeof(PerfectPixelWithZoom), "Zoom", new Type[] { typeof(int) } )]
    public static class ZoomPatch
    {
        static bool Prefix(ref int zoomTo, ref PerfectPixelWithZoom __instance)
        {
            MelonLogger.Msg($"[Zoom] method called with zoomTo {zoomTo}");

            Type PerfectPixelWithZoomType = typeof(PerfectPixelWithZoom);

            // This should not even be called if pixelsPerUnit is too low/high but we check anyway
            float sensibility = StrongholdDeModMain.GetZoomSensibility();
            float zoomToRealValue = zoomTo * sensibility;
            int minZoom = StrongholdDeModMain.GetMinZoom();
            int maxZoom = StrongholdDeModMain.GetMaxZoom();
            float pixelsPerUnitScale = Mathf.Max(Mathf.Min(zoomToRealValue, float.MaxValue), float.MinValue);
            if (pixelsPerUnitScale >= maxZoom || pixelsPerUnitScale <= minZoom)
            {
                MelonLogger.Msg($"[Zoom] pixelsPerUnitScale will be unplayable, do not zoom. ({pixelsPerUnitScale})");
                return false;
            }

            MelonLogger.Msg($"[Zoom] Calling SetZoomImmediate");
            MethodInfo SetZoomImmediateMethod = PerfectPixelWithZoomType.GetMethod("SetZoomImmediate", BindingFlags.Public | BindingFlags.Instance);
            SetZoomImmediateMethod.Invoke(__instance, new object[] { zoomToRealValue });
            MelonLogger.Msg($"[Zoom] SetZoomImmediate called");


            return false;
        }
    }

    [HarmonyPatch(typeof(PerfectPixelWithZoom), "getMaxWidthZoom")]
    public static class getMaxWidthZoomPatch
    {

        static bool Prefix(ref float __result)
        {
            __result = float.MaxValue;
            return false;
        }
    }

    [HarmonyPatch(typeof(PerfectPixelWithZoom), "getMaxHeightZoom")]
    public static class getMaxHeightZoomPatch
    {

        static bool Prefix(ref float __result)
        {
            __result = float.MaxValue;
            return false;
        }
    }
}
