using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassesScript : MonoBehaviour {

	public GameObject prefabAvailableClass;

	// Use this for initialization
	void Start () {
		//Startup (9);
	}

	void OnEnable () {
		ClaseScript.OnClassSelection += ClassSelectionMade;
	}

	void OnDisable () {
		ClaseScript.OnClassSelection -= ClassSelectionMade;
	}
	
	public void Startup (int cantClases) {
		for (int i = 0; i < cantClases; i++) {
			Instantiate (prefabAvailableClass, transform);
		}
	}

	void ClassSelectionMade (Clase clase) {
		if (transform.childCount > 0) 
			transform.GetChild (transform.childCount - 1).GetComponent <Animator> ().SetBool ("selected", true);
	}

	public void RemoveClass () {
		Destroy (transform.GetChild (transform.childCount - 1).gameObject);
	}
}
