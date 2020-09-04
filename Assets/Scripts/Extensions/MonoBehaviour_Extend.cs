using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviour_Extend
{
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
	/// Invoke [action] after [delay] seconds.
	/// </summary>
	public static void DelayedAction(this MonoBehaviour runner, Action action, float delay, bool unscaledTime = false)
	{
		runner.StartCoroutine(DelayedActionRoutine(action, delay, unscaledTime));
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

	private static IEnumerator DoActionOnTickRoutine(Action action)
	{
		while (true)
		{
			action?.Invoke();

			yield return null;
		}
	}

	/// <summary>
	/// Infinite call of an action every frame. Use StopCoroutine to stop calling.
	/// </summary>
	public static Coroutine DoActionOnTick(this MonoBehaviour runner, Action action)
	{
		return runner.StartCoroutine(DoActionOnTickRoutine(action));
	}
}
