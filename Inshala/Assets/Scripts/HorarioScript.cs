using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HorarioScript : MonoBehaviour {

	public GameObject prefabClase;
	public GameObject prefabAmPm;

    Dia thisDia;

    //System.DateTime StartDate;
    //System.DateTime EndDate;
    //int numDays;


    //List<System.DateTime> ListDays;


    public void assignDay(string dia, Clase[] clases)
    {
        thisDia = new Dia(dia);
        //thisDia.clases = new Clase[] 
        //{
        //    new Clase("1", thisDia.dia, "11:00 am", "Mr Sabadrinks", 10)
        //};
        thisDia.clases = clases;
        Startup(thisDia);
    }


    // Use this for initialization
    void Start () {
        AlertScript.Singleton.ActivityIndicator(false);
        //StartDate = System.DateTime.Today;

        //int i = (int)StartDate.DayOfWeek;

        //StartDate = StartDate.AddDays(-i);
        //Debug.Log(StartDate);

        //EndDate = StartDate.AddDays(6);
        //Debug.Log(EndDate);

        //numDays = (int)((EndDate - StartDate).TotalDays);
        //Debug.Log(numDays);

        //ListDays = new List<System.DateTime>();

        //for (int j = 0; j <= numDays; j++)
        //{
        //    ListDays.Add(StartDate);
        //    StartDate = StartDate.AddDays(1);
        //}

        //int count = 0;

        //foreach (System.DateTime day in ListDays)
        //{
        //    //this.transform.GetChild(count).GetComponent<Text>().text = day.Date.ToString();
        //    Debug.Log(day.Date.ToString());
        //    Dia dia = new Dia(day.Date.ToString());
        //    dia.clases = new Clase[] {
        //        new Clase ("1", dia.dia, "11:00 am", "Mr Sabadrinks", 10)
        //    };
        //    Startup(dia);
        //    count++;
        //}

        //StartCoroutine(getClasses(StartDate.Date.ToString("yyyy-MM-dd"), EndDate.Date.ToString("yyyy-MM-dd")));


        // Por lo pronto los creo de manera aleatoria
        //Dia lunes = new Dia(returnSpanishDay(StartDate.DayOfWeek.ToString()) + " " + StartDate.Day.ToString() + "/" + StartDate.Month.ToString());
        //if (Random.value < 0.7f)
        //{
        //    lunes.clases = new Clase[] {
        //        new Clase ("1", lunes.dia, "6:45 am", "Maestro asignado", 2),
        //        new Clase ("2", lunes.dia, "8:15 am", "Nacho Sabelotodo", 3),
        //        new Clase ("3", lunes.dia, "9:30 am", "El Oso Yogui", 7),
        //        new Clase ("4", lunes.dia, "10:45 am", "Sr Flexi", 5),
        //        new Clase ("5", lunes.dia, "5:15 pm", "El Oso Yogui", 1),
        //        new Clase ("6", lunes.dia, "6:30 pm", "Tony Stark", 12),
        //        new Clase ("7", lunes.dia, "7:45 pm", "-Sustituto-", 4)
        //    };
        //}
        //else
        //{
        //    lunes.clases = new Clase[] {
        //        new Clase ("1", lunes.dia, "11:00 am", "Mr Sabadrinks", 10)
        //    };
        //}
        //Startup(lunes);
    }
	
	public void Startup (Dia dia) {
		StartCoroutine ("StartupDia", dia);
	}

	IEnumerator StartupDia (Dia dia) {
		transform.Find ("txtDia").GetComponent <Text> ().text = dia.dia;
		yield return new WaitForSeconds (float.Parse (gameObject.name) * 0.08f);
		gameObject.name = dia.dia;
		bool once = true;

		if (dia.clases.Length == 1)
			GetComponent <VerticalLayoutGroup> ().spacing = 420;

		for (int i=0; i<dia.clases.Length; i++) {

			GameObject nuevaClase;
			if (dia.clases [i].hora.Contains ("pm") && once) {
				nuevaClase = Instantiate (prefabAmPm, transform);
				once = false;
			}
			nuevaClase = Instantiate (prefabClase, transform).gameObject;
			nuevaClase.GetComponent <ClaseScript> ().Startup (dia.clases [i]);
			yield return new WaitForSeconds (0.05f);

		}
		AlertScript.Singleton.ActivityIndicator (false);
	}

    IEnumerator getClasses(string dateStart, string dateEnd)
    {
        yield return 0;
        //Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        //StartCoroutine(thisCaller.getClassesByQuery(dateStart, dateEnd));

        //while (!thisCaller.lastServiceConsult)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}
        //Dia lunes = new Dia(returnSpanishDay(StartDate.DayOfWeek.ToString()) + " " + StartDate.Day.ToString() + "/" + StartDate.Month.ToString());
        //if (Random.value < 0.7f)
        //{
        //    lunes.clases = new Clase[] {
        //        //new Clase ("1", lunes.dia, "11:00 am", "Mr Sabadrinks", 10),
        //        new Clase (thisCaller.clases.classes[0].id.ToString(),
        //                    lunes.dia,thisCaller.clases.classes[0].description,thisCaller.clases.classes[0].instructor.firstname + " " + thisCaller.clases.classes[0].instructor.lastname,
        //                    thisCaller.clases.classes[0].places),
        //        new Clase ("2", lunes.dia, "8:15 am", "Nacho Sabelotodo", 3),
        //        new Clase ("3", lunes.dia, "9:30 am", "El Oso Yogui", 7),
        //        new Clase ("4", lunes.dia, "10:45 am", "Sr Flexi", 5),
        //        new Clase ("5", lunes.dia, "5:15 pm", "El Oso Yogui", 1),
        //        new Clase ("6", lunes.dia, "6:30 pm", "Tony Stark", 12),
        //        new Clase ("7", lunes.dia, "7:45 pm", "-Sustituto-", 4)
        //    };
        //}
        //else
        //{
        //    lunes.clases = new Clase[] {
        //        //new Clase ("1", lunes.dia, "11:00 am", "Mr Sabadrinks", 10)

        //        new Clase (thisCaller.clases.classes[0].id.ToString(),lunes.dia,
        //        thisCaller.clases.classes[0].description,
        //        thisCaller.clases.classes[0].instructor.firstname + " " + thisCaller.clases.classes[0].instructor.lastname,
        //        thisCaller.clases.classes[0].places)

        //    };
        //}
        //Startup(lunes);

    }


}

public class Dia {
	public string dia;
	public Clase[] clases;

	public Dia (string diaDeLaSemana) {
		dia = diaDeLaSemana;
	}
}

public class Clase {
	public string id;
    public int id_class;
	public string dia;
	public string hora;
	public string maestro;
	public int registrados;
    public string fechaWebService;
    public bool isPreregistered;

	public Clase (string idClase) {
		id = idClase;
	}
	public Clase (string idClase, string diaDeLaSemana, string horaClase, string maestroAsignado, int cantAlumnosRegistrados, 
        string fecha, int id_class, bool isPreregistered) {
		id = idClase;
		dia = diaDeLaSemana;
		hora = horaClase;
		maestro = maestroAsignado;
		registrados = cantAlumnosRegistrados;
        fechaWebService = fecha;
        this.id_class = id_class;
        this.isPreregistered = isPreregistered;
	}
}


