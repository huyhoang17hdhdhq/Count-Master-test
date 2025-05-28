Shader "Custom/4Colors(LeshaSimpleMatcapFakeShadowBlink) " {
	Properties {
		_Color ("Color", Vector) = (1,0,0,1)
		_Color2 ("Color 2", Vector) = (0.06159949,1,0,1)
		_Color3 ("Color3", Vector) = (0.0874176,0,1,1)
		_Color4 ("Color4", Vector) = (1,0.9935301,0,1)
		_ShadingColor ("ShadingColor", Vector) = (0,0,0,1)
		_Emission ("Emission", Range(0, 1)) = 0
		_Matcap_Intensity ("Matcap_Intensity", Range(0, 1)) = 0
		_Matcap ("Matcap", 2D) = "white" {}
		_ShadowFloorY ("_ShadowFloorY", Float) = 0
		_ShadowDirZ ("_ShadowDirZ", Range(-1, 1)) = 0
		_ShadowDirX ("_ShadowDirX", Range(-1, 1)) = 0
		[HideInInspector] __dirty ("", Float) = 1
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
	//CustomEditor "ASEMaterialInspector"
}