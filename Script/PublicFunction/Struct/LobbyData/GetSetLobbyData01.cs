using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnityEngine;

using LobbyInformaion;

namespace GetSetLobbyInformation
{
    public class GetSetLobbyData
    {
        IFormatter binaryFormatter;


        //        public void SetLobbyData(string data, string type)
        public void SetLobbyData(LobbyData01 lobbyData)
        {
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("SetAccountData ����");

                Stream writeInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                // ���� ���Ϸ� ����ȭ ����
                binaryFormatter.Serialize(writeInfo, lobbyData);

                writeInfo.Close();
                Debug.Log("Write Complete");
            }
            catch (UnassignedReferenceException error01)
            {
                Debug.Log(error01);
            }
        }

        public void SetSelectedCharacterSkills(LobbyData01 lobbyData)
        {
            JArray characterData = JArray.Parse(File.ReadAllText("./Assets/GameData/CharactersInfo/CharactersInfo.json"));

            lobbyData.ClearCharacterSkill();
            lobbyData.AddCharacterSkill((string)characterData[Int32.Parse(lobbyData.SelecedCharacterNumber) - 1]["CharacterSkill01"]);
            lobbyData.AddCharacterSkill((string)characterData[Int32.Parse(lobbyData.SelecedCharacterNumber) - 1]["CharacterSkill02"]);
        }

        public LobbyData01 GetLobbyData()
        {
            LobbyData01 lobbyData;
            binaryFormatter = new BinaryFormatter();

            try
            {
                Debug.Log("GetAccountData ����.");

                Stream readInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                lobbyData = (LobbyData01)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return lobbyData;
            }
            catch (FileNotFoundException error01)
            {
                // ���� ���� �ڵ� 21
                Debug.Log("���� ���� : " + error01);
                return null;
            }
        }
    }
}


/*// �ش� Ŭ������ �ּ����� ����, LobbyData�� ȣ���ϴ� ��� ��ɵ��� �ϼ��Ǹ�, ����ȭ�� ������ �� ����غ����� �͵��� ������.
// enum�� ����Ͽ� ���� type�� �ľ� --> �˸��� �Լ��� ���� ���� �����̴�.
// �׳� ��� ���ϱ�� ��.

public enum SetType
{
    Initialize,
    AddCharacter,
    AddSkill
}

                switch ((SetType)Enum.Parse(typeof(SetType), type))
                {
                    case SetType.Initialize:
                        SetLobbyDataCharacterSkill();
                        Debug.Log("01");
                        break;
                    case SetType.AddCharacter:
                        Debug.Log("02");
                        break;
                    case SetType.AddSkill:
                        Debug.Log("03");
                        break;
                    default:
                        Debug.Log("04");
                        break;
                }*/


/*// �������� �����͸� Json���� ��ȯ���Ѽ� ����߾�����,
// ���� ���ǵ� ���� �ƴ� List<List<string>> �� ���� ������ json���� ��ȯ�ϱⰡ ���� ��ٷο���.
// �׶� 4�ð� ���� �����ߴ��Ŷ� ����� �� �׷� ���� 


public string GetLobbyDataInJsonType()
{
    LobbyData01 lobbyData;
    binaryFormatter = new BinaryFormatter();

    try
    {
        Debug.Log("GetAccountData ����.");

        Stream readInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

        lobbyData = (LobbyData01)binaryFormatter.Deserialize(readInfo);

        readInfo.Close();

        return ConvertLobbyDataToJson(lobbyData);
    }
    catch (FileNotFoundException error01)
    {
        // ���� ���� �ڵ� 21
        Debug.Log("���� ���� : " + error01);
        return null;
    }
}

public string ConvertLobbyDataToJson(LobbyData01 lobbyData)
{
    // List ������ ����ȭ ��󿡼� ���ܵǹǷ�, ���� List ���� ������ ������ ���� �� ����ȭ ���־�� �Ѵ�.
    JObject tmp01 = JObject.Parse(JsonConvert.SerializeObject(lobbyData, Formatting.Indented));

    JArray tmp02 = new JArray();

    for (int i = 0; i < lobbyData.GetCharacterSkills().Length; i++)
    {
        tmp02.Add(lobbyData.GetCharacterSkill(i));
    }

    // Ư�� Ű ������, JArray �߰�.
    tmp01["characterSkill"] = tmp02;
    tmp02.Clear();
    //            Debug.Log(tmp02);

    for (int i = 0; i < lobbyData.GetSelectedSkills().Length; i++)
    {
        tmp02.Add(lobbyData.GetSelecedSkill(i));
    }

    // Ư�� Ű ������, JArray �߰�.
    tmp01["selecedSkill"] = tmp02;

    //            Debug.Log(tmp01.ToString());

    return tmp01.ToString();
}*/