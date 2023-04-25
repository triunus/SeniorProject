using System;
using System.IO;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SkillInformaion;

public class DisplayMarketEnrollUI : MonoBehaviour
{
    CreatePrefab createPrefab;

    [SerializeField]
    private RectTransform skillSlotPanel01;

    [SerializeField]
    private RectTransform selectedSkillPanel;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }
    
    // 등록 가능한 스킬만 출력한다.
    public void DisplayCanBeEnrollSkills(UserSkillDataInfo[] canBeEnrollProduct)
    {
        // 기존에 생성하였던 프리팹들 삭제
        createPrefab.DestroyChild(skillSlotPanel01);

        createPrefab.CreateSkillSlot01InMarketEnroll(skillSlotPanel01, canBeEnrollProduct);
    }

    // 우측 패널에 내용을 출력한다.
    public void DisplaySkillContent(UserSkillDataInfo selectedSkill)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // 이미지 변경
        selectedSkillPanel.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + selectedSkill.SkillNumber);

        string content = (string)skillData[Int32.Parse(selectedSkill.SkillNumber) - 1]["SkillDamage"] + " "
            + ((string)skillData[Int32.Parse(selectedSkill.SkillNumber) - 1]["SkillDetail"]).Split('_')[0]
            + (string)skillData[Int32.Parse(selectedSkill.SkillNumber) - 1]["NumberOfTargets"]
            + ((string)skillData[Int32.Parse(selectedSkill.SkillNumber) - 1]["SkillDetail"]).Split('_')[1];

        // 텍스트 변경
        selectedSkillPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillData[Int32.Parse(selectedSkill.SkillNumber) - 1]["SkillName"];
        
        selectedSkillPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
    }

    public void InitializingSkillPanel()
    {
        selectedSkillPanel.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;

        selectedSkillPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        selectedSkillPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
    }
}
