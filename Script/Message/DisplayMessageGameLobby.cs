using UnityEngine;
using TMPro;

using System;
using System.IO;

using Newtonsoft.Json.Linq;


public class DisplayMessageGameLobby : MonoBehaviour
{
    [SerializeField]
    private RectTransform messagePanel01;

    public void DisplayMessagePanel(int messageCodeNumber)
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

    public void HideMessagePanel()
    {
        messagePanel01.parent.gameObject.SetActive(false);
        messagePanel01.gameObject.SetActive(false);
    }
}
