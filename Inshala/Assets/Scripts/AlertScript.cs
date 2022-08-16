using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertScript : MonoBehaviour {

	public static AlertScript Singleton;

	public delegate void AlertSelection (string buttonPressed);
	public static AlertSelection OnAlertResponse;

	bool indicator;
	bool isActive;
	bool canCancel;

	[SerializeField] Text message;
	[SerializeField] Button [] btns;

	void Awake () {
		if (Singleton == null) {
			Singleton = this;
			DontDestroyOnLoad (transform.root);
		} else
			Destroy (transform.root.gameObject);
	}

	public void ActivityIndicator (bool shouldShow) {
		indicator = shouldShow;
		StartCoroutine ("ShowBackground");
		transform.parent.Find ("ActivityIndicator").GetComponent <Animator> ().SetBool ("show", shouldShow);
	}

	#region No Button Alert

	public void AlertNoButton (string alertText) {
		// If an alert is already active, wait for it to be removed
		if (isActive) {
			StartCoroutine (WaitAlertNoButton (alertText));
			return;
		}
		isActive = true;
		canCancel = true;
		StartCoroutine ("ShowBackground");
		message.text = alertText;
		GetComponent <Animator> ().SetBool ("show", true);
	}

	IEnumerator WaitAlertNoButton (string alertText) {
		while (isActive) {
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.3f);
		AlertNoButton (alertText);
	}

	#endregion
	#region One Button Alert

	public void AlertOneButton (string alertText, string buttonText) {
		// If an alert is already active, wait for it to be removed
		if (isActive) {
			StartCoroutine (WaitAlertOneButton (alertText, buttonText, true));
			return;
		}
		isActive = true;
		canCancel = true;
		StartCoroutine ("ShowBackground");
		message.text = alertText;
		btns [0].gameObject.SetActive (true);
		btns [0].interactable = true;
		GetComponent <Animator> ().SetBool ("show", true);
	}

	public void AlertOneButton (string alertText, string buttonText, bool canCancelAlert) {
		// If an alert is already active, wait for it to be removed
		if (isActive) {
			StartCoroutine (WaitAlertOneButton (alertText, buttonText, canCancelAlert));
			return;
		}
		isActive = true;
		canCancel = canCancelAlert;
		StartCoroutine ("ShowBackground");
		message.text = alertText;
		btns [0].transform.GetChild (0).GetComponent <Text> ().text = buttonText;
		btns [0].gameObject.SetActive (true);
		btns [0].interactable = true;
		GetComponent <Animator> ().SetBool ("show", true);
	}

	IEnumerator WaitAlertOneButton (string alertText, string buttonText, bool canCancelAlert) {
		while (isActive) {
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.3f);
		AlertOneButton (alertText, buttonText, canCancelAlert);
	}

	#endregion
	#region Two Button Alert

	public void AlertTwoButton (string alertText, string buttonTextLeft, string buttonTextRight) {
		// If an alert is already active, wait for it to be removed
		if (isActive) {
			StartCoroutine (WaitAlertTwoButton (alertText, buttonTextLeft, buttonTextRight, true));
			return;
		}
		isActive = true;
		canCancel = true;
		StartCoroutine ("ShowBackground");
		message.text = alertText;
		btns [1].transform.GetChild (0).GetComponent <Text> ().text = buttonTextLeft;
		btns [1].gameObject.SetActive (true);
		btns [1].interactable = true;
		btns [2].transform.GetChild (0).GetComponent <Text> ().text = buttonTextRight;
		btns [2].gameObject.SetActive (true);
		btns [2].interactable = true;
		GetComponent <Animator> ().SetBool ("show", true);
	}

	public void AlertTwoButton (string alertText, string buttonTextLeft, string buttonTextRight, bool canCancelAlert) {
		// If an alert is already active, wait for it to be removed
		if (isActive) {
			StartCoroutine (WaitAlertTwoButton (alertText, buttonTextLeft, buttonTextRight, canCancelAlert));
			return;
		}
		isActive = true;
		canCancel = canCancelAlert;
		StartCoroutine ("ShowBackground");
		message.text = alertText;
		btns [1].transform.GetChild (0).GetComponent <Text> ().text = buttonTextLeft;
		btns [1].gameObject.SetActive (true);
		btns [1].interactable = true;
		btns [2].transform.GetChild (0).GetComponent <Text> ().text = buttonTextRight;
		btns [2].gameObject.SetActive (true);
		btns [2].interactable = true;
		GetComponent <Animator> ().SetBool ("show", true);
	}

	IEnumerator WaitAlertTwoButton (string alertText, string buttonTextLeft, string buttonTextRight, bool canCancelAlert) {
		while (isActive) {
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.3f);
		AlertTwoButton (alertText, buttonTextLeft, buttonTextRight, canCancelAlert);
	}

	#endregion

	public void ButtonSelection (int selection) {
		if (OnAlertResponse != null)
			OnAlertResponse (btns [selection].transform.GetChild (0).GetComponent <Text> ().text);
		StartCoroutine ("CancelAlert");
	}

	// When user taps on the background, if canCancel is true, cancel the alert
	public void TapCancelAlert () {
		if (canCancel) {
			if (OnAlertResponse != null)
				OnAlertResponse ("Cancelar");
			StartCoroutine ("CancelAlert"); 
		}
	}

	IEnumerator CancelAlert () {
		isActive = false;
		GetComponent <Animator> ().SetBool ("show", false);
		StartCoroutine ("ShowBackground");
		yield return new WaitForSeconds (0.2f);
		for (int i = 0; i < btns.Length; i++)
			btns [i].gameObject.SetActive (false);
	}

	IEnumerator ShowBackground () {
		Image bkgrnd = transform.parent.GetComponent <Image> ();
		bkgrnd.raycastTarget = isActive;

		while ((isActive || indicator) && bkgrnd.color.a < 0.6f) {
			bkgrnd.color = new Color (bkgrnd.color.r, bkgrnd.color.g, bkgrnd.color.b, Mathf.Lerp (bkgrnd.color.a, 0.7f, Time.deltaTime * 2));
			yield return 0f;
		}
		while (!isActive && !indicator && bkgrnd.color.a > 0) {
			bkgrnd.color = new Color (bkgrnd.color.r, bkgrnd.color.g, bkgrnd.color.b, Mathf.Lerp (bkgrnd.color.a, -0.1f, Time.deltaTime * 3));
			yield return 0f;
		}
	}
}
