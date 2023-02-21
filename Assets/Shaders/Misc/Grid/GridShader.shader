shader "Misc/Grid" {
	Properties {
		_LineColor ("Line Color", Color) = (1,1,1,1)
		_CellColor ("Cell Color", Color) = (0,0,0,0)
		_GridSizeX("Grid Size X", float) = 10
		_GridSizeY("Grid Size Y", float) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15
	}
	SubShader {
		Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
		LOD 200
	

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness = 0.0;
		half _Metallic = 0.0;
		float4 _LineColor;
		float4 _CellColor;

		float _GridSizeX;
		float _GridSizeY;
		float _LineSize;


		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float2 uv = IN.uv_MainTex;


			fixed4 c = float4(0.0,0.0,0.0,0.0);

			float brightness = 1.;

			float gsizex = floor(_GridSizeX);
			float gsizey = floor(_GridSizeY);



			gsizex += _LineSize;
			gsizey += _LineSize;

			float2 id;

			id.x = floor(uv.x/(1.0/gsizex));
			id.y = floor(uv.y/(1.0/gsizey));

			float4 color = _CellColor;
			brightness = _CellColor.w;

			

			if (frac(uv.x * gsizex) <= _LineSize || frac(uv.y * gsizey) <= _LineSize)
			{
				brightness = _LineColor.w;
				color = _LineColor;
			}
			

			if (brightness == 0.0) {
				clip(c.a - 1.0);
			}
			

			o.Albedo = float4( color.x*brightness,color.y*brightness,color.z*brightness,brightness);
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			o.Alpha = 0.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
