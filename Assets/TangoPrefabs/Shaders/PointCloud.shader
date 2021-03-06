﻿Shader "Tango/PointCloud" {
	Properties {
		  _MainTex ("Main", 2D) = "white" {}
	      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
	   }
  SubShader {
  	Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}
		Lighting Off Cull Off ZTest Always ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
	
     Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
         	uniform float4 _Color; // define shader property for shaders
         	
			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color;
				return col;
			}
		ENDCG
		}
  }
}
