using UnityEditor;
using UnityEngine;

namespace TP_FaderEditor
{
    [CustomEditor(typeof(TPFaderGUIData))]
    public class FaderEditorGUIDataEditor : ScriptlessFaderEditor
    {
        TPFaderGUIData TPTooltipData;

        void OnEnable()
        {
            TPTooltipData = (TPFaderGUIData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("GUI Skin");
            TPTooltipData.GUISkin =
                (EditorGUILayout.ObjectField(TPTooltipData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Progress Fade example prefab");
            TPTooltipData.ProgressPrefab = (EditorGUILayout.ObjectField(TPTooltipData.ProgressPrefab, typeof(GameObject), true) as GameObject);

            if (GUI.changed)
                EditorUtility.SetDirty(TPTooltipData);

            serializedObject.ApplyModifiedProperties();
        }
    }
}