using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System.IO;
using Newtonsoft.Json.Linq;

// ���Ŀ� ���� �κ�.   ==> DisplaySkillContent01�� ��� ����� �����̴�.
// RectTransform�� ��� �����ִ�. �� ���� �𸣰��� �ФФ�        <== ������ ������������
// �Ƹ� ���� ������ RectTransform�� �����ؼ� �ΰ� ����.         <=== GameObject�� new�� �����ϸ� �߻�.

public class DisplaySkillContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform skillImage;
    GameObject gameManager;

    private void Awake()
    {
        skillImage = GetComponent<RectTransform>();

        gameManager = GameObject.FindWithTag("GameManager");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillImage.parent.GetComponent<Image>().color = Color.yellow;

        DisplayContent();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillImage.parent.GetComponent<Image>().color = Color.white;

        DestroySkillContent();
    }

    public void DisplayContent()
    {
        JArray jArray = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
        //        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));
        //Debug.Log(dataTable.Rows[0][0] + ", " + dataTable.Rows[0][1] + ", " + dataTable.Rows[0][2] + ", " + dataTable.Rows[0][3]);
        gameManager.GetComponent<CreatePrefab>().CreateSkillContent(skillImage, skillImage.name.Split('_'), jArray);
    }

    public void DestroySkillContent()
    {
        gameManager.GetComponent<CreatePrefab>().DestroyPartPanel("SkillContent");
    }
}