using System;
using EFT.UI;
using UnityEngine;
using UnityEngine.UI;
using VisualQuestTree.Utils;

namespace VisualQuestTree.UI
{
    public class QuestTreeScreen : MonoBehaviour
    {
        internal static QuestTreeScreen Create(GameObject parent)
        {
            var go = new GameObject("QuestTreeBlock", typeof(RectTransform));
            go.layer = parent.layer;
            go.transform.SetParent(parent.transform);
            go.ResetRectTransform();
            return go.AddComponent<QuestTreeScreen>();
        }
        internal void OnTaskScreenShow()
        {
            // transform.parent.Find("MapBlock").gameObject.SetActive(false);
            // transform.parent.Find("EmptyBlock").gameObject.SetActive(false);
            // transform.parent.gameObject.SetActive(true);

            Plugin.Log.LogInfo($"OpenTaskScreen");
            //Show();
        }

        internal void OnTaskScreenClose()
        {
            Plugin.Log.LogInfo($"CloseTaskScreen");
            //Hide();
        }
    }
}