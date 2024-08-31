Shader "Custom/Shader-Ensemble"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 50.0
        _Intensity ("Intensity", Float) = 1.0
        _MinDotSize ("Min Dot Size", Float) = 0.1
        _MaxDotSize ("Max Dot Size", Float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;
            float _Intensity;
            float _MinDotSize;
            float _MaxDotSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Scale;
                return o;
            }

            // Hash function for pseudo-random number generation
            float2 hash22(float2 p)
            {
                p = frac(p * float2(443.8975, 441.4232));
                p += dot(p, p + 23.432);
                return frac(float2(p.x * p.y, p.x + p.y));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Create a pseudo Voronoi effect
                float2 uv = i.uv;
                float2 p = floor(uv);
                float2 f = frac(uv);

                float2 mg = float2(0.0, 0.0);
                float2 mr = float2(0.0, 0.0);

                float d = 1.0;
                float randomDotSize = 0.0;

                for (int j = -1; j <= 1; j++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        float2 g = float2(float(i), float(j));
                        float2 r = g - f + hash22(p + g);
                        float dist = dot(r, r);

                        if (dist < d)
                        {
                            d = dist;
                            mr = r;
                            mg = g;

                            // Random dot size between MinDotSize and MaxDotSize
                            float randomValue = hash22(p + g).x; // Using the hash function for randomness
                            randomDotSize = lerp(_MinDotSize, _MaxDotSize, randomValue);
                        }
                    }
                }

                // Scale the distance 'd' by the random dot size
                float adjustedD = d / randomDotSize;

                // Invert colors to get white dots on a black background with randomized sizes
                float invertedD = 1.0 - adjustedD * _Intensity;
                float3 color = float3(invertedD, invertedD, invertedD);
                
                return fixed4(color, 1.0);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}