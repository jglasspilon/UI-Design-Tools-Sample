Shader "Custom/ChartUpDown"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_GreenColor ("Green Color", Color) = (1,1,1,1)
        _RedColor ("Red Color", Color) = (1,1,1,1)
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

				float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			uniform float4 _RedColor;
			uniform float4 _GreenColor;
			uniform float _PDC_Line;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//World Space Position
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				//Screen Space Position
				//o.scrPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {	
				fixed4 col;

				if (i.worldPos.y < _PDC_Line)
				{
					col = _RedColor;
				}
				else
				{
					col = _GreenColor;
				}

                return col;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
