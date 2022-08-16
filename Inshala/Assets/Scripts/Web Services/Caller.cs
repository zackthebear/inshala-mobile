using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caller : MonoBehaviour {
    public bool lastServiceConsult = false;
    public bool succesfulLogin;
    public bool succesfullRegister;
    public WebServices webService;
    public ResponseLoginByPin responseObjectLoginPin;
    public string lastPin;
    public classes_read clases;
    public Classes lastClass;

    //Aqui se guardan los paquetes que existen.
    public grupoPaquetes paquetes;

    void Awake(){
        DontDestroyOnLoad(transform.root);
    }

    // Use this for initialization
    void Start () {
        //StartCoroutine(LoginByPin(1002));
        this.webService = GameObject.Find("serviceManager").GetComponent<WebServices>();

        StartCoroutine(getPackages());

        //StartCoroutine(getRecentClass("2018-03-15T20:30:01.000-06:00"));
    }

    public IEnumerator LoginByPin(string PIN)
    {
        this.lastServiceConsult = false;
        
        WWWForm form = new WWWForm();
        form.AddField("pin", PIN);

        webService.processData("/users/login/pin", "post", form, null);

        while(!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.01f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout){
                Debug.Log("Web service timed out");
                break;
            }
        }

        if (webService.isActualServiceSuccesful)
        {
            responseObjectLoginPin = JsonUtility.FromJson<ResponseLoginByPin>(webService.lastServerResponse);
            this.succesfulLogin = true;
            webService.isActualServiceDone = false;
            webService.actualTimer = 0.0f;
        }
        else
        {
            this.succesfulLogin = false;
        }
        this.lastServiceConsult = true;
    }

    public IEnumerator registerUser(Register userInfo)
    {
        this.lastServiceConsult = false;

        string jsonPost = JsonUtility.ToJson(userInfo);
        jsonPost = jsonPost.Replace("\"aquivaimagen\"", "null");

        Debug.Log(jsonPost);

        webService.processData("/users", "POST", null, jsonPost);

        while(!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.01f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout)
            {
                Debug.Log("Web service timed out");
                break;
            }
        }

        if (webService.isActualServiceSuccesful)
        {
            Debug.Log("Registro Exitoso");
            Debug.Log(webService.lastServerResponse);
            succesfullRegister = true;
        }
        else
        {
            Debug.Log("Falló el registro");
            Debug.Log(webService.lastServerResponse);
            succesfullRegister = false;
        }



            yield return 0;
    }

    public IEnumerator getClassesByQuery(string datetimeStart, string datetimeEnd)
    {
        this.lastServiceConsult = false;
        string urlFinal = "/classes?active=true&start=" + datetimeStart + "&end=" + datetimeEnd;


        this.webService = GameObject.Find("serviceManager").GetComponent<WebServices>();

        webService.processData(urlFinal, "GET", null, null);


        while (!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.01f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout)
            {
                Debug.Log("Web service timed out");
                break;
            }
        }

        if (webService.isActualServiceSuccesful)
        {
            clases = JsonUtility.FromJson<classes_read>(webService.jsonArrayHotfix("classes", webService.lastServerResponse).Replace("class-schedule","class_schedule"));
            this.lastServiceConsult = true;
        }
        else
        {
            Debug.Log("Falló al obtener clases");
            Debug.Log(webService.lastServerResponse);
            this.lastServiceConsult = true;
        }

        yield return 0;
    }

    public IEnumerator PreregisterClass (string PIN, int scheduleId, string day)
    {
        this.lastServiceConsult = false;
        this.succesfulLogin = false;

        WWWForm form = new WWWForm();
        form.AddField("pin", PIN);
        form.AddField("scheduleId", scheduleId);
        form.AddField("day", day);

        webService.processData("/users/preregister", "POST", form, null);

        while (!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.01f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout)
            {
                Debug.Log("Web service timed out");
                break;
            }
        }

        if (webService.isActualServiceSuccesful)
        {
            Debug.Log(webService.lastServerResponse);
            this.succesfulLogin = true;
            webService.isActualServiceDone = false;
        }
        else
        {
            this.succesfulLogin = false;
        }
        this.lastServiceConsult = true;
    }

    public IEnumerator getPackages()
    {
        webService.processData("/package-types", "GET", null, null);

        while(!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.2f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout)
            {
                Debug.Log("Web service timed out");
                break;
            }
        }

        if (webService.isActualServiceSuccesful)
        {
            paquetes = JsonUtility.FromJson<grupoPaquetes>(webService.jsonArrayHotfix("packages", webService.lastServerResponse));
            
        }
        else
        {
            Debug.Log(webService.lastServiceName + " falló");
        }
    }

    public IEnumerator getRecentClass(string starttime)
    {
        this.lastServiceConsult = false;
        webService.processData("/classes?startTime=" + starttime, "GET", null, null);

        while(!webService.isActualServiceDone)
        {
            yield return new WaitForSeconds(0.2f);
            webService.actualTimer += 0.02f;

            if (webService.actualTimer > webService.limitTimeout)
            {
                Debug.Log("Web service timed out");
                break;
            }
        }

        if(webService.isActualServiceSuccesful)
        {
            clases = JsonUtility.FromJson<classes_read>(webService.jsonArrayHotfix("classes", webService.lastServerResponse).Replace("class-schedule", "class_schedule"));
            lastClass = clases.classes[0];
        }
        else
        {
            Debug.Log(webService.lastServerResponse);
           
        }

        this.lastServiceConsult = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
