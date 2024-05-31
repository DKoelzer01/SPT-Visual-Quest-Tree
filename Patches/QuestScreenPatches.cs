using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using HarmonyLib;

namespace VisualQuestTree.Patches
{
    internal class QuestScreenShowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TasksScreen), nameof(TasksScreen.Show));
        }

        [PatchPrefix]
        public static bool PatchPrefix()
        {
            try
            {
                Plugin.Log.LogInfo($"DEWEY Try Load Task Screen");
                Plugin.Instance.QuestsScreen?.OnTaskScreenShow();
                return false;
            }
            catch(Exception e)
            {
                Plugin.Log.LogError($"Caught error while trying to show Tasks Screen");
                Plugin.Log.LogError($"{e.Message}");
                Plugin.Log.LogError($"{e.StackTrace}");

                return true;
            }
        }
    }

    internal class QuestScreenClosePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TasksScreen), nameof(TasksScreen.Close));
        }

        [PatchPrefix]
        public static bool PatchPrefix()
        {
            try
            {
                Plugin.Log.LogInfo($"DEWEY Try Close Task Screen");
                Plugin.Instance.QuestsScreen?.OnTaskScreenClose();
                return false;
            }
            catch(Exception e)
            {
                Plugin.Log.LogError($"Caught error while trying to close Tasks Screen");
                Plugin.Log.LogError($"{e.Message}");
                Plugin.Log.LogError($"{e.StackTrace}");

                return true;
            }
        }
    }
}