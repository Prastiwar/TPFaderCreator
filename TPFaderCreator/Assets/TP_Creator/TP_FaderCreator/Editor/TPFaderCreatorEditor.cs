using UnityEditor;
using TP_Fader;

namespace TP_FaderEditor
{
    [CustomEditor(typeof(TPFaderCreator))]
    public class TPFaderCreatorEditor : ScriptlessFaderEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("This script allows you to manage fader");
            DrawPropertiesExcluding(serializedObject, scriptField);

            serializedObject.ApplyModifiedProperties();
            OpenCreator();
        }
    }
}