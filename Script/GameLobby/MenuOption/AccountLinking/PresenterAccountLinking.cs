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
                // 서버에서 문제발생해 생긴 실패 메시지.
                displayMessage.DisplayMessagePanel(32);
            }
        }
        else
        {
            // 번호 길이 정정 메시지
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

            // 계정 연동 하는 곳
            if (modelAccountLinking.RequestType.Equals("AccountLinking"))
            {
                JObject result = JObject.Parse(requestUserData.RequestWhetherAuthenticationSucceeded(requestData));

                if ((bool)result["success"])
                {
                    // 계정 연동 성공 메시지
                    displayMessage.DisplayMessagePanel(35);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();
                }
                else
                {
                    // 서버에서 문제발생해 생긴 실패 메시지.
                    displayMessage.DisplayMessagePanel(34);
                }
            }
            // 계정 찾는 곳.
            else if (modelAccountLinking.RequestType.Equals("FindAccount"))
            {
                JObject result = JObject.Parse(requestUserData.RequestWhetherAuthenticationSucceeded(requestData));

                /*                Debug.Log("result : " + result);
                                Debug.Log("result[0][success] : " + result[0]["success"]);*/

                if ((bool)result["success"])
                {
                    // 계정 찾기 성공 메시지
                    displayMessage.DisplayMessagePanel(36);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();

                    // 로컬에 저장된 유저 정보 변경.
                    getSetAccountData.SetAccountData(new string[] { (string)result["accountData"]["userID"], (string)result["accountData"]["userPW"] });

                    getSetCheckForChanges.SetCheckForChanges(true);
                }
                else
                {
                    // 서버에서 문제발생해 생긴 실패 메시지.
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
                    // 계정 확인 성공 메시지
                    displayMessage.DisplayMessagePanel(36);

                    viewAccountLinkingPanel.CloseVerificationCodeInputPanel();
                    PresenterWalletAddressInputPanelEnter();
                }
                else
                {
                    // 전달 받은 핸드폰 번호와 연동된 정보가 없는 것.
                    displayMessage.DisplayMessagePanel(34);
                }
            }
        }
        else
        {
            // 인증 번호 길이 확인 메시지
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
                // Wallet Address 등록 성공 메시지
                displayMessage.DisplayMessagePanel(35);

                viewAccountLinkingPanel.CloseWalletAddressInputPanel();
            }
            else
            {
                // 서버에서 문제발생해 생긴 실패 메시지.
                displayMessage.DisplayMessagePanel(34);
            }
        }
        else
        {
            // Wallet Address 길이 확인 메시지
            displayMessage.DisplayMessagePanel(33);
        }
    }
}