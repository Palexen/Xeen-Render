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
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Palexen.XeenRender.Scriptables;

namespace Palexen.XeenRender.Render
{
    public class NatureRenderSolution : EditorWindow
    {
        private APShaderContainer shaders;

        public Material[] natureMaterials;

        GUIStyle maintittle;

        GUIStyle red;
        GUIStyle cyan;
        GUIStyle yellow;
        GUIStyle green;

        GUIStyle cyanBox;
        GUIStyle darkBox;

        private SerializedObject serializedObject;
        private SerializedProperty natureMaterialsProperty;

        [MenuItem("Xeen Render/Nature Render Solution")]
        public static void ShowConfigurations()
        {
            GetWindow<NatureRenderSolution>();
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            natureMaterialsProperty = serializedObject.FindProperty("natureMaterials");
        }

        private void OnGUI()
        {
            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/tree_pro.png") : 
                AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/tree_light.png");

            this.titleContent = new GUIContent("Nature Render", icon);

            maintittle = new GUIStyle(EditorStyles.label);
            maintittle.fontStyle = FontStyle.Bold;
            maintittle.alignment = TextAnchor.MiddleCenter;
            maintittle.fontSize = 25;

            GUILayout.Space(25);

            GUILayout.Label("Nature Settings", maintittle);

            red = new GUIStyle(EditorStyles.label);
            red.normal.textColor = Color.red;

            cyan = new GUIStyle(EditorStyles.label);
            cyan.normal.textColor = Color.cyan;

            darkBox = new GUIStyle(EditorStyles.helpBox);
            darkBox.normal.textColor = Color.black;
            darkBox.fontStyle = FontStyle.Bold;
            darkBox.alignment = TextAnchor.MiddleCenter;

            yellow = new GUIStyle(EditorStyles.helpBox);
            yellow.normal.textColor = Color.yellow;

            green = new GUIStyle(EditorStyles.helpBox);
            green.normal.textColor = Color.green;

            cyanBox = new GUIStyle(EditorStyles.helpBox);
            cyanBox.normal.textColor = Color.cyan;
            cyanBox.fontStyle = FontStyle.Bold;
            cyanBox.alignment = TextAnchor.MiddleCenter;

            GUILayout.Space(25);

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Chose your Nature Shaders profile here", cyanBox, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Chose your Nature Shaders profile here", darkBox, GUILayout.Height(30));
            }

            shaders = (APShaderContainer)EditorGUILayout.ObjectField("Profile", shaders, typeof(APShaderContainer), true);

            GUILayout.Space(25);

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Drop your materials here!", cyanBox, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Drop your materials here!", darkBox, GUILayout.Height(30));
            }

            serializedObject.Update();
            EditorGUILayout.PropertyField(natureMaterialsProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Use this setting to change your custom nature shaders to a shader that is optimized for geometry-based transparency", yellow);
            }
            else
            {
                GUILayout.Box("Use this setting to change your custom nature shaders to a shader that is optimized for geometry-based transparency", darkBox);
            }

            if (GUILayout.Button("Change to Render Lightmaps", PalexenEditorStyles.BigButton))
            {
                if (natureMaterials != null && natureMaterials.Length != 0)
                {
                    for (int i = 0; i < natureMaterials.Length; i++)
                    {
                        if (natureMaterials[i] != null && shaders != null && shaders.alphaShader != null)
                        {
                            natureMaterials[i].shader = shaders.alphaShader;
                        }
                    }

                    Debug.Log("Render Option Applied");
                }
                else
                {
                    Debug.LogWarning("There are no materials to render, or shader container is not assigned. Please assign materials and shader profile.");
                }
            }

            GUILayout.Space(25);

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Use this setting to change your nature shaders from the optimized one to your custom nature shader and see the final result and in production mode", green);
            }
            else
            {
                GUILayout.Box("Use this setting to change your nature shaders from the optimized one to your custom nature shader and see the final result and in production mode", darkBox);
            }

            if (GUILayout.Button("Change to Production", PalexenEditorStyles.BigButton))
            {
                if (natureMaterials != null && natureMaterials.Length != 0)
                {
                    for (int i = 0; i < natureMaterials.Length; i++)
                    {
                        if (natureMaterials[i] != null && shaders != null && shaders.productionShader != null)
                        {
                            natureMaterials[i].shader = shaders.productionShader;
                        }
                    }

                    Debug.Log("Production Mode Applied");
                }
                else
                {
                    Debug.LogWarning("There are no materials to render, or shader container is not assigned. Please assign materials and shader profile.");
                }
            }

            GUILayout.Space(25);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Online Manual", PalexenEditorStyles.BigButton))
            {
                Help.BrowseURL("http://palexen.com");
            }
            GUILayout.Space(25);
        }
    }
}
#endif