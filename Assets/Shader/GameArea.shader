Shader "PingPong/GameArea"
{
    Properties
    {
        _ColorShadow ("Color shadow", Color) = (0,0,0,1)
        _ColorBack ("Color back", Color) = (1,1,1,1)
        _Size ("Size shadow", float) = 0.01
        _Rounded ("Rounded", int) = 8
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="TransparentCutout"
            "RenderQueue"="AlphaTest" 
            "IgnoreProjector"="True"
            "ForceNoShadowCasting"="True"
            "PreviewType"="Plane" 
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
            int _Rounded;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = float2(i.uv.x - 0.5, i.uv.y - 0.5) * 2;
                float result = pow(abs(uv.x), _Rounded) + pow(abs(uv.y), _Rounded);
                float insideBorder = 1 - _Size;
                float chooseColor = (result - insideBorder) / _Size;             

                clip(-result + 1);
                
                return lerp(_ColorBack, _ColorShadow, saturate(chooseColor));
            }
            ENDCG
        }
    }
}
