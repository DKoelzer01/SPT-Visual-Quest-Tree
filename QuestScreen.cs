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

        public void OpenQuestTree()
        {
            Plugin.Log.LogInfo("Open Quest Tree");
            
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
        }

        public void CloseQuestTree() 
        {
            Plugin.Log.LogInfo("Close Quest Tree");
            // do the opposite of open
        }
        internal void OnTaskScreenShow()
        {
            transform.parent.gameObject.SetActive(true);

            return;
        }

        internal void OnTaskScreenClose()
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}