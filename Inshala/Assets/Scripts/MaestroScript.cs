using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaestroScript : MonoBehaviour {

	[SerializeField]
	Transform prefabAlumno;

	[SerializeField]
	Text txtNumMaestro;
	[SerializeField]
	Text txtNombreMaestro;
	[SerializeField]
	Text txtInfoClase;

	// Use this for initialization
	void Start () {
		StartCoroutine ("LoadAlumnos");
	}

	IEnumerator LoadAlumnos () {
		// DELETE
		Debug.LogWarning ("TEMPORAL!!! Aqui se deberia de cargar la clase y los alumnos de la clase"); // TEMPORAL!! Cargar info de clase
		txtNumMaestro.text = "9999";
		txtNombreMaestro.text = "Oso Yogui";
		txtInfoClase.text = "Martes 14/02 | 6:45pm";

		string[] pin = new string[] { "123", "456" }; //TEMPORAL!!! Aqui necesitamos los pines de los registrados

		for (int i = 0; i < pin.Length; i++) {
			StartCoroutine ("AgregarAlumno", pin [i]);
			yield return new WaitForSeconds (0.15f);
		}
	}

	IEnumerator AgregarAlumno (string pin) {
		Debug.Log ("Aqui se crea el prefab con el nuevo alumno que se va a registrar"); // TEMPORAL!! Aqui se consigue la info del alumno
		Transform alumno = Instantiate (prefabAlumno, transform).transform;
		alumno.GetComponent <AlumnoScript> ().Startup ("ASIGNAR! " + pin, pin, "ASIGNAR", null);
		alumno.gameObject.SetActive (true);
		yield break;
	}

	public void Registro (string pin) {
		StartCoroutine ("AgregarAlumno");
	}

	public void Home () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Home");
	}

	public void StartClass () {
		string confirmados = "";
		foreach (Transform child in transform) {
			if (child.GetComponent <AlumnoScript> ().asistencia)
				confirmados += child.GetComponent <AlumnoScript> ().pin + ",";
		}
		if (confirmados.Length < 3) {
			AlertScript.Singleton.AlertNoButton ("Recuerda tomar asistencia y seleccionar los alumnos presentes de la lista");
			return;
		}
		string[] registrar = confirmados.Split (',');

		for (int i = 0; i < registrar.Length - 1; i++) {
			Debug.Log ("Se deben de registrar ahora si: " + registrar [i]);
		}
		AlertScript.OnAlertResponse += AlertRegistered;

		if (true) //Por si hubo un error al registrarlos
			AlertScript.Singleton.AlertOneButton ("Gracias, puede dejar el dispositivo y dar inicio a su clase.", "Ok", false);
		else
			AlertScript.Singleton.AlertOneButton ("Se produjo un error, favor de intentar de nuevo", "Volver", true);
	}

	void AlertRegistered (string button) {
		AlertScript.OnAlertResponse -= AlertRegistered;
		if (button == "Ok")
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Home");
	}
}
