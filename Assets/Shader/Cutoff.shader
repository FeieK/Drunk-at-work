Shader "Custom/Cutoff_ObjectSpace"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PlaneNormal("Plane Normal", Vector) = (0,1,0,0)
        _ClipPercent("Clip Percent", Range(0,100)) = 50
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 objPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _PlaneNormal;
            float _ClipPercent;

            v2f vert(appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                // OBJECT SPACE position
                o.objPos = v.vertex.xyz;

                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float w_min = -5;
                float w_max = 5;

                float w = w_min + (_ClipPercent / 100.0) * (w_max - w_min);

                float3 normal = normalize(_PlaneNormal.xyz);

                // OBJECT SPACE clipping
                if (dot(i.objPos, normal) + w < 0)
                    discard;

                fixed4 col = tex2D(_MainTex, i.uv);

                clip(col.a - 0.01);

                return col;
            }
            ENDCG
        }
    }
}