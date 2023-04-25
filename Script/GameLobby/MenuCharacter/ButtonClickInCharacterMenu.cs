using UnityEngine;

using GetSetAccountInfo;
using ConnectServer;
using GetSetLobbyInformation;

using LobbyInformaion;


public class ButtonClickInCharacterMenu : MonoBehaviour
{
    private RequestUserData requestUserData;
    private GetSetAccountData getSetAccountData;
    private GetSetLobbyData getSetLobbyData;

    DisplayCharacterMenuUI displayCharacterMenuUI;

    private void Awake()
    {
        requestUserData = new RequestUserData();
        getSetAccountData = new GetSetAccountData();
        getSetLobbyData = new GetSetLobbyData();

        displayCharacterMenuUI = GetComponent<DisplayCharacterMenuUI>();
    }

    // Chaarcter 메뉴에 들어오면 시작되는 함수.
    public void Initialize()
    {
        // 로컬에서 유저 데이터 가져옴.
        string[] userData = getSetAccountData.GetAccountData();

        // 유저 데이터로 서버에서 유저가 갖은 캐릭터 리스트 가져옴.
        string[] characterList = ReturnValueParse(requestUserData.RequestPossessedCharacterList(userData));

        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        displayCharacterMenuUI.DisplayCharacterList(characterList, lobbyData.SelecedCharacterNumber);
    }

    // 캐릭터 리스트는 json 형식이 아닌, 일반 stream 형식으로 받아온다.
    // 약간의 parsing 과정이 필요하다.
    public string[] ReturnValueParse(string responseFromServer)
    {
        // 큰 따옴표를 삭제.
        string temp = responseFromServer.Replace("\"", System.String.Empty);
        //        Debug.Log("responseFromServer : " + responseFromServer);
        //        Debug.Log("temp : " + temp);

        char[] delimiter = { ' ', ',', '[', ']' };
        string[] tmp = temp.Split(delimiter);

        // 문자열을 나누고, 맨 앞과 맨 뒤를 제외하는 부분.
        string[] value = new string[tmp.Length - 2];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] = tmp[i + 1];
        }

        return value;
    }

    public void SaveCharacterData()
    {
        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();
        // 해당 클래스에서 간접적으로 알고 있는 변수 값 가져오기.
        lobbyData.SelecedCharacterNumber = displayCharacterMenuUI.GetSelectedCharacterNumber();

        // 캐릭터 변경에 따른, 소지 스킬 변화를 명시. 이후 해당 값 로컬에 저장.
        getSetLobbyData.SetSelectedCharacterSkills(lobbyData);
        getSetLobbyData.SetLobbyData(lobbyData);
    }
}
