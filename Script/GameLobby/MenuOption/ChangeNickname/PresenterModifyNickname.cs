using System;

using UnityEngine;
using TMPro;

using GetSetCheckForChangesInfo;
using GetSetAccountInfo;

using ConnectServer;

public class PresenterModifyNickname : MonoBehaviour
{
    private GetSetCheckForChanges getSetCheckForChanges;
    private GetSetAccountData getSetAccountData;

    private RequestUserData requestUserData;

    private ViewModifyNickname viewModifyNickname;
    private DisplayMessageGameLobby displayMessage;

    private void Awake()
    {
        getSetCheckForChanges = new GetSetCheckForChanges();
        getSetAccountData = new GetSetAccountData();

        requestUserData = new RequestUserData();

        viewModifyNickname = GetComponent<ViewModifyNickname>();
        displayMessage = GameObject.FindWithTag("GameManager").GetComponent<DisplayMessageGameLobby>();
    }

    public void PresenterNicknameInputPanelEnter()
    {
        viewModifyNickname.ViewNicknameInputPanel();
    }

    public void PresenterNicknameInputPanelEnd(TextMeshProUGUI nickname)
    {
        // 닉네임 길이 제한
        if (nickname.text.Length < 5 || nickname.text.Length > 15)
        {
            displayMessage.DisplayMessagePanel(37);
        }
        // 허용할 수 없는 문자
        else if (false)
        {
            //displayMessage.DisplayMessagePanel(38);
        }
        else
        {
            string[] accountData = getSetAccountData.GetAccountData();

            string result = requestUserData.RequestModifyUserNickname(new string[] { accountData[0], accountData[1], nickname.text });

            if (Convert.ToBoolean(result))
            {
                displayMessage.DisplayMessagePanel(39);

                viewModifyNickname.CloseNicknameInputPanel();

                getSetCheckForChanges.SetCheckForChanges(true);
            }
            else
            {
                // 서버에서 변경 안해줌.
                displayMessage.DisplayMessagePanel(40);
            }
        }
    }
}
