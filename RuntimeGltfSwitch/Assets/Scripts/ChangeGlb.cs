using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GLTFast;
using UnityEngine.Networking;
using TMPro;


public class ChangeGlb : MonoBehaviour
{
    static string JsonUrl = "https://raw.githubusercontent.com/takaakikawasaki/dataStorage/main/options.json";
    private GLTFast.GltfAsset gltf;
    List<string> options = new List<string>();
    TMP_Dropdown UiDropDown;
    public JsonData jsonData = new JsonData();

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

    private void Awake()
    {
        UiDropDown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
        UiDropDown.options?.Clear();
        StartCoroutine(GetRequest(JsonUrl));
    }

    private void Start()
    {
        gltf = gameObject.AddComponent<GLTFast.GltfAsset>();
        gltf.Url = jsonData.optionInfo[0].url;
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

                    JsonUtility.FromJsonOverwrite(JsonResult, jsonData);
                    List<string> optionlist = new List<string>();

                    foreach (var item in jsonData.optionInfo)
                    {
                        optionlist.Add(item.name);
                    }
                    UiDropDown.AddOptions(optionlist);

                    break;
            }
        }
    }

    public void OnValueChanged(int value)
    {
        gltf.Dispose();
        gltf = gameObject.AddComponent<GLTFast.GltfAsset>();
        gltf.Url = jsonData.optionInfo[value].url;
    }
}
