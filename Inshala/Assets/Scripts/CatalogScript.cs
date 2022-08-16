using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void WantToBuy (bool clases) {
		AlertScript.Singleton.ActivityIndicator (true);
		transform.root.gameObject.SetActive (true);
	}

	public void ChangedMyMind () {
		AlertScript.Singleton.ActivityIndicator (false);
		transform.root.gameObject.SetActive (false);
	}

	public void Home () {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Home");
	}
}
