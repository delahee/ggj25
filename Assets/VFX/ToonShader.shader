Shader "Supyrb/Unlit/ToonWithOutline"
{
    Properties
    {
        [HDR]_Color("Albedo", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}

        // Outline
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0.001, 0.1)) = 0.01

        [Header(Stencil)]
        _Stencil ("Stencil ID [0;255]", Float) = 0
        _ReadMask ("ReadMask [0;255]", Int) = 255
        _WriteMask ("WriteMask [0;255]", Int) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0

        [Header(Rendering)]
        _Offset("Offset", float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Cull Mode", Int) = 2
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Int) = 4
        [Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask("Color Mask", Int) = 15
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    half4 _Color;
    sampler2D _MainTex;
    float4 _MainTex_ST;

    half4 _OutlineColor;
    float _OutlineWidth;

    struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
    };

    // Vertex shader principal
    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        return o;
    }

    // Fragment shader principal
    half4 frag (v2f i) : SV_Target
    {
        return tex2D(_MainTex, i.uv) * _Color;
    }

    // Outline - Vertex Shader
    struct v2fOutline
    {
        float4 vertex : SV_POSITION;
    };

    v2fOutline vertOutline(appdata v)
    {
        v2fOutline o;

        float isOrtho = unity_OrthoParams.w; // 1 = ortho, 0 = perspective

        if (isOrtho > 0.5)
        {
            // Vue orthographique : Décalage en screen-space
            float4 clipPos = UnityObjectToClipPos(v.vertex);
            float2 screenUV = clipPos.xy / clipPos.w;
            screenUV += normalize(screenUV) * _OutlineWidth;
            clipPos.xy = screenUV * clipPos.w;
            o.vertex = clipPos;
        }
        else
        {
            // Vue perspective : Décalage en world-space
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
            worldPos += worldNormal * _OutlineWidth;
            o.vertex = UnityWorldToClipPos(float4(worldPos, 1.0));
        }

        return o;
    }

    // Fragment shader de l'outline
    half4 fragOutline(v2fOutline i) : SV_Target
    {
        return _OutlineColor;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Stencil
        {
            Ref [_Stencil]
            ReadMask [_ReadMask]
            WriteMask [_WriteMask]
            Comp [_StencilComp]
            Pass [_StencilOp]
            Fail [_StencilFail]
            ZFail [_StencilZFail]
        }

        // Pass de l'outline
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="Always" }
            Cull Front
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #pragma target 3.0
            ENDCG
        }

        // Pass principale (Toon Shader)
        Pass
        {
            Name "ToonShading"
            Cull [_Culling]
            Offset [_Offset], [_Offset]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            ColorMask [_ColorMask]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }

        // Pass pour l'ombre (ShadowCaster)
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            LOD 80
            Cull [_Culling]
            Offset [_Offset], [_Offset]
            ZWrite [_ZWrite]
            ZTest [_ZTest]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            ENDCG
        }
    }
}
