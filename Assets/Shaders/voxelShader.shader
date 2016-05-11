Shader "Custom/voxelShader" {
	Properties {
	   _MainTex ("Base (RGB)", 2D) = "white" {}
	   _TexTiling ("TexTiling", Float) = 1
	   _OffsetX ("Offset X", Float) = 0
	   _OffsetY ("Offset Y", Float) = 0
	   _SideOffsetX ("Side Offset X", Float) = 0
	   _SideOffsetY ("Side Offset Y", Float) = 0
	   _SideTiling ("Side Tiling", Float) = 1
	   _MainTiling ("Main Tiling", Float) = 1
   }
   
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // make sure that all uniforms are correctly set
 
         CGPROGRAM
         #pragma vertex vert  
         #pragma fragment frag 

         uniform float4 _LightColor0; 

         uniform half	_TexTiling;
         uniform half	_OffsetX;
         uniform half	_OffsetY;
         uniform half	_SideOffsetX;
         uniform half	_SideOffsetY;
         uniform half	_SideTiling;
         uniform half	_MainTiling;
         uniform sampler2D _MainTex;
         uniform float4 _MainTex_ST;

         //static half3 normArray[6] = 
         //{
         //	half3(0,0,1), half3(0,0,-1), half3(-1,0,0), half3(1,0,0), half3(0,1,0), half3(0,-1,0)
         //};

		static half precomputedLightDiffuse[6] =
		{
			0.4232123, 0, 0.3189133, 0, 0.8480482, 0
		};
 
         struct vertexInput {
            float4 vertex : POSITION;
            half4 col	  : COLOR0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            half4 col : COLOR;
            half2 uv  : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
 			half4 params = input.col * 255;
			uint normIndex = params.w;
			
 			output.uv = normIndex < 2 ?  input.vertex.xy : normIndex < 4 ? input.vertex.zy : input.vertex.xz;
			//HACK HACK HACK 10 is my voxel res
			output.uv = output.uv * 10 * (normIndex < 4 ? _SideTiling : _MainTiling);
 
 			half ao = 1.0 - params.b * 0.2;
 
            half3 diffuseReflection = _LightColor0.rgb
               * precomputedLightDiffuse[normIndex] + UNITY_LIGHTMODEL_AMBIENT;
 
            output.col = half4(diffuseReflection * ao, params.w + 0.01);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         half4 frag(vertexOutput input) : COLOR
         {
         	uint normIndex = (uint)(input.col.a);

            half2 texuv = fmod(input.uv * _TexTiling,_TexTiling);
            
            if(normIndex < 4)
            	texuv += half2(_SideOffsetX,_SideOffsetY);
            else
            	texuv += half2(_OffsetX,_OffsetY);
            	
         	half4 c = tex2D(_MainTex, texuv);
         	
            return half4(input.col.rgb,1.0) * c;
         }
 
         ENDCG
      }
   }
   Fallback "Mobile/Diffuse"
}
