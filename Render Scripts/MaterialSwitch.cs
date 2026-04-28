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
using Palexen.Tools;

namespace Palexen.XeenRender.Render
{
    [ScriptDescription("Material Switch", "Changes the material in model (Affects Quality)")]
    [AddComponentMenu("Palexen/Xeen Render/Material Switch")]
    public class MaterialSwitch : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Master")]
        public MeshType _meshType;
        public MaterialType _quality;
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.warningMessage)] public MeshRenderer _mesh;
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.warningMessage)] public SkinnedMeshRenderer _skinnedMesh;

        [MyHeader("Ranges")]
        public Material[] _highEnd;
        public Material[] _midRange;
        public Material[] _lowEnd;

        #endregion

        #region METHODS

        private void Start()
        {
            ChangeAtRuntime();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            MeshRenderer ms = GetComponent<MeshRenderer>();
            SkinnedMeshRenderer sm = GetComponent<SkinnedMeshRenderer>();

            if (ms != null)
            {
                _mesh = ms;
                _meshType = MeshType.meshRenderer;
            }
            
            if(sm != null)
            {
                _skinnedMesh = sm;
                _meshType = MeshType.skinnedMeshRenderer;
            }
        }
#endif

#endregion

        #region MECHANICS

        public void ChangeAtRuntime()
        {
            _quality = RangeManager.Instance.GetRange();
            SwitchMaterials();
        }

        public void SwitchMaterials()
        {
            switch (_quality)
            {
                case MaterialType.highEnd:
                    if (_meshType == MeshType.meshRenderer)
                    {
                        _mesh.materials = _highEnd;
                    }
                    if(_meshType == MeshType.skinnedMeshRenderer)
                    {
                        _skinnedMesh.materials = _highEnd;
                    }
                    break;

                case MaterialType.midRange:
                    if (_meshType == MeshType.meshRenderer)
                    {
                        _mesh.materials = _midRange;
                    }
                    if(_meshType == MeshType.skinnedMeshRenderer)
                    {
                        _skinnedMesh.materials = _midRange;
                    }
                    break;

                case MaterialType.lowEnd:
                    if (_meshType == MeshType.meshRenderer)
                    {
                        _mesh.materials = _lowEnd;
                    }
                    if(_meshType == MeshType.skinnedMeshRenderer)
                    {
                        _skinnedMesh.materials = _lowEnd;
                    }
                    break;
            }
        }

        #endregion
    }
}
