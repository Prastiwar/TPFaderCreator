using UnityEditor;
using TP.Fader;

namespace TP.FaderEditor
{
    [CustomEditor(typeof(TPFader))]
    internal class TPFaderEditor : ScriptlessFaderEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script managing fader object");
        }
    }
}