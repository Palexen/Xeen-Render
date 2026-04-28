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

namespace Palexen.XeenRender.Scriptables
{
    [ScriptDescription("Alpha to Production Shader Container", "Chose 2 shaders, 1 for test alpha (for bake lighting) and another to production channel!")]
    [CreateAssetMenu(fileName = "New Alpha to Production Shader", menuName = "Palexen/Xeen Render/Nature Shader Containter")]
    public class APShaderContainer : ScriptableObject
    {
        #region VARIABLES

#if PALEXEN_TOOLS
        [MyHeader("Alpha Shader")]
        [FieldColor(FieldPropertyColor.cyan, ShowObjectMessage.errorMessage)] public Shader alphaShader;

        [MyHeader("Production Shader")]
        [FieldColor(FieldPropertyColor.magenta, ShowObjectMessage.errorMessage)] public Shader productionShader;
#else
    public Shader alphaShader;
    public Shader productionShader;
#endif

        #endregion

        #region METHODS

        #endregion
    }
}