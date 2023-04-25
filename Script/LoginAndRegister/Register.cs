using GetSetAccountInfo;
using ConnectServer;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.NetworkInformation;                    // MAC Address
using System;
using System.Collections.Generic;

using UnityEngine;

public class Register : MonoBehaviour
{
    private LoginRegisterRequest loginRegisterRequest;
    private GetSetAccountData getSetAccountData;

    private DisplayMessage displayMessage;

    private void Awake()
    {
        loginRegisterRequest = new LoginRegisterRequest();
        getSetAccountData = new GetSetAccountData();

        displayMessage = GetComponent<DisplayMessage>();
    }

    public void RegisterButtonOnClick()
    {
        Dictionary<string, string> userMACAddress = new Dictionary<string, string>();

        Debug.Log("01. OnClicked Register Button");
        string[] userData = getSetAccountData.GetAccountData();

        // 로컬에 Account.txt가 존재할 시에, 해당 if 절이 실행된다.      <=== 기존 로그인 정보가 존재함을 의미.
        if(userData != null)
        {
            // 기존 회원 정보 존재 메시지 출력.
            displayMessage.RegisterOrLoginButtonClick(01);
            return;
        }

        userMACAddress.Add("userMACAddress", GetMACAddress());
        string requestData = JsonConvert.SerializeObject(userMACAddress, Formatting.Indented);
/*        JObject userMACAddress = JObject.Parse(GetMACAddress());
        string tmp = JsonConverter<string>(GetMACAddress());
*/        Debug.Log("02. Get userMACAddress : " + requestData);

        JObject userDataInJsonFormat = JObject.Parse(loginRegisterRequest.Register(requestData));
        Debug.Log("03. Get userData At Server : " + userDataInJsonFormat);

        getSetAccountData.SetAccountData(new string[] { (string)userDataInJsonFormat["userID"], (string)userDataInJsonFormat["userPW"] });
        Debug.Log("04. Success Register");

        // 회원가입 성공 : 02
        displayMessage.RegisterOrLoginButtonClick(04);

    }

    public string GetMACAddress()
    {
        // 어떠한 값을 갖고 있는지 궁금하면, Debug로 출력해봐.
        Debug.Log("Get MAC Address");
        return NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
    }

    // 버튼 하나 생성 -> 회원가입을 원하면 해당 버튼을 클릭하고 다시 함.
    // if check == false일시, 회원가입 취소
    // if check == true일시, 기존 정보 삭제 진행.
    public void DecideWhetherToDelete(bool userRequest)
    {
        string userData = getSetAccountData.GetAccountDataInJsonType();

        // userRequest가 true이면 삭제, false이면 그냥 리턴
        if (userRequest)
        {
            JObject userDataInJsonFormat = JObject.Parse(loginRegisterRequest.Delete(userData));
            // 서버에서 정보를 지웠으면 로컬에서도 삭제.
            if (Convert.ToBoolean(userDataInJsonFormat["DeleteSuccess"]))
            {
                getSetAccountData.DeleteAccountData();
                displayMessage.MessageYesOrNotButtonClick(02); // 삭제 성공 메시지
            }
            else // 서버에서 삭제 실패시,
            {
                displayMessage.MessageYesOrNotButtonClick(03); // 다시한번 시도해 달라고 메시지.
            }
        }
        else
        {
            displayMessage.MessageYesOrNotButtonClick(00);
        }
    }
}
