using System;
using System.IO;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCharacterMenuUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform characterPanel;

    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private TextMeshProUGUI characterDetail;

    [SerializeField]
    private RectTransform CharacterSkillDetailPanel;

    private RectTransform previousProductPanel;

    CreatePrefab createPrefab;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();

        previousProductPanel = GetComponent<RectTransform>();
    }

    // 유저가 선택한 캐릭터를 명시해 주고 싶음. 근데, 없어도 상관이 없긴함.
    // 이유 : 오른쪽 패널은 초기화를 할 필요가 없기 떄문에, 이전에 선택했던 데이터가 남아있어서... 마치, 이전에 내가 선택한 내용이 보이는 것 같음.
    public void DisplayCharacterList(string[] charcterList, string selecedCharacter)
    {
//        string[] notSelectedCharacter = Array.FindAll(charcterList, characterNumber => characterNumber != selecedCharacter);

        createPrefab.DestroyChild(characterPanel);

        // 가져온 캐릭터 리스트로 선택 가능한 캐릭터 생성.
        createPrefab.CreateCharacterSlot(characterPanel, charcterList);
    }

    public void DisplayCheckedCharacterSlot(RectTransform currentProductPanel)
    {
        if (previousProductPanel == null || previousProductPanel.CompareTag("MenuCharacter")) previousProductPanel = currentProductPanel;

        Debug.Log("previousProductPanel : " + previousProductPanel);

        previousProductPanel.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 100);

        previousProductPanel.GetComponent<ClickEventCharacterSlot>().Checkable = false;

        currentProductPanel.GetChild(0).GetComponent<Image>().color = Color.yellow;

        currentProductPanel.GetComponent<ClickEventCharacterSlot>().Checkable = true;

        previousProductPanel = currentProductPanel;
    }

    public void DisplaySelectedCharacterUI(string checkedCharacterNumber)
    {

        JArray jArray01 = JArray.Parse(File.ReadAllText("./Assets/GameData/CharactersInfo/CharactersInfo.json"));
        // 캐릭터 이미지 번호에 있는 json 객체를 찾음
        JObject jObject = (JObject)jArray01[Int32.Parse(checkedCharacterNumber) - 1];

        characterImage.sprite = Resources.Load<Sprite>("CharacterImage/Character" + checkedCharacterNumber);
        characterImage.name = checkedCharacterNumber;

        characterName.text = (string)jObject["CharacterName"];
        characterDetail.text = (string)jObject["CharacterDetail"];


        JArray jArray02 = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
        //        Debug.Log(dataTable02.Rows[0][0] + ", " + dataTable02.Rows[0][1] + ", " + dataTable02.Rows[0][2] + ", " + dataTable02.Rows[0][3]);

        createPrefab.DestroyChild(CharacterSkillDetailPanel);

        //스킬 번호에 맞는 json 객체를 찾음. (-1이 있어야지 해당 위치로 간다.)
        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)jArray02[(int)jObject["CharacterSkill01"] - 1]);
        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)jArray02[(int)jObject["CharacterSkill02"] - 1]);
    }

    public string GetSelectedCharacterNumber()
    {
        return characterImage.name;
    }

}
