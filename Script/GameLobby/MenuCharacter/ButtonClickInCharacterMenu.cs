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

    // Chaarcter �޴��� ������ ���۵Ǵ� �Լ�.
    public void Initialize()
    {
        // ���ÿ��� ���� ������ ������.
        string[] userData = getSetAccountData.GetAccountData();

        // ���� �����ͷ� �������� ������ ���� ĳ���� ����Ʈ ������.
        string[] characterList = ReturnValueParse(requestUserData.RequestPossessedCharacterList(userData));

        LobbyData01 lobbyData = getSetLobbyData.GetLobbyData();

        displayCharacterMenuUI.DisplayCharacterList(characterList, lobbyData.SelecedCharacterNumber);
    }

    // ĳ���� ����Ʈ�� json ������ �ƴ�, �Ϲ� stream �������� �޾ƿ´�.
    // �ణ�� parsing ������ �ʿ��ϴ�.
    public string[] ReturnValueParse(string responseFromServer)
    {
        // ū ����ǥ�� ����.
        string temp = responseFromServer.Replace("\"", System.String.Empty);
        //        Debug.Log("responseFromServer : " + responseFromServer);
        //        Debug.Log("temp : " + temp);

        char[] delimiter = { ' ', ',', '[', ']' };
        string[] tmp = temp.Split(delimiter);

        // ���ڿ��� ������, �� �հ� �� �ڸ� �����ϴ� �κ�.
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
        // �ش� Ŭ�������� ���������� �˰� �ִ� ���� �� ��������.
        lobbyData.SelecedCharacterNumber = displayCharacterMenuUI.GetSelectedCharacterNumber();

        // ĳ���� ���濡 ����, ���� ��ų ��ȭ�� ���. ���� �ش� �� ���ÿ� ����.
        getSetLobbyData.SetSelectedCharacterSkills(lobbyData);
        getSetLobbyData.SetLobbyData(lobbyData);
    }
}
