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
        // ���� ���� ������ ���õ� ��ų����Ʈ ������.
        string userData = getSetAccountData.GetAccountDataInJsonType();
        // ���� ��ų�� ������ �迭�� ����.

        List<UserSkillDataInfo> ownedSkills = JsonConvert.DeserializeObject<List<UserSkillDataInfo>>(requestUserData.RequestPossessedSkillInfo(userData));

        // ���� ���� ���� ������ ����.
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

        // ������ ��ų���� �����ϰ� ����ϱ� ���� �ڵ��̴�.
        for (int i = 0; i < canBeEnrollProduct.Count; i++)
        {
//            Debug.Log("ownedSkills[ " + i + " ][uniqueNumber] : " + canBeEnrollProduct[i]["uniqueNumber"]);
            // ������ ��ų�� ownedSkills JArray���� �����Ѵ�.
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
        // ��ų�� �̸��� ���� NFT���� �ƴ��� Ȯ�� ��, NFT�� true ������ setactive�� false�� ���־���.   
        for (int i = 0; i < skillListPanel.childCount; i++)
        {
            skillListPanel.GetChild(i).gameObject.SetActive(true);

            if (skillListPanel.GetChild(i).GetChild(0).name.Equals("true"))
                skillListPanel.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DisplayNFTSkillsPanel(RectTransform skillListPanel)
    {
        // ��ų�� �̸��� ���� NFT���� �ƴ��� Ȯ�� ��, NFT�� false ������ setactive�� false�� ���־���.   
        for (int i = 0; i < skillListPanel.childCount; i++)
        {
            skillListPanel.GetChild(i).gameObject.SetActive(true);

            if (skillListPanel.GetChild(i).GetChild(0).name.Equals("false"))
                skillListPanel.GetChild(i).gameObject.SetActive(false);
        }
    }



    // ���ϰ� ���� ������ �ʿ��� ����� ����� ��, ���� ���� ���θ� �Ǵ��Ѵ�.
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
            // ��� ��ư�� Ŭ�� ��, �������� Minting ������ ���� �����ͼ� Ȯ�� �г� â�� �����ش�.
            displayConfirmPanelEvent.AddProductEnrollPrice(enrollPrice);
            displayConfirmPanelEvent.DisplayProductConfirmPanel(true);
        }
        else
        {
            // �����ؾ� �ȴٰ� �޽��� ���.
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
