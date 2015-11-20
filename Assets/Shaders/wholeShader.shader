Shader "Custom/wholeShader" {
	Properties {
	  _MainTex ("Main", 2D) = "white" {}
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _OutlineColor ("Outline Color", Color) = (0,0,0,0)
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
 		uniform float4  _OutlineColor;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.uv = input.uv;
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float2 cc = (input.uv - 0.5) * 2;
         	
         	if(abs(cc.x) < 0.8 && abs(cc.y) < 0.8)
            	 return _Color;
            	 
            return _OutlineColor;
           
         }
 
         ENDCG
      }
   }
   Fallback "Diffuse"
}
