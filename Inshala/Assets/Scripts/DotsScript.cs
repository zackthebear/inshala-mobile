using System.Collections;
using UnityEngine.UI;
//using System.Collections.Generic;
using UnityEngine;

public class DotsScript : MonoBehaviour {

	int currentScreen;

	void Start () {
		StartCoroutine ("Startup");
	}

	IEnumerator Startup () {
		float count = 0;
		LayoutElement curr = transform.GetChild (currentScreen).GetComponent <LayoutElement> ();
		yield return new WaitForSeconds (1);
		while (count <= 1) {
			curr.preferredWidth = Animate.BounceIn (10, 30, count);
			curr.preferredHeight = Animate.BounceIn (10, 30, count);
			count += Time.deltaTime * 2;
			yield return 0f;
		}
	}

	public void NextScreen () {
		StartCoroutine ("NextDot");
	}

	IEnumerator NextDot () {
		if ((currentScreen + 1) == transform.childCount) {
			Debug.Log ("Done");
			yield break;
		}
		float count = 0;

		LayoutElement curr = transform.GetChild (currentScreen).GetComponent <LayoutElement> ();
		LayoutElement next = transform.GetChild (currentScreen + 1).GetComponent <LayoutElement> ();
		Image dot = transform.GetChild (currentScreen + 1).GetComponent <Image> ();
		currentScreen++;

		while (count <= 1.1f) {
			curr.preferredWidth = Animate.BounceOut (20, 30, count);
			curr.preferredHeight = Animate.BounceOut (20, 30, count);
			next.preferredHeight = Animate.BounceIn (10, 30, count - 0.1f);
			next.preferredWidth = Animate.BounceIn (10, 30, count - 0.1f);
			dot.color = new Color (dot.color.r, dot.color.g, dot.color.b, Mathf.Lerp (0.5f, 1, count - 0.1f));
			count += Time.deltaTime * 2;
			yield return 0f;
		}
	}

	public void PrevScreen () {
		if (currentScreen - 1 < 0) {
			return;
		}
		StartCoroutine ("PrevDot");
	}

	IEnumerator PrevDot () {
		LayoutElement prev = transform.GetChild (currentScreen).GetComponent <LayoutElement> ();
		LayoutElement curr = transform.GetChild (currentScreen - 1).GetComponent <LayoutElement> ();
		float count = 0;
		currentScreen--;

		while (count <= 1.1f) {
			curr.preferredHeight = Animate.BounceIn (20, 30, count);
			curr.preferredWidth = Animate.BounceIn (20, 30, count);
			prev.preferredWidth = Animate.BounceOut (20, 30, count - 0.1f);
			prev.preferredHeight = Animate.BounceOut (20, 30, count - 0.1f);
			count += Time.deltaTime * 2;
			yield return 0f;
		}
	}
}
