using UnityEditor;
using TP_Fader;

namespace TP_FaderEditor
{
    [CustomEditor(typeof(TPFader))]
    public class TPFaderEditor : ScriptlessFaderEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script managing fader object");
        }
    }
}