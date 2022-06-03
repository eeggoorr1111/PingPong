Shader "PingPong/GameArea"
{
    Properties
    {
        _ColorShadow ("Color shadow", Color) = (0,0,0,1)
        _ColorBack ("Color back", Color) = (1,1,1,1)
        _Size ("Size shadow", float) = 0.01
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "IgnoreProjector"="True"
            "ForceNoShadowCasting"="True"
        }

        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _ColorShadow;
            float4 _ColorBack;
            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = float2(i.uv.x - 0.5, i.uv.y - 0.5);
                float endShadow = 0.5 - _Size;
                float startShadow = 0.5;
                float shadowIntensiveByX = (abs(uv.x) - endShadow) / _Size;
                float shadowIntensiveByY = (abs(uv.y) - endShadow) / _Size;
                float shadowIntensive = saturate(max(shadowIntensiveByX, shadowIntensiveByY));

                return lerp(_ColorBack, _ColorShadow, shadowIntensive);
            }
            ENDCG
        }
    }
}
