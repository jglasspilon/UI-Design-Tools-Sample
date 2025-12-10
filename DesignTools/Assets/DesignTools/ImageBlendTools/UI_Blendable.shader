Shader "UI/Blendable"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlendTex ("Blend Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Opacity ("Blend Opacity", Range(0, 1)) = 1
        [KeywordEnum(Normal,Darken,Multiply,ColorBurn,LinearBurn,Lighten,Screen,ColorDodge,LinearDodge,Overlay,SoftLight,HardLight,VividLight,LinearLight,PinLight,HardMix,Difference,Exclusion,Subtract,Divide)] _BlendMode ("Blend Mode", Float) = 0          
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float2 blendtexcoord : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _BlendTex;
            fixed4 _Color;
            float _Opacity;
            float _BlendMode;
            float4 _MainTex_ST;
            float4 _BlendTex_ST;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.blendtexcoord = TRANSFORM_TEX(v.texcoord, _BlendTex);
                OUT.color = v.color * _Color;
                return OUT;
            }

            // Blend mode functions
            half3 BlendDarken(half3 base, half3 blend) { 
                return min(base, blend); 
            }

            half3 BlendMultiply(half3 base, half3 blend) { 
                return base * blend; 
            }

            half3 BlendColorBurn(half3 base, half3 blend) { 
                half3 result;
    
                // Red Channel
                result.r = (base.r >= 0.999) ? base.r : 1.0 - (1.0 - base.r) / (blend.r + 0.0001);
    
                // Green Channel
                result.g = (base.g >= 0.999) ? base.g : 1.0 - (1.0 - base.g) / (blend.g + 0.0001);
    
                // Blue Channel
                result.b = (base.b >= 0.999) ? base.b : 1.0 - (1.0 - base.b) / (blend.b + 0.0001);
    
                return saturate(result);
            }

            half3 BlendLinearBurn(half3 base, half3 blend) { 
                return base + blend - 1.0;           
            }

            half3 BlendLighten(half3 base, half3 blend) { 
                return max(base, blend); 
            }

            half3 BlendScreen(half3 base, half3 blend) { 
                return 1.0 - (1.0 - base) * (1.0 - blend); 
            }

            half3 BlendColorDodge(half3 base, half3 blend) { 
                return base / (1.0 - blend + 0.0001); 
            }

            half3 BlendLinearDodge(half3 base, half3 blend) { 
                return base + blend; 
            }
            
            half3 BlendOverlay(half3 base, half3 blend) {
                return half3(
                    base.r < 0.5 ? (2.0 * base.r * blend.r) : (1.0 - 2.0 * (1.0 - base.r) * (1.0 - blend.r)),
                    base.g < 0.5 ? (2.0 * base.g * blend.g) : (1.0 - 2.0 * (1.0 - base.g) * (1.0 - blend.g)),
                    base.b < 0.5 ? (2.0 * base.b * blend.b) : (1.0 - 2.0 * (1.0 - base.b) * (1.0 - blend.b))
                );
            }
            
            half3 BlendSoftLight(half3 base, half3 blend) {
                half3 result;
    
                // Red Channel
                if (blend.r < 0.5)
                {
                    result.r = base.r - (1.0 - 2.0 * blend.r) * base.r * (1.0 - base.r);
                }
                else
                {
                    if (base.r < 0.25)
                    {
                        float temp = (16.0 * base.r * base.r + 4.0 * base.r) * (base.r - 1.0) + 7.0 * base.r;
                        result.r = base.r + (2.0 * blend.r - 1.0) * temp;
                    }
                    else
                    {
                        result.r = base.r + (2.0 * blend.r - 1.0) * (sqrt(base.r) - base.r);
                    }
                }
    
                // Green Channel
                if (blend.g < 0.5)
                {
                    result.g = base.g - (1.0 - 2.0 * blend.g) * base.g * (1.0 - base.g);
                }
                else
                {
                    if (base.g < 0.25)
                    {
                        float temp = (16.0 * base.g * base.g + 4.0 * base.g) * (base.g - 1.0) + 7.0 * base.g;
                        result.g = base.g + (2.0 * blend.g - 1.0) * temp;
                    }
                    else
                    {
                        result.g = base.g + (2.0 * blend.g - 1.0) * (sqrt(base.g) - base.g);
                    }
                }
    
                // Blue Channel
                if (blend.b < 0.5)
                {
                    result.b = base.b - (1.0 - 2.0 * blend.b) * base.b * (1.0 - base.b);
                }
                else
                {
                    if (base.b < 0.25)
                    {
                        float temp = (16.0 * base.b * base.b + 4.0 * base.b) * (base.b - 1.0) + 7.0 * base.b;
                        result.b = base.b + (2.0 * blend.b - 1.0) * temp;
                    }
                    else
                    {
                        result.b = base.b + (2.0 * blend.b - 1.0) * (sqrt(base.b) - base.b);
                    }
                }
    
                return result;
            }
            
            half3 BlendHardLight(half3 base, half3 blend) { 
                return BlendOverlay(blend, base); 
            }
            
            half3 BlendVividLight(half3 base, half3 blend) {
                half3 result;
    
                // Red Channel
                result.r = (blend.r < 0.5)
                    ? 1.0 - (1.0 - base.r) / (2.0 * blend.r + 0.0001)
                    : base.r / (1.0 - 2.0 * (blend.r - 0.5) + 0.0001);
    
                // Green Channel
                result.g = (blend.g < 0.5)
                    ? 1.0 - (1.0 - base.g) / (2.0 * blend.g + 0.0001)
                    : base.g / (1.0 - 2.0 * (blend.g - 0.5) + 0.0001);
    
                // Blue Channel
                result.b = (blend.b < 0.5)
                    ? 1.0 - (1.0 - base.b) / (2.0 * blend.b + 0.0001)
                    : base.b / (1.0 - 2.0 * (blend.b - 0.5) + 0.0001);
    
                return saturate(result);
            }
            
            half3 BlendLinearLight(half3 base, half3 blend) {
                half3 result;
    
                // Red Channel
                result.r = saturate((blend.r < 0.5)
                    ? base.r + 2.0 * blend.r - 1.0
                    : base.r + 2.0 * (blend.r - 0.5));
    
                // Green Channel
                result.g = saturate((blend.g < 0.5)
                    ? base.g + 2.0 * blend.g - 1.0
                    : base.g + 2.0 * (blend.g - 0.5));
    
                // Blue Channel
                result.b = saturate((blend.b < 0.5)
                    ? base.b + 2.0 * blend.b - 1.0
                    : base.b + 2.0 * (blend.b - 0.5));
    
                return result;
            }
            
            half3 BlendPinLight(half3 base, half3 blend) {
                half3 result;
    
                // Red Channel
                result.r = (blend.r < 0.5)
                    ? min(base.r, 2.0 * blend.r)
                    : max(base.r, 2.0 * (blend.r - 0.5));
    
                // Green Channel
                result.g = (blend.g < 0.5)
                    ? min(base.g, 2.0 * blend.g)
                    : max(base.g, 2.0 * (blend.g - 0.5));
    
                // Blue Channel
                result.b = (blend.b < 0.5)
                    ? min(base.b, 2.0 * blend.b)
                    : max(base.b, 2.0 * (blend.b - 0.5));
    
                return result;
            }
            
            half3 BlendHardMix(half3 base, half3 blend) {
                half3 result;
    
                // Red Channel
                float vividR = (blend.r < 0.5)
                    ? 1.0 - (1.0 - base.r) / (2.0 * blend.r + 0.0001)
                    : base.r / (1.0 - 2.0 * (blend.r - 0.5) + 0.0001);
                result.r = (vividR < 0.5) ? 0.0 : 1.0;
    
                // Green Channel
                float vividG = (blend.g < 0.5)
                    ? 1.0 - (1.0 - base.g) / (2.0 * blend.g + 0.0001)
                    : base.g / (1.0 - 2.0 * (blend.g - 0.5) + 0.0001);
                result.g = (vividG < 0.5) ? 0.0 : 1.0;
    
                // Blue Channel
                float vividB = (blend.b < 0.5)
                    ? 1.0 - (1.0 - base.b) / (2.0 * blend.b + 0.0001)
                    : base.b / (1.0 - 2.0 * (blend.b - 0.5) + 0.0001);
                result.b = (vividB < 0.5) ? 0.0 : 1.0;
    
                return result;
            }
            
            half3 BlendDifference(half3 base, half3 blend) { 
                return abs(base - blend);   
            }

            half3 BlendExclusion(half3 base, half3 blend) { 
                return base + blend - 2.0 * base * blend; 
            }

            half3 BlendSubtract(half3 base, half3 blend) { 
                return max(base - blend, 0.0); 
            }

            half3 BlendDivide(half3 base, half3 blend) { 
                return base / (blend + 0.0001); 
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = tex2D(_BlendTex, IN.texcoord) * IN.color;
                half4 blend = tex2D(_MainTex, IN.blendtexcoord);
                
                // Apply selected blend mode
                half3 blendedColor = color.rgb;
                
                if (_BlendMode < 0.5) // Normal
                {
                    blendedColor = blend.rgb;
                }
                else if (_BlendMode < 1.5) // Darken
                {
                    blendedColor = BlendDarken(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 2.5) // Multiply
                {
                    blendedColor = BlendMultiply(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 3.5) // Color Burn
                {
                    blendedColor = BlendColorBurn(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 4.5) // Linear Burn
                {
                    blendedColor = BlendLinearBurn(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 5.5) // Lighten
                {
                    blendedColor = BlendLighten(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 6.5) // Screen
                {
                    blendedColor = BlendScreen(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 7.5) // Color Dodge
                {
                    blendedColor = BlendColorDodge(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 8.5) // Linear Dodge
                {
                    blendedColor = BlendLinearDodge(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 9.5) // Overlay
                {
                    blendedColor = BlendOverlay(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 10.5) // Soft Light
                {
                    blendedColor = BlendSoftLight(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 11.5) // Hard Light
                {
                    blendedColor = BlendHardLight(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 12.5) // Vivid Light
                {
                    blendedColor = BlendVividLight(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 13.5) // Linear Light
                {
                    blendedColor = BlendLinearLight(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 14.5) // Pin Light
                {
                    blendedColor = BlendPinLight(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 15.5) // Hard Mix
                {
                    blendedColor = BlendHardMix(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 16.5) // Difference
                {
                    blendedColor = BlendDifference(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 17.5) // Exclusion
                {
                    blendedColor = BlendExclusion(color.rgb, blend.rgb);
                }
                else if (_BlendMode < 18.5) // Subtract
                {
                    blendedColor = BlendSubtract(color.rgb, blend.rgb);
                }
                else // Divide
                {
                    blendedColor = BlendDivide(color.rgb, blend.rgb);
                }
                
                // Apply opacity
                color.rgb = lerp(color.rgb, blendedColor, _Opacity);
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}