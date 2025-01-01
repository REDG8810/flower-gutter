Shader "Custom/character"
{
    Properties
    {
        
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Color3 ("Color3", Color) = (1,1,1,1)
        _CharacterColor("CharacterColor", Color) = (1,1,1,1)
        _GradientRange("Gradient Range", Float) = 10.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 3.0

       

        struct Input
        {
            
            float3 worldPos;
        };

        float4 _Color1;
        float4 _Color2;
        float4 _Color3;
        float4 _CharacterColor;
        float _GradientRange;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // ワールド座標のY成分を取得し、0〜1に正規化
            float gradientFactor = saturate(IN.worldPos.y / _GradientRange);

            // 疑似UV座標を生成（ローカル座標のXZ成分）
            float2 pseudoUV = frac(IN.worldPos.xz);

            // 疑似UVに基づいたアルファ値（シンプルなパターン生成）
            float alphaFactor = sin(pseudoUV.x * UNITY_PI) * sin(pseudoUV.y * UNITY_PI);
            alphaFactor = saturate(alphaFactor);


            // 3色の線形補間
            fixed4 worldColor;
            if (gradientFactor < 0.5)
            {
                worldColor = lerp(_Color1, _Color2, gradientFactor * 2);
            }
            else
            {
                worldColor = lerp(_Color2, _Color3, (gradientFactor - 0.5) * 2);
            }

            fixed4 objectColor = lerp(_CharacterColor, worldColor, alphaFactor);


            o.Albedo = objectColor.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
