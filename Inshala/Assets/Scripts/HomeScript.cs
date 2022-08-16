using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour {
	
	public void ShowKeypad () {
		StopCoroutine ("HideKeypadAnimation");
		StartCoroutine ("ShowKeypadAnimation");
	}

	IEnumerator ShowKeypadAnimation () {
		float count = 0;
		Image blurred = transform.GetChild (0).GetComponent <Image> ();
		transform.Find ("Logo").GetComponent <Animator> ().SetBool ("logo", false);
		bool once = true;

		while (count < 4) {
			blurred.color = new Color (1, 1, 1, Mathf.Lerp (blurred.color.a, 1, Time.deltaTime * 10));
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.one * 1.1f, Time.deltaTime * 3);
			count += Time.deltaTime;
			yield return 0f;
			if (once && count > 0.3f) {
				once = false;
				transform.Find ("Keypad").GetComponent <Animator> ().SetBool ("keypad", true);
			}
		}
	}

	public void HideKeypad () {
		StopCoroutine ("ShowKeypadAnimation");
		StartCoroutine ("HideKeypadAnimation");
		transform.Find ("Keypad").GetComponent <Animator> ().SetBool ("keypad", false);
	}

	IEnumerator HideKeypadAnimation () {
		float count = 0;
		Image blurred = transform.GetChild (0).GetComponent <Image> ();
		bool once = true;
		transform.Find ("Logo").GetComponent <Animator> ().SetBool ("logo", true);

		while (count < 3) {
			blurred.color = new Color (1, 1, 1, Mathf.Lerp (blurred.color.a, 0, Time.deltaTime * 6));
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.one, Time.deltaTime * 4);
			count += Time.deltaTime;
			yield return 0f;
			if (count > 0.2f) 
				transform.Find ("Logo").GetComponent <Animator> ().SetBool ("logo", true);
			if (once && count > 1.2f) {
				once = false;
				GetComponent <Image> ().raycastTarget = true;
			}
		}
	}

	public void LoadDashboard () {
		StartCoroutine ("LoadDashboardScene");
	}

	IEnumerator LoadDashboardScene () {
		float count = 0;
		Image white = transform.Find ("White").GetComponent <Image> ();
		white.raycastTarget = true;
		bool once = true;

		while (count < 1.5f) {
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.one * 1.3f, Time.deltaTime * 4);
			white.color = new Color (white.color.r, white.color.g, white.color.b, Mathf.Lerp (white.color.a, 1, Time.deltaTime * 4));
			count += Time.deltaTime;
			yield return 0f;
			if (once && count > 1) {
				once = false;
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Dashboard");
			}
		}
	}
}