using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GetDropDownList : MonoBehaviour
{
    public class JsonData
    {
        public OptionInfo[] optionInfo;
    }

    [System.Serializable]
    public class OptionInfo
    {
        public string name;
        public string url;
    }

    static string JsonUrl = "https://raw.githubusercontent.com/takaakikawasaki/dataStorage/main/options.json";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest(JsonUrl));

    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    string JsonResult = webRequest.downloadHandler.text;
                    JsonData jsonData = new JsonData();

                    JsonUtility.FromJsonOverwrite(JsonResult, jsonData);
                    foreach (var item in jsonData.optionInfo)
                    {
                        Debug.Log("name:" + item.name);
                        Debug.Log("url:" + item.url);
                    }

                    break;
            }
        }
    }

}
