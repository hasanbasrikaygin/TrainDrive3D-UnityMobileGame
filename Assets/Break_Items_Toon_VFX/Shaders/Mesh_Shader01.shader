// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mesh_Shader01"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0.6775098,0.7433916,0.8207547,0)
		[HDR]_Light_Color("Light_Color", Color) = (1,1,1,0)
		_Fresnel("Fresnel", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask On
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Light_Color;
			uniform float4 _Color0;
			uniform float _Fresnel;
			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				o.ase_color = v.color;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
				float3 ase_worldlightDir = 0;
				#else //aseld
				float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( WorldPosition ) );
				#endif //aseld
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float fresnelNdotV28 = dot( ase_worldNormal, ase_worldlightDir );
				float f028 = 0.08;
				float fresnelNode28 = ( f028 + ( 1.0 - f028 ) * pow( 1.0 - fresnelNdotV28, 5 ) );
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV39 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode39 = ( 0.08 + _Fresnel * pow( 1.0 - fresnelNdotV39, 2.0 ) );
				float2 uv_Texture = i.ase_texcoord2.xy * _Texture_ST.xy + _Texture_ST.zw;
				float4 appendResult9 = (float4(( ( ( _Light_Color + ( saturate( ( _Color0 * ( 1.0 - fresnelNode28 ) ) ) * ( fresnelNode39 * 5.0 ) ) ) * i.ase_color ) * tex2D( _Texture, uv_Texture ) ).rgb , 1.0));
				
				
				finalColor = appendResult9;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;126.5,-174.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-224.5,-53.5;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;80b55913dce69ac4aa663fa8451ce121;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;5;631.3221,-160.9839;Float;False;True;-1;2;ASEMaterialInspector;100;5;Mesh_Shader01;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;;10;False;;0;5;False;;10;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;158.7128,-313.5336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;34.78149,-543.5511;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-736.4599,-446.0096;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-276.0487,-425.9967;Inherit;True;2;2;0;COLOR;2,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;36;-488.2411,-466.2151;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;33;-1009.22,-309.1371;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-1072.373,-576.3955;Inherit;False;Property;_Color0;Color 0;1;1;[HDR];Create;True;0;0;0;False;0;False;0.6775098,0.7433916,0.8207547,0;0.1960784,1.090196,1.498039,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-613.8644,-736.2728;Inherit;False;Property;_Light_Color;Light_Color;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.01568628,0.8705882,1.819608,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;9;345.25,-80.5;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-401.4559,-176.5377;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;28;-1419.427,-309.716;Inherit;True;Schlick;WorldNormal;LightDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.08;False;2;FLOAT;1;False;3;FLOAT;0.62;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;3;-675.4624,-212.9615;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;39;-670.6164,32.00066;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.08;False;2;FLOAT;1;False;3;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-960.006,78.5112;Inherit;False;Property;_Fresnel;Fresnel;3;0;Create;True;0;0;0;False;0;False;0;4.058824;0;5;0;1;FLOAT;0
WireConnection;4;0;11;0
WireConnection;4;1;2;0
WireConnection;5;0;9;0
WireConnection;11;0;34;0
WireConnection;11;1;3;0
WireConnection;34;0;23;0
WireConnection;34;1;29;0
WireConnection;35;0;10;0
WireConnection;35;1;33;0
WireConnection;29;0;36;0
WireConnection;29;1;38;0
WireConnection;36;0;35;0
WireConnection;33;0;28;0
WireConnection;9;0;4;0
WireConnection;38;0;39;0
WireConnection;39;2;40;0
ASEEND*/
//CHKSM=56C6FBE21CD88C49322E19266EB6F0C63A0ECCEC