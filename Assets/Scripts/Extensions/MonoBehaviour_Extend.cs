using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviour_Extend
{
	/// <summary>
	/// Invoke [action] after [delay] seconds.
	/// </summary>
	public static void DelayedAction(this MonoBehaviour runner, Action action, float delay, bool unscaledTime = false)
	{
		runner.StartCoroutine(DelayedActionRoutine(action, delay, unscaledTime));
	}

	private static IEnumerator DelayedActionRoutine(Action action, float delay, bool unscaledTime = false)
	{
		if (unscaledTime)
		{
			yield return new WaitForSecondsRealtime(delay);
		}
		else
		{
			yield return new WaitForSeconds(delay);
		}

		action?.Invoke();
	}

	/// <summary>
	/// Disable the GameObject after [delay] seconds.
	/// </summary>
	public static void DisableAfterDelay(this MonoBehaviour runner, float delay, bool unscaledTime = false)
	{
		runner.StartCoroutine(DelayedActionRoutine(() =>
		{
			runner.gameObject.SetActive(false);
		}, delay, unscaledTime));
	}
}
