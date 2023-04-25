using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System.IO;
using Newtonsoft.Json.Linq;

// 차후에 정리 부분.   ==> DisplaySkillContent01을 대신 사용할 예정이다.
// RectTransform이 계속 남아있다. 왜 인지 모르겠음 ㅠㅠㅠ        <== 정리함 ㅎㅎㅎㅎㅎㅎ
// 아마 전역 변수로 RectTransform을 설정해서 인거 같음.         <=== GameObject를 new로 생성하면 발생.

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