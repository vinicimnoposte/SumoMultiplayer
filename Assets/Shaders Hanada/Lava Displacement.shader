Shader "Custom/Lava Displacement"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NormalTex("NormalTexture", 2D) = "white" {}
        _NormalForce("NormalForce", Range(-2,2)) = 1
        _DisplacementMap("Displacement Map", 2D) = "white"
        _Displacement("Displacement", float) = 1.0
        [Toggle] _FlipYZ("FlipYZ", float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Pass
            {
                HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


                texture2D _MainTex;
                SamplerState sampler_MainTex;
                float4 _MainTex_ST;
                texture2D _NormalTex;

                SamplerState sampler_NormalTex;
                float _NormalForce;


                texture2D _DisplacementMap;
                SamplerState sampler_DisplacementMap;

                CBUFFER_START(UnityPerMaterial)
                    float4 _DisplacementMap_ST;
                half _Displacement, _FlipYZ;
                CBUFFER_END

                struct Attributes
                {
                    float4 position :POSITION;
                    half2 uv       :TEXCOORD0;
                    half3 normal : NORMAL;
                    half4 color : COLOR;

                    float4 positionOS :POSITION;


                };

                struct Varyings
                {
                    float4 positionVAR :SV_POSITION;
                    half2 uvVAR       : TEXCOORD0;
                    half3 normalVar : NORMAL;
                    half4 colorVar : COLOR0;

                    //float4 positionHCS : SV_POSITION;
                    //float2 uv: TEXCOORD0;


                };

                Varyings vert(Attributes Input)
                {
                    Varyings Output;
                    float3 position = Input.position.xyz;
                    Output.positionVAR = TransformObjectToHClip(position);
                    Output.uvVAR = (Input.uv * _MainTex_ST.xy + _MainTex_ST.zw);//tiling
                    Output.colorVar = Input.color;

                    Output.normalVar = TransformObjectToWorldNormal(Input.normal);



                    Output.uvVAR = TRANSFORM_TEX(Input.uv, _DisplacementMap);
                    half pos = SAMPLE_TEXTURE2D_LOD(_DisplacementMap, sampler_DisplacementMap, Input.uv, 1).r * _Displacement;
                    half4 newPos = Input.positionOS;
                    if (_FlipYZ)
                    {
                        newPos.z += pos;
                    }
                    else
                    {
                        newPos.y += pos;
                    }
                    Output.positionVAR = TransformObjectToHClip(newPos);

                    return Output;
                }

                half4 frag(Varyings Input) :SV_TARGET
                {
                    half4 color = Input.colorVar;

                    Light l = GetMainLight();

                   half4 normalmap = _NormalTex.Sample(sampler_NormalTex, half2(_Time.x + Input.uvVAR.x, Input.uvVAR.y)) * 2 - 1;
                   half3 normal = Input.normalVar + normalmap.xzy * _NormalForce;


                   float intensity = dot(l.direction, normal);

                    color *= _MainTex.Sample(sampler_MainTex, half2(_Time.x + Input.uvVAR.x, Input.uvVAR.y));
                    color *= intensity;



                    return color;
                }



            ENDHLSL
        }
        }
}
