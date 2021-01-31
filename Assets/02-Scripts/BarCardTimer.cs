using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;

public class BarCardTimer : MonoBehaviour {
	[SerializeField] private Image filler;
	[SerializeField] private Image background;
	[Space]
	[SerializeField] private AnimationCurve fadeInCurve;
	[SerializeField] private float fillAmount;
	
	public void SetSlider(float value) {
		filler.fillAmount = value;
		fillAmount = value;
		float alpha = fadeInCurve.Evaluate(1 - filler.fillAmount);
		Color color = filler.color;
		filler.color = new Color(color.r, color.g, color.b, alpha);
		color = background.color;
		background.color = new Color(color.r, color.g, color.b, alpha);
	}
}