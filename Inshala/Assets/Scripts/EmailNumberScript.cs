using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class EmailNumberScript : MonoBehaviour {

	public void Startup () {
		AlertScript.Singleton.ActivityIndicator (true);
		StartCoroutine ("Show", true);
	}

	IEnumerator Show (bool show) {
		Vector3 target = show ? Vector3.one : Vector3.zero;
		float count = 0;

		while (count <= 1) {
			transform.localScale = Vector3.Slerp (transform.localScale, target, Time.deltaTime * 10);
			count += Time.deltaTime;
			yield return 0f;
		}
		GameObject.Find ("Home").GetComponent <HomeScript> ().HideKeypad ();
	}

	public void SendEmail () {
		string email = transform.Find ("info_Correo").Find ("InputField").GetComponent <InputField> ().text;
		Debug.Log ("Enviar correo a: " + email);
		Back ();
	}

	public void Back () {
		StartCoroutine ("Show", false);
		AlertScript.Singleton.ActivityIndicator (false);
	}
}
