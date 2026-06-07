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

        public MaterialType GetRange()
        {
            return _currentType;
        }

        public void SetRange(MaterialType type)
        {
            _currentType = type;

            MaterialSwitch[] allRenderers = FindObjectsByType<MaterialSwitch>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for(int i = 0; i <  allRenderers.Length; i++)
            {
                allRenderers[i].ChangeAtRuntime();
            }
        }

        #endregion
    }
}
