using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelingScript : MonoBehaviour {

	float speed = 1;
	float level;
	public float target;
	public bool start;

	void Update () {
		if (start) {
			start = false;
			target += 0.1f;
			StartCoroutine ("AnimateLevel");
		}
	}

	public void UpdateLevel () {
		StartCoroutine ("AnimateLevel");
	}

	IEnumerator AnimateLevel () {
		while (level < target) {
			level += Time.deltaTime * speed;
			if (level < 1) {
				if (level < 0.4f)
					level += 0.4f;
				if (level > 0.85f)
					level += 0.15f;
				GetComponent <Image> ().fillAmount = level;
			} else if (level < 2) {
				if (level < 1.4f)
					level += 0.4f;
				if (level > 1.85f)
					level += 0.15f;
				transform.GetChild (0).GetComponent <Image> ().fillAmount = level - 1;
			} else if (level < 3) {
				if (level < 2.4f)
					level += 0.4f;
				if (level > 2.85f)
					level += 0.15f;
				transform.GetChild (1).GetComponent <Image> ().fillAmount = level - 2;
			} else if (level < 4) {
				if (level < 3.4f)
					level += 0.4f;
				if (level > 3.85f)
					level += 0.15f;
				transform.GetChild (2).GetComponent <Image> ().fillAmount = level - 3;
			} else if (level < 5) {
				if (level < 4.4f)
					level += 0.4f;
				if (level > 4.85f)
					level += 0.15f;
				transform.GetChild (3).GetComponent <Image> ().fillAmount = level - 4;
			}
			yield return 0f;
		}
		speed = 0.2f;
	}
}
