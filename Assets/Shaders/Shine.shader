Shader "Sprites/Shine"
{
    Properties
    {
       [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} 
       _Color ("Tint", Color) = (1,1,1,1)
       _Mask ("Mask", 2D) = "white" {}
       _Speed ("Speed", Float) = 1
       _Direction ("Direction", Vector) = (1,1, 0, 0)
       _TimeOffset ("Time Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Lighting Off
        Cull Off

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            sampler2D _Mask;
            float _Speed;
            float4 _Direction;
            float _TimeOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if(col.a < .5) discard;
                float x = frac(_Time[1]* _Speed + _TimeOffset) + dot(_Direction.xy, i.uv);
                float t = sin(x * 6.28318530718) * .5 + .5; 
                fixed4 overlay = tex2D(_Mask, float2(t, .5));
                return col * _Color * i.color + overlay;
            }
            ENDCG
        }
    }
}
