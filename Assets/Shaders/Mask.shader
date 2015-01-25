    Shader "Custom/Mask" {
	    Properties {
      _Opacity ("Opacity", Float) = 1.0
    }
        SubShader {
            Pass {

    CGPROGRAM

    #pragma vertex vert
    #pragma fragment frag
    #include "UnityCG.cginc"

    struct v2f {
        float4 pos : SV_POSITION;
    };

    v2f vert (appdata_base v)
    {
        v2f o;
        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
        return o;
    }

	float _Opacity;

    half4 frag (v2f i) : COLOR
    {
        return half4 (_Opacity,0,0,0);
    }
    ENDCG

            }
        }
    }
