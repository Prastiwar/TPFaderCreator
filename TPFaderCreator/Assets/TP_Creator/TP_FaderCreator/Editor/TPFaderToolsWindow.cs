using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Fader;
using UnityEditor.SceneManagement;

namespace TP_FaderEditor
{
    [InitializeOnLoad]
    public class TPFaderToolsWindow : EditorWindow
    {
        public static TPFaderToolsWindow window;
        public enum ToolEnum
        {
            Alpha,
            Progress,
            Faders
        }

        static ToolEnum tool;

        SerializedProperty ProgressFader;
        SerializedProperty ProgressPrefab;
        SerializedProperty LoadingBar;
        SerializedProperty LoadingImage;
        SerializedProperty LoadingProgressText;
        SerializedProperty LoadingText;
        SerializedProperty LoadingTextString;
        SerializedProperty ProgressFadeSpeed;
        SerializedProperty LoadingKeyToStart;
        SerializedProperty MustKeyToStart;
        SerializedProperty LoadingAnyKeyToStart;

        SerializedProperty AlphaFader;
        SerializedProperty FadeSpeed;
        SerializedProperty FadeTexture;
        SerializedProperty FadeImage;
        SerializedProperty FadeColor;

        SerializedProperty FaderList;
        string[] enumNamesList = System.Enum.GetNames(typeof(TPFader.FaderType));
        GUIContent content = new GUIContent("                          Size:");

        Texture2D mainTexture;
        Vector2 scrollPos = Vector2.zero;
        Rect mainRect;

        static float windowSize = 450;
        static string currentScene;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;

            if (window != null)
                window.Close();

            tool = _tool;
            window = (TPFaderToolsWindow)GetWindow(typeof(TPFaderToolsWindow));
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(windowSize, windowSize);
            window.maxSize = new Vector2(windowSize, windowSize);
            window.Show();
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if(window)
                    window.Close();
                if (TPFaderDesigner.window)
                    TPFaderDesigner.window.Close();
            }
        }

        void Update()
        {
            if (EditorApplication.isCompiling)
                this.Close();
        }

        void OnEnable()
        {
            InitTextures();

            FindProperties();
        }

        void FindProperties()
        {
            if (TPFaderDesigner.creator == null)
                return;

            ProgressFader = TPFaderDesigner.creator.FindProperty("ProgressFader");
            ProgressPrefab = ProgressFader.FindPropertyRelative("ProgressPrefab");
            LoadingBar = ProgressFader.FindPropertyRelative("LoadingBar"); ;
            LoadingImage = ProgressFader.FindPropertyRelative("LoadingImage");
            LoadingProgressText = ProgressFader.FindPropertyRelative("LoadingProgressText");
            LoadingText = ProgressFader.FindPropertyRelative("LoadingText");
            LoadingTextString = ProgressFader.FindPropertyRelative("LoadingTextString");
            ProgressFadeSpeed = ProgressFader.FindPropertyRelative("ProgressFadeSpeed");
            MustKeyToStart = ProgressFader.FindPropertyRelative("MustKeyToStart");
            LoadingAnyKeyToStart = ProgressFader.FindPropertyRelative("LoadingAnyKeyToStart");
            LoadingKeyToStart = ProgressFader.FindPropertyRelative("LoadingKeyToStart");

            AlphaFader = TPFaderDesigner.creator.FindProperty("AlphaFader");
            FadeSpeed = AlphaFader.FindPropertyRelative("FadeSpeed");
            FadeTexture = AlphaFader.FindPropertyRelative("FadeTexture");
            FadeColor = AlphaFader.FindPropertyRelative("FadeColor");

            FadeImage = TPFaderDesigner.creator.FindProperty("FadeImage");
            FaderList = TPFaderDesigner.creator.FindProperty("Faders");
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
        }

        static void OnProjectChanged()
        {
            Debug.Log("OnProjectChanged");
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                this.Close();
            }

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
                case ToolEnum.Faders:
                    DrawFadersTool();
                    break;
                default:
                    break;
            }
        }

        void DrawAlphaTool()
        {
            TPFaderDesigner.creator.Update();
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
            {
                (FadeImage.objectReferenceValue as UnityEngine.UI.Image).sprite = FadeTexture.objectReferenceValue as Sprite;
                (FadeImage.objectReferenceValue as UnityEngine.UI.Image).color = FadeColor.colorValue;
                TPFaderDesigner.creator.ApplyModifiedProperties();
            }
        }

        void DrawProgressTool()
        {
            TPFaderDesigner.creator.Update();
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Progress Layout Prefab", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(ProgressPrefab, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Slider", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingBar, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Image", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingImage, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Progress Text UI", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingProgressText, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Text UI", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingText, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Loading Text when scene is ready to load", TPFaderDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(LoadingTextString, GUIContent.none);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Progress Fade Speed", TPFaderDesigner.skin.GetStyle("TipLabel"));
            ProgressFadeSpeed.floatValue = EditorGUILayout.Slider(ProgressFadeSpeed.floatValue, 10, 20);
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

        void DrawFadersTool()
        {
            if (FaderList == null)
                return;

            if (GUILayout.Button("Add new", TPFaderDesigner.EditorData.GUISkin.button))
            {
                AddFader();
            }
            if (FaderList.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No faders loaded!", MessageType.Error);
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Scenes loaded from build settings:", GUILayout.Width(200));
            EditorGUILayout.Popup(0, ReadSceneNames(true));
            EditorGUILayout.EndHorizontal();

            FaderList.serializedObject.UpdateIfRequiredOrScript();
            ShowFaders(FaderList);
        }

        void ShowFaders(SerializedProperty list)
        {
            if (Event.current.type == EventType.DragPerform)
            {
                if (DragAndDrop.objectReferences.Length > 1)
                {
                    return;
                }
                else
                {
                    if (!PrefabUtility.GetPrefabObject(DragAndDrop.objectReferences[0]))
                    {
                        return;
                    }
                }
            }
            
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"), content, GUILayout.Width(200));

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Faders loaded:", GUILayout.Width(110));
            EditorGUILayout.LabelField("Fade to Scene at index", GUILayout.Width(155));
            EditorGUILayout.LabelField("Fade Type", GUILayout.Width(100));
            GUILayout.EndHorizontal();
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                Check(list, i);
                SetFadeTo(list, i);
                SetFadeType(list, i);
                EditAsset(list, i);
                RemoveAsset(list, i);
                GUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                AddComponent(FaderList);
                FaderList.serializedObject.ApplyModifiedProperties();
            }
        }

        void SetFadeTo(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue == null ||
                !(list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>())
                return;

            var fader = (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>();

            fader.FadeToSceneIndex = EditorGUILayout.IntField(fader.FadeToSceneIndex);

            if (fader.FadeToSceneIndex > UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1)
                EditorGUILayout.HelpBox("There is no scene at this build index", MessageType.Error);

            list.serializedObject.ApplyModifiedProperties();
        }

        void SetFadeType(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue == null)
                return;
            if ((list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>() == null)
                return;

            int actualSelected = 1;
            int selectionFromInspector =
                (int)(list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>().FadeType;

            actualSelected = EditorGUILayout.Popup(selectionFromInspector, enumNamesList, GUILayout.Width(100));

            (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>().FadeType
                = (TPFader.FaderType)actualSelected;

            EditorUtility.SetDirty(list.GetArrayElementAtIndex(index).objectReferenceValue);
        }

        void AddComponent(SerializedProperty list)
        {

            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                if (list.GetArrayElementAtIndex(i) == null || list.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    continue;
                else
                {
                    if (!(list.GetArrayElementAtIndex(i).objectReferenceValue as GameObject).GetComponent<TPFader>())
                        (list.GetArrayElementAtIndex(i).objectReferenceValue as GameObject).AddComponent<TPFader>();
                }
            }
        }

        void Check(SerializedProperty list, int index)
        {
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                if (i == index)
                    continue;
                if (list.GetArrayElementAtIndex(index).objectReferenceValue == list.GetArrayElementAtIndex(i).objectReferenceValue)
                {
                    list.GetArrayElementAtIndex(i).objectReferenceValue = null;
                }
            }
        }

        void RemoveAsset(SerializedProperty list, int index)
        {
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                if (list.GetArrayElementAtIndex(index).objectReferenceValue != null || index == list.arraySize - 1)
                {
                    if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                    {
                        TPFader script = (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPFader>();
                        DestroyImmediate(script);
                        list.GetArrayElementAtIndex(index).objectReferenceValue = null;
                    }
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }

        void EditAsset(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                if (GUILayout.Button("Edit", GUILayout.Width(35)))
                {
                    AssetDatabase.OpenAsset(list.GetArrayElementAtIndex(index).objectReferenceValue);
                }
        }

        void AddFader()
        {
            FaderList.arraySize++;
            FaderList.serializedObject.ApplyModifiedProperties();
            TPFaderDesigner.UpdateManager();
        }

        string[] ReadSceneNames(bool withIndex)
        {
            List<string> temp = new List<string>();
            foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    string name = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.Length - 6);
                    temp.Add(withIndex ? "Index: " + temp.Count + " | " + name : name);
                }
            }
            return temp.ToArray();
        }
    }
}