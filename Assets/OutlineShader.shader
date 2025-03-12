Shader "Custom/FullObjectOutline"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (1, 1, 0, 1)
		_OutlineWidth("Outline Width", Range(0, 0.1)) = 0.03
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		// Première passe : Outline (plus grand)
		Pass
		{
			Cull Front
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			float _OutlineWidth;
			float4 _OutlineColor;

			v2f vert(appdata_t v)
			{
				v2f o;
				float3 norm = normalize(v.normal);
				v.vertex.xyz += norm * _OutlineWidth; // Étend l’objet
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}

		// Deuxième passe : Modèle normal
		Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 pos : SV_POSITION;
			};

			float4 _Color;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
