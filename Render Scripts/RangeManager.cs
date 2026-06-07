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
using Palexen.Tools;

namespace Palexen.XeenRender.Render
{
    [ScriptDescription("Range Manager", "Switch Materials")]
    [AddComponentMenu("Palexen/Xeen Render/Range Manager")]
    public class RangeManager : MonoBehaviour
    {
        #region VARIABLES

        public static RangeManager Instance;

        [MyHeader("Quality Range")]
        public MaterialType _currentType;

        #endregion

        #region METHODS

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(gameObject.name != "Range Manager")
            {
                gameObject.name = "Range Manager";
            }
        }
#endif

        [Obsolete("Obsolete method, use CurrentRange instead to perform a current query.")]
        public MaterialType GetRange()
        {
            return _currentType;
        }

        [Obsolete("Obsolete method, use the Range property instead")]
        public void SetRange(MaterialType type)
        {
            _currentType = type;

            MaterialSwitch[] allRenderers = FindObjectsByType<MaterialSwitch>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for(int i = 0; i <  allRenderers.Length; i++)
            {
                allRenderers[i].ChangeAtRuntime();
            }
        }

        /// <summary>
        /// Check the current status of the range
        /// </summary>
        public MaterialType CurrentRange { get { return _currentType; } }

        /// <summary>
        /// Modifies the current range and updates all materials of all mesh renderers or skinned mesh 
        /// renderers that the Material Switch has in its configuration.
        /// </summary>
        public MaterialType Range
        {
            set
            {
                _currentType = value;

                MaterialSwitch[] allRenderers = FindObjectsByType<MaterialSwitch>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                for (int i = 0; i < allRenderers.Length; i++)
                {
                    allRenderers[i].ChangeAtRuntime();
                }
            }
        }

        #endregion
    }
}
