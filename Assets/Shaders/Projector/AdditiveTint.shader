Shader "Projector/AdditiveTint" {
	Properties {
		_Color ("Tint Color", Color) = (1,1,1,1)
		_Attenuation("Falloff", Range(0.0, 1.0)) = 1.0
		_LineWidth("Line Width", Range(0.0, 1.0)) = 1.0
	}
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha One
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			sampler2D _ShadowTex;
			fixed4 _Color;
			float _Attenuation;
			float _LineWidth;
			v2f vert(float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.uvShadow = mul(unity_Projector, vertex);
				return o;
			}
			fixed PixelShaderFunction(float2 coords: TEXCOORD0)
			{
				float dx = coords.x - 0.5f;
				float dy = coords.y - 0.5f;
				if (abs(dx * dx + dy * dy-0.25f) <= _LineWidth * _LineWidth)
					return 1.0;
				else
					return 0.0;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed circle = PixelShaderFunction(UNITY_PROJ_COORD(i.uvShadow));
				fixed4 outColor = _Color * circle;
				float depth = i.uvShadow.z;
				return outColor * clamp(1.0 - abs(depth) + _Attenuation, 0.0, 1.0);
			}
			
			ENDCG
		}
	}
}
