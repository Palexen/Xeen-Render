/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

#endif

#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.Scriptables;
#endif

#region LIGHTMAPPING MANAGER

namespace Palexen.XeenRender.Render
{
    [Serializable]
    public class LightmapEntry
    {
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public Texture2D _colorMaps;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public Texture2D _directionalMaps;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)] public Texture2D _shadowMaskMaps;
    }
}

#endregion

#if UNITY_EDITOR

#region CUSTOM EDITORS

#region LIGHTING MANAGER
[CustomEditor(typeof(LightmapManager))]
[CanEditMultipleObjects]
public class LightmapManagerEditor : Editor
{
    LightmapManager lpc;
    SerializedProperty lightmapping;
    SerializedProperty _currentProbes;

    private void OnEnable()
    {
        lpc = (LightmapManager)target;
        lightmapping = serializedObject.FindProperty("lightmapping");
        _currentProbes = serializedObject.FindProperty("_currentProbes");
    }

    public override void OnInspectorGUI()
    {
        string customMessagePath = "Environment Settings/Palexen Environment Settings";
        CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

        GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Lighting Manager</color>",
            PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

        GUILayout.Box("Manage the type of lighting you will use in your scene after you bake it.",
            PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

        Color color = setting.contextSeparatorColor;

        serializedObject.Update();

        EditorGUILayout.PropertyField(lightmapping);

        GUILayout.Space(10);

        GUI.color = color;
        EditorGUILayout.HelpBox("", MessageType.None);
        GUI.color = Color.white;

        GUILayout.Space(10);

        if (lpc._currentProbes != null)
        {
            EditorGUILayout.PropertyField(_currentProbes);
        }

        if (!EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Extract data from LightProbes", PalexenEditorStyles.BigButton))
            {
                lpc.GetLightProbes();
            }
            EditorGUILayout.HelpBox("Make sure to copy the data and paste it into a preset after you've baked the lighting.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endregion

#endregion

#region PREFABS CREATOR
public class XeenRenderGOInstancer
{
    [MenuItem("GameObject/Xeen Render/Animation Recorder")]
    static void CreateAnimationRecorder()
    {
        GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Animation Recorder");

        if (prefabAsset != null)
        {
            GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
            Selection.activeGameObject = clone;
            EditorGUIUtility.PingObject(clone);
        }
        else
        {
            Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
        }
    }

    [MenuItem("GameObject/Xeen Render/360 Capture")]
    static void Create360Capture()
    {
        GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/360 Capture");

        if (prefabAsset != null)
        {
            GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
            Selection.activeGameObject = clone;
            EditorGUIUtility.PingObject(clone);
        }
        else
        {
            Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
        }
    }

    [MenuItem("GameObject/Xeen Render/Range Manager")]
    static void CreateRangeManager()
    {
        GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Range Manager");

        if (prefabAsset != null)
        {
            GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
            Selection.activeGameObject = clone;
            EditorGUIUtility.PingObject(clone);
        }
        else
        {
            Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
        }
    }

    [MenuItem("GameObject/Xeen Render/Lighting Manager")]
    static void CreateLightingManager()
    {
        GameObject prefabAsset = Resources.Load<GameObject>("Prefabs/Lighting Manager");

        if (prefabAsset != null)
        {
            GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);
            Selection.activeGameObject = clone;
            EditorGUIUtility.PingObject(clone);
        }
        else
        {
            Debug.LogError("Can't Find prefab in the <color=yellow>Prefabs/ </color> folder, " +
                "create new prefab and put in there, or <color=cyan>Reimport</color> the package");
        }
    }
}

#endregion

#region RENDER PRIORITY

namespace Palexen.XeenRender.Render
{
    [CustomEditor(typeof(RegionRenderPriority))]
    [CanEditMultipleObjects]
    public class RegionRenderPriorityEditor : Editor
    {
        RegionRenderPriority regionRenderPriority;
        private void OnEnable()
        {
            regionRenderPriority = (RegionRenderPriority)target;
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Region Render Priority</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Set the render priority for this region. Higher values will be rendered with more quality.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            Color color = setting.contextSeparatorColor;

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_priority"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_gizmoColor"));
            GUILayout.Space(10);
            GUI.color = color;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            GUILayout.Space(10);
            if (GUILayout.Button("Apply Render Priority", PalexenEditorStyles.BigButton))
            {
                regionRenderPriority.ApplyPriority();
                EditorUtility.SetDirty(regionRenderPriority);
                Debug.Log("<color=green>Render priority applied successfully!</color>");
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endregion

#region SHADER WARMING

namespace Palexen.XeenRender
{
    [CustomEditor(typeof(ShaderWarming))]
    public class ShaderWarmingEditor : Editor
    {
        ShaderWarming shaderWarming;
        private void OnEnable()
        {
            shaderWarming = (ShaderWarming)target;
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Shader Warming</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Pre warm shaders to avoid stuttering during gameplay.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            Color color = setting.contextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_shaderVariantCollection"));

            GUILayout.Space(10);

            GUI.color = color;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_onWarmingComplete"));

            GUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endregion

#endif
