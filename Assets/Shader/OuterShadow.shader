Shader "PingPong/OuterShadow"
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
            "Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"ForceNoShadowCasting"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
        }

        Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

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
                
                clip(-result + 1);

                float insideBorder = 1 - _Size;
                float4 col = lerp(_ColorBack, _ColorShadow, step(insideBorder, result));
                float alpha = abs(((result - insideBorder) / _Size) - 1); 
                
                return float4(col.xyz * alpha, alpha);
            }
            ENDCG
        }
    }
}
