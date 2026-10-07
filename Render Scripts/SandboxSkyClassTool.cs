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
using System;
using UnityEngine;

#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.Scriptables;

#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Gameplay
{
    public enum CycleConditions { day, night, rain, windy, overcast}
    public enum WeatherRandomizer { no, yes }
    public enum  WeatherType { clear, cloudy, rain, storm, windy }

    [Serializable]
    public class SandboxSkyAudioPreset
    {
        [SerializeField] private string _presetName;
        [SerializeField] private CycleConditions _condition;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)][SerializeField] private AudioClip _targetClip;
        [SerializeField] [Range(0, 1)] private float _startAt;
        [SerializeField] private bool _isPlaying;

        public string PresetName { get { return _presetName; } }
        public CycleConditions Condition { get { return _condition; } }
        public AudioClip TargetClip { get { return _targetClip; } }
        public float StartAt { get { return _startAt; } }
        public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }
    }

    [Serializable]
    public class WeatherProbabilities
    {
        [SerializeField] private string _presetName;
        [SerializeField] private WeatherType[] _probabilities;
        [SerializeField] [Range(0, 1)] private float _atHour;

        public WeatherType GetRandomWeather()
        {
            return _probabilities[UnityEngine.Random.Range(0, _probabilities.Length)];
        }
        public float AtHour { get { return _atHour; } }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SandboxSkyManager))]

    public class SandboxSkyEditor : Editor
    {
        SandboxSkyManager ssm;
        SerializedProperty _dayCycle;
        SerializedProperty _currentWeather;
        SerializedProperty _randomizeWeather;
        SerializedProperty _weatherPreset;
        SerializedProperty _weatherChangeInterval;
        SerializedProperty _sandboxKybox;
        SerializedProperty _skyDayTint;
        SerializedProperty _skyNightTint;
        SerializedProperty _dayVariations;
        SerializedProperty _nightVariations;
        SerializedProperty _cloudyDayVariations;
        SerializedProperty _cloudyNightVariations;
        SerializedProperty _dayCycleLengthInMinutes;
        SerializedProperty _sunColor;
        SerializedProperty _moonColor;
        SerializedProperty _sun;
        SerializedProperty useFog;
        SerializedProperty _fogDayColor;
        SerializedProperty _fogNightColor;
        SerializedProperty _atmos;
        SerializedProperty presets;

        private void OnEnable()
        {
            ssm = (SandboxSkyManager)target;

            _dayCycle = serializedObject.FindProperty("_cycle");
            _currentWeather = serializedObject.FindProperty("_currentWeather");
            _randomizeWeather = serializedObject.FindProperty("_randomizeWeather");
            _weatherPreset = serializedObject.FindProperty("_weatherPreset");
            _weatherChangeInterval = serializedObject.FindProperty("_weatherChangeInterval");
            _sandboxKybox = serializedObject.FindProperty("_sandboxKybox");
            _skyDayTint = serializedObject.FindProperty("_skyDayTint");
            _skyNightTint = serializedObject.FindProperty("_skyNightTint");
            _dayVariations = serializedObject.FindProperty("_dayVariations");
            _nightVariations = serializedObject.FindProperty("_nightVariations");
            _cloudyDayVariations = serializedObject.FindProperty("_cloudyDayVariations");
            _cloudyNightVariations = serializedObject.FindProperty("_cloudyNightVariations");
            _dayCycleLengthInMinutes = serializedObject.FindProperty("_dayCycleLengthInMinutes");
            _sunColor = serializedObject.FindProperty("_sunColor");
            _moonColor = serializedObject.FindProperty("_moonColor");
            _sun = serializedObject.FindProperty("_sun");
            useFog = serializedObject.FindProperty("useFog");
            _fogDayColor = serializedObject.FindProperty("_fogDayColor");
            _fogNightColor = serializedObject.FindProperty("_fogNightColor");
            _atmos = serializedObject.FindProperty("_atmos");
            presets = serializedObject.FindProperty("presets");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.ScriptTitleColor.ConvertToHex()}>Sandbox Sky</color>",
            PalexenEditorStyles.CoolTitle(setting.ScriptTitleSize));

            GUILayout.Box("The Sandbox Sky system allows you to create dynamic sky environments with " +
                "various weather conditions and atmospheric effects.", PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            if (RenderSettings.skybox != ssm.SkyboxMaterial)
            {
                EditorGUILayout.HelpBox("The current skybox is different from the Sandbox Skybox. Click the button below to set it up.", MessageType.Warning);

                if (GUILayout.Button(PalexenEditorStyles.MyGUIContent (" Setup Sandbox Sky", IconDrawer.other, "Skybox Icon"), PalexenEditorStyles.BigButton))
                {
                    RenderSettings.skybox = ssm.SkyboxMaterial;
                }
            }

            EditorGUILayout.PropertyField(_dayCycle);
            EditorGUILayout.PropertyField(_currentWeather);
            EditorGUILayout.PropertyField(_weatherPreset);
            EditorGUILayout.PropertyField(_weatherChangeInterval);
            EditorGUILayout.PropertyField(_sandboxKybox);
            EditorGUILayout.PropertyField(_skyDayTint);
            EditorGUILayout.PropertyField(_skyNightTint);
            EditorGUILayout.PropertyField(_dayVariations);
            EditorGUILayout.PropertyField(_nightVariations);
            EditorGUILayout.PropertyField(_cloudyDayVariations);
            EditorGUILayout.PropertyField(_cloudyNightVariations);
            EditorGUILayout.PropertyField(_dayCycleLengthInMinutes);
            EditorGUILayout.PropertyField(_sunColor);
            EditorGUILayout.PropertyField(_moonColor);
            EditorGUILayout.PropertyField(_sun);
            EditorGUILayout.PropertyField(useFog, new GUIContent("Use Fog?"));
            EditorGUILayout.PropertyField(_fogDayColor);
            EditorGUILayout.PropertyField(_fogNightColor);
            EditorGUILayout.PropertyField(_atmos);
            EditorGUILayout.PropertyField(presets);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
