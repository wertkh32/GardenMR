Shader "Custom/glowShader" {
	Properties {
	  _MainTex ("Main", 2D) = "white" {}
      _Color ("Diffuse Material Color", Color) = (1,1,1,1)
      _floatTime ("float time", Float) = 0
   }
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // make sure that all uniforms are correctly set
         Cull Off
 		 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 

		 uniform sampler2D _MainTex;
         uniform float4 _Color; // define shader property for shaders
 		 uniform float4 _LightColor0;
 		 uniform float _floatTime; 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 uv : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 col : COLOR;
            float2 uv : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 
            float3 diffuseReflection = _LightColor0.rgb
               * max(0.0, dot(input.normal, lightDirection)) + UNITY_LIGHTMODEL_AMBIENT;
 
 			diffuseReflection = min(diffuseReflection, 1.0);
 
            output.col = diffuseReflection;
 
            output.uv = input.uv;
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float4 c = tex2D(_MainTex, input.uv) * float4(input.col,1.0);
           	
            return c + _floatTime * 0.1 * _Color;
         }
 
         ENDCG
      }
   }
   Fallback "Diffuse"
}
