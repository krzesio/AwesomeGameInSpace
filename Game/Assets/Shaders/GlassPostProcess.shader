﻿Shader "Custom/GlassPostProcess" {
	Properties {
		_MainTex ("", 2D) = "white" {}
		_Glass ("Heigth", 2D) = "white" {}
		_Delta ("Sampling radius", range(0.0001, 0.005)) = 0.001
		_Magnitude ("Magnitude", range(-0.001, 0.001)) = 0.0001
	}
 
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
 
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" 
			//we include "UnityCG.cginc" to use the appdata_img struct
    
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
   
			//Our Vertex Shader 
			v2f vert (appdata_img v){
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				return o; 
			}
    
			sampler2D _MainTex;
			sampler2D _Glass;
			float _Delta;
			float _Magnitude;
    
			//Our Fragment Shader
			float4 frag (v2f i) : COLOR{
				float2 bump = float2(0, 0);
				float temp;

				bump.x -= tex2D( _Glass, i.uv + float2(-_Delta, 0)).a;
				bump.x += tex2D( _Glass, i.uv + float2(_Delta, 0)).a;

				bump.y -= tex2D( _Glass, i.uv + float2(0, -_Delta)).a;
				bump.y += tex2D( _Glass, i.uv + float2(0, _Delta)).a;

				temp = tex2D( _Glass, i.uv + float2(-_Delta, -_Delta)).a;
				bump.x -= temp;
				bump.y -= temp;

				temp = tex2D( _Glass, i.uv + float2(_Delta, -_Delta)).a;
				bump.x += temp;
				bump.y -= temp;

				temp = tex2D( _Glass, i.uv + float2(-_Delta, _Delta)).a;
				bump.x -= temp;
				bump.y += temp;

				temp = tex2D( _Glass, i.uv + float2(_Delta, _Delta)).a;
				bump.x += temp;
				bump.y += temp;

				bump /= _Delta;

				bump *= _Magnitude;

				float2 uv = i.uv + bump;

				float4 original = tex2D( _MainTex, uv );
				float4 modulation = tex2D(_Glass, i.uv);
				//return tex2D(_Glass, i.uv).aaaa;
				return original * modulation;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}