using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using System.Runtime.CompilerServices;

namespace StrongholdDeMod
{
    public class StrongholdDeModMain : MelonMod
    {
        private MelonPreferences_Category category;
        private MelonPreferences_Entry<float> zoomSensibilityEntry;
        private MelonPreferences_Entry<int> minZoomEntry;
        private MelonPreferences_Entry<int> maxZoomEntry;

        private static StrongholdDeModMain Instance { get; set; }

        public override void OnInitializeMelon()
        {
            category = MelonPreferences.CreateCategory("Preferences");
            zoomSensibilityEntry = category.CreateEntry<float>("Zoom sensibility", 0.1f);

            minZoomEntry = category.CreateEntry<int>("Min zoom", 0);
            maxZoomEntry = category.CreateEntry<int>("Max zoom", 4);

            Instance = this;
        }
        public static StrongholdDeModMain GetInstance()
        {
            return Instance;
        }

        public static float GetZoomSensibility()
        {
            return Instance.zoomSensibilityEntry.Value;
        }

        public static int GetMinZoom()
        {
            return Instance.minZoomEntry.Value;
        }

        public static int GetMaxZoom()
        {
            return Instance.maxZoomEntry.Value;
        }

        public override void OnUpdate()
        {

        }
    }
}
