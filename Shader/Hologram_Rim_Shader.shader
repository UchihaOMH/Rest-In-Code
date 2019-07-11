Shader "Custom/RImLight_Hologram_Shader"
{
    Properties
    {
        _BumpMap ("Normal Map", 2D) = "bump" {}
		_RimLightColor ("Rim Light Color (RGB)", Color) = (0,1,0,1)
		_RimLightPower ("Rim Light Power", Range(1, 10)) = 3

		_HologramColorRange ("Hologram ColorRange", Range(-1, 1)) = 1
		_HologramEffectRange ("HologramEffectRange", Range(1, 10)) = 3
		_HologramNoiseRange ("HologramNoiseRange", Range(1, 100)) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        CGPROGRAM
        #pragma surface surf Lambert noambient alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;

		float4 _RimLightColor;
		float _RimLightPower;
		float _HologramColorRange;
		float _HologramEffectRange;
		float _HologramNoiseRange;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;

			float3 viewDir;
			float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			float rimLight = abs(dot(o.Normal, IN.viewDir));
			rimLight = pow(1 - rimLight, _RimLightPower);

			float hologram = pow(frac(IN.worldPos.g * _HologramEffectRange - _Time.y), 100 - _HologramNoiseRange);

			o.Emission = (_RimLightColor * rimLight) + (hologram * (_RimLightColor * _HologramColorRange));
			o.Alpha = rimLight;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
