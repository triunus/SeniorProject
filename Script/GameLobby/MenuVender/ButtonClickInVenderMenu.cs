using GetSetAccountInfo;
using ConnectServer;

using UnityEngine;

public class ButtonClickInVenderMenu : MonoBehaviour
{
    GetSetAccountData getSetAccountData;
    RequestUserData requestUserData;

    DisplayVenderMenuUI displayVenderMenuUI;
    DisplayConfirmPanelEvent displayConfirmPanelEvent;

    RectTransform venderPanel;

    private void Awake()
    {
        getSetAccountData = new GetSetAccountData();
        requestUserData = new RequestUserData();

        displayVenderMenuUI = GetComponent<DisplayVenderMenuUI>();
        displayConfirmPanelEvent = GameObject.FindWithTag("GameManager").GetComponent<DisplayConfirmPanelEvent>();

        venderPanel = GetComponent<RectTransform>();
    }

    // ���� ���� �Լ�
    public void Initialize()
    {
        string[] userData = getSetAccountData.GetAccountData();

        string productsStinrg = requestUserData.RequestVenderProductInfo(new string[] { userData[0], userData[1], "select" });

        displayVenderMenuUI.DisplayProductsList(productsStinrg);

        displayConfirmPanelEvent.BlockConfirmPanel();
    }

    // Vender Panel �ȿ��� �޴� ����
    public void OnClickedChangeMenuButton(int childNumber)
    {
        venderPanel.GetChild(0).gameObject.SetActive(true);
        venderPanel.GetChild(1).gameObject.SetActive(false);
        venderPanel.GetChild(2).gameObject.SetActive(false);

        venderPanel.GetChild(childNumber).gameObject.SetActive(true);
    }

    // ���� ��ư
    public void VenderProductsUpdate()
    {
        string[] userData = getSetAccountData.GetAccountData();

        string productsStinrg = requestUserData.RequestVenderProductInfo(new string[] { userData[0], userData[1], "update" });

        displayVenderMenuUI.DisplayProductsList(productsStinrg);

        displayConfirmPanelEvent.BlockConfirmPanel();
    }

    // �Ʒ� ''���� BytButton�� Ŭ������ �� �߻��ϴ� �Լ����̴�.
    public void OnClickedBuyButton()
    {
        displayConfirmPanelEvent.DisplayProductConfirmPanel(true);
    }

    public void OnClickedBuyYesButton()
    {
        string[] userData = getSetAccountData.GetAccountData();
        string selectedProductData = displayConfirmPanelEvent.GetSelectedProduct();

        string success = requestUserData.RequestVenderProductBuy(new string[] { userData[0], userData[1], selectedProductData });

        if (success == "false")
        {
            Debug.Log("error");
        }

        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);

        Initialize();
    }

    public void OnClickedBuyNoButton()
    {
        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);
    }
}
