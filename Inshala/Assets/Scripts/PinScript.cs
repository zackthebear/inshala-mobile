using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PinScript : MonoBehaviour {

	private string pin = "";
	bool wrong;

	string scene;

	void Start () {
		scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
	}

	public void Startup (string type) {
		transform.parent.GetComponent <Animator> ().SetBool ("keypad", true);
	}

	public void EnterPIN (string num) {
		// Wait for currently animating wrong entry to finish
		if (wrong)
			return;
		if (num == "?") {
			AlertScript.OnAlertResponse += AlertResponded;
			AlertScript.Singleton.AlertTwoButton ("¿Has olvidado tu número de cliente?", "Volver", "Sí");
			return;
		}
		
		// First check if 'x' to delete last number or return to Home screen
		if (num == "x") {
			if (pin.Length > 0) {
				pin = pin.Substring (0, pin.Length - 1);
				transform.GetChild (pin.Length).transform.GetChild (0).gameObject.SetActive (true);
			} else {
				if (scene == "Home")
					transform.parent.parent.GetComponent <HomeScript> ().HideKeypad ();
				else
					transform.parent.GetComponent <Animator> ().SetBool ("keypad", false);
			}
		} else {
			if (num.Length < 3)
				pin += num;
			else
				pin = num;
			transform.GetChild (pin.Length - 1).transform.GetChild (0).gameObject.SetActive (false); // False to hide circle (show as complete)
		}
		if (pin.Length > 3) {
            // HERE IS WHERE WE NEED TO COMPARE PIN WITH USER INFO
            StartCoroutine("ComparePin");
		}
	}

	IEnumerator Wrong () {
		GetComponent <Animator> ().SetBool ("wrong", wrong);
		yield return new WaitForSeconds (0.2f);
		foreach (Transform child in transform) {
			yield return new WaitForSeconds (0.03f);
			child.GetChild (0).gameObject.SetActive (true);
		}
		pin = "";
		wrong = false;
		GetComponent <Animator> ().SetBool ("wrong", wrong);
        AlertScript.Singleton.ActivityIndicator(false);

    }

    IEnumerator ComparePin ()
    {
        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        
        AlertScript.Singleton.ActivityIndicator(true);

		StartCoroutine( thisCaller.LoginByPin(pin) );

        while (!thisCaller.lastServiceConsult)
        {
            yield return new WaitForSeconds(0.01f);
        }

        if (thisCaller.succesfulLogin)
        {
			thisCaller.lastPin = pin;
			if (scene == "Home")
            	transform.parent.parent.GetComponent<HomeScript>().LoadDashboard();
			if (scene == "Maestro") {
				AlertScript.OnAlertResponse += RegisterResponse;
				// Necesitamos conseguir el nombre del alumno...
				string nombre = "Nombre - " + pin;
				AlertScript.Singleton.AlertTwoButton ("Confirmar registro de:\n" + nombre, "Cancelar", "Confirmar", false);
			}
        }
        else
        {
            wrong = true;
            StartCoroutine("Wrong");
            AlertScript.Singleton.AlertOneButton("Usuario no encontrado", "Continuar", true);
            AlertScript.OnAlertResponse += continuarError;
        }
    }

	void AlertResponded (string button) {
		AlertScript.OnAlertResponse -= AlertResponded;
		if (button == "Sí") {
			GameObject.Find ("EmailNumber").GetComponent <EmailNumberScript> ().Startup ();
		}
	}

    void continuarError(string boton)
    {
        AlertScript.OnAlertResponse -= continuarError;
    }

	void RegisterResponse (string button) {
		AlertScript.OnAlertResponse -= RegisterResponse;
		if (button == "Confirmar") { // AQUI SE REGISTRA EL NUEVO ALUMNO A LA CLASE!
			Debug.Log ("Aqui se debe registrar el nuevo alumno a la clase");
			GameObject.Find ("Content").GetComponent <MaestroScript> ().Registro (pin);

		}
		foreach (Transform child in transform) 
			child.GetChild (0).gameObject.SetActive (true);
		pin = "";
		transform.parent.GetComponent <Animator> ().SetBool ("keypad", false);
	}
}
