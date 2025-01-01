Shader "Custom/character_v2"
{
    Properties
    {
        _Color("Gradient Color", Color) = (1,1,1,1) //係数

        _Color1("Gradient Color 1", Color) = (1,0,0,1) // 赤
        _Color2("Gradient Color 2", Color) = (0,1,0,1) // 緑
        _Color3("Gradient Color 3", Color) = (0,0,1,1) // 青
        _GradientRange("Gradient Range", Float) = 10.0 // グラデーション範囲
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _Color;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _GradientRange;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ワールド座標のY成分を取得し、0〜1に正規化
                float gradientFactor = saturate(i.worldPos.y / _GradientRange);

                // 3色の線形補間
                fixed4 color;
                if (gradientFactor < 0.5)
                {
                    // 最初の2色の間で補間
                    color = lerp(_Color1, _Color2, gradientFactor * 2);
                }
                else
                {
                    // 後半の2色の間で補間
                    color = lerp(_Color2, _Color3, (gradientFactor - 0.5) * 2);
                }

                color = color * _Color;

                return color;
            }
        ENDCG
        }
    }
}
