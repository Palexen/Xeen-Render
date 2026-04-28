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
using UnityEditor;
using UnityEngine;
using Palexen.Tools;
using System.Collections.Generic;

namespace Palexen.XeenRender.Render
{
    public class MeshLightProbes : EditorWindow
    {
        [FieldColor(FieldPropertyColor.cyan)]
        public MeshFilter[] _meshTarget;

        [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.warningMessage)]
        public LightProbeGroup _targetLighProbeGroup;
        public float probeHeightOffset = 1f;
        float minDistanceBetweenProbes = 2;

        SerializedObject serializedObject;
        SerializedProperty meshTargetProp;
        SerializedProperty targetLPGProp;

        GUIStyle maintittle;

        GUIStyle red;
        GUIStyle cyan;
        GUIStyle yellow;
        GUIStyle green;

        GUIStyle cyanBox;
        GUIStyle darkBox;

        public int targetLayerIndex = 0;

        private List<Vector3> _previewPositions = new();
        private Vector2 _meshListScroll;
        public float previewGizmoScale = .3f;
        bool isPreviewing;

        [MenuItem("Xeen Render/Auto Light Probes (Mesh)")]
        public static void ShowWindow()
        {
            GetWindow<MeshLightProbes>("Mesh Light Probe Generator");
        }

        void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            meshTargetProp = serializedObject.FindProperty(nameof(_meshTarget));
            targetLPGProp = serializedObject.FindProperty(nameof(_targetLighProbeGroup));

            SceneView.duringSceneGui += OnSceneGUI;
            serializedObject = new SerializedObject(this);
            meshTargetProp = serializedObject.FindProperty(nameof(_meshTarget));
            targetLPGProp = serializedObject.FindProperty(nameof(_targetLighProbeGroup));
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            isPreviewing = false;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (_previewPositions == null) return;

            Handles.color = Color.cyan;
            foreach (var pos in _previewPositions)
            {
                Handles.SphereHandleCap(0, pos, Quaternion.identity, previewGizmoScale, EventType.Repaint);
            }
        }

        void OnGUI()
        {
            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/mesh_probe_pro.png") :
                AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/mesh_probe_light.png");

            this.titleContent = new GUIContent("ALPG (Mesh)", icon);

            maintittle = new GUIStyle(EditorStyles.label);
            maintittle.fontStyle = FontStyle.Bold;
            maintittle.alignment = TextAnchor.MiddleCenter;
            maintittle.fontSize = 25;

            red = new GUIStyle(EditorStyles.label);
            red.normal.textColor = Color.red;

            cyan = new GUIStyle(EditorStyles.label);
            cyan.normal.textColor = Color.cyan;

            yellow = new GUIStyle(EditorStyles.helpBox);
            yellow.normal.textColor = Color.yellow;
            yellow.fontStyle = FontStyle.Bold;
            yellow.alignment = TextAnchor.MiddleCenter;

            green = new GUIStyle(EditorStyles.helpBox);
            green.normal.textColor = Color.green;

            cyanBox = new GUIStyle(EditorStyles.helpBox);
            cyanBox.normal.textColor = Color.cyan;
            cyanBox.fontStyle = FontStyle.Bold;
            cyanBox.alignment = TextAnchor.MiddleCenter;

            darkBox = new GUIStyle(EditorStyles.helpBox);
            darkBox.normal.textColor = Color.black;
            darkBox.fontStyle = FontStyle.Bold;
            darkBox.alignment = TextAnchor.MiddleCenter;

            GUILayout.Space(25);
            GUILayout.Label("Light Probes On Mesh", maintittle);
            GUILayout.Space(25);

            serializedObject.Update();

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Select LayerMask in meshes for Scan when generate", cyanBox, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Select LayerMask in meshes for Scan when generate", darkBox, GUILayout.Height(30));
            }
            targetLayerIndex = EditorGUILayout.LayerField("Target Layer", targetLayerIndex);

            GUILayout.Space(15);

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Chose your Light Probes Group here (If not, it will create automatically)", yellow, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Chose your Light Probes Group here (If not, it will create automatically)", darkBox, GUILayout.Height(30));
            }
            EditorGUILayout.PropertyField(targetLPGProp);
            GUILayout.Box("Light Probes Offset and distance", GUILayout.Height(30));
            probeHeightOffset = EditorGUILayout.FloatField("Probe Offset", probeHeightOffset);
            minDistanceBetweenProbes = EditorGUILayout.FloatField("Probe Spacing", minDistanceBetweenProbes);

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Manage your mesh filter model here", cyanBox, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Manage your mesh filter model here", darkBox, GUILayout.Height(30));
            }
            GUILayout.Label("Meshes Found:", maintittle);
            _meshListScroll = EditorGUILayout.BeginScrollView(_meshListScroll, GUILayout.Height(150));

            if (_meshTarget != null)
            {
                for (int i = 0; i < _meshTarget.Length; i++)
                {
                    if (_meshTarget[i] == null) continue;

                    _meshTarget[i] = (MeshFilter)EditorGUILayout.ObjectField(
                        _meshTarget[i],
                        typeof(MeshFilter),
                        true
                    );
                }
            }
            else
            {
                GUILayout.Space(15);
                GUILayout.Label("No data yet", maintittle);
            }

            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            GUILayout.Box("Preview Gizmo Scale", GUILayout.Height(30));
            previewGizmoScale = EditorGUILayout.Slider(previewGizmoScale, .3f, 2f);
            if (GUILayout.Button("Preview Light Probes", PalexenEditorStyles.BigButton))
            {
                GeneratePreview(targetLayerIndex);
                isPreviewing = true;
            }

            if (GUILayout.Button("Scan Meshes", PalexenEditorStyles.BigButton))
            {
                ScanMeshes(targetLayerIndex);
            }

            if (GUILayout.Button("Generate Light Probes", PalexenEditorStyles.BigButton))
            {
                GenerateProbes(targetLayerIndex);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnInspectorUpdate()
        {
            if (isPreviewing)
            {
                GeneratePreview(targetLayerIndex);
            }
        }

        void GeneratePreview(int targetLayerIndex)
        {
            _previewPositions.Clear();

            MeshFilter[] allMeshFilters = FindObjectsOfType<MeshFilter>();

            float minDistanceSqr = minDistanceBetweenProbes * minDistanceBetweenProbes;

            List<Vector3> filteredPreview = new List<Vector3>();

            foreach (var mf in allMeshFilters)
            {
                if (mf.gameObject.layer != targetLayerIndex) continue;
                if (mf.sharedMesh == null) continue;

                foreach (var vertex in mf.sharedMesh.vertices)
                {
                    Vector3 worldPos = mf.transform.TransformPoint(vertex) +
                        new Vector3(vertex.x * probeHeightOffset, vertex.y * probeHeightOffset, vertex.z * probeHeightOffset);

                    bool tooClose = false;
                    foreach (var pos in filteredPreview)
                    {
                        if ((pos - worldPos).sqrMagnitude < minDistanceSqr)
                        {
                            tooClose = true;
                            break;
                        }
                    }

                    if (!tooClose)
                        filteredPreview.Add(worldPos);
                }
            }

            _previewPositions.AddRange(filteredPreview);

            if (_previewPositions.Count == 0)
            {
                Debug.Log("<color=cyan>No preview data found. Make sure the correct layer is selected.</color>");
            }
            else
            {
                Debug.Log($"Preview generated with <color=yellow>{_previewPositions.Count}</color> probes (approx).");
            }

            SceneView.RepaintAll();
        }


        void ScanMeshes(int targetLayer)
        {
            MeshFilter[] allMeshFilters = FindObjectsOfType<MeshFilter>();

            List<MeshFilter> foundMeshes = new();

            foreach (var mf in allMeshFilters)
            {
                if (mf.gameObject.layer != targetLayer)
                    continue;

                if (mf.sharedMesh == null)
                    continue;

                foundMeshes.Add(mf);
            }

            _meshTarget = foundMeshes.ToArray();

            Debug.Log($"<color=cyan>{_meshTarget.Length} MeshFilters</color> with <color=magenta>{LayerMask.LayerToName(targetLayer)}</color> Layer.");
        }

        void GenerateProbes(int targetLayerIndex)
        {
            MeshFilter[] allMeshFilters = FindObjectsOfType<MeshFilter>();
            HashSet<Vector3> uniqueVertices = new HashSet<Vector3>();

            float minDistanceSqr = minDistanceBetweenProbes * minDistanceBetweenProbes;

            List<Vector3> filteredPositions = new List<Vector3>();

            foreach (var mf in allMeshFilters)
            {
                if (mf.gameObject.layer != targetLayerIndex) continue;
                if (mf.sharedMesh == null) continue;

                Mesh mesh = mf.sharedMesh;

                foreach (var vertex in mesh.vertices)
                {
                    Vector3 worldPos = mf.transform.TransformPoint(vertex) +
                        new Vector3(vertex.x * probeHeightOffset, vertex.y * probeHeightOffset, vertex.z * probeHeightOffset);

                    bool tooClose = false;

                    foreach (var pos in filteredPositions)
                    {
                        if ((pos - worldPos).sqrMagnitude < minDistanceSqr)
                        {
                            tooClose = true;
                            break;
                        }
                    }

                    if (!tooClose)
                        filteredPositions.Add(worldPos);
                }
            }

            if (_targetLighProbeGroup == null)
            {
                GameObject newLPGObject = new GameObject("Generated LightProbeGroup");
                _targetLighProbeGroup = newLPGObject.AddComponent<LightProbeGroup>();
            }

            _targetLighProbeGroup.probePositions = filteredPositions.ToArray();

            Debug.Log($"Placed <color=magenta>{filteredPositions.Count}</color> optimized <color=yellow>light probes</color> from <color=magenta>{LayerMask.LayerToName(targetLayerIndex)}</color> layer.");
        }
    }

    public class TerrainLightProbes : EditorWindow
    {
        [FieldColor(FieldPropertyColor.green, ShowObjectMessage.errorMessage)] 
        public Terrain targetTerrain;
        public float probeSpacing = 5f;
        public float probeHeightOffset = 1f;
        public int probeSteps = 1;

        public float previewGizmoScale = .3f;

        SerializedObject serializedObject;
        SerializedProperty terrainTargetProp;

        GUIStyle maintittle;

        GUIStyle red;
        GUIStyle cyan;
        GUIStyle yellow;
        GUIStyle green;

        GUIStyle cyanBox;
        GUIStyle darkBox;

        private List<Vector3> _previewPositions = new();
        bool isPreviewing;

        [MenuItem("Xeen Render/Auto Light Probes (Terrain)")]
        public static void ShowWindow()
        {
            GetWindow<TerrainLightProbes>("Terrain Light Probe Generator");
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            terrainTargetProp = serializedObject.FindProperty(nameof(targetTerrain));

            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            isPreviewing = false;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (_previewPositions == null) return;

            Handles.color = Color.green;
            foreach (var pos in _previewPositions)
            {
                Handles.SphereHandleCap(0, pos, Quaternion.identity, previewGizmoScale, EventType.Repaint);
            }
        }

        void OnGUI()
        {
            Texture icon = EditorGUIUtility.isProSkin ? AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/terrain_probe_pro.png") :
                AssetDatabase.LoadAssetAtPath<Texture>
                ("Packages/com.xeenrender.tools/Editor Default Resources/terrain_probe_light.png");
            this.titleContent = new GUIContent("ALPG (Terrain)", icon);

            maintittle = new GUIStyle(EditorStyles.label);
            maintittle.fontStyle = FontStyle.Bold;
            maintittle.alignment = TextAnchor.MiddleCenter;
            maintittle.fontSize = 25;

            red = new GUIStyle(EditorStyles.label);
            red.normal.textColor = Color.red;

            cyan = new GUIStyle(EditorStyles.label);
            cyan.normal.textColor = Color.cyan;

            yellow = new GUIStyle(EditorStyles.helpBox);
            yellow.normal.textColor = Color.yellow;
            yellow.fontStyle = FontStyle.Bold;
            yellow.alignment = TextAnchor.MiddleCenter;

            green = new GUIStyle(EditorStyles.helpBox);
            green.normal.textColor = Color.green;

            cyanBox = new GUIStyle(EditorStyles.helpBox);
            cyanBox.normal.textColor = Color.cyan;
            cyanBox.fontStyle = FontStyle.Bold;
            cyanBox.alignment = TextAnchor.MiddleCenter;

            darkBox = new GUIStyle(EditorStyles.helpBox);
            darkBox.normal.textColor = Color.black;
            darkBox.fontStyle = FontStyle.Bold;
            darkBox.alignment = TextAnchor.MiddleCenter;

            GUILayout.Space(25);
            GUILayout.Label("Light Probes On Terrain", maintittle);
            GUILayout.Space(25);

            serializedObject.Update();

            if (EditorGUIUtility.isProSkin)
            {
                GUILayout.Box("Chose your Terrain here", cyanBox, GUILayout.Height(30));
            }
            else
            {
                GUILayout.Box("Chose your Terrain here", darkBox, GUILayout.Height(30));
            }

            targetTerrain = (Terrain)EditorGUILayout.ObjectField("Target Terrain", targetTerrain, typeof(Terrain), true);
            probeSpacing = EditorGUILayout.FloatField("Probe Spacing", probeSpacing);
            probeHeightOffset = EditorGUILayout.FloatField("Probe Height Offset", probeHeightOffset);
            GUILayout.Box("Cells (it will duplicated when generate, it's just a preview)");
            probeSteps = (int)EditorGUILayout.Slider(probeSteps, 1, 4);

            GUILayout.FlexibleSpace();
            GUILayout.Box("Preview Gizmo Scale", GUILayout.Height(30));
            previewGizmoScale = EditorGUILayout.Slider(previewGizmoScale, .3f, 2f);

            GUILayout.Space(15);

            if (GUILayout.Button("Preview Light Probes", PalexenEditorStyles.BigButton))
            {
                GeneratePreview();
                isPreviewing = true;
            }


            if (GUILayout.Button("Create Light Probes", PalexenEditorStyles.BigButton))
            {
                GenerateProbes();
            }
        }

        private void OnInspectorUpdate()
        {
            if (isPreviewing)
            {
                GeneratePreview();
            }
        }

        void GeneratePreview()
        {
            _previewPositions.Clear();

            if (targetTerrain == null) return;

            TerrainData terrainData = targetTerrain.terrainData;
            Vector3 terrainPos = targetTerrain.transform.position;

            int step = 4;

            for (float x = 0; x < terrainData.size.x; x += probeSpacing * step)
            {
                for (float z = 0; z < terrainData.size.z; z += probeSpacing * step)
                {
                    float y = terrainData.GetHeight(
                        (int)(x / terrainData.size.x * terrainData.heightmapResolution),
                        (int)(z / terrainData.size.z * terrainData.heightmapResolution)
                    );

                    if (probeSteps == 1)
                    {
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset, z));
                    }
                    if (probeSteps == 2)
                    {
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 5, z));
                    }
                    if (probeSteps == 3)
                    {
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 5, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 10, z));
                    }
                    if (probeSteps == 4)
                    {
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 5, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 10, z));
                        _previewPositions.Add(terrainPos + new Vector3(x, y + probeHeightOffset + 15, z));
                    }
                }
            }

            SceneView.RepaintAll();
        }

        void GenerateProbes()
        {
            if (targetTerrain == null)
            {
                Debug.LogError("Please assign a target terrain.");
                return;
            }

            GameObject probeGroupObject = new("Terrain Light Probe Group");
            LightProbeGroup probeGroup = probeGroupObject.AddComponent<LightProbeGroup>();
            List<Vector3> probePositions = new();

            TerrainData terrainData = targetTerrain.terrainData;
            Vector3 terrainPos = targetTerrain.transform.position;

            for (float x = 0; x < terrainData.size.x; x += probeSpacing)
            {
                for (float z = 0; z < terrainData.size.z; z += probeSpacing)
                {
                    float y = terrainData.GetHeight((int)(x / terrainData.size.x * terrainData.heightmapResolution),
                                                    (int)(z / terrainData.size.z * terrainData.heightmapResolution));
                    if (probeSteps == 1)
                    {
                        Vector3 worldPos = terrainPos + new Vector3(x, y + probeHeightOffset, z);
                        probePositions.Add(worldPos);
                    }
                    if (probeSteps == 2)
                    {
                        Vector3 worldPos0 = terrainPos + new Vector3(x, y + probeHeightOffset, z);
                        probePositions.Add(worldPos0);

                        Vector3 worldPos1 = terrainPos + new Vector3(x, y + probeHeightOffset + 5, z);
                        probePositions.Add(worldPos1);
                    }

                    if (probeSteps == 3)
                    {
                        Vector3 worldPos0 = terrainPos + new Vector3(x, y + probeHeightOffset, z);
                        probePositions.Add(worldPos0);

                        Vector3 worldPos1 = terrainPos + new Vector3(x, y + probeHeightOffset + 5, z);
                        probePositions.Add(worldPos1);

                        Vector3 worldPos2 = terrainPos + new Vector3(x, y + probeHeightOffset + 10, z);
                        probePositions.Add(worldPos2);
                    }

                    if (probeSteps == 4)
                    {
                        Vector3 worldPos0 = terrainPos + new Vector3(x, y + probeHeightOffset, z);
                        probePositions.Add(worldPos0);

                        Vector3 worldPos1 = terrainPos + new Vector3(x, y + probeHeightOffset + 5, z);
                        probePositions.Add(worldPos1);

                        Vector3 worldPos2 = terrainPos + new Vector3(x, y + probeHeightOffset + 10, z);
                        probePositions.Add(worldPos2);

                        Vector3 worldPos3 = terrainPos + new Vector3(x, y + probeHeightOffset + 15, z);
                        probePositions.Add(worldPos3);
                    }
                }
            }

            probeGroup.probePositions = probePositions.ToArray();
            Debug.Log($"Generated <color=magenta>{probePositions.Count}</color> <color=yellow>light probes</color> on the <color=#C4FF5E>Terrain</color>.");
        }
    }
}
#endif