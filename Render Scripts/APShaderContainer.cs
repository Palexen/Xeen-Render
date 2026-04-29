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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Palexen.XeenRender.Scriptables
{
    [ScriptDescription("Alpha to Production Shader Container", "Chose 2 shaders, 1 for test alpha (for bake lighting) and another to production channel!")]
    [CreateAssetMenu(fileName = "New Alpha to Production Shader", menuName = "Palexen/Xeen Render/Nature Shader Containter")]
    public class APShaderContainer : ScriptableObject
    {
        #region VARIABLES

#if PALEXEN_TOOLS
        [MyHeader("Alpha Shader")]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public Shader alphaShader;

        [MyHeader("Production Shader")]
        [FieldColor(FieldPropertyColor.magenta, ShowObjectMessage.errorMessage)] public Shader productionShader;
#else
    public Shader alphaShader;
    public Shader productionShader;
#endif

        #endregion

        #region METHODS

        #endregion
    }

#if UNITY_EDITOR

    #region MAIN CUSTOM EDITOR
    [CustomEditor(typeof(APShaderContainer))]
    [CanEditMultipleObjects]
    public class APShaderContainerEditor : Editor
    {
        private SerializedProperty alphaShader;
        private SerializedProperty productionShader;
        private void OnEnable()
        {
            alphaShader = serializedObject.FindProperty("alphaShader");
            productionShader = serializedObject.FindProperty("productionShader");
        }
        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Alpha to Production \n Shader Container</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize, TextAnchor.MiddleCenter, FontStyle.Bold, 60));

            GUILayout.Box("In the Alpha Shader field, you must select a shader that supports alpha rendering, and in the " +
                "Production Shader field, select your original shader if it doesn't support alpha rendering." +
                "\r\n\r\nThis will help when you perform a traditional bake in Lighting Settings, ensuring that the shadows " +
                "of your vegetation models are well-defined in your scene.",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic, 150));


            serializedObject.Update();

            EditorGUILayout.PropertyField(alphaShader);
            EditorGUILayout.PropertyField(productionShader);

            serializedObject.ApplyModifiedProperties();
        } 
    }
    #endregion

#endif
}