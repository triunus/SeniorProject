using System;

using UnityEngine;
using TMPro;

using GetSetCheckForChangesInfo;
using AccountLinkingInfo;

using GetSetAccountInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using ConnectServer;

public class PresenterAccountLinking : MonoBehaviour
{
    private GetSetCheckForChanges getSetCheckForChanges;
    private ModelAccountLinking modelAccountLinking;

    private GetSetAccountData getSetAccountData;

    private RequestUserData requestUserData;

    private ViewAccountLinkingPanel viewAccountLinkingPanel;
    private DisplayMessageGameLobby displayMessage;

    private string[] accountData = null;

    private void Awake()
    {

        getSetCheckForChanges = new GetSetCheckForChanges();
        modelAccountLinking = new ModelAccountLinking();

        getSetAccountData = new GetSetAccountData();

        requestUserData = new RequestUserData();

        viewAccountLinkingPanel = GetComponent<ViewAccountLinkingPanel>();
        displayMessage = GameObject.FindWithTag("GameManager").GetComponent<DisplayMessageGameLobby>();
    }

    public void PresenterPhoneNumberInputPanelEnter(string requestType)
    {
        viewAccountLinkingPanel.ViewPhoneNumberInputPanel();

        accountData = getSetAccountData.GetAccountData();

        modelAccountLinking.UserID = accountData[0];
        modelAccountLinking.UserPW = accountData[1];
        modelAccountLinking.RequestType = requestType;
        modelAccountLinking.PhoneNumber = null;
        modelAccountLinking.VerificationCode = null;
        modelAccountLinking.WalletAddress = null;

        //        getSetModelAccountLinkingData.SetModelAccountLinking(modelAccountLinking);
    }

    public void PresenterPhoneNumberInputPanelEnd(TextMeshProUGUI phoneNumber)
    {
/*        Debug.Log("phoneNumber.text : " + phoneNumber.text);
        Debug.Log("phoneNumber.text.Length : " + phoneNumber.text.Length);
        Debug.Log("phoneNumber.text.Length.Equals(11) : " + phoneNumber.text.Length.Equals(11));*/

        if (phoneNumber.text.Length.Equals(12))
        {
            modelAccountLinking.PhoneNumber = phoneNumber.text;

            Debug.Log("modelAccountLinking.UserID : " + modelAccountLinking.UserID);

            string requestData = JsonConvert.SerializeObject(modelAccountLinking, Formatting.Indented);

            string result = requestUserData.RequestVerificationCode(requestData);

            if (Convert.ToBoolean(result))
            {
//                getSetModelAccountLinkingData.SetModelAccountLinking(modelAccountLinking);

                viewAccountLinkingPanel.ClosePhoneNumberInputPanel();
                PresenterVerificationCodeInputPanelEnter();
            }
            else
            {
                // �������� �����߻��� ���� ���� �޽���.
                displayMessage.DisplayMessagePanel(32);
            }
        }
        else
        {
            // ��ȣ ���� ���� �޽���
            displayMessage.DisplayMessagePanel(31);
        }
    }

    public void PresenterVerificationCodeInputPanelEnter()
    {
        viewAccountLinkingPanel.ViewVerificationCodeInputPanel();
    }

    public void PresenterVerificationCodeInputPanelEnd(TextMeshProUGUI verificationCode)
    {
        if (verificationCode.text.Length.Equals(7))
        {
            modelAccountLinking.VerificationCode = verificationCode.text;

            //            Debug.Log("modelAccountLinking.UserID : " + modelAccountLinking.UserID);

            string requestData = JsonConvert.SerializeObject(modelAccountLinking, Formatting.Indented);

            // ���� ���� �ϴ� ��
            if (modelAccountLinking.RequestType.Equals("AccountLinking"))
            {
                JObject result = JObject.Parse(requestUserData.RequestWhetherAuthenticationSucceeded(requestData));

                if ((bool)result["success"])
                {
                    // ���� ���� ���� �޽���
                    displayMessage.DisplayMessagePanel(35);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();
                }
                else
                {
                    // �������� �����߻��� ���� ���� �޽���.
                    displayMessage.DisplayMessagePanel(34);
                }
            }
            // ���� ã�� ��.
            else if (modelAccountLinking.RequestType.Equals("FindAccount"))
            {
                JObject result = JObject.Parse(requestUserData.RequestWhetherAuthenticationSucceeded(requestData));

                /*                Debug.Log("result : " + result);
                                Debug.Log("result[0][success] : " + result[0]["success"]);*/

                if ((bool)result["success"])
                {
                    // ���� ã�� ���� �޽���
                    displayMessage.DisplayMessagePanel(36);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();

                    // ���ÿ� ����� ���� ���� ����.
                    getSetAccountData.SetAccountData(new string[] { (string)result["accountData"]["userID"], (string)result["accountData"]["userPW"] });

                    getSetCheckForChanges.SetCheckForChanges(true);
                }
                else
                {
                    // �������� �����߻��� ���� ���� �޽���.
                    displayMessage.DisplayMessagePanel(34);
                }
            }
            else if (modelAccountLinking.RequestType.Equals("WalletAddressLinking"))
            {
                JObject result = JObject.Parse(requestUserData.RequestWhetherAuthenticationSucceeded(requestData));

                /*                Debug.Log("result : " + result);
                                Debug.Log("result[0][success] : " + result[0]["success"]);*/

                if ((bool)result["success"])
                {
                    // ���� Ȯ�� ���� �޽���
                    displayMessage.DisplayMessagePanel(36);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();
                    PresenterWalletAddressInputPanelEnter();
                }
                else
                {
                    // ���� ���� �ڵ��� ��ȣ�� ������ ������ ���� ��.
                    displayMessage.DisplayMessagePanel(34);
                }
            }
        }
        else
        {
            // ���� ��ȣ ���� Ȯ�� �޽���
            displayMessage.DisplayMessagePanel(33);
        }
    }


    public void PresenterWalletAddressInputPanelEnter()
    {
        viewAccountLinkingPanel.ViewWalletAddressInputPanel();
    }


    public void PresenterWalletAddressInputPanelEnd(TextMeshProUGUI walletAddress)
    {
        if (walletAddress.text.Length > 6)
        {
            modelAccountLinking.WalletAddress = walletAddress.text;

            //            Debug.Log("modelAccountLinking.UserID : " + modelAccountLinking.UserID);

            string requestData = JsonConvert.SerializeObject(modelAccountLinking, Formatting.Indented);

            JObject result = JObject.Parse(requestUserData.RequestRegisterWalletAddress(requestData));

            if ((bool)result["success"])
            {
                // Wallet Address ��� ���� �޽���
                displayMessage.DisplayMessagePanel(35);

                viewAccountLinkingPanel.CloseWalletAddressInputPanel();
            }
            else
            {
                // �������� �����߻��� ���� ���� �޽���.
                displayMessage.DisplayMessagePanel(34);
            }
        }
        else
        {
            // Wallet Address ���� Ȯ�� �޽���
            displayMessage.DisplayMessagePanel(33);
        }
    }
}