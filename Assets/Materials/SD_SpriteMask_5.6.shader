Shader "Custom/Mask/SD_SpriteMask_5.6" 
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		_MaskTex ("Mask Texture", 2D) = "white" {}
		_Progress ("Progress", Range(0.0000, 1.0000)) = 0
		[MaterialToggle] _Reverse ("Reverse", Float) = 0
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment MySpriteFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

//			sampler2D _MainTex;
//			sampler2D _AlphaTex;

			sampler2D _MaskTex;
			float _Progress;
            float _Reverse;

			fixed4 MySampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				fixed4 colorMask = tex2D (_MaskTex, uv);

				#if ETC1_EXTERNAL_ALPHA
					fixed4 alpha = tex2D (_AlphaTex, uv);
					color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
				#endif

				if (_Reverse == 0) {
					if (colorMask.a <= _Progress) {
						color.a = 0;
					}
				} else {
					if (colorMask.a > _Progress) {
						color.a = 0;
					}
				}

    			return color;
			}

			fixed4 MySpriteFrag (v2f IN) : SV_Target
			{
				fixed4 c = MySampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}

		ENDCG
		}
	}
}
