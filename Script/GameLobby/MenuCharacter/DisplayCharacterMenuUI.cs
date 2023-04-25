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

    // ������ ������ ĳ���͸� ����� �ְ� ����. �ٵ�, ��� ����� ������.
    // ���� : ������ �г��� �ʱ�ȭ�� �� �ʿ䰡 ���� ������, ������ �����ߴ� �����Ͱ� �����־... ��ġ, ������ ���� ������ ������ ���̴� �� ����.
    public void DisplayCharacterList(string[] charcterList, string selecedCharacter)
    {
//        string[] notSelectedCharacter = Array.FindAll(charcterList, characterNumber => characterNumber != selecedCharacter);

        createPrefab.DestroyChild(characterPanel);

        // ������ ĳ���� ����Ʈ�� ���� ������ ĳ���� ����.
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
        // ĳ���� �̹��� ��ȣ�� �ִ� json ��ü�� ã��
        JObject jObject = (JObject)jArray01[Int32.Parse(checkedCharacterNumber) - 1];

        characterImage.sprite = Resources.Load<Sprite>("CharacterImage/Character" + checkedCharacterNumber);
        characterImage.name = checkedCharacterNumber;

        characterName.text = (string)jObject["CharacterName"];
        characterDetail.text = (string)jObject["CharacterDetail"];


        JArray jArray02 = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
        //        Debug.Log(dataTable02.Rows[0][0] + ", " + dataTable02.Rows[0][1] + ", " + dataTable02.Rows[0][2] + ", " + dataTable02.Rows[0][3]);

        createPrefab.DestroyChild(CharacterSkillDetailPanel);

        //��ų ��ȣ�� �´� json ��ü�� ã��. (-1�� �־���� �ش� ��ġ�� ����.)
        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)jArray02[(int)jObject["CharacterSkill01"] - 1]);
        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)jArray02[(int)jObject["CharacterSkill02"] - 1]);
    }

    public string GetSelectedCharacterNumber()
    {
        return characterImage.name;
    }

}
