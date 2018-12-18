// 
//   NatCorder
//   Copyright (c) 2018 Yusuf Olokoba
//

Shader "Hidden/NatCorder/Transform" {
    Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
			CGPROGRAM
            #pragma multi_compile __ PLATFORM_WEBGL
			#pragma vertex vert_img
			#pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;

            fixed4 frag (v2f_img i) : SV_Target	{
                fixed4 color = tex2D(_MainTex, half2(i.uv.x, 1.0 - i.uv.y));
                #ifdef PLATFORM_WEBGL
                return fixed4(color.gb, 1.0, color.r); // BGAR // Even weirder than Windows :/
                #elif SHADER_API_D3D11
                return fixed4(color.gr, 1.0, color.b); // GRAB // Weirdest swizzling scheme I've ever seen :/                
                #else
                return color;
                #endif
			}
            ENDCG
        }
    }
}