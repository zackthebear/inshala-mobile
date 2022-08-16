using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TeacherInfo : MonoBehaviour {

    Classes thisClase;
	// Use this for initialization
	void Start () {
        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();
        setTextOnChild(1, thisCaller.lastPin);
        setTextOnChild(2, thisCaller.responseObjectLoginPin.firstname + " " + thisCaller.responseObjectLoginPin.lastname);



        StartCoroutine(setNearestClass());

    }
	
	// Update is called once per frame
	void Update () {
		
	}



    void setTextOnChild(int child, string textToPut)
    {
        this.transform.GetChild(child).GetComponent<Text>().text = textToPut;
    }


    IEnumerator setNearestClass()
    {

        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        System.DateTime now = System.DateTime.Now;
        string cadena = now.Year + "-" + now.Month.ToString().PadLeft(2, '0') + "-" + now.Day.ToString().PadLeft(2, '0') + "T" + now.Hour + ":" + now.Minute + ":" + "01.000-06:00";

        StartCoroutine(thisCaller.getRecentClass(cadena));

        while (!thisCaller.lastServiceConsult)
        {
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.2f);

        thisClase = thisCaller.lastClass;
        AlertScript.Singleton.ActivityIndicator(false);
        yield return 0;
        setClassInformation();
    }

    void setClassInformation()
    {
        //System.DateTime dt = System.DateTime.ParseExact(thisClase.classDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        System.DateTime dt = System.DateTime.Parse(thisClase.classDate);


        setTextOnChild(3, returnSpanishDay(dt.DayOfWeek.ToString()) + " " + dt.Day + "/" + dt.Month.ToString().PadLeft(2, '0') + " | " + thisClase.description);
        GameObject.Find("Content").GetComponent<TeacherAssignement>().setStudentsPreregistereds(thisClase);
    }


    string returnSpanishDay(string day)
    {
        switch (day)
        {
            case "Monday":
                return "Lunes";
            case "Tuesday":
                return "Martes";
            case "Wednesday":
                return "Miercoles";
            case "Thursday":
                return "Jueves";
            case "Friday":
                return "Viernes";
            case "Saturday":
                return "Sabado";
            case "Sunday":
                return "Domingo";
            default:
                return "Monday";

        }
    }
}
