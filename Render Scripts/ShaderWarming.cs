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
using UnityEngine.Events;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.XeenRender
{
#if PALEXEN_TOOLS
    [ScriptDescription("Shader Warming", "Pre warm shaders")]
#endif
    [AddComponentMenu("Palexen/Xeen Render/Shader Warming")]
    public class ShaderWarming : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Game Shaders")]
        [FieldColor(FieldPropertyColor.neonGreen, ShowObjectMessage.errorMessage, true)][SerializeField]
        private ShaderVariantCollection _shaderVariantCollection;

        [MyHeader("On warming complete")]
        [SerializeField] private UnityEvent _onWarmingComplete;

        bool _hasWarmedUp = false;

        #endregion

        #region PROPERTIES

        public bool IsWarmedUp { get { return _shaderVariantCollection.isWarmedUp; } }

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            if (!_shaderVariantCollection.isWarmedUp)
            {
                _shaderVariantCollection.WarmUp();
            }
            else
            {
                _hasWarmedUp = true;
                _onWarmingComplete.Invoke();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(_shaderVariantCollection.isWarmedUp)
            {
                if (!_hasWarmedUp)
                {
                    _onWarmingComplete.Invoke();
                    _hasWarmedUp = true;
                }
            }
        }

        #endregion

        #region MECHANICS



        #endregion

        #region API



        #endregion
    }
}
