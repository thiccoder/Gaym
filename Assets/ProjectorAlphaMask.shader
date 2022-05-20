Shader "Projector/AlphaMask" {
	Properties {
		_Color ("Tint Color", Color) = (1,1,1,1)
		_Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
		_OpaqueTex("Opaque Texture", 2D) = "white" {}
		_TransparentTex("Semi-Transparent Texture", 2D) = "white" {}
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

			sampler2D _OpaqueTex;
			sampler2D _TransparentTex;
			fixed4 _Color;
			float _Opacity;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float aOpaque = tex2Dproj(_OpaqueTex, UNITY_PROJ_COORD(i.uv)).a;
				float aTransparent = tex2Dproj(_TransparentTex, UNITY_PROJ_COORD(i.uv)).a;
				float a = aOpaque + _Opacity * aTransparent;
				_Color.a = max(0, _Color.a - a);
				return _Color;
			}
			ENDCG
		}
	}
}
