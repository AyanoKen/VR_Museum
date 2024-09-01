Shader "Unlit/Shader-Ensemble"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _EdgeColor ("Edge Color", Color) = (0, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Float) = 1.0
        _MinEdge ("Min Edge Width", Float) = 0.3
        _MaxEdge ("Max Edge Width", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _EdgeColor;
        half _EmissionStrength;
        half _MinEdge;
        half _MaxEdge;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            // Calculate distance from the center of the object in UV space
            float2 uvCenter = IN.uv_MainTex - 0.5;
            float distanceFromCenter = length(uvCenter) * 2.0;

            // Calculate edge falloff
            float edgeFalloff = smoothstep(_MinEdge, _MaxEdge, distanceFromCenter);

            // Apply emission based on distance
            fixed4 emission = _EdgeColor * _EmissionStrength * (1.0 - edgeFalloff);
            o.Emission = emission.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}