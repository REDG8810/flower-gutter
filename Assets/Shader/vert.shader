Shader "Unlit/vert"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Cull Off ZWrite On Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
            };

            float4 _Color;

            v2f vert (appdata_base v)
            {
                v2f o;
                //オブジェクトの頂点座標をワールド座標に写す
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                //alphaにしているので値は0~1の範囲(分かりやすい様にAlbedoの数値を直す様に表記)
                float4 red = float4(255.0 / 255, 70.0 / 255, 0 / 255, 1);
                float4 blue = float4(90.0 / 255, 90.0 / 255, 250.0 / 255, 1);
                return lerp(red, blue, i.worldPos.y*0.2);
            }
            ENDCG
        }
    }
}
