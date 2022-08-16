using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseOption : MonoBehaviour {

	string id;
	string texto;
	int precio;
	bool selected = false;

	// Use this for initialization
	public void Startup (string opcionId, string opcionTexto, int opcionPrecio) {
		id = opcionId;
		texto = opcionTexto;
		precio = opcionPrecio;
	}
	
	public void SelectOption () {
		selected = !selected;
		transform.Find ("PackageSelected").GetComponent <Animator> ().SetBool ("checked", selected);
	}
}
