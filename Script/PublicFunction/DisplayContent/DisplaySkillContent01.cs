using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using Newtonsoft.Json.Linq;

using prefabTagData;

/** 항상 프리팹의 현재 RectTransform이 전달되어 시작된다.**/
public class DisplaySkillContent01 : MonoBehaviour
{
    CreatePrefab createPrefab;
    ClassifictationByPrefab.PrefabPanelTag prefabPanelTag;

    RectTransform skillSlotPanel;

    JArray skillData;
    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();

        skillSlotPanel = GetComponent<RectTransform>();

        skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
    }

    // contentPrefab 생성
    // 해당 함수는 마우스가 가리키는 스킬이 고유한 값을 스킬이 아닌,
    // 상품 or 캐릭터의 스킬과 같이 고유값이 없는 스킬이 스킬에 대한 내용을 나타낼때 사용하는 함수입니다.
    public void ClassificationAccordingToUsage(RectTransform panelPosition)
    {
        prefabPanelTag = (ClassifictationByPrefab.PrefabPanelTag)Enum.Parse(typeof(ClassifictationByPrefab.PrefabPanelTag), panelPosition.tag);
        //        Debug.Log("panelPosition.tag : " + panelPosition.tag + ", " + "prefabpanelTag : " + prefabpanelTag);

        switch (prefabPanelTag)
        {
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InLobby:
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetChild(0).GetComponent<RectTransform>();

                // 인자로 받는 RectTransform은 항상 스킬 Image가 위치한 패널이여야 합니다.
                createPrefab.CreateSkillContent01(skillSlotPanel, skillData);
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot02InLobby:             // 대표적으로 count와 isnft 또한 전달해 주어야 한다.
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetComponent<RectTransform>();

                // 인자로 받는 RectTransform은 스킬 Image가 위치한 부모 패널이여야 합니다.
                createPrefab.CreateSkillContent02(skillSlotPanel, skillData);
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot02InInventory:

                skillSlotPanel = panelPosition.GetComponent<RectTransform>();

                createPrefab.CreateSkillContent02(skillSlotPanel, skillData);
                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductVender:

                panelPosition.GetChild(1).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetChild(1).GetComponent<RectTransform>();

                createPrefab.CreateSkillContent01(skillSlotPanel, skillData);
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InEnroll:

                panelPosition.GetChild(0).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetComponent<RectTransform>();

                createPrefab.CreateSkillContent02(skillSlotPanel, skillData);
                break;
            default:
                Debug.Log("you have to register prefab tag in this cs file");
                break;
        }

    }

    // contentPrefab 삭제
    public void DestroyConstent(RectTransform panelPosition)
    {
        prefabPanelTag = (ClassifictationByPrefab.PrefabPanelTag)Enum.Parse(typeof(ClassifictationByPrefab.PrefabPanelTag), panelPosition.tag);
        //        Debug.Log("panelPosition.tag : " + panelPosition.tag + ", " + "prefabpanelTag : " + prefabpanelTag);

        switch (prefabPanelTag)
        {
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InLobby :
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.white;
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot02InLobby:
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.white;
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot02InInventory:
                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductVender:
                panelPosition.GetChild(1).GetComponent<Image>().color = Color.white;
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InEnroll:
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.white;
                break;
            default:
                Debug.Log("you have to register prefab tag in this cs file");
                break;
        }

        createPrefab.DestroyPartPanel("SkillContent");
    }
}
