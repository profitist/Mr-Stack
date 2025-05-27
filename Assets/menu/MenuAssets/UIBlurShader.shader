Shader "Custom/UIBlur" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Blur Radius", Range(0, 20)) = 5
    }
    SubShader {
        Tags { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent" 
        }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                fixed4 col = fixed4(0, 0, 0, 0);
                float sum = 0;
                
                for (float x = -_Radius; x <= _Radius; x++) {
                    for (float y = -_Radius; y <= _Radius; y++) {
                        float2 offset = float2(x, y) * 0.001;
                        col += tex2D(_MainTex, uv + offset);
                        sum += 1;
                    }
                }

                col /= sum;
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}