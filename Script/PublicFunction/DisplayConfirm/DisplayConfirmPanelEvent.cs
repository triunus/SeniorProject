using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.IO;
using Newtonsoft.Json.Linq;

using prefabTagData;

// Confirm �гΰ� �ۿ��ϴ� Ŭ�����̴�.
// �����丵 �۾� ��, Market �г��� ����, ���, ��� ��ư�� �� ���� ������ ����.
public class DisplayConfirmPanelEvent : MonoBehaviour
{
    // ���� ���� ��ǰ�� ���� ������ �߻� �� ���� �ִ�.
    private RectTransform previousProductPanel;
    private RectTransform confirmPanel;

    [SerializeField]
    private RectTransform confirmParentPanel;

    ClassifictationByPrefab.PrefabPanelTag prefabPanelTag;

    private void Awake()
    {
        previousProductPanel = GetComponent<RectTransform>();
    }

    // �Ʒ� 4���� �������� Ŭ���� �����Ͽ� �۵��ϴ� �ڵ��Դϴ�.
    public void DisplayCheckedProductPanel(RectTransform currentProductPanel)
    {

        if (previousProductPanel == null) previousProductPanel = currentProductPanel;

        //        Debug.Log("previousProductPanel : " + previousProductPanel);

        previousProductPanel.GetComponent<Image>().color = new Color(255, 255, 255, 100);

        previousProductPanel.GetComponent<ClickEventKeepProductClicked>().Checkable = false;

        currentProductPanel.GetComponent<Image>().color = Color.yellow;

        currentProductPanel.GetComponent<ClickEventKeepProductClicked>().Checkable = true;

        previousProductPanel = currentProductPanel;
    }

    // Enroll ���� ���� ��, �ǸŰ���(txt)�� ���� ������ �����Ѵ�.  ������(txt)�� �߰��� �����Ͽ� ���� �ۼ��Ͽ���.
    public void PrintClickedProductData01(string[] productData)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // confirm �г��� �̸����� ��ǰ(���) ��ȣ�� �����Ѵ�.
        confirmPanel.name = productData[0];

        // �̹��� ����
        confirmPanel.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + productData[1]);

        // ���� ����
        string content = (string)skillData[Int32.Parse(productData[1]) - 1]["SkillDamage"] + " "
            + ((string)skillData[Int32.Parse(productData[1]) - 1]["SkillDetail"]).Split('_')[0]
            + (string)skillData[Int32.Parse(productData[1]) - 1]["NumberOfTargets"]
            + ((string)skillData[Int32.Parse(productData[1]) - 1]["SkillDetail"]).Split('_')[1];

        // ��ų ���� �ؽ�Ʈ ����
        confirmPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillData[Int32.Parse(productData[1]) - 1]["SkillName"];
        confirmPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
    }

    // skill or product �������� Ŭ�� ��, �ش� ��ǰ or skill���� ���� ������ �����տ��� �����´�.
    public void AddProductPurchasePrice(string productPrice)
    {
        // ���� ����
        confirmPanel.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = productPrice;
    }

    // market enroll menu���� ��� ��ư�� Ŭ���ϸ�, �����Ḧ �������� �����´�.
    public void AddProductEnrollPrice(string productPrice)
    {
        // ���� ����
        confirmPanel.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = productPrice;
    }

    // ������ Ŭ�� ��, �۵��ϴ� �κ�.
    // ��õ� comfirmPanel�� ��� ��û�� �����Ѵ�.
    // ��ǰ��ȣ, ��ų ��ȣ, ���� ����
    public void ClassificationConfirmPanel(RectTransform productPanel)
    {
        string[] productData;

        prefabPanelTag = (ClassifictationByPrefab.PrefabPanelTag)Enum.Parse(typeof(ClassifictationByPrefab.PrefabPanelTag), productPanel.tag);
        //        Debug.Log("panelPosition.tag : " + panelPosition.tag + ", " + "prefabpanelTag : " + prefabpanelTag);

        switch (prefabPanelTag)
        {
            case ClassifictationByPrefab.PrefabPanelTag.ProductVender:
                // ��ǰ��ȣ, ��ų ��ȣ, ����
                confirmPanel = confirmParentPanel.GetChild(1).GetComponent<RectTransform>();

                productData = new string[2] { 
                    productPanel.name, productPanel.GetChild(1).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // Ȯ�� �г� ���� ���
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductMarketMain:
                // ��ǰ��ȣ, ��ų ��ȣ, ����
                confirmPanel = confirmParentPanel.GetChild(2).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(1).GetChild(0).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // Ȯ�� �г� ���� ���
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InEnroll:
                // ���� ��ȣ, ��ų ��ȣ
                confirmPanel = confirmParentPanel.GetChild(3).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty)
                };

                PrintClickedProductData01(productData);

                // Ȯ�� �г� ���� ���
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductMyTransaction:
                confirmPanel = confirmParentPanel.GetChild(4).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(1).GetChild(0).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // Ȯ�� �г� ���� ���
                AllowAccessConfirmPanel();

                break;
            default:
                Debug.Log("you have to register prefab tag in this cs file");
                break;
        }
    }


    // �Ʒ� 2���� Confirm Panel������ ��ư �̺�Ʈ�� ���� �۵� �ڵ��Դϴ�.
    // �� �Լ��� �߸� ����Ѵٸ�, Ư�� ������ Ư�� ��ų�� �����Ϸ� �Ҷ�, �ٸ� ����� ������ ���� �� �ִ�.
    public void DisplayProductConfirmPanel(bool inputValue)
    {
        // Ȯ�� �г� ���� ����. (true �� �г� ���, false�� �� ����)
        if (string.Equals(confirmParentPanel.GetChild(0).name, "false")) return;

        confirmParentPanel.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = !inputValue;

        confirmParentPanel.gameObject.SetActive(inputValue);
        confirmPanel.gameObject.SetActive(inputValue);
    }

    public void AllowAccessConfirmPanel()
    {
        confirmParentPanel.GetChild(0).name = "true";
    }

    // �ش� �޴����� �г��� ������ ���� ��, �Ǵ� �ڷΰ��� ��ư�� ���� ū �޴��� ��ȭ�� ���� ��, ������ �����ߴ� Ȯ�� �гο� ���� ������ �����Ѵ�.
    public void BlockConfirmPanel()
    {
        confirmParentPanel.GetChild(0).name = "false";
    }

    public string GetAccessRightsConfirmPanel()
    {
        return confirmParentPanel.GetChild(0).name;
    }

    public string GetSelectedProduct()
    {
        //        Debug.Log("00. BuyPanel.GetChild(0).GetChild(0).name : " + buyPanel.GetChild(0).GetChild(0).name);
        return confirmPanel.name;
    }

    // Market Enroll Menu���� Yse ��ư Ŭ�� ��, ������ �Ǹ��ϰ��� ����� ������ Ȯ�� �гο��� �����´�.
    public string GetProductSellingPrice()
    {
        return confirmPanel.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
    }

}