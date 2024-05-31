using System;
using EFT.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VisualQuestTree.Utils;

namespace VisualQuestTree.UI
{
    public class QuestTreeScreen : MonoBehaviour
    {
        internal static QuestTreeScreen Create(GameObject parent)
        {
            Plugin.Log.LogInfo("Create QuestTreeScreen");
            var go = new GameObject("QuestTreeBlock", typeof(RectTransform));
            go.layer = parent.layer;
            go.transform.SetParent(parent.transform);
            go.ResetRectTransform();
            Plugin.Log.LogInfo("Finish QuestTreeScreen");
            return go.AddComponent<QuestTreeScreen>();
        }
        internal void OnTaskScreenShow()
        {
            // transform.parent.Find("MapBlock").gameObject.SetActive(false);
            // transform.parent.Find("EmptyBlock").gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(true);

            Plugin.Log.LogInfo($"OpenTaskScreen");

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

            //TODO: Kill UI for quest tree

        }
    }
}