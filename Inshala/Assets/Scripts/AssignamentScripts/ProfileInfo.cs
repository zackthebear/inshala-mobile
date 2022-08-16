using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProfileInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Caller thisCaller = GameObject.Find("serviceCaller").GetComponent<Caller>();
        
        setTextOnChild(0, thisCaller.lastPin.ToString());

        setTextOnChild(1, thisCaller.responseObjectLoginPin.firstname + " " + thisCaller.responseObjectLoginPin.lastname);
        
        setTextOnChild(2, "$" + thisCaller.responseObjectLoginPin.credit.ToString());

        GameObject.Find("Classes").GetComponent<ClassesScript>().Startup(thisCaller.responseObjectLoginPin.remainingVisits);

        if (thisCaller.responseObjectLoginPin.maxPackageEndDate == "")
            setTextOnChild(3, "No hay paquete activo");
        else
            setTextOnChild(3, returnDayDiffFromToday(thisCaller.responseObjectLoginPin.maxPackageEndDate.Substring(0, 10)));




    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void  setTextOnChild(int child, string textToPut)
    {
        this.transform.GetChild(child).GetComponent<Text>().text = textToPut;
    }

    string returnDayDiffFromToday(string date)
    {
        System.DateTime dt = System.DateTime.ParseExact(date,
            "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        System.DateTime today = System.DateTime.Today;

        return (dt - today).Days.ToString();
    }
}
