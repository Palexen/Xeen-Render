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
using Palexen.Audio.Atmos;
using Palexen.XeenRender.Scriptables;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.XeenRender.Gameplay
{
#if PALEXEN_TOOLS
    [ScriptDescription("Sandbox Sky Manager", "Create and manage the sandbox sky")]
#endif
    [AddComponentMenu("Palexen/Xeen Render/Sandbox Sky Manager")]
    public class SandboxSkyManager : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Cylce Settings")]
        [SerializeField] [Range(0, 1)] private float _cycle;

        [MyHeader("Weather Settings")]
        [SerializeField] private WeatherType _currentWeather;
        [SerializeField] private WeatherRandomizer _randomizeWeather = WeatherRandomizer.yes;
        [FieldColor(FieldPropertyColor.blue, ShowObjectMessage.errorMessage)][SerializeField] private SandboxWeatherPreset _weatherPreset;
        [Notepad("<color=yellow>In Minutes</color>", FontStyle.BoldAndItalic)] [SerializeField] private int _weatherChangeInterval = 5;
        float _weatherChangeTimer = 1;

        [MyHeader("Skybox Settings")]
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)] [SerializeField] private Material _sandboxKybox;
        [SerializeField] private Gradient _skyDayTint;
        [SerializeField] private Gradient _skyNightTint;

        [Header("Skybox Variation Settings")]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)][SerializeField] private SkyboxPreset _dayVariations;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)][SerializeField] private SkyboxPreset _nightVariations;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)][SerializeField] private SkyboxPreset _cloudyDayVariations;
        [FieldColor(FieldPropertyColor.clearBlue, ShowObjectMessage.errorMessage)][SerializeField] private SkyboxPreset _cloudyNightVariations;
        [Separator]
        [Notepad("<color=yellow>In Minutes</color>", FontStyle.BoldAndItalic)]
        [SerializeField] private float _dayCycleLengthInMinutes = 24f;
        float _updateSpeed = 1;

        [MyHeader("Sun Settings")]
        [SerializeField] private Gradient _sunColor;
        [SerializeField] private Gradient _moonColor;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] [SerializeField] private Light _sun;

        bool inDay = true;

        [MyHeader("Fog Settings")]
        [SerializeField] private bool useFog;
        [SerializeField] private Gradient _fogDayColor;
        [SerializeField] private Gradient _fogNightColor;

        [MyHeader("Audio Settings")]
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.message)] [SerializeField] private Atmos _atmos;
        [SerializeField] private SandboxSkyAudioPreset[] presets;

        private bool changingWeather;
        private bool blendToSecondTexture;


        #endregion

        #region PROPERTIES

        public float DayCycle { get { return _cycle; } set { _cycle = value; } }
        public WeatherType CurrentWeather { get { return _currentWeather; } }
        public Material SkyboxMaterial { get { return _sandboxKybox; } set { _sandboxKybox = value; } }

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            _atmos = FindFirstObjectByType<Atmos>();

            for(int i = 0; i < presets.Length; i++)
            {
                presets[i].IsPlaying = false;
            }

            //Skybox timer Formula: 1 / 60 / your minutes eg: 1 / 60 / 24 = 0.0006944446
            _updateSpeed = 1;
            _updateSpeed /= 60;
            _updateSpeed /= _dayCycleLengthInMinutes;

            //Weather Update Formula 1 * 60 (1 Minute) * Interval (5) = 300s;
            _weatherChangeTimer = 1;
            _weatherChangeTimer *= 60;
            _weatherChangeTimer *= _weatherChangeInterval;

            UpdateSFX();
            InvokeRepeating(nameof(EvaluateWeather), 0, _weatherChangeTimer);
        }

        void Update()
        {
            UpdateSkybox();
            UpdateSun();
            UpdateFog();
            UpdateSFX();
        }

        #endregion

        #region MECHANICS

        void EvaluateWeather()
        {
            foreach (var preset in _weatherPreset.WeatherProbabilities)
            {
                if (_cycle > preset.AtHour)
                {
                    ChangeWeather(preset.GetRandomWeather());
                }
            }
        }

        private void UpdateSkybox()
        {
            _cycle += _updateSpeed * Time.deltaTime;

            if (_cycle > 1)
            {
                _cycle = 0;

                inDay = !inDay;

                if (!inDay)
                {
                    StartSkyTransition(_nightVariations.GetRandomSkybox());
                }
                else
                {
                    StartSkyTransition(_dayVariations.GetRandomSkybox());
                }
            }

            if (_randomizeWeather == WeatherRandomizer.yes)
            {
                if (changingWeather)
                {
                    float target = blendToSecondTexture ? 1f : 0f;

                    float blend = Mathf.MoveTowards(
                        _sandboxKybox.GetFloat("_BlendFactor"),
                        target,
                        .25f * Time.deltaTime
                    );

                    _sandboxKybox.SetFloat("_BlendFactor", blend);

                    if (Mathf.Approximately(blend, target))
                    {
                        changingWeather = false;

                        if (blendToSecondTexture)
                        {
                            _sandboxKybox.SetTexture(
                                "_Tex",
                                _sandboxKybox.GetTexture("_Tex2")
                            );

                            _sandboxKybox.SetFloat("_BlendFactor", 0f);
                        }
                    }
                }
                else
                {
                    Color tint = inDay
                        ? _skyDayTint.Evaluate(_cycle)
                        : _skyNightTint.Evaluate(_cycle);

                    _sandboxKybox.SetColor("_TintColor", tint);
                }
            }
        }
        private void StartSkyTransition(Cubemap newSky)
        {
            if (changingWeather)
                return;

            _sandboxKybox.SetTexture("_Tex2", newSky);

            blendToSecondTexture = true;
            changingWeather = true;
        }
        private void UpdateSun()
        {
            if (inDay)
            {
                Color sc = _sunColor.Evaluate(_cycle);
                _sun.color = sc;

                if (_sun != null)
                {
                    _sun.transform.localRotation = Quaternion.Euler(_cycle * 180, 0, 0);
                }
            }
            else
            {
                Color sc = _moonColor.Evaluate(_cycle);
                _sun.color = sc;

                if (_sun != null)
                {
                    _sun.transform.localRotation = Quaternion.Euler(_cycle * 180, 0, 0);
                }
            }
        }
        private void UpdateFog()
        {
            if (useFog)
            {
                if (inDay)
                {
                    RenderSettings.fogColor = _fogDayColor.Evaluate(_cycle);
                }
                else
                {
                    RenderSettings.fogColor = _fogNightColor.Evaluate(_cycle);
                }
            }
        }
        private void UpdateSFX()
        {
            if (inDay && _atmos != null)
            {
                foreach (var preset in presets)
                {
                    if (_currentWeather == WeatherType.clear)
                    {
                        if (preset.Condition == CycleConditions.day && _cycle > preset.StartAt && !preset.IsPlaying)
                        {
                            _atmos.SetAmbience(preset.TargetClip);
                            preset.IsPlaying = true;
                        }

                        if (preset.Condition == CycleConditions.night && _cycle > preset.StartAt && !preset.IsPlaying)
                        {
                            _atmos.SetAmbience(preset.TargetClip);
                            preset.IsPlaying = true;
                        }
                    }
                }
            }

            if (!inDay)
            {
                foreach(var preset in presets)
                {
                    if(preset.Condition == CycleConditions.day)
                    {
                        preset.IsPlaying = false;
                    }

                    if (preset.Condition == CycleConditions.night)
                    {
                        preset.IsPlaying = false;
                    }
                }
            }
        }

        #endregion

        #region API

        /// <summary>
        /// Force Weather to Update
        /// </summary>
        /// <param name="weather"></param>
        public void ChangeWeather(WeatherType weather)
        {
            if (changingWeather)
                return;

            _currentWeather = weather;
            for (int i = 0; i < presets.Length; i++)
            {
                presets[i].IsPlaying = false;
            }

            if (_currentWeather == WeatherType.clear)
            {
                SkyboxPreset preset = inDay ? _dayVariations : _nightVariations;
                _sandboxKybox.SetTexture("_Tex2", preset.GetRandomSkybox());

                foreach (var pres in presets)
                {
                    if (pres.Condition == CycleConditions.day && inDay && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }

                    if (pres.Condition == CycleConditions.night && !inDay && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }
                }
            }

            if (_currentWeather == WeatherType.cloudy)
            {
                SkyboxPreset preset = inDay ? _cloudyDayVariations : _cloudyNightVariations;
                _sandboxKybox.SetTexture("_Tex2", preset.GetRandomSkybox());

                foreach (var pres in presets)
                {
                    if (pres.Condition == CycleConditions.overcast && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }
                }
            }

            if (_currentWeather == WeatherType.rain)
            {
                SkyboxPreset preset = inDay ? _cloudyDayVariations : _cloudyNightVariations;
                _sandboxKybox.SetTexture("_Tex2", preset.GetRandomSkybox());

                foreach (var pres in presets)
                {
                    if (pres.Condition == CycleConditions.rain && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }
                }
            }

            if (_currentWeather == WeatherType.storm)
            {
                SkyboxPreset preset = inDay ? _cloudyDayVariations : _cloudyNightVariations;
                _sandboxKybox.SetTexture("_Tex2", preset.GetRandomSkybox());

                foreach (var pres in presets)
                {
                    if (pres.Condition == CycleConditions.rain && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }
                }
            }

            if (_currentWeather == WeatherType.windy)
            {
                SkyboxPreset preset = inDay ? _dayVariations : _nightVariations;
                _sandboxKybox.SetTexture("_Tex2", preset.GetRandomSkybox());

                foreach (var pres in presets)
                {
                    if (pres.Condition == CycleConditions.windy && !pres.IsPlaying)
                    {
                        _atmos.SetAmbience(pres.TargetClip);
                        pres.IsPlaying = true;
                    }
                }
            }

            blendToSecondTexture = true;
            changingWeather = true;
        }

        #endregion
    }
}
