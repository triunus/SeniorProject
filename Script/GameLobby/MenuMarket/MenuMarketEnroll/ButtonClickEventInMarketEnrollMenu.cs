using GetSetAccountInfo;
using GetSetLobbyInformation;
using ConnectServer;

using UnityEngine;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Newtonsoft.Json.Linq;

using LobbyInformaion;
using SkillInformaion;

public class ButtonClickEventInMarketEnrollMenu : MonoBehaviour
{
    GetSetAccountData getSetAccountData;
    GetSetLobbyData getSetLobbyData;
    RequestUserData requestUserData;

    private DisplayMarketEnrollUI displayMarketEnrollUI;
    private DisplayConfirmPanelEvent displayConfirmPanelEvent;
    private DisplayMessageGameLobby displayMessage;

    private void Awake()
    {
        getSetAccountData = new GetSetAccountData();
        requestUserData = new RequestUserData();
        getSetLobbyData = new GetSetLobbyData();

        displayMarketEnrollUI = GetComponent<DisplayMarketEnrollUI>();
        displayConfirmPanelEvent = GameObject.FindWithTag("GameManager").GetComponent<DisplayConfirmPanelEvent>();
        displayMessage = GameObject.FindWithTag("GameManager").GetComponent<DisplayMessageGameLobby>();
    }

    public void Initialzie()
    {
        // 로컬 유저 정보로 관련된 스킬리스트 가져옴.
        string userData = getSetAccountData.GetAccountDataInJsonType();
        // 관련 스킬을 이차원 배열로 정리.

        List<UserSkillDataInfo> ownedSkills = JsonConvert.DeserializeObject<List<UserSkillDataInfo>>(requestUserData.RequestPossessedSkillInfo(userData));

        // 로컬 유저 선택 정보를 참고.
        LobbyData01 lobbyData01 = getSetLobbyData.GetLobbyData();

        UserSkillDataInfo[] selectedSkillnames = lobbyData01.GetSelectedSkills();

        List<UserSkillDataInfo> canBeEnrollProduct = new List<UserSkillDataInfo>();

        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (Int32.Parse(ownedSkills[i].Count) == 5)
            {
                canBeEnrollProduct.Add(ownedSkills[i]);
            }
        }

        // 선택한 스킬들을 제외하고 출력하기 위한 코드이다.
        for (int i = 0; i < canBeEnrollProduct.Count; i++)
        {
//            Debug.Log("ownedSkills[ " + i + " ][uniqueNumber] : " + canBeEnrollProduct[i]["uniqueNumber"]);
            // 선택한 스킬은 ownedSkills JArray에서 제외한다.
            for (int j = 0; j < selectedSkillnames.Length; j++)
            {
                if ((canBeEnrollProduct[i].UniqueNumber).Equals(selectedSkillnames[j].UniqueNumber))
                {
                    Debug.Log(i);
                    canBeEnrollProduct.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        displayMarketEnrollUI.InitializingSkillPanel();

        displayConfirmPanelEvent.BlockConfirmPanel();

        displayMarketEnrollUI.DisplayCanBeEnrollSkills(canBeEnrollProduct.ToArray());
    }

    public void DisplayTotalSkillsPanel(RectTransform skillListPanel)
    {
        for (int i = 0; i < skillListPanel.childCount; i++)
        {
            skillListPanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void DisplayNotNFTSkillsPanel(RectTransform skillListPanel)
    {
        // 스킬의 이름을 통해 NFT인지 아닌지 확인 후, NFT가 true 값들은 setactive를 false로 해주었다.   
        for (int i = 0; i < skillListPanel.childCount; i++)
        {
            skillListPanel.GetChild(i).gameObject.SetActive(true);

            if (skillListPanel.GetChild(i).GetChild(0).name.Equals("true"))
                skillListPanel.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DisplayNFTSkillsPanel(RectTransform skillListPanel)
    {
        // 스킬의 이름을 통해 NFT인지 아닌지 확인 후, NFT가 false 값들은 setactive를 false로 해주었다.   
        for (int i = 0; i < skillListPanel.childCount; i++)
        {
            skillListPanel.GetChild(i).gameObject.SetActive(true);

            if (skillListPanel.GetChild(i).GetChild(0).name.Equals("false"))
                skillListPanel.GetChild(i).gameObject.SetActive(false);
        }
    }



    // 마켓과 같이 인증이 필요한 기능을 사용할 때, 접근 가능 여부를 판단한다.
    public bool DetermineAccessibility()
    {
        string userDataInJson = getSetAccountData.GetAccountDataInJsonType();
        JObject success = JObject.Parse(requestUserData.RequestMarketAccessibility_test(userDataInJson));

        Debug.Log(success["success"]);

        return (bool)success["success"];
    }



    public void OnClickedEnrollButton()
    {
        if (DetermineAccessibility())
        {
            if (!Convert.ToBoolean(displayConfirmPanelEvent.GetAccessRightsConfirmPanel())) return;

            string enrollPrice = requestUserData.RequestProductEnrollPrice();

            Debug.Log("enrollPrice : " + enrollPrice);
            // 등록 버튼을 클릭 시, 서버에서 Minting 수수료 값을 가져와서 확인 패널 창에 보여준다.
            displayConfirmPanelEvent.AddProductEnrollPrice(enrollPrice);
            displayConfirmPanelEvent.DisplayProductConfirmPanel(true);
        }
        else
        {
            // 인증해야 된다고 메시지 출력.
            displayMessage.DisplayMessagePanel(21);
        }
    }

    public void OnClickedEnrollYesButton()
    {
        string[] userData = getSetAccountData.GetAccountData();
        string selectedSkillData = displayConfirmPanelEvent.GetSelectedProduct();
        string salePrice = displayConfirmPanelEvent.GetProductSellingPrice();

        string success = requestUserData.RequestProductEnroll(new string[] { userData[0], userData[1], selectedSkillData, salePrice });

        if (success == "false")
        {
            Debug.Log("error");
        }

        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);

        displayMarketEnrollUI.InitializingSkillPanel();

        Initialzie();
    }

    public void OnClickedEnrollNoButton()
    {
        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);
    }

}
