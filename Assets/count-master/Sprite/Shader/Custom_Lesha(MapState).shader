Shader "Custom/Lesha(MapState)" {
	Properties {
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[ASEBegin] _Color ("Color", Vector) = (1,1,1,1)
		_ShadingColor ("ShadingColor", Vector) = (0,0,0,1)
		_Emission ("Emission", Range(0, 1)) = 0
		_TexOpen ("TexOpen", 2D) = "white" {}
		_TexMy ("TexMy", 2D) = "white" {}
		_TexAttack ("TexAttack", 2D) = "white" {}
		_Attack ("Attack", Range(0, 1)) = 0
		[ASEEnd] _My ("My", Range(0, 1)) = 1
		[HideInInspector] _texcoord ("", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Hidden/InternalErrorShader"
	//CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
}