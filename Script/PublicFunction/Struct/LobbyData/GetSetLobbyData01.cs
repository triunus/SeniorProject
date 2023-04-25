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
                Debug.Log("SetAccountData 시작");

                Stream writeInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Create, FileAccess.Write, FileShare.None);

                // 로컬 파일로 직렬화 저장
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
                Debug.Log("GetAccountData 시작.");

                Stream readInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

                lobbyData = (LobbyData01)binaryFormatter.Deserialize(readInfo);
                readInfo.Close();

                return lobbyData;
            }
            catch (FileNotFoundException error01)
            {
                // 파일 없음 코드 21
                Debug.Log("파일 없음 : " + error01);
                return null;
            }
        }
    }
}


/*// 해당 클래스의 주석들은 차후, LobbyData를 호출하는 모든 기능들이 완성되면, 최적화를 수행할 때 사용해볼만한 것들을 적었다.
// enum을 사용하여 설정 type을 파악 --> 알맞은 함수를 실행 같은 느낌이다.
// 그냥 사용 안하기로 함.

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


/*// 옛날에는 데이터를 Json으로 변환시켜서 사용했었지만,
// 아직 정의된 것이 아닌 List<List<string>> 과 같은 형식은 json으로 변환하기가 조금 까다로웠다.
// 그때 4시간 정도 삽질했던거라 지우기 좀 그럼 ㅎㅎ 


public string GetLobbyDataInJsonType()
{
    LobbyData01 lobbyData;
    binaryFormatter = new BinaryFormatter();

    try
    {
        Debug.Log("GetAccountData 시작.");

        Stream readInfo = new FileStream("./Assets/LocalData/usr/LobbyData/LobbyData01.txt", FileMode.Open, FileAccess.Read, FileShare.Read);

        lobbyData = (LobbyData01)binaryFormatter.Deserialize(readInfo);

        readInfo.Close();

        return ConvertLobbyDataToJson(lobbyData);
    }
    catch (FileNotFoundException error01)
    {
        // 파일 없음 코드 21
        Debug.Log("파일 없음 : " + error01);
        return null;
    }
}

public string ConvertLobbyDataToJson(LobbyData01 lobbyData)
{
    // List 변수는 직렬화 대상에서 제외되므로, 따로 List 내의 값들을 꺼내어 연결 후 직렬화 해주어야 한다.
    JObject tmp01 = JObject.Parse(JsonConvert.SerializeObject(lobbyData, Formatting.Indented));

    JArray tmp02 = new JArray();

    for (int i = 0; i < lobbyData.GetCharacterSkills().Length; i++)
    {
        tmp02.Add(lobbyData.GetCharacterSkill(i));
    }

    // 특정 키 값으로, JArray 추가.
    tmp01["characterSkill"] = tmp02;
    tmp02.Clear();
    //            Debug.Log(tmp02);

    for (int i = 0; i < lobbyData.GetSelectedSkills().Length; i++)
    {
        tmp02.Add(lobbyData.GetSelecedSkill(i));
    }

    // 특정 키 값으로, JArray 추가.
    tmp01["selecedSkill"] = tmp02;

    //            Debug.Log(tmp01.ToString());

    return tmp01.ToString();
}*/