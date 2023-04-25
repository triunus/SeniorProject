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

    /** JArray 유저가 갖는 스킬, string[] 유저가 선택한 스킬 **/
    public void DisplaySkillList(UserSkillDataInfo[] ownedSkills, UserSkillDataInfo[] selectedSkillnames)
    {
        // 기존에 생성하였던 프리팹들 삭제
        createPrefab.DestroyChild(skillSlotPanel01);
        createPrefab.DestroyChild(selectedSkillsPanel);

        // 원래, 보유 스킬 중에, 선택한 스킬을 분리 시킨 후 작성하는게 효과적이긴 하다.
        // 근데, 순서대로 보여주고 싶었기에, 이와 같은 방법을 취햐였다.
        for (int i = 0; i < ownedSkills.Length; i++)
        {
            bool create = false;
            for (int j = 0; j < selectedSkillnames.Length; j++)
            {
                if (ownedSkills[i].UniqueNumber.Equals(selectedSkillnames[j].UniqueNumber))
                {
                    // 좌측 패널에 생성 및 색 변경.
                    createPrefab.CreateSkillSlot02InInventory_test(skillSlotPanel01, ownedSkills[i]);

                    // 선택한 스킬이 등록되는 패널 자식 개수가 6개를 초과할 수가 없다.
                    // 넘어가면 안된다, 이 조건문은 에러를 방지하기 위해 작성.
                    if (selectedSkillsPanel.childCount > 6) break;

                    // 우측 패널에 생성.
                    createPrefab.CreateSkillSlot01InInventory_test(selectedSkillsPanel, ownedSkills[i]);

                    create = true;
                    break;
                }
            }
            // 선택하지 않은 스킬이 였을 시, 좌측 패널 생성.
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
            // 선택한 스킬의 개수가 6개 이상이면 그냥 리턴.
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

            createPrefab.CreateSkillSlot01InInventory_test(selectedSkillsPanel, userSkillDataInfo); // 여기 매개변수 수정

            skillSlotPanel.GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);     // 클릭 됨
            skillSlotPanel.GetComponent<SkillSlotClickEvent>().Checkable = false;
        }

        if (skillSlotPanel.parent.CompareTag("SelectedSkillListPanel"))
        {
            Destroy(skillSlotPanel.gameObject);

            // 프리팹이 사라지면서, pointer exit가 발생할 수가 없기에, 해당 content를 이곳에서 삭제해 준다.
            createPrefab.DestroyPartPanel("SkillContent");

            // 우측에서 선택한 스킬의 이름과 동일한 스킬 이름을 좌측 패널에서 찾는다.
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