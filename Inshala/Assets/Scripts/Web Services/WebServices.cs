using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebServices : MonoBehaviour {

    //Liga principal del domino alojador, serivrá para no tener que mandar todo el parametro de la función que se mandará a llamar.
    private string ligaMain = "http://ec2-13-59-147-111.us-east-2.compute.amazonaws.com/inshala/api/rest";
    //Booleano que nos servirá para saber si el servicio consultado ya terminó de procesar.
    public bool isActualServiceDone = false;
    //String con la que recuperaremos la cadena JSON que nos regresa el servidor. 
    public string lastServerResponse;

    public bool isActualServiceSuccesful;

    //String de autorización -Username-
    public string username = "inshala";
    //String de autorización -Password-
    public string password = "123456";

    public double limitTimeout = 300.0f;
    public double actualTimer = 0.0f;

    //String para saber que servicio se procesó la ultima vez
    public string lastServiceName;

    void Awake(){
        DontDestroyOnLoad(transform.root);
    }


    // Use this for initialization
    void Start() {
        //StartCoroutine( mainServiceFunction("/posts") );

        //processData("/posts", "get", null);
    }


    //Funcion que se mandará a llamar cuando se crea el objeto Webservices
    //Sus parametros son la url final del webservice, el metodo de conexion (Get, Post, Put, Delete, etc) y en caso de que sea un post
    //su formulario WWW para mandar la informacion al servidor.
    public void processData(string getUrl, string methodConnection, WWWForm formPost, string formJson)
    {
        //Crea la liga final de conexion
        this.lastServiceName = getUrl;
        getUrl = ligaMain + getUrl;
        //Settea la booleana de servicio en uso, como falso, para validacion de while
        this.isActualServiceDone = false;

        //Procesa que tipo de metodo de conexion se solicita y segun sea el caso manda a llamar el proceso indicado.
        switch (methodConnection.ToUpper())
        {
            case "GET":
                {
                    //Manda a llamar la corutina del Get
                    StartCoroutine(getServiceFunction(getUrl, onSuccessGetConnection, onFailureGetConnection ));
                    break;
                }
            case "POST":
                {
                    //Hay dos formas de mandar a llamar un post, por json y por formulario, si el formulario es nulo, tomará el post
                    //del string json, la información se lee igual que en GET
                    //Si lleva formulario desde el caller, este tomará en cuenta el formulario y lo procesará con ese, si no,
                    //Procesará con formjson que es la cadena json del request.
                    //**Nunca usar ambos**
                    StartCoroutine(postServiceFunction(getUrl,onSuccessGetConnection, onFailureGetConnection, formJson, formPost));
                    break;
                }
            default:
                Debug.Log("No se encuentra este tipo de llamada al servidor. Validar.");
                break;
        }

    }

    //Funcion que manda a llamar una funcion al servidor de tipo GET
    private IEnumerator getServiceFunction(string liga, Action<string> onSuccess, Action<long> onFailure)
    {
        UnityWebRequest request = UnityWebRequest.Get(liga);

        //Como los servicios GET no llevan datos al servidor, se usa un formato basico json de respuesta.
        request = setHeadersBasedOnService(request, "AUTHENTICATED");

        //Variable que settea el buffer de descarga para la respuesta json
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //Manda el request al servidor y espera por su respuesta
        yield return request.Send();

        if (request.isError)
        {
            onFailure(request.responseCode);
        }
        else
        {
            if (request.responseCode == 200)
                onSuccess(request.downloadHandler.text);
            else
                onFailure(request.responseCode);
        }



    }

    //Funcion que consume un servicio web por metodo post, recibe de parametros la liga final de conexion, callbacks para succes y error
    // y un string en caso de que la información en post sea en formato json, o un formulario en caso de que sea formulario.
    private IEnumerator postServiceFunction(string liga, Action<string> onSuccess, Action<long> onFailure, string formJson, WWWForm form)
    {


        UnityWebRequest requestPost;

        //Determina si será un servicio por formulario o por json
        if(form != null)
        {
            requestPost = UnityWebRequest.Post(liga, form);
            requestPost = setHeadersBasedOnService(requestPost, "Authenticated_Form");
        }
        else
        {
            requestPost = UnityWebRequest.Post(liga, formJson);
            requestPost = setHeadersBasedOnService(requestPost, "Authenticated");
        }

        requestPost.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        this.isActualServiceDone = false;

        //Manda el request al servidor y espera por su respuesta
        yield return requestPost.Send();

        

        //Si es un error manda a llamar el callback de error, y si es exitoso, manda sucess
        if(requestPost.isError)
        {
            onFailure(requestPost.responseCode);
        }
        else
        {

            if (requestPost.responseCode == 200)
                onSuccess(requestPost.downloadHandler.text);
            else
            {
                onFailure(requestPost.responseCode);
                Debug.Log(requestPost.downloadHandler.text);
            }
        }

        
    }

    private IEnumerator deleteServiceFunctiion(string liga, Action<string> onSucess, Action<long> onFailure)
    {
        yield return 0;
    }

    //Callback que procesa la información(?
    private void onSuccessGetConnection(string jsonResponse)
    {
        Debug.Log("The Service: "+ this.lastServiceName +" has been successful, watch response on next line");
        this.lastServerResponse = jsonResponse;
        this.isActualServiceDone = true;
        this.isActualServiceSuccesful = true;
    }

    //Callback para fallos
    private void onFailureGetConnection(long responseCode)
    {
        Debug.Log("The request has been failed by error number: " + responseCode);
        Debug.Log(errorDescription(responseCode));
        this.isActualServiceDone = true;
        this.isActualServiceSuccesful = false;
    }

    



    #region Funciones de utilidad extra
    //Funcion que regresa la credencial del usuario en formato base_64 para autentificacion
    //con el servidor.
    public string credentialsBase64(string data1, string data2)
    {
        string formatoCadena = data1 + ":" + data2;
        byte[] bytesFormato = Encoding.UTF8.GetBytes(formatoCadena);
        return "Basic " + Convert.ToBase64String(bytesFormato);
    }

    public string pingBase64(string pin)
    {
        byte[] bytesFormato = Encoding.UTF8.GetBytes(pin);
        return "Basic " + Convert.ToBase64String(bytesFormato);
    }

    //Regresa una cadena de texto que define el error correspondiente al codigo enviado con anterioridad.
    string errorDescription(long responseCode)
    {
        switch (responseCode)
        {
            case 404:
                return "404: Page not found";
            case 500:
                return "500: Internal server error";
            default:
                return "Unknow server error, try again please";

        }
    }

    //Settea los headers de un servicio, recibe un servicio y un parametro de permisos, para asignarle headers en base a los permisos solicitados
    //BASIC - Solo agrega el formato json tanto de entrada como de salida.
    //BASIC_FORM - Igual que el anterior pero para formularios
    //Authenticated - Agrega el header de autentificacion para el servidor
    //Authenticated_form - Igual que el anterior pero con formularios
    //Languages - Experimental por si se ocupa multilenguaje
    UnityWebRequest setHeadersBasedOnService(UnityWebRequest req, string levelOfPermission)
    {

        switch (levelOfPermission.ToUpper())
        {
            case "BASIC":
                {
                    req.SetRequestHeader("Content-type", "application/json");
                    break;
                }
            case "BASIC_FORM":
                {
                    req.SetRequestHeader("Content-type", "application/x-www-form-urlencoded");
                    break;
                }
            case "AUTHENTICATED":
                {
                    req.SetRequestHeader("Authorization", credentialsBase64(this.username, this.password));
                    req.SetRequestHeader("Content-type", "application/json");
                    break;

                }
            case "AUTHENTICATED_FORM":
                {
                    req.SetRequestHeader("Authorization", credentialsBase64(this.username,this.password));
                    req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                    break;
                }
            case "LANGUAGES":
                {

                    req.SetRequestHeader("Content-Language", "es-MX");
                    break;
                }
            default:
                {
                    req.SetRequestHeader("Content-type", "application/json");
                    break;
                }

        }
        return req;
    }

    //Fix para cuando en arreglos en formato JSON no se encuentre la llave del primer valor
    //Se agrega el nombre del arreglo al principio para poder hacer el serializado
    //Tiene de parametro el nombre del arreglo, y el data json.
    public string jsonArrayHotfix(string keyParent, string data)
    {
        return string.Format("{{ \"{0}\": {1}}}", keyParent, data);
    }

    //Funcion que transforma un arreglo string a una imagen, sirve cuando en el servidor las imagenes son guardadas en cadenas de texto
    //en formato base_64, esta funcion crea una textura 2d en base al string base_64
    public Texture2D convertStringtoImage(string image)
    {
        byte[] bytes = System.Convert.FromBase64String(image);

        Texture2D newImage = new Texture2D(1, 1);
        newImage.LoadImage(bytes);

        return newImage;
    }


    //Funcion que transforma una imagen en una cadena String base 64 para poder subirla al servidor.
    public string convertImagetoString(Texture2D tex)
    {
        /*Texture2D newTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

        newTexture.SetPixels(0, 0, tex.width, tex.height, tex.GetPixels());
        newTexture.Apply();

        byte[] bytes = newTexture.EncodeToPNG();*/

        RenderTexture tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        Graphics.Blit(tex, tmp);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmp;
        Texture2D textura = new Texture2D(tex.width, tex.height);
        textura.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        textura.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmp);
        Debug.Log("Convertida: " + textura.width + " " + textura.height + " " + textura.dimension + "  " + textura.filterMode);
        switch (tex.format)
        {
            case TextureFormat.BC7:
                return System.Convert.ToBase64String(textura.EncodeToJPG(100));
            case TextureFormat.DXT1:
                return System.Convert.ToBase64String(textura.EncodeToPNG());
            default:
                return System.Convert.ToBase64String(textura.EncodeToJPG(100));
        }
    }

    #endregion
}
/*
 public IEnumerator logoutRequest(string username, string device)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        string urlLogout = urlBase + "usuarios/logout";
        UnityWebRequest req = UnityWebRequest.Post(urlLogout, form);

        //byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

        //req.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);


        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        req.SetRequestHeader("device-id", device);



        yield return req.Send();

        if (req.responseCode == 200)
        {
            response = true;
            AlertScript.Singleton.OneButtonAlert("Logged out", "You have been logged out", "Ok");
        }
        else
        {
            response = false;
            AlertScript.Singleton.OneButtonAlert("Error", "Error logging out " + req.downloadHandler.text, "Ok");

        }
    }

    public IEnumerator registerRequest(string username, string password, string language, string nombre, Texture2D tex)
    {
        string image = tex == null ? "null" : convertImagetoString(tex);
        string authorization = convertStringtoBase64("APP_REGISTER", "4pp_R3g1st3r");
        Debug.Log(authorization);
        formRegisterUsuario newform = new formRegisterUsuario(username, password, language, image, nombre);
        string jsonData = JsonUtility.ToJson(newform);

        Debug.Log(jsonData);
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);

        UnityWebRequest req = new UnityWebRequest(urlRegister, "POST");

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);

        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        //req.SetRequestHeader("Authorization", "Basic QVBQX1JFR0lTVEVSOkFQUF9SRUdJU1RFUg==");
        req.SetRequestHeader("Authorization", authorization);




        yield return req.Send();

        Debug.Log(req.responseCode + "    " + req.error);
        Debug.Log(req.downloadHandler.text);
        if (req.responseCode == 200)
        {
            datos = JsonUtility.FromJson<responseDatosUsuario>(req.downloadHandler.text);
            response = true;
        }
        else if (req.responseCode == 409)
        {
            error = "The username is already taken";
            lastError = ErrorsEnum.userExists;
            AlertScript.Singleton.OneButtonAlert(traduccion.languageManager.GetString("keyTitleErrorUsernameTaken", ProjectClicked.currentLanguage),
                traduccion.languageManager.GetString("userExists", ProjectClicked.currentLanguage),
                "Ok");
            response = false;
        }
        else
        {
            error = "Couldn't connect to the server, please try again";
            lastError = ErrorsEnum.wrongCredentials;
            AlertScript.Singleton.OneButtonAlert(traduccion.languageManager.GetString("keyTitleErrorLostConnection", ProjectClicked.currentLanguage),
                traduccion.languageManager.GetString("coudlntConnect", ProjectClicked.currentLanguage),
                "Ok");
        }
    } 
*/
