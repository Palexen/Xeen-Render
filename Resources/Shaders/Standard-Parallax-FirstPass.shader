// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Nature/Terrain/Standard-WithParallax" {
    Properties {
        _OcclusionStrength ("Global Occlusion Strength", Range(0, 1)) = 1.0
        _ParallaxStrength ("Global Parallax Height", Range(0.005, 0.05)) = 0.02
        _ParallaxOffset ("Global Parallax Offset", Range(0.005, 0.05)) = 0.02

        // used in fallback on old cards & base map
        [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "Queue" = "Geometry-100"
            "RenderType" = "Opaque"
        }

        CGPROGRAM
        #pragma surface surf StandardSpecular vertex:SplatmapVertCustom finalcolor:SplatmapFinalColorCustom finalgbuffer:SplatmapFinalGBufferCustom addshadow fullforwardshadows
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
        #pragma multi_compile_fog 
        #pragma target 3.0
        
        #include "UnityCG.cginc"
        #include "UnityPBSLighting.cginc"

        #pragma multi_compile_local __ _ALPHATEST_ON
        #pragma multi_compile_local __ _NORMALMAP

        struct Input {
            float4 tc;
            float3 viewDir; 
            #ifndef TERRAIN_BASE_PASS
                UNITY_FOG_COORDS(0)
            #endif
        };

        sampler2D _Control;
        float4 _Control_ST;
        float4 _Control_TexelSize;
        sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
        float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;

        #ifdef _NORMALMAP
            sampler2D _Normal0, _Normal1, _Normal2, _Normal3;
            float _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3;
        #endif

        #if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X)
            sampler2D _TerrainHeightmapTexture;
            sampler2D _TerrainNormalmapTexture;
            float4    _TerrainHeightmapRecipSize;   
            float4    _TerrainHeightmapScale;       
        #endif

        UNITY_INSTANCING_BUFFER_START(Terrain)
            UNITY_DEFINE_INSTANCED_PROP(float4, _TerrainPatchInstanceData) 
        UNITY_INSTANCING_BUFFER_END(Terrain)

        half _Metallic0; half _Metallic1; half _Metallic2; half _Metallic3;
        half _Smoothness0; half _Smoothness1; half _Smoothness2; half _Smoothness3;

        half _OcclusionStrength;
        half _ParallaxStrength;

        void SplatmapVertCustom(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

        #if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X)
            float2 patchVertex = v.vertex.xy;
            float4 instanceData = UNITY_ACCESS_INSTANCED_PROP(Terrain, _TerrainPatchInstanceData);

            float4 uvscale = instanceData.z * _TerrainHeightmapRecipSize;
            float4 uvoffset = instanceData.xyxy * uvscale;
            uvoffset.xy += 0.5f * _TerrainHeightmapRecipSize.xy;
            float2 sampleCoords = (patchVertex.xy * uvscale.xy + uvoffset.xy);

            float hm = UnpackHeightmap(tex2Dlod(_TerrainHeightmapTexture, float4(sampleCoords, 0, 0)));
            v.vertex.xz = (patchVertex.xy + instanceData.xy) * _TerrainHeightmapScale.xz * instanceData.z;  
            v.vertex.y = hm * _TerrainHeightmapScale.y;
            v.vertex.w = 1.0f;

            v.texcoord.xy = (patchVertex.xy * uvscale.zw + uvoffset.zw);
            v.texcoord3 = v.texcoord2 = v.texcoord1 = v.texcoord;

            float3 nor = tex2Dlod(_TerrainNormalmapTexture, float4(sampleCoords, 0, 0)).xyz;
            v.normal = 2.0f * nor - 1.0f;
        #endif

            v.tangent.xyz = cross(v.normal, float3(0,0,1));
            v.tangent.w = -1;

            data.tc.xy = v.texcoord.xy;
        #ifndef TERRAIN_BASE_PASS
            float4 pos = UnityObjectToClipPos(v.vertex);
            UNITY_TRANSFER_FOG(data, pos);
        #endif
        }

        void SplatmapFinalColorCustom(Input IN, SurfaceOutputStandardSpecular o, inout fixed4 color)
        {
            color *= o.Alpha;
            #ifdef TERRAIN_SPLAT_ADDPASS
                UNITY_APPLY_FOG_COLOR(IN.fogCoord, color, fixed4(0,0,0,0));
            #else
                UNITY_APPLY_FOG(IN.fogCoord, color);
            #endif
        }

        void SplatmapFinalGBufferCustom(Input IN, SurfaceOutputStandardSpecular o, inout half4 outGBuffer0, inout half4 outGBuffer1, inout half4 outGBuffer2, inout half4 emission)
        {
            UnityStandardDataApplyWeightToGbuffer(outGBuffer0, outGBuffer1, outGBuffer2, o.Alpha);
            emission *= o.Alpha;
        }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
            float2 splatUV = (IN.tc.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
            half4 splat_control = tex2D(_Control, splatUV);
            half weight = dot(splat_control, half4(1,1,1,1));

            #if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
                clip(weight == 0.0f ? -1 : 1);
            #endif

            splat_control /= (weight + 1e-3f);

            float2 uv0 = TRANSFORM_TEX(IN.tc.xy, _Splat0);
            float2 uv1 = TRANSFORM_TEX(IN.tc.xy, _Splat1);
            float2 uv2 = TRANSFORM_TEX(IN.tc.xy, _Splat2);
            float2 uv3 = TRANSFORM_TEX(IN.tc.xy, _Splat3);

            #ifdef _NORMALMAP
                // Alpha Chanel
                half h0 = tex2D(_Normal0, uv0).a;
                half h1 = tex2D(_Normal1, uv1).a;
                half h2 = tex2D(_Normal2, uv2).a;
                half h3 = tex2D(_Normal3, uv3).a;

                // Parallax Offset
                uv0 += ParallaxOffset(h0, _ParallaxStrength, IN.viewDir);
                uv1 += ParallaxOffset(h1, _ParallaxStrength, IN.viewDir);
                uv2 += ParallaxOffset(h2, _ParallaxStrength, IN.viewDir);
                uv3 += ParallaxOffset(h3, _ParallaxStrength, IN.viewDir);
            #endif

            fixed4 c0 = tex2D(_Splat0, uv0);
            fixed4 c1 = tex2D(_Splat1, uv1);
            fixed4 c2 = tex2D(_Splat2, uv2);
            fixed4 c3 = tex2D(_Splat3, uv3);

            fixed4 mixedDiffuse = 0.0f;
            mixedDiffuse += splat_control.r * c0 * half4(1.0, 1.0, 1.0, _Smoothness0);
            mixedDiffuse += splat_control.g * c1 * half4(1.0, 1.0, 1.0, _Smoothness1);
            mixedDiffuse += splat_control.b * c2 * half4(1.0, 1.0, 1.0, _Smoothness2);
            mixedDiffuse += splat_control.a * c3 * half4(1.0, 1.0, 1.0, _Smoothness3);

            o.Albedo = mixedDiffuse.rgb;
            o.Alpha = weight;
            o.Smoothness = mixedDiffuse.a;
            
            // Base Specular Color
            o.Specular = mixedDiffuse.rgb;

            // Occlusion Red Chanel
            half mixedOcclusion = 0.0f;
            mixedOcclusion += splat_control.r * c0.r;
            mixedOcclusion += splat_control.g * c1.r;
            mixedOcclusion += splat_control.b * c2.r;
            mixedOcclusion += splat_control.a * c3.r;
            
            o.Occlusion = lerp(1.0, mixedOcclusion, _OcclusionStrength);

            #ifdef _NORMALMAP
                fixed3 mixedNormal  = UnpackNormalWithScale(tex2D(_Normal0, uv0), _NormalScale0) * splat_control.r;
                mixedNormal += UnpackNormalWithScale(tex2D(_Normal1, uv1), _NormalScale1) * splat_control.g;
                mixedNormal += UnpackNormalWithScale(tex2D(_Normal2, uv2), _NormalScale2) * splat_control.b;
                mixedNormal += UnpackNormalWithScale(tex2D(_Normal3, uv3), _NormalScale3) * splat_control.a;
                mixedNormal.z += 1e-5f;
                o.Normal = mixedNormal;
            #endif

            #if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X) && defined(TERRAIN_INSTANCED_PERPIXEL_NORMAL)
                float3 geomNormal = normalize(tex2D(_TerrainNormalmapTexture, IN.tc.zw).xyz * 2 - 1);
                #ifdef _NORMALMAP
                    float3 geomTangent = normalize(cross(geomNormal, float3(0, 0, 1)));
                    float3 geomBitangent = normalize(cross(geomTangent, geomNormal));
                    o.Normal = o.Normal.x * geomTangent + o.Normal.y * geomBitangent + o.Normal.z * geomNormal;
                #else
                    o.Normal = geomNormal;
                #endif
                o.Normal = o.Normal.xzy;
            #endif
        }
        ENDCG

        UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
        UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
    }

    Dependency "AddPassShader"    = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
    Dependency "BaseMapShader"    = "Hidden/TerrainEngine/Splatmap/Standard-Base"
    Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/Standard-BaseGen"

    Fallback "Nature/Terrain/Diffuse"
}
