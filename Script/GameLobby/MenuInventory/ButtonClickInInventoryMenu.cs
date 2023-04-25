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
        // 로컬 유저 정보로 관련된 스킬리스트 가져옴.
        string userData = getSetAccountData.GetAccountDataInJsonType();

        string responseData = requestUserData.RequestPossessedSkillInfo(userData);
        if (responseData == "") return;
        List<UserSkillDataInfo> ownedSkills = JsonConvert.DeserializeObject<List<UserSkillDataInfo>>(responseData);

//        Debug.Log(ownedSkills[0].UniqueNumber);

        // 로컬 유저 선택 정보를 참고.
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        UserSkillDataInfo[] selectedSkillnames =lobbyData.GetSelectedSkills();

        displayInventoryMenuUI.DisplayCharacterSkill(lobbyData.SelecedCharacterNumber, lobbyData.GetCharacterSkills());

        // List Type의 ToArray()는 List 안의 내용들을 배열로 리턴해 준다.
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

    public void SaveSelectedSkillData()
    {
        // DisplayInventoryUI에서 스킬에서 대한 정보를 가져온다.
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // 로컬에 저장된 선택 정보 삭제 후 재 기록.
        lobbyData.ClearSelecedSkillNumber();
        lobbyData.AddSelectedSkills(displayInventoryMenuUI.ClickEventSaveButton());

        // 로컬에 저장
        getSetLobbyData.SetLobbyData(lobbyData);
    }
}
