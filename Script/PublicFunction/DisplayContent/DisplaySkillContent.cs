using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System.IO;
using Newtonsoft.Json.Linq;

// 託板拭 舛軒 採歳.   ==> DisplaySkillContent01聖 企重 紫遂拝 森舛戚陥.
// RectTransform戚 域紗 害焼赤陥. 訊 昔走 乞牽畏製 ばばば        <== 舛軒敗 ぞぞぞぞぞぞ
// 焼原 穿蝕 痕呪稽 RectTransform聖 竺舛背辞 昔暗 旭製.         <=== GameObject研 new稽 持失馬檎 降持.

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