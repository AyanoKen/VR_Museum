Shader "Custom/Shader-Ensemble2"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _CenterColor ("Center Color", Color) = (0, 0.5, 0.5, 1)
        _EdgeColor ("Edge Color", Color) = (0, 1, 1, 1)
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
        fixed4 _CenterColor;
        fixed4 _EdgeColor;
        half _MinEdge;
        half _MaxEdge;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb; // Base texture color

            // Calculate distance from the center of the object in UV space
            float2 uvCenter = IN.uv_MainTex - 0.5;
            float distanceFromCenter = length(uvCenter) * 2.0;

            // Calculate edge falloff
            float edgeFalloff = smoothstep(_MinEdge, _MaxEdge, distanceFromCenter);

            // Interpolate between center color and edge color based on distance
            fixed4 color = lerp(_CenterColor, _EdgeColor, edgeFalloff);
            o.Albedo = color.rgb; // Assign the final color to the albedo
        }
        ENDCG
    }
    FallBack "Diffuse"
}
