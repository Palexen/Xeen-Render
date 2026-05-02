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
using UnityEngine.Rendering;

#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.Scriptables;

#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Render
{
    [CreateAssetMenu(fileName = "New Lightmap Container", menuName = "Palexen/Xeen Render/Lightmap Preset")]
    public class LightmapContainer : ScriptableObject
    {
        [MyHeader("Lightmap Setup")]
        public string managerName = "New Lightmap Container";

        public LightmapEntry[] lightmaps;
        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.errorMessage)] public LightProbes probes;

        LightmapData[] data;

        public SphericalHarmonicsL2[] _sphericalHarmonics;

        LightmapData[] CreateLightMaps()
        {
            LightmapData[] data = new LightmapData[lightmaps.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new LightmapData();
                data[i].lightmapColor = lightmaps[i]._colorMaps;
                data[i].lightmapDir = lightmaps[i]._directionalMaps;
                data[i].shadowMask = lightmaps[i]._shadowMaskMaps;
            }
            return data;
        }

        public SphericalHarmonicsL2[] GetSphericalHarmonics()
        {
            return _sphericalHarmonics;
        }

        #region API

        public void BuildLighting()
        {
            data = CreateLightMaps();
            LightmapSettings.lightmaps = data;
            LightmapSettings.lightProbes = probes;
        }

        #endregion
    }

    #region MAIN CUSTOM EDITOR
#if UNITY_EDITOR

    [CustomEditor(typeof(LightmapContainer))]
    [CanEditMultipleObjects]
    public class LightmapContainerEditor : Editor
    {
        LightmapContainer lp;
        SerializedProperty managerName;
        SerializedProperty lightmaps;
        SerializedProperty probes;

        private void OnEnable()
        {     
            lp = (LightmapContainer)target;
            managerName = serializedObject.FindProperty("managerName");
            lightmaps = serializedObject.FindProperty("lightmaps");
            probes = serializedObject.FindProperty("probes");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Lightmap Preset</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Here you can save lighting data for your current scene, but to do so you need to move all " +
                "your current data to a new, unique folder for this preset, assign all the data, and call it from the <color=magenta>Lighting Manager</color> " +
                "using the <color=green>ChangeLightmapsTo(index);</color> method.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 100));

            Color color = setting.contextSeparatorColor;

            serializedObject.Update();

            PalexenEditorStyles.DrawHorizontalLine(Color.gray);

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(managerName);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray);

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(lightmaps);

            GUILayout.Space(10);

            GUI.color = color;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.color = Color.white;

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(probes);
            GUILayout.Box("When you finish baking the lights in your scene, use the <color=red> \"Extract data from light probes\"</color> " +
                "button, copy the property by right-clicking on the field and selecting <color=green>\"copy,\"</color> then <color=green>paste</color> it " +
                "into this field of this preset.", PalexenEditorStyles.CoolBox(10, TextAnchor.MiddleLeft, FontStyle.Normal, 80));

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
    #endregion
}
