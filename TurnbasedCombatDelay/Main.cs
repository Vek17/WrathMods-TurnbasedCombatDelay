using HarmonyLib;
using Kingmaker.UI._ConsoleUI.CombatStartScreen;
using Kingmaker.UI.TurnBasedMode;
using Owlcat.Runtime.UniRx;
using System;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace TurnbasedCombatDelay {
    static class Main {

        public static Settings Settings;
        public static bool Enabled;
        public static ModEntry Mod;
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg) {
            Mod.Logger.Log(msg);
        }

        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            Mod = modEntry;
            Settings = Settings.Load<Settings>(modEntry);
            Mod.OnGUI = OnGUI;
            Mod.OnSaveGUI = OnSaveGUI;
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry) {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(
                string.Format("Turn Delay: {0}", Settings.Delay), GUILayout.ExpandWidth(false)
            );
            if (GUILayout.Button("0", GUILayout.ExpandWidth(false))) {
                Settings.Delay = 0;
            }
            if (GUILayout.Button("1", GUILayout.ExpandWidth(false))) {
                Settings.Delay = 1;
            }
            if (GUILayout.Button("2", GUILayout.ExpandWidth(false))) {
                Settings.Delay = 2;
            }
            if (GUILayout.Button("3", GUILayout.ExpandWidth(false))) {
                Settings.Delay = 3;
            }
            if (GUILayout.Button("4", GUILayout.ExpandWidth(false))) {
                Settings.Delay = 4;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Settings.Save(modEntry);
        }

        [HarmonyPatch(typeof(TurnBasedModeUIController), "ShowCombatStartWindow")]
        static class Difficulty_Override_Patch {

            static bool Prefix(TurnBasedModeUIController __instance) {
                if (__instance.m_CombatStartWindowVM == null) {
                    __instance.HideTurnPanel();
                    __instance.m_CombatStartWindowVM = new CombatStartWindowVM(new Action(__instance.HideCombatStartWindow));
                    __instance.m_Config.CombatStartWindowView.Bind(__instance.m_CombatStartWindowVM);
                    object p = DelayedInvoker.InvokeInTime(new Action(__instance.HideCombatStartWindow), Settings.Delay, true);
                }
                return false;
            }
        }
    }
}