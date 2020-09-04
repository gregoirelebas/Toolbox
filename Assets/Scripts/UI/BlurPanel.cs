using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/BlurPanel")]
public class BlurPanel : Image
{
	[Header("Blur")]
	[SerializeField] [Range(0.0f, 4.0f)] private float blurValue = 1.0f;
	[SerializeField] private float delay = 0.0f;
	[SerializeField] private bool isAnimated = true;
	[SerializeField] private float time = 0.5f;

	private MonoBehaviour mono = null;
	private CanvasGroup group = null;
	private LerpData blurLerp = null;

	protected override void Awake()
	{
		base.Awake();

		mono = gameObject.GetComponent<MonoBehaviour>();
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
					blurLerp = LerpHelper.SetLerp(mono, 0.0f, blurValue, time);

					this.DoActionOnTick(() =>
					{
						if (blurLerp != null && blurLerp.infos.isRunning)
						{
							SetBlurValue(blurLerp.infos.@float);
						}
						else
						{
							StopAllCoroutines();
						}
					});
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
