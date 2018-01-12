using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

            DrawPropertiesExcluding(serializedObject, scriptField);

            serializedObject.ApplyModifiedProperties();
            OpenCreator();
        }
    }
}