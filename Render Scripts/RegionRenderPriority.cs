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
#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.XeenRender.Render
{
#if PALEXEN_TOOLS
    [ScriptDescription("Region Priority", "Set priority in meshes inside of this transform")]
#endif
    [AddComponentMenu("Palexen/Xeen Render/Region Render Priority")]
    public class RegionRenderPriority : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Render Priority")]
        [SerializeField] private float _priority = 1f;

        [MyHeader("Gizmo Settings")]
        [SerializeField] private Color _gizmoColor = Color.green;

        #endregion

        #region UNITY METHODS

        private void OnDrawGizmos()
        {
            Bounds combinedBounds = GetCombinedBounds(gameObject);

            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireCube(combinedBounds.center, combinedBounds.size);
        }

        private void OnDrawGizmosSelected()
        {
            Bounds combinedBounds = GetCombinedBounds(gameObject);

            Gizmos.color = new(_gizmoColor.r, _gizmoColor.g, _gizmoColor.b, _gizmoColor.a / 4);
            Gizmos.DrawCube(combinedBounds.center, combinedBounds.size);
        }

        #endregion

        #region MECHANICS

        Bounds GetCombinedBounds(GameObject rootObj)
        {
            Renderer[] renderers = rootObj.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                return new Bounds(rootObj.transform.position, Vector3.zero);
            }

            Bounds combinedBounds = new(renderers[0].bounds.center, Vector3.zero);
            foreach (Renderer renderer in renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
            return combinedBounds;
        }

        #endregion

        #region API

#if UNITY_EDITOR
        [ContextMenu("Apply Render Priority")]
        public void ApplyPriority()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer renderer in renderers)
            {
                renderer.scaleInLightmap = _priority;
            }
        }

#endif

#endregion
    }
}
