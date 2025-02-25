Shader "Custom/GlowingShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (0.5, 0, 0.5, 1) // Jaune par défaut
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2
        _PulseSpeed ("Pulse Speed", Range(0.1, 50)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha // Mode transparent
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _GlowIntensity;
            float _PulseSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5; // Oscillation entre 0 et 1
                fixed4 color = _Color * (1 + _GlowIntensity * pulse);
                return color;
            }
            ENDCG
        }
    }
}