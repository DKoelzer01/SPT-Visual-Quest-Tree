using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using EFT.UI.Map;
using HarmonyLib;

namespace VisualQuestTree.Patches
{
    internal class CommonUIAwakePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(CommonUI), nameof(CommonUI.Awake));
        }

        [PatchPostfix]
        public static void PatchPostfix(CommonUI __instance)
        {
            var tasksScreen = Traverse.Create(__instance.InventoryScreen).Field("_tasksScreen").GetValue<TasksScreen>();

            Plugin.Instance.TryAttachToTasksScreen(tasksScreen);
        }
    }
}