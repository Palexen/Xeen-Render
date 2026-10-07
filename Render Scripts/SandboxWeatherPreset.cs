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
using Palexen.XeenRender.Gameplay;
#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.Scriptables;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Scriptables
{
    [CreateAssetMenu(fileName = "New Weather Preset", menuName = "Palexen/Xeen Render/Weather Preset")]
    public class SandboxWeatherPreset : ScriptableObject
    {
        [SerializeField] private WeatherProbabilities[] _weatherProbabilities;
        public WeatherProbabilities[] WeatherProbabilities { get { return _weatherProbabilities; } }
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR

    [CustomEditor(typeof(SandboxWeatherPreset))]
    [CanEditMultipleObjects]
    public class SandboxWeatherPresetEditor : Editor
    {
        SandboxWeatherPreset swp;
        SerializedProperty _weatherProbabilities;

        private void OnEnable()
        {
            swp = (SandboxWeatherPreset)target;
            _weatherProbabilities = serializedObject.FindProperty("_weatherProbabilities");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Weather Preset</color>",
                PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));

            GUILayout.Box("Setup your Weather Presets, Set Hour and conditions",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 100));

            Color color = setting.ContextSeparatorColor;

            serializedObject.Update();

            EditorGUILayout.PropertyField(_weatherProbabilities);

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CreateWeatherPreset
    {
#if PALEXEN_UP_TOOLBAR
        [MenuItem("Weather/Create Weather Preset")]
#else
        [MenuItem("Xeen Render/Create Weather Preset", false)]
#endif
        private static void CreateAsset()
        {
            SandboxWeatherPreset asset = ScriptableObject.CreateInstance<SandboxWeatherPreset>();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            string folderPath = setting.ScriptablesFolderPath;

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder($"{folderPath}", "Weather Preset");
            }

            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/New Weather Preset.asset");

            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            Debug.Log($"<color=green>Weather Preset created at: </color><color=cyan>{assetPath}</color>");
        }
    }

#endif
    #endregion
}