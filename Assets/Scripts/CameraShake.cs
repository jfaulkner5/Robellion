using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour 
{
	public static CameraShake cs;

	void Awake ()
	{
		cs = this;
	}

	public void Shake (float duration, float amount, float intensity)
	{
		StartCoroutine(ShakeCam(duration, amount, intensity));
	}

	IEnumerator ShakeCam (float dur, float amount, float intensity)
	{
		float t = dur;
		Vector3 originalPos = transform.position;
		Vector3 targetPos = Vector3.zero;

		while(t > 0.0f)
		{
			if(targetPos == Vector3.zero)
			{
				targetPos = originalPos + (Random.insideUnitSphere * amount);
			}

			transform.position = Vector3.Lerp(transform.position, targetPos, intensity * Time.deltaTime);

			if(Vector3.Distance(transform.position, targetPos) < 0.02f)
			{
				targetPos = Vector3.zero;
			}

			t -= Time.deltaTime;
			yield return null;
		}

		transform.position = originalPos;
	}
}
