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
                //�I�u�W�F�N�g�̒��_���W�����[���h���W�Ɏʂ�
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                //alpha�ɂ��Ă���̂Œl��0~1�͈̔�(������₷���l��Albedo�̐��l�𒼂��l�ɕ\�L)
                float4 red = float4(255.0 / 255, 70.0 / 255, 0 / 255, 1);
                float4 blue = float4(90.0 / 255, 90.0 / 255, 250.0 / 255, 1);
                return lerp(red, blue, i.worldPos.y*0.2);
            }
            ENDCG
        }
    }
}
