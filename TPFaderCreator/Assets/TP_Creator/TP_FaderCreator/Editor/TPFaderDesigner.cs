﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Fader;

namespace TP_FaderEditor
{ 
public class TPFaderDesigner : EditorWindow
{
        [MenuItem("TP_Creator/TP_FaderCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Fader Designer runtime!");
                return;
            }
            TPFaderDesigner window = (TPFaderDesigner)GetWindow(typeof(TPFaderDesigner));
            window.minSize = new Vector2(615, 290);
            window.maxSize = new Vector2(615, 290);
            window.Show();
        }

        public static TPFaderGUIData EditorData;
        public static TPFaderCreator FaderCreator;
        public static GUISkin skin;

        Texture2D headerTexture;
        Texture2D managerTexture;
        Texture2D toolTexture;

        Rect headerSection;
        Rect managerSection;
        Rect toolSection;

        bool existManager;

        public static SerializedObject creator;

        void OnEnable()
        {
            InitEditorData();
            InitTextures();
            InitCreator();

            if (FaderCreator)
                creator = new SerializedObject(FaderCreator);
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_FaderCreator/EditorResources/FaderEditorGUIData.asset",
                   typeof(TPFaderGUIData)) as TPFaderGUIData;

            if (EditorData == null)
                CreateEditorData();
            else
                CheckGUIData();

            skin = EditorData.GUISkin;
        }

        void CheckGUIData()
        {
            if (EditorData.GUISkin == null)
                EditorData.GUISkin = AssetDatabase.LoadAssetAtPath(
                      "Assets/TP_Creator/TP_FaderCreator/EditorResources/TPFaderGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            //if (EditorData.TooltipPrefab == null)
            //    EditorData.TooltipPrefab = AssetDatabase.LoadAssetAtPath(
            //        "Assets/TP_Creator/TP_TooltipCreator/EditorResources/TooltipCanvas.prefab",
            //        typeof(GameObject)) as GameObject;

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPFaderGUIData newEditorData = ScriptableObject.CreateInstance<TPFaderGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/TP_FaderCreator/EditorResources/FaderEditorGUIData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorData = newEditorData;
            CheckGUIData();
        }

        void InitTextures()
        {
            Color colorHeader = new Color(0.19f, 0.19f, 0.19f);
            Color color = new Color(0.15f, 0.15f, 0.15f);

            headerTexture = new Texture2D(1, 1);
            headerTexture.SetPixel(0, 0, colorHeader);
            headerTexture.Apply();

            managerTexture = new Texture2D(1, 1);
            managerTexture.SetPixel(0, 0, color);
            managerTexture.Apply();

            toolTexture = new Texture2D(1, 1);
            toolTexture.SetPixel(0, 0, color);
            toolTexture.Apply();
        }

        static void InitCreator()
        {
            if (FaderCreator == null)
            {
                FaderCreator = FindObjectOfType<TPFaderCreator>();

                if (FaderCreator != null)
                    UpdateManager();
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (TPFaderToolsWindow.window)
                    TPFaderToolsWindow.window.Close();
                this.Close();
            }
            DrawLayouts();
            DrawHeader();
            DrawManager();
            DrawTools();
        }

        void DrawLayouts()
        {
            headerSection = new Rect(0, 0, Screen.width, 50);
            managerSection = new Rect(0, 50, Screen.width / 2, Screen.height);
            toolSection = new Rect(Screen.width / 2, 50, Screen.width / 2, Screen.height);

            GUI.DrawTexture(headerSection, headerTexture);
            GUI.DrawTexture(managerSection, managerTexture);
            GUI.DrawTexture(toolSection, toolTexture);
        }

        void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Label("TP Fader Creator - Manage your Fade!", skin.GetStyle("HeaderLabel"));
            GUILayout.EndArea();
        }

        void DrawManager()
        {
            GUILayout.BeginArea(managerSection);
            GUILayout.Label("Fader Manager - Core", skin.box);

            if (FaderCreator == null)
            {
                InitializeManager();
            }
            else
            {
                SpawnEmpty();
                ResetManager();

                if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(70)))
                {
                    UpdateManager();
                }
            }

            GUILayout.EndArea();
        }

        void InitializeManager()
        {
            if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(60)))
            {
                GameObject go = (new GameObject("TP_FaderManager", typeof(TPFaderCreator)));
                FaderCreator = go.GetComponent<TPFaderCreator>();
                UpdateManager();
                Debug.Log("Fader Manager created!");
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(60)))
                existManager = !existManager;

            if (existManager)
                FaderCreator = EditorGUILayout.ObjectField(FaderCreator, typeof(TPFaderCreator), true,
                    GUILayout.Height(30)) as TPFaderCreator;
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(45)))
                FaderCreator = null;
        }

        void SpawnEmpty()
        {
            if (GUILayout.Button("Spawn empty Progress Fade", skin.button, GUILayout.Height(50)))
            {
                if (EditorData.ProgressPrefab == null)
                {
                    Debug.LogError("There is no progress prefab in EditorGUIData file!");
                    return;
                }
                Instantiate(EditorData.ProgressPrefab);
                Debug.Log("Progress fade example Created");
            }
        }

        public static void UpdateManager()
        {
            //if (FaderCreator.TooltipLayout != null)
            //    FaderCreator.TooltipLayout.Refresh();
            //if (FaderCreator)
            //    FaderCreator.Refresh();
            if (creator != null)
                creator.ApplyModifiedProperties();
        }

        void DrawTools()
        {

            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Fader Manager - Tools", skin.box);

            if (FaderCreator == null)
            {
                GUILayout.EndArea();
                return;
            }

            if (GUILayout.Button("Alpha Fade", skin.button, GUILayout.Height(60)))
            {
                TPFaderToolsWindow.OpenToolWindow(TPFaderToolsWindow.ToolEnum.Alpha);
            }
            if (GUILayout.Button("Progress Fade", skin.button, GUILayout.Height(60)))
            {
                TPFaderToolsWindow.OpenToolWindow(TPFaderToolsWindow.ToolEnum.Progress);
            }
            //if (GUILayout.Button("Layout", skin.button, GUILayout.Height(60)))
            //{
            //    TPFaderToolsWindow.OpenToolWindow(TPFaderToolsWindow.ToolEnum.Layout);
            //}
            GUILayout.EndArea();
        }

    }
}