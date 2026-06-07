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
using Palexen.Tools;
using Palexen.Scriptables;
using Palexen.XeenRender.Render;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Render
{
    public enum MeshType { meshRenderer, skinnedMeshRenderer }
    public enum MaterialType { highEnd, midRange, lowEnd }
}

#if UNITY_EDITOR

    #region RANGE MANAGER
    [CustomEditor(typeof(RangeManager))]
    public class RangeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            RangeManager tgt = (RangeManager)target;

            GUI.color = Color.cyan;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;


            if (GUILayout.Button("High-End", PalexenEditorStyles.BigButton))
            {
                if (EditorApplication.isPlaying)
                {
                    tgt.SetRange(MaterialType.highEnd);
                }
            }

            if (EditorGUIUtility.isProSkin)
            {
                PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2, 10, 0);
            }
            else
            {
                PalexenEditorStyles.DrawHorizontalLine(Color.magenta, 2, 10, 0);
            }

            if (GUILayout.Button("Mid-range", PalexenEditorStyles.BigButton))
            {
                if (EditorApplication.isPlaying)
                {
                    tgt.SetRange(MaterialType.midRange);
                }
            }

            if (EditorGUIUtility.isProSkin)
            {
                PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2, 10, 0);
            }
            else
            {
                PalexenEditorStyles.DrawHorizontalLine(Color.magenta, 2, 10, 0);
            }

            if (GUILayout.Button("Low-End", PalexenEditorStyles.BigButton))
            {
                if (EditorApplication.isPlaying)
                {
                    tgt.SetRange(MaterialType.lowEnd);
                }
            }
        }

    }
#endregion

    #region MATERIAL SWITCH

    [CustomEditor(typeof(MaterialSwitch))]
    [CanEditMultipleObjects]
    public class MaterialSwitchEditor : Editor
    {
        MaterialSwitch ms;
        SerializedProperty _meshType;
        SerializedProperty _quality;
        SerializedProperty _mesh;
        SerializedProperty _skinnedMesh;
        SerializedProperty _highEnd;
        SerializedProperty _midRange;
        SerializedProperty _lowEnd;

        private void OnEnable()
        {
            ms = (MaterialSwitch)target;
            _meshType = serializedObject.FindProperty("_meshType");
            _quality = serializedObject.FindProperty("_quality");
            _mesh = serializedObject.FindProperty("_mesh");
            _skinnedMesh = serializedObject.FindProperty("_skinnedMesh");
            _highEnd = serializedObject.FindProperty("_highEnd");
            _midRange = serializedObject.FindProperty("_midRange");
            _lowEnd = serializedObject.FindProperty("_lowEnd");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Material Switch</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("It manages material quality, is ideal for working across different ranges, but should be used " +
                "carefully and thoughtfully. As a tip, load low-end materials first in all your models, then, using the " +
                "<color=green>SetRange(MaterialType type);</color> method in the <color=magenta>Range Manager</color> singleton, " +
                "you can change the material type if you plan " +
                "to use platforms with devices that have <color=red>different performance levels</color>.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 150));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_meshType);

            if (ms._meshType == MeshType.meshRenderer)
            {
                EditorGUILayout.PropertyField(_mesh);
            }
            else
            {
                EditorGUILayout.PropertyField(_skinnedMesh);
            }
            GUILayout.Space(10);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray, 2, 10, 0);
            EditorGUILayout.PropertyField(_quality);
            EditorGUILayout.HelpBox("During the editor, change this value to view and configure the quality of the materials.",
                MessageType.Info);

            if (ms._quality == MaterialType.highEnd)
            {
                EditorGUILayout.PropertyField(_highEnd);
            }
            else if (ms._quality == MaterialType.midRange)
            {
                EditorGUILayout.PropertyField(_midRange);
            }
            else
            {

                EditorGUILayout.PropertyField(_lowEnd);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endregion

#endif
