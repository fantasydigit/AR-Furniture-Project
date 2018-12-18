// 
//   NatCorder
//   Copyright (c) 2018 Yusuf Olokoba
//

Shader "Hidden/NatCorder/AspectFitter" {
    Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

            #include "UnityCG.cginc"

            uniform float2 aspectCorrection;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ((v.texcoord - 0.5) * aspectCorrection) + 0.5; // Aspect fit
                return o;
            }

            uniform sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target	{
                if (i.uv.x < 0.0 || i.uv.x > 1.0 || i.uv.y < 0.0 || i.uv.y > 1.0)
                    return fixed4(0.0, 0.0, 0.0, 1.0);
                else
                    return tex2D(_MainTex, i.uv.xy);
			}
            ENDCG
        }
    }
}