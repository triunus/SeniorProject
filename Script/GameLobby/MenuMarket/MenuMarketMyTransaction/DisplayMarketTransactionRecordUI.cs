using UnityEngine;

using System.IO;
using Newtonsoft.Json.Linq;


public class DisplayMarketTransactionRecordUI : MonoBehaviour
{
    CreatePrefab createPrefab;

    [SerializeField]
    private RectTransform parentOfProductsPanel;
    [SerializeField]
    private RectTransform PageNumberButtonPanel;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();
    }

    public void DisplayProductsOnSales(JArray history)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // 기존에 사용하던 값 초기화
        createPrefab.DestroyChild02(parentOfProductsPanel);
        // json 데이터의 상품 점보를 나타낼 패널들 생성.
        createPrefab.CreateProductsPanel(parentOfProductsPanel, history.Count);
        createPrefab.CreateProductOnSales(parentOfProductsPanel, history, skillData);

        //        Debug.Log("parentOfProductsPanel.childCount02 : " + parentOfProductsPanel.childCount);
        PageNumberButtonPanel.parent.GetChild(1).gameObject.SetActive(true);
        parentOfProductsPanel.GetChild(0).gameObject.SetActive(true);
    }

    public void DisplayProductsTransactionRecord(JArray history)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // 기존에 사용하던 값 초기화
        createPrefab.DestroyChild02(parentOfProductsPanel);

        // json 데이터의 상품 점보를 나타낼 패널들 생성.
        createPrefab.CreateProductsPanel(parentOfProductsPanel, history.Count);
        createPrefab.CreateProductRecordSlot(parentOfProductsPanel, history, skillData);

        //        Debug.Log("parentOfProductsPanel.childCount02 : " + parentOfProductsPanel.childCount);
        PageNumberButtonPanel.parent.GetChild(1).gameObject.SetActive(false);
        parentOfProductsPanel.GetChild(0).gameObject.SetActive(true);
    }
}