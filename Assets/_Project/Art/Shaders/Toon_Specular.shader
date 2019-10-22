//Written by Parker Hamilton as part of Grandma's Favourite Indie, September 2019
Shader "Grandma'sFavouriteIndie/Toon_Specular"
{
	//compiles to forward renderer
	//properties are the variables we can read in the inspector - some are exposed and editable, others are not
	Properties
	{
		//color of the material
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		//assign the main texture to a default white - a value Unity can read and assign
		_MainTex("Main Texture", 2D) = "white" {}

		//Ambient light represents light that bounces off the surfaces of objects in the area and is scattered 
		//in any direction. Modeled here as a light that affects all surfaces equally and is additive to the main directional light
		[HDR]	//attribute for _AmbientColor that lets us get bloom
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

		//Components for specular aspect of the Blinn-Phong model 
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		[HDR]
		_Glossiness("Glossiness", Float) = 32
	}
	SubShader
	{
		Pass
		{
			Tags
			{
				//requests some lighting data to be passed into our shader
				"LightMode" = "ForwardBase"
				// further requests to restrict this data to only the main directional light -- easier to render from one light source.
				//change this to call from point lights.
				"PassFlags" = "OnlyDirectional"
			}

			//based on Blinn-Phong (https://en.wikipedia.org/wiki/Blinn%E2%80%93Phong_reflection_model) shading model = 
			// ---> H = (L+V)/((absoluteValue)L+V) , where L is the vector to the light source and N is the normal of the surface.

			//cgprogram is basically graphics code.
			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members _Glossiness,_SpecularColor)
			#pragma exclude_renderers d3d11
			//vertexes of the normals
			#pragma vertex vert

			//fragements/pieces of normals
			#pragma fragment frag
			
			//CG code for Unity
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			//this struct contains some functions our appdata types can ref or call
			struct appdata
			{
				//access the model/material's normals -- automatically populated by Unity
				float3 normal : NORMAL;
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
			};

			//this struct contains some functions we can use in our v2f vert() function
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;

				//get vector position of normals -- manually populated in the vertex shader
				float3 worldNormal : NORMAL;
				//direction we view the shader, in relation to light source
				float3 viewDir : TEXCOORD1;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				// transform the normal from object space to world space, as the light's direction is provided in world space.
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				//Specular Reflection!!! models the individual, distinct reflections made by light sources. This reflection is view dependent, 
				//in that it is affected by the angle that the surface is viewed at
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			float4 _Color;
			//this along with lightIntensity down below will make a softer light/dark distinction so it's nor pur black or wite
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;

			float4 frag (v2f i) : SV_Target
			{
				//These use what's called Dot Product -- takes in two vectors (of any length) and returns a single number
				//When they are perpendicular, it returns 0. As you move a vector away from parallel — towards perpendicular — 
				//the dot product result will move from 1 to 0 non-linearly
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				//if we just multiply the sample by NdotL, we get realistic lighting.
				//But by using lightIntensity to divide the NdotL into TWO bands, light and dark, we get a toon-like effect since light and dark are separated
				//a lower bound, an upper bound and a value expected to be between these two bounds - returns value between 0 and 1 based on how far this third value is between first two
				float lightIntensity = smoothstep(0, 0.1, NdotL);
				
				//include the light's color in our calculation. _LightColor0 is the color of the main directional light. It is a fixed4 declared in the Lighting.cginc
				float4 light = lightIntensity * _LightColor0;

				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);

				//controlling the size of specular intensity using pow function - NdotH * lightIntensity ensures reflection is drawn only when lit to save rendering power
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);

				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light + specular);
			}
			ENDCG
		}
	}
}