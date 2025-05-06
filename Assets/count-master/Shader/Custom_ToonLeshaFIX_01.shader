Shader "Custom/ToonLeshaFIX_01" {
	Properties {
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[ASEBegin] _Color ("Color", Vector) = (0.7075472,0.03337486,0.03337486,0)
		_ColorLight ("Color Light", Vector) = (0.7075472,0.03337486,0.03337486,0)
		_Toon ("Toon", Vector) = (0.08490568,0,0,0)
		_HardLightColor ("Hard Light Color", Vector) = (1,0.9718938,0,0)
		_HardLightStrength ("Hard Light Strength", Float) = 0.47
		_Texture ("Texture", 2D) = "white" {}
		_TextureLight ("Texture Light", 2D) = "white" {}
		_ToonStrength ("Toon Strength", Range(-1, 1)) = 0
		[ASEEnd] _LightDir ("LightDir", Vector) = (0,1,0,0)
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