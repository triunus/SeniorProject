using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using Newtonsoft.Json.Linq;

using prefabTagData;

/** �׻� �������� ���� RectTransform�� ���޵Ǿ� ���۵ȴ�.**/
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

    // contentPrefab ����
    // �ش� �Լ��� ���콺�� ����Ű�� ��ų�� ������ ���� ��ų�� �ƴ�,
    // ��ǰ or ĳ������ ��ų�� ���� �������� ���� ��ų�� ��ų�� ���� ������ ��Ÿ���� ����ϴ� �Լ��Դϴ�.
    public void ClassificationAccordingToUsage(RectTransform panelPosition)
    {
        prefabPanelTag = (ClassifictationByPrefab.PrefabPanelTag)Enum.Parse(typeof(ClassifictationByPrefab.PrefabPanelTag), panelPosition.tag);
        //        Debug.Log("panelPosition.tag : " + panelPosition.tag + ", " + "prefabpanelTag : " + prefabpanelTag);

        switch (prefabPanelTag)
        {
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InLobby:
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetChild(0).GetComponent<RectTransform>();

                // ���ڷ� �޴� RectTransform�� �׻� ��ų Image�� ��ġ�� �г��̿��� �մϴ�.
                createPrefab.CreateSkillContent01(skillSlotPanel, skillData);
                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot02InLobby:             // ��ǥ������ count�� isnft ���� ������ �־�� �Ѵ�.
                panelPosition.GetChild(0).GetComponent<Image>().color = Color.yellow;

                skillSlotPanel = panelPosition.GetComponent<RectTransform>();

                // ���ڷ� �޴� RectTransform�� ��ų Image�� ��ġ�� �θ� �г��̿��� �մϴ�.
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

    // contentPrefab ����
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
