using UnityEngine;
using System.Collections;

public static class Animate {


	// Bouncy animation in from start to end
	public static float BounceIn (float start, float end, float value) {
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin (value * Mathf.PI * (2.5f * Mathf.Pow (value, 2))) * Mathf.Pow (1 - value, 2.2f) + value) * (1 + 1.2f * (1 - value));
		return start + (end - start) * value;
	}

	// Bouncy animation out from end to start
	public static float BounceOut (float end, float start, float value) {
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin (value * Mathf.PI * (2.5f * Mathf.Pow (value, 2))) * Mathf.Pow (1 - value, 2.2f) + value) * (1 + (1 - value));
		return start + (end - start) * value;
	}
}
