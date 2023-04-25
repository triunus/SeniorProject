using GetSetAccountInfo;
using ConnectServer;

using UnityEngine;

using System;
using Newtonsoft.Json.Linq;

public class Login : MonoBehaviour
{
    LoginRegisterRequest loginRegisterRequest;
    GetSetAccountData getSetAccountData;

    private DisplayMessage displayMessage;

    private void Awake()
    {
        getSetAccountData = new GetSetAccountData();
        loginRegisterRequest = new LoginRegisterRequest();

        displayMessage = GetComponent<DisplayMessage>();
    }

    public void LoginButtonOnClick()
    {
//        Debug.Log("01. OnClicked Login Button");

        string userData = getSetAccountData.GetAccountDataInJsonType();

        if (userData == null)
        {
            Debug.Log("02-1. 파일이 없어서 userData가 null값임.");
            displayMessage.RegisterOrLoginButtonClick(11);
            return;
        }
        //        Debug.Log("02-2. userData : " + userData + ", " + userData[0] + ", " + userData[1]);

        JObject userDataInJsonFormat = JObject.Parse(loginRegisterRequest.Login(userData));

/*        Debug.Log("userDataInJsonFormat : " + userDataInJsonFormat["LoginSuccess"]);
        Debug.Log("userDataInJsonFormat : " + Convert.ToBoolean(userDataInJsonFormat["LoginSuccess"]));
*/
        if (Convert.ToBoolean(userDataInJsonFormat["LoginSuccess"]))
        {
            Debug.Log("로그인 성공");
            displayMessage.RegisterOrLoginButtonClick(14);
        }
        else
        {
            Debug.Log("로그인 실패");
            displayMessage.RegisterOrLoginButtonClick(13);
        }
    }

/*    public string[] GetSkillsNumber(string characterNnumber)
    {
        string[] skills = new string[2];

        DataTable dataTable01 = JsonConvert.DeserializeObject<DataTable>(File.ReadAllText("./Assets/GameData/CharactersInfo/CharactersInfo.json"));

        for (int i = 0; i < dataTable01.Rows.Count; i++)
        {
            if ((string)dataTable01.Rows[i][0] == characterNnumber)
            {
                skills[0] = (string)dataTable01.Rows[i][3];
                skills[1] = (string)dataTable01.Rows[i][4];
                break;
            }
        }

        return skills;
    }*/
}
