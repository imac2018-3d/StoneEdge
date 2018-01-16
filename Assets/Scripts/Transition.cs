using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
	public float time = 2.0f;
	public delegate void TransitionCallBack();

	private TransitionCallBack transitionEndCallback = null;

	private CanvasGroup canvasElement;
	private Coroutine currentCoroutine = null;

	bool isClosed;
	float currentTarget=-1;

	public void close(TransitionCallBack endCallback = null, float time = -1)
	{
		if (isClosed || currentTarget==1)
			return;
		if (time < 0)
			time = this.time;
		if (currentCoroutine != null)
			StopCoroutine(currentCoroutine);
		transitionEndCallback = endCallback;
		currentCoroutine = StartCoroutine(FadeCanvasGroup(canvasElement, canvasElement.alpha, 1, time));
	}
	public void open(TransitionCallBack endCallback = null, float time = -1)
	{
		if (!isClosed || currentTarget==0)
			return;
		if (time < 0)
			time = this.time;
		if (currentCoroutine != null)
			StopCoroutine(currentCoroutine);
		transitionEndCallback = endCallback;
		StartCoroutine(FadeCanvasGroup(canvasElement, canvasElement.alpha, 0, time));
	}

	public void animate()
	{
		if (isClosed)
			open();
		else
			close();
	}

	public void Awake()
	{
		canvasElement = GetComponentInChildren<CanvasGroup>();
		open(null,0.01f);
	}

	public bool IsRunning()
	{
		return currentCoroutine != null;
	}

	// Use this for initialization
	public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float time = 0.5f)
	{
		float velocity = 0.05f;

		transform.hideFlags = HideFlags.None;
		cg.alpha = start;
		currentTarget = end;
		while (true)
		{
			time -= Time.smoothDeltaTime;
			if (time <= 0) {
				cg.alpha = end;
				break;
			}

			cg.alpha = Mathf.SmoothDamp(cg.alpha, end, ref velocity, time, Mathf.Infinity, Time.smoothDeltaTime);

			yield return new WaitForEndOfFrame();
		}
		if (cg.alpha <= 0)
		{
			transform.hideFlags = HideFlags.HideInHierarchy;
			isClosed = false;
		}
		else
			isClosed = true;
		currentCoroutine = null;
		currentTarget = -1;
		if (transitionEndCallback != null)
			transitionEndCallback();
		transitionEndCallback = null;
	}
}
