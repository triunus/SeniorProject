using Newtonsoft.Json;

using UnityEngine;

using LobbyInformaion;

using ConnectServer;
using GetSetAccountInfo;
using GetSetLobbyInformation;
using ChangeSceneManager;

// ���� �κ񿡼� �߻��ϴ� ��ư �̺�Ʈ���� ���ۺκе��� ��� ���� �����̴�.
public class ButtonClickInGameLobbyMenu : MonoBehaviour
{
    private RequestUserData requestUserData;
    private GetSetAccountData getSetAccountData;
    private GetSetLobbyData getSetLobbyData;
    private ChangeScene changeScene;

    private DisplayGameLobbyUI displayGameLobbyUI;


    [SerializeField]
    private RectTransform SelectedInfoContent;

    private bool panelActive;

    private void Awake()
    {
        requestUserData = new RequestUserData();
        getSetAccountData = new GetSetAccountData();
        getSetLobbyData = new GetSetLobbyData();
        changeScene = new ChangeScene();

        displayGameLobbyUI = GetComponent<DisplayGameLobbyUI>();

        panelActive = false;

        Initialize();
    }

    // �κ�� ���ƿ� �� ����, �ش� �Լ��� ȣ���Ѵ�. ==> LobbyData Ŭ������ �ֽ�ȭ?
    // ���� ȣ�� ��, ���� ������ �������� �޾Ƽ� ���ÿ� �����Ѵ�.
    public void Initialize()
    {
        string userData = getSetAccountData.GetAccountDataInJsonType();

        // �������� ���� ������ ��ü�� ����.
        // Json�� key�̸��� class�� �������� ��ġ�ؾ�, ����� ���� �Էµȴ�.
        LobbyData01 lobbyData = JsonConvert.DeserializeObject<LobbyData01>(requestUserData.RequestGameLobbyUserInfomaion(userData));

        // ĳ���� ��ȣ�� ����, ĳ���� ��ų ���.
        getSetLobbyData.SetSelectedCharacterSkills(lobbyData);

        // ���ÿ� ����
        getSetLobbyData.SetLobbyData(lobbyData);

        // ���� ��� ���
        displayGameLobbyUI.DisplayUserInfoUI(lobbyData);

        // ���� ��� �г� ��Ȱ��
        // �Լ� ȣ��
        panelActive = false;
        SelectedInfoContent.gameObject.SetActive(panelActive);
    }

    public void ReDisplayUserInfoUI()
    {
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // ���� ��� ���
        displayGameLobbyUI.DisplayUserInfoUI(lobbyData);

        // ���� ��� �г� ��Ȱ��
        // �Լ� ȣ��
        panelActive = false;
        SelectedInfoContent.gameObject.SetActive(panelActive);
    }

    // ���� ��ܿ� ǥ�õ�, ĳ���� ��ų ���� ��ư Ŭ��
    public void OnClickedSelectedSkillCheckButton()
    {
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // ���� �� ���� ������, ������ �������� �Ѵ�. => ������, �������� ����ũ ���� ���� ��ų ������ �ҷ��´�.
        if (!panelActive)
        {
            panelActive = true;

            // ���� ����� ��ư �� �κ� ���
            displayGameLobbyUI.DisplayUserSkillInfoUI(lobbyData);

            SelectedInfoContent.gameObject.SetActive(panelActive);
        }
        else  // ���� ���� ������, ������ �������� �Ѵ�.
        {
            panelActive = false;

            SelectedInfoContent.gameObject.SetActive(panelActive);
        }
    }

    // ���� ���� ��ư Ŭ��
    public void OnClickedGameStartButton()
    {
        changeScene.SceneLoader("InGame");
    }

}
