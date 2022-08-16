using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassesAsignement : MonoBehaviour {
    System.DateTime StartDate;
    System.DateTime EndDate;
    int numDays;


    List<System.DateTime> ListDays;

    // Use this for initialization
    void Start() {

        StartDate = System.DateTime.Today;

        int i = (int)StartDate.DayOfWeek;

        StartDate = StartDate.AddDays(-i);

        EndDate = StartDate.AddDays(6);

        numDays = (int)((EndDate - StartDate).TotalDays);

        ListDays = new List<System.DateTime>();

        StartCoroutine(getClasses(StartDate.Date.ToString("yyyy-MM-dd"), EndDate.Date.ToString("yyyy-MM-dd")));





    }

    int returnNumberDay(string day)
    {
        switch(day)
        {
            case "Monday":
                return 1;
            case "Tuesday":
                return 2;
            case "Wednesday":
                return 3;
            case "Thursday":
                return 4;
            case "Friday":
                return 5;
            case "Saturday":
                return 6;
            case "Sunday":
                return 7;
            default:
                return 0;

        }
    }

    IEnumerator getClasses(string dateStart, string dateEnd)
    {
        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();

        StartCoroutine(thisCaller.getClassesByQuery(dateStart, dateEnd));

        while(!thisCaller.lastServiceConsult)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for (int j = 0; j <= numDays; j++)
        {
            ListDays.Add(StartDate);
            StartDate = StartDate.AddDays(1);
        }
        StartDate = System.DateTime.Today;

        int count = 0;

        foreach (System.DateTime day in ListDays)
        {
            List<Clase> listaClasesEsteDia = new List<Clase>();

            string dateNowFormatted = day.Year + "-" + day.Month.ToString().PadLeft(2, '0') + "-" + day.Day.ToString().PadLeft(2, '0');
            foreach(Classes clase in thisCaller.clases.classes)
            {
                bool inscribed = false ;
                if(clase.classDate == dateNowFormatted)
                {
                    foreach(preRegisters pre in clase.preregisters)
                    {
                        if(pre.client == thisCaller.responseObjectLoginPin.mail)
                        {
                            inscribed = true;
                        }
                    }
                    listaClasesEsteDia.Add(new Clase(clase.class_schedule.id, "Dia",
                        clase.description
                        , clase.instructor.firstname + " " + clase.instructor.lastname,
                        Random.Range(0,50),dateNowFormatted,clase.id,inscribed));

                }
            }

            

            Clase[] clasesitas = listaClasesEsteDia.ToArray();

            this.transform.GetChild(count).GetComponent<HorarioScript>().assignDay(
                returnSpanishDay(day.DayOfWeek.ToString()) + " " + day.Day.ToString() + "/" + day.Month.ToString(),
                clasesitas
                );


            count++;
        }


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
