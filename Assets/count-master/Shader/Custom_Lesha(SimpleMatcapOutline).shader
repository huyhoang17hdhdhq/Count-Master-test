Shader "Custom/Lesha(SimpleMatcapOutline)" {
	Properties {
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[ASEBegin] _Color ("Color", Vector) = (1,1,1,1)
		_OutlineColor ("OutlineColor", Vector) = (0,0,0,0)
		_ShadingColor ("ShadingColor", Vector) = (0,0,0,1)
		_OutlineWidthMin ("OutlineWidthMin", Range(0, 1)) = 0.1
		_MainTex ("MainTex", 2D) = "white" {}
		_OutlineWidthMax ("OutlineWidthMax", Range(0, 1)) = 0.1
		_Emission ("Emission", Range(0, 1)) = 0
		_BlinkSpeed ("BlinkSpeed", Float) = 3
		_Matcap_Intensity ("Matcap_Intensity", Range(0, 1)) = 0
		[ASEEnd] _Matcap ("Matcap", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/InternalErrorShader"
	//CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
}