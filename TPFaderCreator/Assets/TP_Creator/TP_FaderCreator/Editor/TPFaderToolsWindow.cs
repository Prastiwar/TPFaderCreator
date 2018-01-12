using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Fader;

namespace TP_FaderEditor
{
    public class TPFaderToolsWindow : EditorWindow
    {
        public static TPFaderToolsWindow window;
        public enum ToolEnum
        {
            Alpha,
            Progress
        }

        static ToolEnum tool;
        //string[] enumNamesList = System.Enum.GetNames(typeof(TPTooltipObserver.ToolTipType));
        
        SerializedProperty ProgressFader;
        SerializedProperty ProgressPrefab;
        SerializedProperty LoadingBar;
        SerializedProperty LoadingImage;
        SerializedProperty LoadingText;
        SerializedProperty LoadingTextString;
        SerializedProperty LoadingKeyToStart;
        SerializedProperty MustKeyToStart;
        SerializedProperty LoadingAnyKeyToStart;

        SerializedProperty FadeSpeed;
        SerializedProperty FadeTexture;
        SerializedProperty FadeColor;

        //GUIContent content = new GUIContent("You can drag there multiple observers   |  Size");

        Texture2D mainTexture;

        Vector2 scrollPos = Vector2.zero;
        Rect mainRect;

        static float windowSize = 450;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            if (window != null)
                window.Close();

            tool = _tool;
            window = (TPFaderToolsWindow)GetWindow(typeof(TPFaderToolsWindow));
            window.minSize = new Vector2(windowSize, windowSize);
            window.maxSize = new Vector2(windowSize, windowSize);
            window.Show();
        }

        void OnEnable()
        {
            InitTextures();

            FindProperties();
        }

        void FindProperties()
        {
            ProgressFader = TPFaderDesigner.creator.FindProperty("ProgressFader");
            ProgressPrefab = ProgressFader.FindPropertyRelative("ProgressPrefab");
            LoadingBar = ProgressFader.FindPropertyRelative("LoadingBar"); ;
            LoadingImage = ProgressFader.FindPropertyRelative("LoadingImage");
            LoadingText = ProgressFader.FindPropertyRelative("LoadingText");
            LoadingTextString = ProgressFader.FindPropertyRelative("LoadingTextString");
            MustKeyToStart = ProgressFader.FindPropertyRelative("MustKeyToStart");
            LoadingAnyKeyToStart = ProgressFader.FindPropertyRelative("LoadingAnyKeyToStart");
            LoadingKeyToStart = ProgressFader.FindPropertyRelative("LoadingKeyToStart");

            FadeSpeed = TPFaderDesigner.creator.FindProperty("FadeSpeed");
            FadeTexture = TPFaderDesigner.creator.FindProperty("FadeTexture");
            FadeColor = TPFaderDesigner.creator.FindProperty("FadeColor");
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
        }

        void OnGUI()
        {
            mainRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(mainRect, mainTexture);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));
            DrawTool();
            GUILayout.EndScrollView();
        }

        void DrawTool()
        {
            switch (tool)
            {
                case ToolEnum.Alpha:
                    DrawAlphaTool();
                    break;
                case ToolEnum.Progress:
                    DrawProgressTool();
                    break;
                default:
                    break;
            }
        }

        void DrawAlphaTool()
        {
            TPFaderDesigner.creator.Update();

            EditorGUILayout.LabelField("Fade Alpha Tool", TPFaderDesigner.skin.GetStyle("HeaderLabel"));
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Fade Speed", TPFaderDesigner.skin.GetStyle("TipLabel"));
            FadeSpeed.floatValue = EditorGUILayout.Slider(FadeSpeed.floatValue, 0, 10);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Fade Texture", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(FadeTexture, GUIContent.none);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Fade Color", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(FadeColor, GUIContent.none);

            if (GUI.changed)
                TPFaderDesigner.creator.ApplyModifiedProperties();
        }

        void DrawProgressTool()
        {
            TPFaderDesigner.creator.Update();

            EditorGUILayout.LabelField("Progress Fader", TPFaderDesigner.skin.GetStyle("HeaderLabel"));
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Progress Layout Prefab", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(ProgressPrefab, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Bar", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingBar, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Image", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingImage, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Text UI", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingText, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Text when scene is ready to load", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingTextString, GUIContent.none);
            EditorGUILayout.Separator();


            EditorGUILayout.LabelField("Pressing Key behaviour", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Should press key to start scene?", GUILayout.Width(150));
            EditorGUILayout.PropertyField(MustKeyToStart, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Loading ANY key to start after scene is ready to load?", GUILayout.Width(150));
            EditorGUILayout.PropertyField(LoadingAnyKeyToStart, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Custom loading key", GUILayout.Width(150));
            EditorGUILayout.PropertyField(LoadingKeyToStart, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
                TPFaderDesigner.creator.ApplyModifiedProperties();
        }

    }
}