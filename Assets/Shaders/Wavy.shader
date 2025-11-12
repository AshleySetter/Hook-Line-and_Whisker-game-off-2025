Shader "Custom/GrassWaveRandom"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Amplitude("Amplitude", Float) = 0.1
        _Speed("Speed", Float) = 2.0
        _PhaseScale("Phase Scale", Float) = 10.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Amplitude;
            float _Speed;
            float _PhaseScale;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            // Simple hash function to generate a pseudo-random phase from vertex position
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            v2f vert(appdata v)
            {
                v2f o;

                // Generate pseudo-random phase for this vertex
                float phase = hash21(v.vertex.xy * _PhaseScale) * 6.28318; // 0 → 2π

                // Grass sway based on time, vertex height, and random phase
                float sway = sin(_Time.y * _Speed + phase) * _Amplitude * v.uv.y;
                v.vertex.x += sway;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
