using GetSetAccountInfo;
using GetSetLobbyInformation;
using ConnectServer;

using LobbyInformaion;
using SkillInformaion;

using UnityEngine;

using System.Collections.Generic;
using Newtonsoft.Json;


public class ButtonClickInInventoryMenu : MonoBehaviour
{
    GetSetAccountData getSetAccountData;
    GetSetLobbyData getSetLobbyData;
    RequestUserData requestUserData;
    DisplayInventoryMenuUI displayInventoryMenuUI;

    private void Awake()
    {
        getSetAccountData = new GetSetAccountData();
        getSetLobbyData = new GetSetLobbyData();
        requestUserData = new RequestUserData();
        displayInventoryMenuUI = GetComponent<DisplayInventoryMenuUI>();
    }

    public void Initialize()
    {
        // ���� ���� ������ ���õ� ��ų����Ʈ ������.
        string userData = getSetAccountData.GetAccountDataInJsonType();

        string responseData = requestUserData.RequestPossessedSkillInfo(userData);
        if (responseData == "") return;
        List<UserSkillDataInfo> ownedSkills = JsonConvert.DeserializeObject<List<UserSkillDataInfo>>(responseData);

//        Debug.Log(ownedSkills[0].UniqueNumber);

        // ���� ���� ���� ������ ����.
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        UserSkillDataInfo[] selectedSkillnames =lobbyData.GetSelectedSkills();

        displayInventoryMenuUI.DisplayCharacterSkill(lobbyData.SelecedCharacterNumber, lobbyData.GetCharacterSkills());

        // List Type�� ToArray()�� List ���� ������� �迭�� ������ �ش�.
        displayInventoryMenuUI.DisplaySkillList(ownedSkills.ToArray(), selectedSkillnames);
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

    public void SaveSelectedSkillData()
    {
        // DisplayInventoryUI���� ��ų���� ���� ������ �����´�.
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // ���ÿ� ����� ���� ���� ���� �� �� ���.
        lobbyData.ClearSelecedSkillNumber();
        lobbyData.AddSelectedSkills(displayInventoryMenuUI.ClickEventSaveButton());

        // ���ÿ� ����
        getSetLobbyData.SetLobbyData(lobbyData);
    }
}
