using System;
using System.IO;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SkillInformaion;

public class DisplayInventoryMenuUI : MonoBehaviour
{
    CreatePrefab createPrefab;

    [SerializeField]
    private RectTransform skillSlotPanel01;

    [SerializeField]
    private RectTransform selectedSkillsPanel;

    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private RectTransform CharacterSkillDetailPanel;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }

    /** JArray ������ ���� ��ų, string[] ������ ������ ��ų **/
    public void DisplaySkillList(UserSkillDataInfo[] ownedSkills, UserSkillDataInfo[] selectedSkillnames)
    {
        // ������ �����Ͽ��� �����յ� ����
        createPrefab.DestroyChild(skillSlotPanel01);
        createPrefab.DestroyChild(selectedSkillsPanel);

        // ����, ���� ��ų �߿�, ������ ��ų�� �и� ��Ų �� �ۼ��ϴ°� ȿ�����̱� �ϴ�.
        // �ٵ�, ������� �����ְ� �;��⿡, �̿� ���� ����� ���ῴ��.
        for (int i = 0; i < ownedSkills.Length; i++)
        {
            bool create = false;
            for (int j = 0; j < selectedSkillnames.Length; j++)
            {
                if (ownedSkills[i].UniqueNumber.Equals(selectedSkillnames[j].UniqueNumber))
                {
                    // ���� �гο� ���� �� �� ����.
                    createPrefab.CreateSkillSlot02InInventory_test(skillSlotPanel01, ownedSkills[i]);

                    // ������ ��ų�� ��ϵǴ� �г� �ڽ� ������ 6���� �ʰ��� ���� ����.
                    // �Ѿ�� �ȵȴ�, �� ���ǹ��� ������ �����ϱ� ���� �ۼ�.
                    if (selectedSkillsPanel.childCount > 6) break;

                    // ���� �гο� ����.
                    createPrefab.CreateSkillSlot01InInventory_test(selectedSkillsPanel, ownedSkills[i]);

                    create = true;
                    break;
                }
            }
            // �������� ���� ��ų�� ���� ��, ���� �г� ����.
            if (!create) { createPrefab.CreateSkillSlot01InInventory_test(skillSlotPanel01, ownedSkills[i]); }
        }
    }

    public void DisplayCharacterSkill(string selectedCharacterNumber, string[] characterSkills)
    {
        characterImage.sprite = Resources.Load<Sprite>("CharacterImage/Character" + selectedCharacterNumber);
        characterImage.name = selectedCharacterNumber;

        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
        //        Debug.Log(dataTable02.Rows[0][0] + ", " + dataTable02.Rows[0][1] + ", " + dataTable02.Rows[0][2] + ", " + dataTable02.Rows[0][3]);

        createPrefab.DestroyChild(CharacterSkillDetailPanel);

        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)skillData[Int32.Parse(characterSkills[0]) - 1]);
        createPrefab.CreateCharacterSkillDetailPanel(CharacterSkillDetailPanel, (JObject)skillData[Int32.Parse(characterSkills[1]) - 1]);
    }

    public void ClickEventSkillSlot(RectTransform skillSlotPanel)
    {
        if(skillSlotPanel.parent.CompareTag("SkillListPanel") && skillSlotPanel.GetComponent<SkillSlotClickEvent>().Checkable)
        {
            // ������ ��ų�� ������ 6�� �̻��̸� �׳� ����.
            if (selectedSkillsPanel.childCount > 5) return;

            UserSkillDataInfo userSkillDataInfo = new UserSkillDataInfo(
                    skillSlotPanel.name,
                    skillSlotPanel.transform.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty),
                    skillSlotPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text,
                    skillSlotPanel.transform.GetChild(0).name
                );

/*            Debug.Log("UniqueNumber : " + userSkillDataInfo.UniqueNumber);
            Debug.Log("SkillNumber : " + userSkillDataInfo.SkillNumber);
            Debug.Log("Count : " + userSkillDataInfo.Count);
            Debug.Log("IsNFT : " + userSkillDataInfo.IsNFT);*/

            createPrefab.CreateSkillSlot01InInventory_test(selectedSkillsPanel, userSkillDataInfo); // ���� �Ű����� ����

            skillSlotPanel.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);     // Ŭ�� ��
            skillSlotPanel.GetComponent<SkillSlotClickEvent>().Checkable = false;
        }

        if (skillSlotPanel.parent.CompareTag("SelectedSkillListPanel"))
        {
            Destroy(skillSlotPanel.gameObject);

            // �������� ������鼭, pointer exit�� �߻��� ���� ���⿡, �ش� content�� �̰����� ������ �ش�.
            createPrefab.DestroyPartPanel("SkillContent");

            // �������� ������ ��ų�� �̸��� ������ ��ų �̸��� ���� �гο��� ã�´�.
            RectTransform selectedskill = GameObject.Find(skillSlotPanel.name).GetComponent<RectTransform>();

            selectedskill.GetChild(0).GetComponent<Image>().color = Color.white;

            selectedskill.GetComponent<SkillSlotClickEvent>().Checkable = true;
        }
    }

    public UserSkillDataInfo[] ClickEventSaveButton()
    {
        UserSkillDataInfo[] userSkillsDataInfo = new UserSkillDataInfo[selectedSkillsPanel.childCount];

        for (int i = 0; i < selectedSkillsPanel.childCount; i++)
        {
            UserSkillDataInfo userSkillDataInfo = new UserSkillDataInfo(
                selectedSkillsPanel.GetChild(i).name,
                selectedSkillsPanel.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty),
                selectedSkillsPanel.GetChild(i).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text,
                selectedSkillsPanel.GetChild(i).transform.GetChild(0).name
            );

            userSkillsDataInfo[i] = userSkillDataInfo;
        }

        return userSkillsDataInfo;
    }
}