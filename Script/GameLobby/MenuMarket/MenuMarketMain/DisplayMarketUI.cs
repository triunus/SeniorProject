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

    // ��ǰ�� ������ �гΰ� �ش� �г� �ȿ� �� ��ǰ���� �����Ѵ�.
    public void DisplayProducts(JArray products)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // ������ ����ϴ� �� �ʱ�ȭ
        createPrefab.DestroyChild02(parentOfProductsPanel);

        // json �������� ��ǰ ������ ��Ÿ�� �гε� ����.
        createPrefab.CreateProductsPanel(parentOfProductsPanel, products.Count);
        createPrefab.CreateProductSlot(parentOfProductsPanel, products, skillData);

//        Debug.Log("parentOfProductsPanel.childCount02 : " + parentOfProductsPanel.childCount);

        parentOfProductsPanel.GetChild(0).gameObject.SetActive(true);
    }
}