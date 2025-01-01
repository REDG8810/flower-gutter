Shader "Unlit/sphere"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}


		_Color1("Gradient Color 1", Color) = (1,0,0,1) // 赤
		_Color2("Gradient Color 2", Color) = (0,1,0,1) // 緑
		_Color3("Gradient Color 3", Color) = (0,0,1,1) // 青
		_GradientRange("Gradient Range", Float) = 10.0 // グラデーション範囲

		//X方向のシフトとスピードに関するパラメータを追加
		_XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
		_XSpeed("X Scroll Speed", Range(0, 100.0)) = 10.0

		//Y方向のシフトとスピードに関するパラメータを追加
		_YShift("Yuv Shift", Range(-1.0, 1.0)) = 0.1
		_YSpeed("Y Scroll Speed", Range(0, 100.0)) = 10.0
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}
		//内側に対応
		Cull Front
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;

			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//追加したパラメータを宣言する
			float _XShift;
			float _YShift;
			float _XSpeed;
			float _YSpeed;

			float4 _Color1;
			float4 _Color2;
			float4 _Color3;
			float _GradientRange;

			//バーテクスシェーダー（変更なし）
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			//フラグメントシェーダー（変更箇所）
			fixed4 frag(v2f i) : SV_Target
			{
				// ワールド座標のY成分を取得し、0〜1に正規化
				float gradientFactor = saturate(i.uv.y / _GradientRange);

				// 3色の線形補間
				fixed4 worldCol;
				if (gradientFactor < 0.5)
				{
					// 最初の2色の間で補間
					worldCol = lerp(_Color1, _Color2, gradientFactor * 2);
				}
				else
				{
					// 後半の2色の間で補間
					worldCol = lerp(_Color2, _Color3, (gradientFactor - 0.5) * 2);
				}

				//Speed
				_XShift = _XShift * _XSpeed;
				_YShift = _YShift * _YSpeed;

				//add Shift
				i.uv.x = i.uv.x + _XShift * _Time;
				i.uv.y = i.uv.y + _YShift * _Time;

				//i.uvの適用
				fixed4 col = tex2D(_MainTex, i.uv) * worldCol;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
