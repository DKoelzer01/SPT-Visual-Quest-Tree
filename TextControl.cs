using VisualQuestTree.Utils;
using TMPro;
using UnityEngine;

namespace VisualQuestTree.UI.Controls
{
    public abstract class TextControl : MonoBehaviour
    {
        public TextMeshProUGUI Text { get; protected set; }
        public RectTransform RectTransform => gameObject.transform as RectTransform;

        private bool _hasSetOutline = false;

        public static T Create<T>(GameObject parent, string name, float fontSize) where T : TextControl
        {
            var go = UIUtils.CreateUIGameObject(parent, name);

            var textControl = go.AddComponent<T>();
            textControl.Text = go.AddComponent<TextMeshProUGUI>();
            textControl.Text.autoSizeTextContainer = true;
            textControl.Text.fontSize = fontSize;
            textControl.Text.alignment = TextAlignmentOptions.Left;

            textControl._hasSetOutline = UIUtils.TrySetTMPOutline(textControl.Text);

            return textControl;
        }

        private void OnEnable()
        {
            if (_hasSetOutline || Text == null)
            {
                return;
            }

            _hasSetOutline = UIUtils.TrySetTMPOutline(Text);
            Text.text = Text.text;  // try resetting text, since it seems like if outline fails, it doesn't size properly
        }
    }

    public class TextBox : TextControl 
    {
        public static TextBox Create(GameObject parent, float fontSize)
        {
            var text = Create<TextBox>(parent, "TextBox", fontSize);
            return text;
        }

        public void SetText(string newText) 
        {
            Text.text = newText;
        }
    }

    public class QuestBox : TextControl
    {
        public string questName;
        public int questLvl;
        public string questGiver;
        
        public static QuestBox Create(GameObject parent, float fontSize)
        {
            var text = Create<QuestBox>(parent, "QuestBox", fontSize);
            return text;
        }
    }
}