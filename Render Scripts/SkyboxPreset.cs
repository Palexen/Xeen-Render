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
#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.Scriptables;

#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Scriptables
{
    [CreateAssetMenu(fileName = "New Skybox Preset", menuName = "Palexen/Xeen Render/Skybox Preset")]
    public class SkyboxPreset : ScriptableObject
    {
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] [SerializeField] private Cubemap[] _varaints;

        public Cubemap GetRandomSkybox()
        {
            return _varaints[Random.Range(0, _varaints.Length)];
        }
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR

    [CustomEditor(typeof(SkyboxPreset))]
    [CanEditMultipleObjects]
    public class SkyboxPresetPresetEditor : Editor
    {
        SkyboxPreset sp;
        SerializedProperty _varaints;

        private void OnEnable()
        {
            sp = (SkyboxPreset)target;
            _varaints = serializedObject.FindProperty("_varaints");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Skybox Variation Preset</color>",
                PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));

            GUILayout.Box("Setup your Skyboxes what you want in your Sandbox Game",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 100));

            Color color = setting.ContextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_varaints);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateSkyboxVariationPreset
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Weather/Create Skybox Variation Preset")]
#else
        [MenuItem("Xeen Render/Create Skybox Variation Preset", false)]
#endif
        private static void CreateAsset()
        {
            SkyboxPreset asset = ScriptableObject.CreateInstance<SkyboxPreset>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.ScriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Skybox Variation Preset");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Skybox Variation Preset.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Skybox Variation Preset created at: </color><color=cyan>{assetPath}</color>");
        }
    }

#endif
    #endregion
}
