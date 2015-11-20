Shader "DM/MetalPlate" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {} 	
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125 
		_Bumpiness ("Bumpiness", Range (-1, 1)) = 1.0
		_BumpMap ("Normalmap", 2D) = "bump" {}	
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 800
		
		CGPROGRAM
		#pragma surface surf MetalSpecular
		float4 _Color;
		float _Shininess;
		float _Bumpiness;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		
		half4 LightingMetalSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);
			half diff = max (0, dot (s.Normal, lightDir));
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, 48.0);
			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec * s.Gloss * s.Specular) * (atten * 2);
			c.a = s.Alpha;
			return c;
		}
		
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 tex = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex.rgb;
			o.Gloss = tex.a;
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Normal.y = o.Normal.y * _Bumpiness;
			o.Alpha = tex.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}


