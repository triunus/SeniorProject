using UnityEngine;
using TMPro;
using ChangeSceneManager;

using System;
using System.IO;

using Newtonsoft.Json.Linq;


public class DisplayMessage : MonoBehaviour
{
    [SerializeField]
    private RectTransform messagePanel01;
    [SerializeField]
    private RectTransform messagePanel02;

    private ChangeScene changeScene;
    private int code;

    private void Awake()
    {
        changeScene = new ChangeScene();
    }

    public void RegisterOrLoginButtonClick(int messageCodeNumber)
    {
        code = messageCodeNumber;

        if (code == 01)
        {
            DisplayMessagePanel02(code);
        }
        else
        {
            DisplayMessagePanel01(code);
        }
    }

    public void MessageYesOrNotButtonClick(int messageCodeNumber)
    {
        messagePanel02.parent.gameObject.SetActive(false);
        messagePanel02.gameObject.SetActive(false);

        if(messageCodeNumber == 02 || messageCodeNumber == 03)
            DisplayMessagePanel01(messageCodeNumber);
    }

    public void MessageConfirmButtonOnClick()
    {
        messagePanel01.parent.gameObject.SetActive(false);
        messagePanel01.gameObject.SetActive(false);

        if (code == 14)
        {
            changeScene.SceneLoader("GameLobbyScene");
            return;
        }
    }

    public void DisplayMessagePanel01(int messageCodeNumber)
    {
        JArray messageTable = JArray.Parse(File.ReadAllText("./Assets/err/MessageCode.json"));

        for (int i = 0; i < messageTable.Count; i++)
        {
            if (Convert.ToInt32(messageTable[i]["Code"]) == messageCodeNumber)
            {
                messagePanel01.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(messageTable[i]["Title"]);
                messagePanel01.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(messageTable[i]["Detail"]);

//                Debug.Log(messageTable[i]["Code"] + ", " + messageTable[i]["Title"] + ", " + messageTable[i]["Detail"]);
                break;
            }
        }

        messagePanel01.parent.gameObject.SetActive(true);
        messagePanel01.gameObject.SetActive(true);
    }

    public void DisplayMessagePanel02(int messageCodeNumber)
    {
        JArray messageTable = JArray.Parse(File.ReadAllText("./Assets/err/MessageCode.json"));

        for (int i = 0; i < messageTable.Count; i++)
        {
            if (Convert.ToInt32(messageTable[i]["Code"]) == messageCodeNumber)
            {
                messagePanel02.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(messageTable[i]["Title"]);
                messagePanel02.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(messageTable[i]["Detail"]);

                //                Debug.Log(messageTable[i]["Code"] + ", " + messageTable[i]["Title"] + ", " + messageTable[i]["Detail"]);
                break;
            }
        }

        messagePanel02.parent.gameObject.SetActive(true);
        messagePanel02.gameObject.SetActive(true); ;
    }
}
