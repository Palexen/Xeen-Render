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
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

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

        [FieldColor(FieldPropertyColor.neonGreen, ShowObjectMessage.errorMessage, true)][SerializeField]
        private ShaderVariantCollection _shaderVariantCollection;
        [SerializeField] private int _shadersPerFrame = 10;

        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.warningMessage)][SerializeField] private Slider _sliderProgress;
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.warningMessage)][SerializeField] private TMP_Text _progressInfo;

        [MyHeader("On warming complete")]
        [SerializeField] private UnityEvent _onWarmingComplete;

        bool _hasWarmedUp = false;

        #endregion

        #region PROPERTIES

        public ShaderVariantCollection ShaderVariants { get { return _shaderVariantCollection; } set { _shaderVariantCollection = value; } }
        public bool IsWarmedUp { get { return _shaderVariantCollection.isWarmedUp; } }

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            StartCoroutine(WarmupAsync());
        }

        private void Update()
        {
            if (_shaderVariantCollection.isWarmedUp)
            {
                if (_shaderVariantCollection.isWarmedUp && !_hasWarmedUp)
                {
                    _hasWarmedUp = true;
                    _onWarmingComplete.Invoke();

                    this.enabled = false;
                }
            }
        }

        IEnumerator WarmupAsync()
        {
            int totalVariants = _shaderVariantCollection.variantCount;
            int processed = 0;

            while (processed < totalVariants)
            {
                _shaderVariantCollection.WarmUpProgressively(_shadersPerFrame);
                processed += _shadersPerFrame;

                if(_sliderProgress != null)
                {
                    _sliderProgress.value = Mathf.Clamp01((float)processed / totalVariants);
                    float p = _sliderProgress.value * 100;
                    _progressInfo.text = p.ToString("F0") + "%";
                }

                yield return null;
            }
        }

        #endregion

        #region MECHANICS



        #endregion

        #region API



        #endregion
    }
}
