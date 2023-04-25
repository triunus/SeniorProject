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

    // �ش� �±װ� �� ��Ұ� �ñ��ϸ�, ���� ���� Tag.txt ���� ����.

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }

    // ���� �κ� ���� ����� UI�� �����Ѵ�.
    public void DisplayUserInfoUI(LobbyData01 lobbyData)
    {
        // ���� �г��� ����
        userNickname.text = lobbyData.UserNickname;

        // ���� �̹��� ����
        userImage.sprite = Resources.Load<Sprite>("UserImage/User" + lobbyData.UserImage);

        // ������ ������ ĳ���� ����.
        selectedCharacterImage.sprite = Resources.Load<Sprite>("CharacterImage/Character" + lobbyData.SelecedCharacterNumber);

        // ���� ��ȭ ����
        possessedCoin.text = lobbyData.PossessedCoin;
        possessedSpecialCoin.text = lobbyData.PossessedSpecialCoin;
    }


    // ���� ��ų ������ ����Ѵ�.
    public void DisplayUserSkillInfoUI(LobbyData01 lobbyData)
    {
        // ���� ��ų ���� ����.
        createPrefab.DestroyChild(selectedCharacterSkillsPanel);
        createPrefab.DestroyChild(selectedSkillsPanel);

        // ĳ���Ͱ� ���� ��ų ����.
        createPrefab.CreateSkillSlot01InLobby(selectedCharacterSkillsPanel, lobbyData.GetCharacterSkills());

        // ���� ��ų ����.
        createPrefab.CreateSkillSlot02InLobby(selectedSkillsPanel, lobbyData.GetSelectedSkills());
    }

}
