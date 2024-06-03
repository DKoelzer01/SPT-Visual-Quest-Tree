using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VisualQuestTree.Utils
{
    public static class UIUtils
    {
        public static void ResetRectTransform(this GameObject gameObject)
        {
            var rectTransform = gameObject.transform as RectTransform;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public static bool TrySetTMPOutline(TextMeshProUGUI text)
        {
            if (text == null)
            {
                Plugin.Log.LogWarning($"TrySetTMPOutline: text cannot be null");
                return false;
            }

            try
            {
                text.outlineColor = new Color32(0, 0, 0, 255);
                text.outlineWidth = 0.15f;
                text.fontStyle = FontStyles.Bold;
                text.ForceMeshUpdate(true, true);
                return true;
            }
            catch
            {
                Plugin.Log.LogWarning($"Failed at setting outline. Will likely try again on next enable");
            }

            return false;
        }
        public static RectTransform GetRectTransform(this GameObject gameObject)
        {
            return gameObject.transform as RectTransform;
        }

        public static RectTransform GetRectTransform(this Component component)
        {
            return component.gameObject.transform as RectTransform;
        }

        public static GameObject CreateUIGameObject(GameObject parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = parent.layer;
            go.transform.SetParent(parent.transform);
            go.ResetRectTransform();

            return go;
        }
    }
}