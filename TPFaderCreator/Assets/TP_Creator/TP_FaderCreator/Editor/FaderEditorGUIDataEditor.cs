﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TP_FaderEditor
{
    public class FaderEditorGUIDataEditor : ScriptlessFaderEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OpenCreator();
        }
    }
}