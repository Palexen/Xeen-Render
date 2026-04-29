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
#if UNITY_EDITOR
using UnityEditor;
#endif
#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

#if UNITY_EDITOR

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
}

#endif
