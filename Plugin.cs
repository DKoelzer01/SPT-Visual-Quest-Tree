using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using VisualQuestTree.UI;
using VisualQuestTree.Patches;
using EFT.UI;

namespace VisualQuestTree
{
    [BepInPlugin("com.dewdongers.visualquesttree", "Visual Quest Tree", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public static ManualLogSource Log => Instance.Logger;
        public QuestTreeScreen Quests;
        void Awake()
        {
            Instance = this;
            Logger.LogInfo($"Visual Quest Tree loaded.");

            new QuestScreenShowPatch().Enable();
            new QuestScreenClosePatch().Enable();
        }    

        internal void TryAttachToTasksScreen(TasksScreen tasksScreen) 
        {
            Logger.LogInfo($"TryAttach");
            if (Quests != null)
            {
                return;
            }

            Log.LogInfo("Trying to attach to TasksScreen");

            // attach to common UI first to call awake and set things up, then attach to sleeping tasks screen
            Quests = QuestTreeScreen.Create(Singleton<CommonUI>.Instance.gameObject);
            Quests.transform.SetParent(tasksScreen.transform);
        }
    }
}