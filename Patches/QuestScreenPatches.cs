using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using Comfort.Common;
using UnityEngine;
using HarmonyLib;
using VisualQuestTree.Utils;
using VisualQuestTree.UI;
using VisualQuestTree.UI.Controls;


namespace VisualQuestTree.Patches
{
    internal class QuestScreenShowPatch : ModulePatch
    {
        private static DefaultUIButton QuestTreeButton;
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TasksScreen), nameof(TasksScreen.Show));
        }

        [PatchPrefix]
        public static void Prefix(TasksScreen __instance)
        {
            Plugin.Log.LogInfo($"Task Screen Prefix");
            if(__instance.transform.Find("QuestTreeButton") == null) {
                var prefab = Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/BackButton").gameObject;
                var go = UnityEngine.Object.Instantiate(prefab,Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/Tasks Panel/QuestTreeBlock").gameObject.transform.parent,false);
                QuestTreeButton = go.GetComponent<DefaultUIButton>();

                go.name = "QuestTreeButton";
                QuestTreeButton.SetHeaderText("Quest Tree", 24);
                QuestTreeButton.SetRawText("Quest Tree",24);
                
                QuestTreeButton.Transform.position = new Vector3(1120,1355,0);

                QuestTreeButton.OnClick.AddListener(() =>
                {
                    if(QuestTreeButton.HeaderText == "Quest Tree") {
                        Plugin.Instance.QuestsScreen.OpenQuestTree();
                        QuestTreeButton.SetHeaderText("Task List", 24);
                    } 
                    else {
                        Plugin.Instance.QuestsScreen.CloseQuestTree();
                        QuestTreeButton.SetHeaderText("Quest Tree", 24);
                    }
                });
            } else {
                if(__instance.transform.Find("QuestTreeButton").gameObject.GetComponent<DefaultUIButton>().HeaderText == "Task List") {
                    Plugin.Instance.QuestsScreen.CloseQuestTree();
                }
            }
        }

        [PatchPostfix]
        public static void PatchPostfix()
        {
            try
            {
                Plugin.Instance.QuestsScreen?.OnTaskScreenShow();
            }
            catch(Exception e)
            {
                Plugin.Log.LogError($"Caught error while trying to show Tasks Screen");
                Plugin.Log.LogError($"{e.Message}");
                Plugin.Log.LogError($"{e.StackTrace}");
            }
        }
    }

    internal class QuestScreenClosePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TasksScreen), nameof(TasksScreen.Close));
        }

        [PatchPostfix]
        public static void PatchPostfix()
        {
            try
            {
                Plugin.Instance.QuestsScreen?.OnTaskScreenClose();
            }
            catch(Exception e)
            {
                Plugin.Log.LogError($"Caught error while trying to close Tasks Screen");
                Plugin.Log.LogError($"{e.Message}");
                Plugin.Log.LogError($"{e.StackTrace}");
            }
        }
    }
}