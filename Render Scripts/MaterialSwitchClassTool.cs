/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © 2023 Palexen | Xeen Render & Devward. All rights reserved.
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

#endif
