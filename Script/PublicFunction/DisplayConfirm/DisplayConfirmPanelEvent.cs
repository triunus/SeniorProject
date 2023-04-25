using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.IO;
using Newtonsoft.Json.Linq;

using prefabTagData;

// Confirm 패널과 작용하는 클래스이다.
// 리펙토링 작업 시, Market 패널의 구입, 등록, 취소 버튼을 이 곳과 연결해 주자.
public class DisplayConfirmPanelEvent : MonoBehaviour
{
    // 이전 선택 상품에 대한 오류가 발생 할 수도 있다.
    private RectTransform previousProductPanel;
    private RectTransform confirmPanel;

    [SerializeField]
    private RectTransform confirmParentPanel;

    ClassifictationByPrefab.PrefabPanelTag prefabPanelTag;

    private void Awake()
    {
        previousProductPanel = GetComponent<RectTransform>();
    }

    // 아래 4개는 프리팹의 클릭에 대응하여 작동하는 코드입니다.
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

    // Enroll 동작 수행 시, 판매가격(txt)을 받은 다음에 진행한다.  수수료(txt)를 추가로 기입하여 따로 작성하였다.
    public void PrintClickedProductData01(string[] productData)
    {
        JArray skillData = JArray.Parse(File.ReadAllText("./Assets/GameData/SkillsInfo/SkillsInfo.json"));

        // confirm 패널의 이름으로 상품(등록) 번호를 지정한다.
        confirmPanel.name = productData[0];

        // 이미지 변경
        confirmPanel.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillImage/Skill" + productData[1]);

        // 내용 설정
        string content = (string)skillData[Int32.Parse(productData[1]) - 1]["SkillDamage"] + " "
            + ((string)skillData[Int32.Parse(productData[1]) - 1]["SkillDetail"]).Split('_')[0]
            + (string)skillData[Int32.Parse(productData[1]) - 1]["NumberOfTargets"]
            + ((string)skillData[Int32.Parse(productData[1]) - 1]["SkillDetail"]).Split('_')[1];

        // 스킬 정보 텍스트 변경
        confirmPanel.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)skillData[Int32.Parse(productData[1]) - 1]["SkillName"];
        confirmPanel.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = content;
    }

    // skill or product 프리팹을 클릭 시, 해당 상품 or skill에서 대한 내용을 프리팹에서 가져온다.
    public void AddProductPurchasePrice(string productPrice)
    {
        // 가격 변경
        confirmPanel.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = productPrice;
    }

    // market enroll menu에서 등록 버튼을 클릭하면, 수수료를 서버에서 가져온다.
    public void AddProductEnrollPrice(string productPrice)
    {
        // 가격 변경
        confirmPanel.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = productPrice;
    }

    // 프리팹 클릭 시, 작동하는 부분.
    // 명시된 comfirmPanel이 모든 요청과 반응한다.
    // 상품번호, 스킬 번호, 가격 리턴
    public void ClassificationConfirmPanel(RectTransform productPanel)
    {
        string[] productData;

        prefabPanelTag = (ClassifictationByPrefab.PrefabPanelTag)Enum.Parse(typeof(ClassifictationByPrefab.PrefabPanelTag), productPanel.tag);
        //        Debug.Log("panelPosition.tag : " + panelPosition.tag + ", " + "prefabpanelTag : " + prefabpanelTag);

        switch (prefabPanelTag)
        {
            case ClassifictationByPrefab.PrefabPanelTag.ProductVender:
                // 상품번호, 스킬 번호, 가격
                confirmPanel = confirmParentPanel.GetChild(1).GetComponent<RectTransform>();

                productData = new string[2] { 
                    productPanel.name, productPanel.GetChild(1).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // 확인 패널 접근 허용
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductMarketMain:
                // 상품번호, 스킬 번호, 가격
                confirmPanel = confirmParentPanel.GetChild(2).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(1).GetChild(0).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // 확인 패널 접근 허용
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.SkillSlot01InEnroll:
                // 고유 번호, 스킬 번호
                confirmPanel = confirmParentPanel.GetChild(3).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", String.Empty)
                };

                PrintClickedProductData01(productData);

                // 확인 패널 접근 허용
                AllowAccessConfirmPanel();

                break;
            case ClassifictationByPrefab.PrefabPanelTag.ProductMyTransaction:
                confirmPanel = confirmParentPanel.GetChild(4).GetComponent<RectTransform>();

                productData = new string[2] {
                    productPanel.name, productPanel.GetChild(1).GetChild(0).name
                };

                PrintClickedProductData01(productData);
                AddProductPurchasePrice(productPanel.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
                // 확인 패널 접근 허용
                AllowAccessConfirmPanel();

                break;
            default:
                Debug.Log("you have to register prefab tag in this cs file");
                break;
        }
    }


    // 아래 2개는 Confirm Panel에서의 버튼 이벤트에 대한 작동 코드입니다.
    // 이 함수는 잘만 사용한다면, 특정 유저가 특정 스킬을 구입하려 할때, 다른 사람의 접근을 막을 수 있다.
    public void DisplayProductConfirmPanel(bool inputValue)
    {
        // 확인 패널 접근 통제. (true 면 패널 출력, false면 무 반응)
        if (string.Equals(confirmParentPanel.GetChild(0).name, "false")) return;

        confirmParentPanel.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = !inputValue;

        confirmParentPanel.gameObject.SetActive(inputValue);
        confirmPanel.gameObject.SetActive(inputValue);
    }

    public void AllowAccessConfirmPanel()
    {
        confirmParentPanel.GetChild(0).name = "true";
    }

    // 해당 메뉴에서 패널의 변경이 있을 때, 또는 뒤로가기 버튼과 같이 큰 메뉴의 변화가 생길 때, 기존에 존재했던 확인 패널에 대한 접근을 통제한다.
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

    // Market Enroll Menu에서 Yse 버튼 클릭 시, 유저가 판매하고자 명시한 가격을 확인 패널에서 가져온다.
    public string GetProductSellingPrice()
    {
        return confirmPanel.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
    }

}