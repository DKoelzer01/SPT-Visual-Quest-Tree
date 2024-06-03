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
            go.SetActive(false);
            return go.AddComponent<QuestTreeScreen>();

            /*TODO: Draw UI for quest tree
                    Create scrollable / zoomable screen
                    Create prefab for quest box
                        Instantiate prefabs for each quest
                            Show all quests
                            Filter quests by Trader.
                        Show quest info when clicked?
            */
        }

        public void OpenQuestTree()
        {
            Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/Tab Bar/Tabs").gameObject.SetActive(false);
            Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/BackButton").gameObject.SetActive(false);
            foreach(Transform child in this.transform.parent) 
            {
                switch(child.name)
                {   //Disable all children except QuestTreeBlock, ignore QuestTreeButton as it is how you return from QuestTreeBlock
                    case("QuestTreeBlock"):
                    child.gameObject.SetActive(true);
                    break;
                    case("QuestTreeButton"):
                    break;
                    default:
                    child.gameObject.SetActive(false);
                    break;
                }
            }
        }

        public void CloseQuestTree() 
        {
            Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/Tab Bar/Tabs").gameObject.SetActive(true);
            Singleton<CommonUI>.Instance.transform.Find("Common UI/InventoryScreen/BackButton").gameObject.SetActive(true);
            foreach(Transform child in this.transform.parent) 
            {
            switch(child.name)
                {   //Enable all children except QuestTreeBlock, Ignore QuestTreeButton,NotesImage,ExitButton, and NoteWindow. Set the Quest Items / Notes toggle to Quest Items.
                    case("QuestTreeBlock"):
                    child.gameObject.SetActive(false);
                    break;
                    case("QuestItemsToggleGroup"):
                    child.gameObject.transform.GetChild(0).GetComponent<UIAnimatedToggleSpawner>().Boolean_0 = false;
                    child.gameObject.SetActive(true);
                    break;
                    case("QuestTreeButton"):
                    case("NotesImage"):
                    case("ExitButton"):
                    case("NoteWindow"):
                    case("NotesPart"):
                    break;
                    default:
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
        internal void OnTaskScreenShow()
        {
            transform.parent.gameObject.SetActive(true);
            return;
        }

        internal void OnTaskScreenClose()
        {
            CloseQuestTree();
            transform.parent.Find("QuestTreeButton").gameObject.GetComponent<DefaultUIButton>().SetHeaderText("Quest Tree", 24);
            transform.parent.gameObject.SetActive(false);
        }
    }
}