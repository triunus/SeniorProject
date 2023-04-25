using Newtonsoft.Json;

using UnityEngine;

using LobbyInformaion;

using ConnectServer;
using GetSetAccountInfo;
using GetSetLobbyInformation;
using ChangeSceneManager;

// 게임 로비에서 발생하는 버튼 이벤트들의 시작부분들을 모아 놓을 예정이다.
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

    // 로비로 돌아올 때 마다, 해당 함수를 호출한다. ==> LobbyData 클래스의 최신화?
    // 최초 호출 시, 유저 정보를 서버에서 받아서 로컬에 저장한다.
    public void Initialize()
    {
        string userData = getSetAccountData.GetAccountDataInJsonType();

        // 서버에서 받은 데이터 객체에 대입.
        // Json의 key이름과 class의 변수명이 일치해야, 제대로 값이 입력된다.
        LobbyData01 lobbyData = JsonConvert.DeserializeObject<LobbyData01>(requestUserData.RequestGameLobbyUserInfomaion(userData));

        // 캐릭터 번호에 따른, 캐릭터 스킬 명시.
        getSetLobbyData.SetSelectedCharacterSkills(lobbyData);

        // 로컬에 저장
        getSetLobbyData.SetLobbyData(lobbyData);

        // 좌측 상단 출력
        displayGameLobbyUI.DisplayUserInfoUI(lobbyData);

        // 왼쪽 상단 패널 비활성
        // 함수 호출
        panelActive = false;
        SelectedInfoContent.gameObject.SetActive(panelActive);
    }

    public void ReDisplayUserInfoUI()
    {
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // 좌측 상단 출력
        displayGameLobbyUI.DisplayUserInfoUI(lobbyData);

        // 왼쪽 상단 패널 비활성
        // 함수 호출
        panelActive = false;
        SelectedInfoContent.gameObject.SetActive(panelActive);
    }

    // 좌측 상단에 표시될, 캐릭터 스킬 정보 버튼 클릭
    public void OnClickedSelectedSkillCheckButton()
    {
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        // 현재 안 켜저 있으면, 누르면 켜지도록 한다. => 켜질때, 서버에서 유니크 값을 통해 스킬 정보를 불러온다.
        if (!panelActive)
        {
            panelActive = true;

            // 좌측 상단의 버튼 밑 부분 출력
            displayGameLobbyUI.DisplayUserSkillInfoUI(lobbyData);

            SelectedInfoContent.gameObject.SetActive(panelActive);
        }
        else  // 현재 켜저 있으면, 누르면 꺼지도록 한다.
        {
            panelActive = false;

            SelectedInfoContent.gameObject.SetActive(panelActive);
        }
    }

    // 게임 시작 버튼 클릭
    public void OnClickedGameStartButton()
    {
        changeScene.SceneLoader("InGame");
    }

}
