using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AlumnoScript : MonoBehaviour {

	public string pin;
	public bool asistencia = false;

	public void Startup (string nombre, string pinDeAlumno, string clasesRestantes, Sprite foto) {
		pin = pinDeAlumno;
		transform.Find ("txtNombre").GetComponent <Text> ().text = nombre;
		transform.Find ("txtClases").GetComponent <Text> ().text = clasesRestantes;
		transform.Find ("PhotoShadow").GetChild (0).GetChild (0).GetComponent <Image> ().sprite = foto;
	}
	
	public void Select () {
		asistencia = !asistencia;
		transform.Find ("btnPresent").GetComponent <Animator> ().SetBool ("checked", asistencia);
	}
}
