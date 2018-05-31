Shader "Custom/RotateShader" {
	Properties
	{
		_MainTex("Main Tex",2D) = "Write"{}
	    _RotateAngle("Rotate Angle",float) = 0
	    _UA("RotatePointX",Float) = 0.5
		_UB("RotatePointY",Float) = 0.5
		_CenterX("MoveX",float) = 0
		_CenterY("MoveY",float) = 0
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _RotateAngle;
		float _UA;
		float _UB;
		float _CenterX;
		float _CenterY;
	struct v2f
	{
		float4 pos:POSITION;
		float4 uv:TEXCOORD;
	};
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
		o.uv = v.texcoord;
		return o;
	}
	fixed4 frag(v2f i) :SV_Target
	{  
		float2 di = float2(_UA,_UB);
		float2 uv = mul(float3(i.uv - di, 1), float3x3(1, 0, 0, 0, 1, 0, _CenterX, _CenterY,1)).xy;
		uv = mul(uv,float2x2(cos((_RotateAngle*3.141592653)/180), - sin((_RotateAngle*3.141592653) / 180),
			sin((_RotateAngle*3.141592653) / 180) , cos((_RotateAngle*3.141592653) / 180))) + di;
		uv += float2(0.5,0.5);

		fixed4 c = tex2D(_MainTex,TRANSFORM_TEX(uv,_MainTex));
		return c;
	}
		ENDCG
	}
	}
}

