  Shader "Example/Custom Vertex Data" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
          float3 customColor;
		  float4 screenPos;
      };
      void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          o.customColor = abs(v.normal);
		  o.screenPos = mul (UNITY_MATRIX_MVP, v.vertex);
		  o.screenPos.xyz/=o.screenPos.w;
      }
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
		   float2 wcoord = IN.screenPos.xy;///_ScreenParams.xy;
         // o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          o.Albedo = float3(wcoord.x,wcoord.y,0);//IN.customColor;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }