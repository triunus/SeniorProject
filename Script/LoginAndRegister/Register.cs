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

        // ���ÿ� Account.txt�� ������ �ÿ�, �ش� if ���� ����ȴ�.      <=== ���� �α��� ������ �������� �ǹ�.
        if(userData != null)
        {
            // ���� ȸ�� ���� ���� �޽��� ���.
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

        // ȸ������ ���� : 02
        displayMessage.RegisterOrLoginButtonClick(04);

    }

    public string GetMACAddress()
    {
        // ��� ���� ���� �ִ��� �ñ��ϸ�, Debug�� ����غ�.
        Debug.Log("Get MAC Address");
        return NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
    }

    // ��ư �ϳ� ���� -> ȸ�������� ���ϸ� �ش� ��ư�� Ŭ���ϰ� �ٽ� ��.
    // if check == false�Ͻ�, ȸ������ ���
    // if check == true�Ͻ�, ���� ���� ���� ����.
    public void DecideWhetherToDelete(bool userRequest)
    {
        string userData = getSetAccountData.GetAccountDataInJsonType();

        // userRequest�� true�̸� ����, false�̸� �׳� ����
        if (userRequest)
        {
            JObject userDataInJsonFormat = JObject.Parse(loginRegisterRequest.Delete(userData));
            // �������� ������ �������� ���ÿ����� ����.
            if (Convert.ToBoolean(userDataInJsonFormat["DeleteSuccess"]))
            {
                getSetAccountData.DeleteAccountData();
                displayMessage.MessageYesOrNotButtonClick(02); // ���� ���� �޽���
            }
            else // �������� ���� ���н�,
            {
                displayMessage.MessageYesOrNotButtonClick(03); // �ٽ��ѹ� �õ��� �޶�� �޽���.
            }
        }
        else
        {
            displayMessage.MessageYesOrNotButtonClick(00);
        }
    }
}
