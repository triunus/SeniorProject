using UnityEngine;

using System.IO;
using Newtonsoft.Json.Linq;


public class DisplayMarketUI : MonoBehaviour
{
    CreatePrefab createPrefab;

    [SerializeField]
    private RectTransform parentOfProductsPanel;

    private void Awake()
    {
        createPrefab = GameObject.FindWithTag("GameManager").GetComponent<CreatePrefab>();

    }

    // 상품을 저장할 패널과 해당 패널 안에 들어갈 상품들을 생성한다.
    public void DisplayProducts(JArray products)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // 기존에 사용하던 값 초기화
        createPrefab.DestroyChild02(parentOfProductsPanel);

        // json 데이터의 상품 점보를 나타낼 패널들 생성.
        createPrefab.CreateProductsPanel(parentOfProductsPanel, products.Count);
        createPrefab.CreateProductSlot(parentOfProductsPanel, products, skillData);

//        Debug.Log("parentOfProductsPanel.childCount02 : " + parentOfProductsPanel.childCount);

        parentOfProductsPanel.GetChild(0).gameObject.SetActive(true);
    }
}