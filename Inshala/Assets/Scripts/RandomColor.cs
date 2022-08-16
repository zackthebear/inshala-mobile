using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RandomColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log (GetComponent <Image> ().color);
		GetComponent <Image> ().color = new Color (Random.Range (0.676f, 0.912f), Random.Range (0.946f, 0.985f), 1, 1);
	}
}
