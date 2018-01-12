using UnityEditor;
using UnityEngine;

namespace TP_FaderEditor
{
    public class ScriptlessFaderEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (GUILayout.Button("Open Fader Manager", GUILayout.Height(30)))
            {
                TPFaderDesigner.OpenWindow();
            }
        }
    }
}