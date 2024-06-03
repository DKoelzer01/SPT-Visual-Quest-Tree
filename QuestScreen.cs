using System;
using EFT;
using EFT.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Comfort.Common;
using VisualQuestTree.Utils;
using VisualQuestTree.UI.Controls;

namespace VisualQuestTree.UI
{
    public class QuestTreeScreen : MonoBehaviour
    {
        internal static QuestTreeScreen Create(GameObject parent)
        {
            var go = UIUtils.CreateUIGameObject(parent,"QuestTreeBlock");
            return go.AddComponent<QuestTreeScreen>();
        }

        private void Awake() 
        {
            Plugin.Log.LogInfo("Create QuestTreeToggle");
            // var QuestTreeToggle = UIUtils.CreateUIGameObject(this.gameObject,"QuestTreeToggle");
            // var QuestTreeToggleText = TextBox.Create(QuestTreeToggle, 12);
            // QuestTreeToggle.transform.SetParent(this.transform);
            // QuestTreeToggleText.SetText("QUEST TREE");
            // QuestTreeToggle.transform.localPosition = new Vector3(1280,1350,0);


        }

        /*  Toggle the following items:
                WhiteBackground
                OverallBackground
                QuestItemsToggleGroup
                NotesImage
                QuestItemsPanel
                NotesPart
                TasksPanel
                NoteWindow
        */

        internal void OpenQuestTree()
        {
            Plugin.Log.LogInfo("Open Quest Tree");
        }

        internal void CloseQuestTree() 
        {
            Plugin.Log.LogInfo("Close Quest Tree");
            // do the opposite of open
        }
        internal void OnTaskScreenShow()
        {
            Plugin.Log.LogInfo($"OpenTaskScreen");

            transform.parent.gameObject.SetActive(true);

            /*TODO: Draw UI for quest tree
                    Button for entering quest tree view;
                        Persist between screen switches
                        Button to return to normal view
                    Disable Other task screen ui elements - Common UI -> Inventory Screen -> Tasks Panel -> Disable Everything
                    Create prefab for quest box
                        Instantiate prefabs for each quest
                    Show all quests
                    Filter quests by Trader.
            */
            return;
        }

        internal void OnTaskScreenClose()
        {
            Plugin.Log.LogInfo($"CloseTaskScreen");

            transform.parent.gameObject.SetActive(false);
        }
    }
}