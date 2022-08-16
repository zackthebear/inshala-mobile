using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RegisterScript : MonoBehaviour {

	int currentScreen;

	[SerializeField]
	Transform btnNext;

	[SerializeField]
	InputField[] input;

    Register userInfo = new Register();

	public void OpenRegister () {
		AlertScript.Singleton.ActivityIndicator (true);
		StartCoroutine ("Startup");
	}
	
	IEnumerator Startup () {
		float count = 0;

		while (count <= 1) {
			transform.localScale = Animate.BounceIn (0, 1, count) * Vector3.one;
			count += Time.deltaTime * 2;
			yield return 0f;
		}
	}

	void Update () {
		Vector3 target;
		switch (currentScreen) {
		case 0:
                {
                    target = input[currentScreen].text.Length > 5 && input[currentScreen].text.Contains(" ") ? Vector3.one : Vector3.zero;
                    userInfo.firstname = input[0].text;
                    break;
                }
		case 1:
                {
                    target = input[currentScreen].text.Contains("@") && input[currentScreen].text.Contains(".") ? Vector3.one : Vector3.zero;
                    userInfo.username = input[1].text;
                    userInfo.mail = input[1].text;
                    break;
                }
		case 2:
                {
                    target = input[currentScreen].text.Length == 10 ? Vector3.one : Vector3.zero;
                    userInfo.cellphone = input[2].text;
                    break;
                }
		case 3:
                {
                    if (input[3].text != "" && input[4].text != "" && input[5].text != "")
                        target = int.Parse(input[3].text) < 32 && int.Parse(input[4].text) <= 12 && int.Parse(input[5].text) > 1920 ? Vector3.one : Vector3.zero;
                    else
                        target = Vector3.zero;
                    break;
                }
		default:
			target = Vector3.one;
			break;
		}
		transform.Find ("btnNext").localScale = Vector3.Slerp (transform.Find ("btnNext").localScale, target, Time.deltaTime * 8);
	}

	public void NextScreen () {
		if (currentScreen + 1 > 4) // Aqui compara con el total de campos
			StartCoroutine ("Finish", "5555");
		else
			StartCoroutine ("ChangeScreens", true);
	}

	public void PrevScreen () {
		if (currentScreen - 1 < 0)
			StartCoroutine ("Finish", "x");
		else
			StartCoroutine ("ChangeScreens", false);
	}

	IEnumerator ChangeScreens (bool next) {
		Transform prev = transform.GetChild (currentScreen);
		currentScreen = next ? currentScreen + 1 : currentScreen - 1;
		Transform current = transform.GetChild (currentScreen);
		float count = 0;

		while (count <= 1) {
			if (next)
				prev.localPosition = Vector3.Lerp (prev.localPosition, new Vector3 (-Screen.width * 2, 155, 0), Time.deltaTime * 5);
			else
				prev.localPosition = Vector3.Lerp (prev.localPosition, new Vector3 (Screen.width * 2, 155, 0), Time.deltaTime * 5);
			current.localPosition = Vector3.Lerp (current.localPosition, new Vector3 (0, 155f, 0), Time.deltaTime * 5);
			count += Time.deltaTime;
			yield return 0f;
		}
	}

	IEnumerator Finish (string clientNumber) {
		float count = 0;


        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        StartCoroutine ( thisCaller.registerUser(this.userInfo) );

        while (!thisCaller.lastServiceConsult)
        {
            Debug.Log("Procesando");
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("Termino registro");

        GameObject.Find ("Home").GetComponent <HomeScript> ().HideKeypad ();
		while (count <= 1) {
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.zero, Time.deltaTime * 10);
			count += Time.deltaTime;
			yield return 0f;
		}
		AlertScript.Singleton.ActivityIndicator (false);
		if (clientNumber != "x")
			GameObject.Find ("Passcode").GetComponent <PinScript> ().EnterPIN (clientNumber);
		else {
			for (int i = 0; i < input.Length; i++)
				input [i].text = "";
		}
	}
}
