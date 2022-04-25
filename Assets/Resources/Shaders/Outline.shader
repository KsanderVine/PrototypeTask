Shader "Custom/ComposedOutline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,0,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(0, 1)) = .05
		_OutlineOffset("Outline Offset", Range(1,5)) = 1.5
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass //Normal Render
		{
			ZWrite On

			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}

		}

		Pass
		{
			Name "OFFSET"

			Blend One One
			ZWrite Off

			Stencil
			{
				Ref 1
				Comp Always
				ReadMask 1
				WriteMask 1
				Pass Replace
				Fail Replace
				ZFail Replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _OutlineOffset;

			v2f vert(appdata v)
			{
				v.vertex.xyz *= _OutlineOffset;
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return half4(0,0,0,0);
			}
			ENDCG
		}

		Pass
		{
			Name "OUTLINE"

			ZTest Greater
			ZWrite Off

			Stencil
			{
				Ref 0
				Comp Equal
				ReadMask 1
				WriteMask 1
				Pass Keep
				Fail Keep
				ZFail Keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineOffset;

			v2f vert(appdata v)
			{
				v.vertex.xyz *= _OutlineOffset * (1 + _OutlineWidth);
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _OutlineColor;
				return col;
			}
			ENDCG
		}
	}
}