Shader "Custom/HighlightEdge"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Threshold("Threshold", float) = 0.01
		_EdgeColor("Edge Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
		float4 _MainTex_TexelSize;
        fixed4 _Color;
		float4 _EdgeColor;
		float _Threshold;

		sampler2D _CameraDepthNormalsTexture;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float4 GetPixelDepthNormal(in float2 uv)
		{
			float3 normal;
			float depth;
			DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv), depth, normal);

			return float4(normal, depth);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			float4 orValue = GetPixelDepthNormal(IN.uv_MainTex);

			//offsetting the pixel directions
			float2 offsets[8] = 
			{
				float2(-1, -1),
				float2(-1, 0),
				float2(-1, 1),
				float2(0, -1),
				float2(0, 1),
				float2(1, -1),
				float2(1, 0),
				float2(1, 1)
			};

			float4 sampledValue = float4(0, 0, 0, 0);
			for (int i = 0; i < 8; i++)
			{
				sampledValue += GetPixelDepthNormal(IN.uv_MainTex + offsets[i] * _MainTex_TexelSize.xy);
			}

			sampledValue /= 8;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

			o.Albedo = c.rgb;//lerp(c.rgb, _EdgeColor, step(_Threshold, length(orValue - sampledValue)));

			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), IN.worldNormal));
			o.Emission = float4(1,0,0,1) * pow(rim, 2);
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
