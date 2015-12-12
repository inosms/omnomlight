Shader "Custom/AlphaMask"{
	Properties {
		_MainTex ("Render Input", 2D) = "white" {}
		_AlphaMask ("Alpha Mask", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				sampler2D _AlphaMask;
			
				float4 frag(v2f_img IN) : COLOR {
					half4 c = tex2D (_MainTex, IN.uv) * tex2D (_AlphaMask, IN.uv);
					return c;
				}
			ENDCG
		}
	}
}
