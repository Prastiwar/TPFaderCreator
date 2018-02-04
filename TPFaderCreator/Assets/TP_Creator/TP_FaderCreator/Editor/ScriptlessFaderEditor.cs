using TP.Fader;
using UnityEditor;
using UnityEngine;

namespace TP.FaderEditor
{
    internal class ScriptlessFaderEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (TPFaderCreator.DebugMode)
            {
                if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                    serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
                return;
            }

            if (serializedObject.targetObject.hideFlags != HideFlags.None)
                serializedObject.targetObject.hideFlags = HideFlags.None;

            if (GUILayout.Button("Open Fader Manager", GUILayout.Height(30)))
            {
                TPFaderDesigner.OpenWindow();
            }
        }
    }
}