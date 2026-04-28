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
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using Palexen.Scriptables;


#endif

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.XeenRender.Render
{
    #if PALEXEN_TOOLS
    [ScriptDescription("HDR Capture", "Use 360 plugin to render HDR image")]
#endif
    [AddComponentMenu("Palexen/Xeen Render/HDR Capture")]
    public class HDRCapture : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Setup")]
        public string _renderName = "Render_0";
        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] public Camera _targetCamera;

        int _res = 2048;
        bool _saveAsJPEG = true;
        string path;

        public HDRResolution _resolution = HDRResolution._2048;
        public format _saveAs;

        #endregion

        #region UNITY METHODS



        #endregion

        #region MECHANICS

        [ContextMenu("Render HDR")]
        public void CaptureIt()
        {
            switch (_resolution)
            {
                case HDRResolution._512:
                    _res = 512;
                    break;

                case HDRResolution._1024:
                    _res = 1024;
                    break;
                default:

                case HDRResolution._2048:
                    _res = 2048;
                    break;

                case HDRResolution._4096:
                    _res = 4096;
                    break;
            }

            switch (_saveAs)
            {
                case format.PNG:
                    _saveAsJPEG = false;
                    break;

                case format.JPG:
                    _saveAsJPEG = true;
                    break;
            }

            byte[] bytes = I360Render.Capture(_res, _saveAsJPEG, _targetCamera);
            if (bytes != null)
            {
                string fileName = _renderName + (_saveAsJPEG ? ".jpeg" : ".png");
                string fullPath = Path.Combine(path, fileName);

                File.WriteAllBytes(fullPath, bytes);

                Debug.Log("Capture has been saved to <color=cyan>" + fullPath + "</color>");
            }
        }

        public void SelectPath(string newPath)
        {
            path = newPath;
        }

        public void CreateCamera()
        {
            gameObject.AddComponent<Camera>();
            _targetCamera = transform.GetComponent<Camera>();
        }

        #endregion

        #region API

    

        #endregion
    }

    #region ENUMS
    public enum HDRResolution { _512, _1024, _2048, _4096 }
    public enum format { PNG, JPG }
    #endregion

#if UNITY_EDITOR

    [CustomEditor(typeof(HDRCapture))]
    public class RenderHDR : Editor
    {
        HDRCapture hdr;

        SerializedProperty _RName;
        SerializedProperty _cam;
        SerializedProperty _res;
        SerializedProperty _saveAs;

        bool weHavePath;

        private void OnEnable()
        {
            hdr = (HDRCapture)target;

            _cam = serializedObject.FindProperty("_targetCamera");
            _RName = serializedObject.FindProperty("_renderName");
            _res = serializedObject.FindProperty("_resolution");
            _saveAs = serializedObject.FindProperty("_saveAs");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            string customMessagePath = "Environment Settings/Palexen Environment Settings";
            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>360 Capture</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Take 360 screenshot",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_RName);
            EditorGUILayout.PropertyField(_cam);
            EditorGUILayout.PropertyField(_res);
            EditorGUILayout.PropertyField(_saveAs);



            serializedObject.ApplyModifiedProperties();

            if (!weHavePath || hdr._targetCamera == null)
            {
                PalexenEditorStyles.DrawHorizontalLine(Color.gray, 5);
            }
            else
            {
                GUI.color = setting.contextSeparatorColor;
                EditorGUILayout.HelpBox("", MessageType.None);
                GUI.color = Color.white;
            }

            if (!weHavePath)
            {
                if (GUILayout.Button("Select Path to Save"))
                {
                    string selectedPath = EditorUtility.OpenFolderPanel("Select Path", "", "");

                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        hdr.SelectPath(selectedPath);
                        weHavePath = true;
                    }
                }
            }

            if(hdr._targetCamera == null)
            {
                if (GUILayout.Button("Place Camera here", PalexenEditorStyles.BigButton))
                {
                    hdr.CreateCamera();
                }
            }

            if (weHavePath && hdr._targetCamera != null)
            {
                if (GUILayout.Button("Render HDR", PalexenEditorStyles.BigButton))
                {
                    hdr.CaptureIt();
                    AssetDatabase.Refresh();
                }
            }
        }
    }

#endif
}
