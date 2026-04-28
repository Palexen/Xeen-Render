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
#if UNITY_EDITOR
using UnityEngine;
using Palexen.Tools;
using UnityEditor;
using Palexen.Scriptables;

using UnityEditor.Animations;

namespace Palexen.XeenRender
{
    [AddComponentMenu("Palexen/Xeen Render/Animation Recorder")]
    public class AnimationRecorder : MonoBehaviour
    {
        #region VARIABLES

        [FieldColor(FieldPropertyColor.red, ShowObjectMessage.errorMessage)] public AnimationClip _RAWAnimationClip;

        private GameObjectRecorder m_Recorder;

        #endregion

        #region GLOBAL METHODS

        void Start()
        {
            // Create recorder and record the script GameObject.
            m_Recorder = new GameObjectRecorder(gameObject);

            // Bind all the Transforms on the GameObject and all its children.
            m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        }

        void LateUpdate()
        {
            if (_RAWAnimationClip == null)
                return;

            // Take a snapshot and record all the bindings values for this frame.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }

        void OnDisable()
        {
            if (_RAWAnimationClip == null)
                return;

            if (m_Recorder.isRecording)
            {
                // Save the recorded session to the clip.
                m_Recorder.SaveToClip(_RAWAnimationClip);
            }
        }

        #endregion
    }

    #region RECORDER CUSTOM EDITOR
    [CustomEditor(typeof(AnimationRecorder))]
    public class AnimatorRecorderEditor : Editor
    {
        AnimationRecorder ar;
        SerializedProperty _animationClip;

        private void OnEnable()
        {
            ar = (AnimationRecorder)target;
            _animationClip = serializedObject.FindProperty("_RAWAnimationClip");
        }

        public override void OnInspectorGUI()
        {
            string customMessagePath = "Environment Settings/Palexen Environment Settings";

            CustomEnvironment setting = Resources.Load<CustomEnvironment>(customMessagePath);

            GUILayout.Label($"<color={"#" + setting.scriptTitleColor.ConvertToHex()}>Animation Recorder</color>",
                PalexenEditorStyles.CoolTitle(setting.scriptTitleSize));

            GUILayout.Box("Record everything that happens in play mode on this GameObject",
                PalexenEditorStyles.CoolBox(12, TextAnchor.MiddleCenter, FontStyle.BoldAndItalic));

            serializedObject.Update();

            EditorGUILayout.PropertyField(_animationClip);
            PalexenEditorStyles.DrawHorizontalLine(Color.gray);

            if (EditorApplication.isPlaying)
            {
                if (ar._RAWAnimationClip != null)
                {
                    GUI.color = Color.red;
                    GUILayout.Box("Recording •", PalexenEditorStyles.CoolBox(20, TextAnchor.MiddleCenter, FontStyle.Bold));
                }
            }

            if (ar._RAWAnimationClip == null)
            {
                if (!EditorApplication.isPlaying)
                {
                    GUI.color = Color.yellow;
                    GUILayout.Box("Standby", PalexenEditorStyles.CoolBox(20, TextAnchor.MiddleCenter, FontStyle.Bold));
                }
            }
            else
            {
                if (!EditorApplication.isPlaying)
                {
                    GUI.color = Color.cyan;
                    GUILayout.Box("Ready!", PalexenEditorStyles.CoolBox(20, TextAnchor.MiddleCenter, FontStyle.Bold));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    #endregion
}
#endif