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
#endif

namespace Palexen.XeenRender.Render
{
#if PALEXEN_TOOLS
    [ScriptDescription("ShadowsRenderer", "Improved Monobehavior")]
#endif
    [AddComponentMenu("Palexen/Xeen Render/Shadows Renderer")]
    public class ShadowsRenderer : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private ShadowCastingMode _castShadows;

        #endregion

        #region UNITY METHODS

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region MECHANICS



        #endregion

        #region API

        [ContextMenu("Apply")]
        public void RenderShadows()
        {
            MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].shadowCastingMode = _castShadows;
            }

            SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int a =  0; a < smr.Length; a++)
            {
                smr[a].shadowCastingMode = _castShadows;
            }
        }

        #endregion
    }
}
