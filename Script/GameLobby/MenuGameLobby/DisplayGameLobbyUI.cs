using UnityEngine;
using UnityEngine.UI;
using TMPro;

using LobbyInformaion;

public class DisplayGameLobbyUI : MonoBehaviour
{
    [SerializeField]
    private Image userImage;
    [SerializeField]
    private TextMeshProUGUI userNickname;
    [SerializeField]
    private Image selectedCharacterImage;
    [SerializeField]
    private TextMeshProUGUI possessedCoin;
    [SerializeField]
    private TextMeshProUGUI possessedSpecialCoin;

    [SerializeField]
    private RectTransform selectedCharacterSkillsPanel;
    [SerializeField]
    private RectTransform selectedSkillsPanel;

    CreatePrefab createPrefab;

    // 해당 태그가 들어간 장소가 궁금하면, 따로 만든 Tag.txt 파일 참조.

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }

    // 게임 로비 좌측 상단의 UI를 조정한다.
    public void DisplayUserInfoUI(LobbyData01 lobbyData)
    {
        // 유저 닉네임 설정
        userNickname.text = lobbyData.UserNickname;

        // 유저 이미지 설정
        userImage.sprite = Resources.Load<Sprite>("UserImage/User" + lobbyData.UserImage);

        // 유저가 선택한 캐릭터 설정.
        selectedCharacterImage.sprite = Resources.Load<Sprite>("CharacterImage/Character" + lobbyData.SelecedCharacterNumber);

        // 유저 재화 정보
        possessedCoin.text = lobbyData.PossessedCoin;
        possessedSpecialCoin.text = lobbyData.PossessedSpecialCoin;
    }


    // 유저 스킬 인포를 출력한다.
    public void DisplayUserSkillInfoUI(LobbyData01 lobbyData)
    {
        // 기존 스킬 슬롯 지움.
        createPrefab.DestroyChild(selectedCharacterSkillsPanel);
        createPrefab.DestroyChild(selectedSkillsPanel);

        // 캐릭터가 갖은 스킬 설정.
        createPrefab.CreateSkillSlot01InLobby(selectedCharacterSkillsPanel, lobbyData.GetCharacterSkills());

        // 선택 스킬 설정.
        createPrefab.CreateSkillSlot02InLobby(selectedSkillsPanel, lobbyData.GetSelectedSkills());
    }

}
