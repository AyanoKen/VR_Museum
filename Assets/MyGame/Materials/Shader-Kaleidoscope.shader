Shader "Unlit/Shader-Kaleidoscope"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Segments ("Segments", Int) = 6
        _Brightness ("Brightness", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
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

            sampler2D _MainTex;
            float _Segments;
            float _Brightness;
            float _Speed;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0; // Convert UV to range [-1, 1]

                // Introduce time-based movement to the UVs
                float time = _Time.y * _Speed; // Time variable with speed control
                uv = float2(
                    uv.x * cos(time) - uv.y * sin(time), 
                    uv.x * sin(time) + uv.y * cos(time)
                );

                float angle = atan2(uv.y, uv.x); // Calculate polar angle
                float radius = length(uv); // Calculate polar radius

                float segment = 2.0 * UNITY_PI / _Segments; // Calculate segment size
                angle = fmod(angle + UNITY_PI, segment) - segment / 2.0; // Mirror effect

                uv = float2(cos(angle), sin(angle)) * radius; // Back to Cartesian coordinates
                uv = uv * 0.5 + 0.5; // Map to [0, 1] UV range

                fixed4 col = tex2D(_MainTex, uv) * _Brightness; // Sample texture with brightness
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}