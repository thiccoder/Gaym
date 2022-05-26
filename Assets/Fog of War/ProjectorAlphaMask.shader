Shader "Projector/AlphaMaskBlend" {
	Properties {
		_Color ("Tint Color", Color) = (1,1,1,1)
		_Opacity1("Texture 1 Opacity", Range(0.0, 1.0)) = 1.0
		_Opacity2("Texture 2 Opacity", Range(0.0, 1.0)) = 1.0
		_Tex1("Texture 1", 2D) = "white" {}
		_Tex2("Texture 2", 2D) = "white" {}
	}
	Subshader {
		Tags {"Queue"="Transparent+100"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (vertex);
				o.uv = mul (unity_Projector, vertex);
				return o;
			}

			sampler2D _Tex1;
			sampler2D _Tex2;
			float _Opacity1;
			float _Opacity2;
			fixed4 _Color;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float a1 = tex2Dproj(_Tex1, UNITY_PROJ_COORD(i.uv)).a;
				float a2 = tex2Dproj(_Tex2, UNITY_PROJ_COORD(i.uv)).a;
				float a = _Opacity1 * a1 + _Opacity2 * a2;
				_Color.a = max(0, _Color.a - a);
				return _Color;
			}
			ENDCG
		}
	}
}
