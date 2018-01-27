using UnityEditor;
using TP_Fader;
using UnityEngine;
using UnityEngine.UI;

namespace TP_FaderEditor
{
    [CustomEditor(typeof(TPFaderCreator))]
    internal class TPFaderCreatorEditor : ScriptlessFaderEditor
    {
        TPFaderCreator creator;

        void OnEnable()
        {
            creator = target as TPFaderCreator;
        }

        public override void OnInspectorGUI()
        {
            if (TPFaderCreator.DebugMode)
            {
                if (creator.GetComponent<CanvasRenderer>().hideFlags != HideFlags.NotEditable)
                    creator.GetComponent<CanvasRenderer>().hideFlags = HideFlags.NotEditable;
                if (creator.GetComponent<CanvasGroup>().hideFlags != HideFlags.NotEditable)
                    creator.GetComponent<CanvasGroup>().hideFlags = HideFlags.NotEditable;
                if (creator.GetComponent<Image>().hideFlags != HideFlags.NotEditable)
                    creator.GetComponent<Image>().hideFlags = HideFlags.NotEditable;
            }
            else
            {
                if (creator.GetComponent<CanvasRenderer>().hideFlags != HideFlags.HideInInspector)
                    creator.GetComponent<CanvasRenderer>().hideFlags = HideFlags.HideInInspector;
                if (creator.GetComponent<CanvasGroup>().hideFlags != HideFlags.HideInInspector)
                    creator.GetComponent<CanvasGroup>().hideFlags = HideFlags.HideInInspector;
                if (creator.GetComponent<Image>().hideFlags != HideFlags.HideInInspector)
                    creator.GetComponent<Image>().hideFlags = HideFlags.HideInInspector;
            }

            EditorGUILayout.LabelField("This script allows you to manage fader");
            if (TPFaderCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}