using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ClaseScript : MonoBehaviour {

	public delegate void ClaseSelection (Clase clase);
	public static ClaseSelection OnClassSelection;

	Clase clase;
	public static float cupo = 18f;

	void OnEnable () {
		OnClassSelection += ClassSelected;
	}

	void OnDisable () {
		OnClassSelection -= ClassSelected;
	}

	public void Startup (Clase claseAInicializar) {
		clase = claseAInicializar;
		gameObject.name = clase.id;
		transform.Find ("txtHora").GetComponent <Text> ().text = clase.hora;
		transform.Find ("txtMaestro").GetComponent <Text> ().text = clase.maestro;
		GetComponent <Image> ().color = Color.Lerp (new Color (0.846f, 0.955f, 1, 1), new Color (0.015f, 0.715f, 1, 1), clase.registrados/cupo);
		GetComponent <Animator> ().enabled = true;

        if(clase.isPreregistered)
        {
            GetComponent<Animator>().SetBool("registered", true);
			GetComponent <Button> ().interactable = false;
            transform.Find("txtMaestro").GetComponent<Text>().text = "(Inscrita)";
            GameObject.Find("Classes").GetComponent<ClassesScript>().RemoveClass();
            GameObject.Find("ClassConfirm").GetComponent<Animator>().SetBool("checked", false);
        }
	}
	
	public void Selected () {
		GetComponent <Animator> ().SetBool ("selected", true);
		GameObject.Find ("ClassConfirm").GetComponent <Animator> ().SetBool ("checked", true);
        AlertScript.OnAlertResponse += ClassConfirmed;
        AlertScript.Singleton.AlertTwoButton("Deseas inscribirte a la clase del\n" + clase.dia + " a las " + clase.hora + " con:\n" + clase.maestro, "No", "Sí");
        AlertScript.Singleton.ActivityIndicator(true);
        OnClassSelection (clase);
        // Confirm here
	}


    void ClassSelected(Clase selection)
    {
        if (clase == selection)
            return;
        GetComponent<Animator>().SetBool("selected", false);
        GetComponent<Button>().interactable = true;
    }

    void ClassConfirmed(string response)
    {
        AlertScript.OnAlertResponse -= ClassConfirmed;
        if (response == "Sí")
        {
            // Aqui se debe registrar a la clase seleccionada
            StartCoroutine(preregisterClass());

        }
        else
            AlertScript.Singleton.ActivityIndicator(false);
    }
    IEnumerator preregisterClass()
    {
        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        StartCoroutine(thisCaller.PreregisterClass(thisCaller.lastPin, int.Parse( clase.id ), clase.fechaWebService));

        while(!thisCaller.lastServiceConsult)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if(thisCaller.succesfulLogin)
        {
            GetComponent<Animator>().SetBool("registered", true);
            transform.Find("txtMaestro").GetComponent<Text>().text = "(Inscrita)";
            GameObject.Find("Classes").GetComponent<ClassesScript>().RemoveClass();
            GameObject.Find("ClassConfirm").GetComponent<Animator>().SetBool("checked", false);
            AlertScript.OnAlertResponse += Done;
            AlertScript.Singleton.AlertOneButton("Clase inscrita", "Salir");
        }
        else
        {
            AlertScript.Singleton.AlertOneButton("Error al inscribir","Ok");
            AlertScript.Singleton.ActivityIndicator(false);
        }
        yield return 0;
    }
    void Done(string response)
    {
        AlertScript.OnAlertResponse -= Done;
        if (response == "Salir")
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Home");
        AlertScript.Singleton.ActivityIndicator(false);
    }
}
