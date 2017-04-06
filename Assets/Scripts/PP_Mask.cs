using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Mask : MonoBehaviour {
	[SerializeField] SpriteRenderer[] myMaskSpriteRenderers;

	// Update is called once per frame
	public void SetMyMeterialStencilRef (int g_StencilRef) {
		for (int i = 0; i < myMaskSpriteRenderers.Length; i++) {
//			myMaskSpriteRenderers [i].material.SetInt ("_StencilRef", g_StencilRef + 1);
//			myMaskSpriteRenderers [i].material.renderQueue = 3000 + g_StencilRef + 1;


			myMaskSpriteRenderers [i].material.SetInt ("_StencilRef", 1);
			myMaskSpriteRenderers [i].material.renderQueue = 3001;
		}
	}
}
