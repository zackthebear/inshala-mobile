using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Temp ());
	}
	
	IEnumerator Temp () {
		yield return new WaitForSeconds (3);
		AlertScript.Singleton.AlertNoButton ("Esto es un mensaje de prueba\nToque cualquier parte fuera del cuadro para cerrar.");
		AlertScript.Singleton.AlertOneButton ("Otra alerta.\nTiene un boton, seleccionando el boton o cualquier parte lo cierra", "Super!");
		AlertScript.Singleton.AlertTwoButton ("Ultima alerta, con DOS botones.\nAun se puede cancelar seleccionando cualquier parte, o uno de los botones", "Ok", "Bien!");
		AlertScript.Singleton.ActivityIndicator (false);
	}
}
