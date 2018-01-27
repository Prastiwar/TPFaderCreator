using TP_Fader;
using UnityEditor;
using UnityEngine;

namespace TP_FaderEditor
{
    [CustomEditor(typeof(TPFaderGUIData))]
    internal class FaderEditorGUIDataEditor : ScriptlessFaderEditor
    {
        TPFaderGUIData TPTooltipData;

        void OnEnable()
        {
            TPTooltipData = (TPFaderGUIData)target;
            if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Container for editor data");
            if (!TPFaderCreator.DebugMode)
                return;

            EditorGUILayout.LabelField("GUI Skin");
            TPTooltipData.GUISkin =
                (EditorGUILayout.ObjectField(TPTooltipData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Progress Fade example prefab");
            TPTooltipData.ProgressPrefab = (EditorGUILayout.ObjectField(TPTooltipData.ProgressPrefab, typeof(GameObject), true) as GameObject);
        }
    }
}