using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolbox
{
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("UI/BlurPanel")]
	public class BlurPanel : Image
	{
		[Header("Blur")]
		[SerializeField] [Range(0.0f, 4.0f)] private float blurValue = 1.0f;
		[SerializeField] private float delay = 0.0f;
		[SerializeField] private bool isAnimated = true;
		[SerializeField] private float time = 0.5f;

		private CanvasGroup group = null;

		protected override void Awake()
		{
			base.Awake();

			group = GetComponent<CanvasGroup>();
		}

		protected override void Reset()
		{
			base.Reset();

			color = Color.black * 0.1f;
			SetBlurValue(blurValue);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (Application.isPlaying)
			{
				if (isAnimated)
				{
					SetBlurValue(0.0f);

					this.DelayedAction(() =>
					{
						LeanTween.value(0.0f, blurValue, time).setOnUpdate(SetBlurValue);
					}, delay);
				}
				else
				{
					SetBlurValue(blurValue);
				}
			}
		}

		private void SetBlurValue(float value)
		{
			material.SetFloat("_Size", value);
			group.alpha = value;
		}
	}
}
