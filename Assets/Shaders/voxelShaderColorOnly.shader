Shader "Custom/voxelShaderColorOnly" {
	Properties {
	   _Color ("Color", Color) = (0,0,0,0)
	   _SideColor("Side Color", Color) = (0,0,0,0)
   }
   
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // make sure that all uniforms are correctly set
 
         CGPROGRAM
         #pragma vertex vert  
         #pragma fragment frag 

         uniform float4 _LightColor0; 

         uniform half4 _Color;
         uniform half4 _SideColor;

         static half3 normArray[6] = 
         {
         	half3(0,0,1), half3(0,0,-1), half3(-1,0,0), half3(1,0,0), half3(0,1,0), half3(0,-1,0)
         };

 
         struct vertexInput {
            float4 vertex : POSITION;
            half4 col	  : COLOR0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            half4 col : COLOR;
            //half3 obc  : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
 			half4 params = input.col * 255;
			uint normIndex = (uint)(params.w);
			
			//output.obc = input.vertex.xyz;
			//HACK HACK HACK 10 is my voxel res
			//output.obc = output.obc * 10;
			
			half ao = clamp(1.0 - params.b * 0.2,0,5);
			
            half3 normalDirection = normArray[ normIndex ];
            half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 
 			half4 vcolor;
 			
 			if(normIndex < 4)
            	vcolor = _SideColor;
            else
            	vcolor = _Color;
 
            half3 diffuseReflection = _LightColor0.rgb
               * max(0.0, dot(normalDirection, lightDirection)) + UNITY_LIGHTMODEL_AMBIENT;
 
 			//diffuseReflection = min(diffuseReflection, 1.0);
 
            output.col = half4(diffuseReflection * ao, params.w + 0.5) * vcolor;
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         half4 frag(vertexOutput input) : COLOR
         {
         	uint normIndex = (uint)(input.col.a);
			//half3 normalDirection = normArray[ normIndex ];
         	//half3 obc = floor(input.obc - normalDirection * 0.5);// / 8; 
         	
         	//half4 finalc = half4(sin(obc.x * 3), cos(obc.y * 3), cos(obc.z * 3),1.0);
         	
            return half4(input.col.rgb,1.0);
         }
 
         ENDCG
      }
   }
   Fallback "Mobile/Diffuse"
}
