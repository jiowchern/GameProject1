Shader "MyShader/BladeCharacter" {

//author: shallway.
//2012.3.19
//Notice, this is not the perfect solution to render blade soul model in unity.

Properties {
  _Color ("Main Color", Color) = (1,1,1,1)
  _MainTex ("Texture", 2D) = "white" {}
  _SpecMap ("SpecMap", 2D) = "white" {}
  _BumpMap ("Normalmap", 2D) = "bump" {}
  _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
  _Shines ("Shinese", Range(0,1)) = 0.5
}

SubShader {
    Tags { "Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
	//Cull Off
CGPROGRAM
#pragma surface surf ColoredSpecular alphatest:_Cutoff

struct MySurfaceOutput {
    half3 Albedo;
    half3 Normal;
    half3 Emission;
    half Specular;
    half3 GlossColor;
    half Alpha;
};

inline half4 LightingColoredSpecular (MySurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
{

  half3 h = normalize (lightDir + viewDir);
  half diff = max (0, dot (s.Normal, lightDir));
  float nh = max (0, dot (s.Normal, h));
  float spec = pow (nh, 32.0);
  half3 specCol = spec * s.GlossColor;
  half4 c;

  c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * specCol) * (atten * 2);
  c.a = s.Alpha;
  return c;

}

 

inline half4 LightingColoredSpecular_PrePass (MySurfaceOutput s, half4 light)
{
    half3 spec = light.a * s.GlossColor;
    half4 c;
    c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
    c.a = s.Alpha + Luminance(spec);
    return c;
}

struct Input {
  float2 uv_MainTex;
  float2 uv_SpecMap;
  float2 uv_BumpMap;
};

sampler2D _MainTex;
sampler2D _SpecMap;
sampler2D _BumpMap;
float4 _Color;
float _Shines;
 

void surf (Input IN, inout MySurfaceOutput o)
{
  half4 tex = tex2D(_MainTex, IN.uv_MainTex);
  o.Albedo = tex.rgb * _Color.rgb;
  half4 spec = tex2D (_SpecMap, IN.uv_SpecMap);
  o.GlossColor = spec.rgb * _Shines;
  o.Alpha = tex.a * _Color.a;
  o.Specular = 32.0/128.0;
  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}

ENDCG
}

Fallback "Diffuse"

}
