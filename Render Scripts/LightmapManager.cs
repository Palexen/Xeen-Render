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
using System;


#if PALEXEN_TOOLS
using Palexen.Tools;
using Palexen.XeenRender.Render;
#endif

#if PALEXEN_TOOLS
[ScriptDescription("Lightmap Manager", "Manage the type of lighting you will use in your scene.")]
#endif
[AddComponentMenu("Palexen/Xeen Render/Lighting Manager")]
public class LightmapManager : MonoBehaviour
{
    #region VARIABLES

    [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)] public LightmapContainer[] lightmapping;

    public SphericalHarmonicsL2[] _sphericalHarmonics;
    [FieldColor(FieldPropertyColor.yellow, ShowObjectMessage.no)] public LightProbes _currentProbes;

    #endregion

    #region UNITY METHODS

    #endregion

    #region MECHANICS

    [ContextMenu("Get Spherical Harmonics")]
    public void GetSphericalHarmonics()
    {
        if (LightmapSettings.lightProbes != null)
            _sphericalHarmonics = LightmapSettings.lightProbes.bakedProbes;
        else
            _sphericalHarmonics = null;
    }

    [ContextMenu("Get Current Light Probes")]
    public void GetLightProbes()
    {
        _currentProbes = LightmapSettings.lightProbes;
    }

    //Test Area

    [ContextMenu("Set A")]
    public void SetA()
    {
        lightmapping[0].BuildLighting();
    }

    [ContextMenu("Set B")]
    public void SetB()
    {
        lightmapping[1].BuildLighting();
    }

    [ContextMenu("Set C")]
    public void SetC()
    {
        lightmapping[2].BuildLighting();
    }

    #endregion

    #region API

    public void ChangeLightmapsTo(int index)
    {
        lightmapping[index].BuildLighting();
    }

    #endregion
}
