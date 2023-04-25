using GetSetSearchFilterInfo;
using GetSetAccountInfo;
using ConnectServer;

using UnityEngine;
using TMPro;

using System;
using Newtonsoft.Json.Linq;

public class ButtonEventInMarketMain : MonoBehaviour
{
    SetSearchFilterData setSearchFilterData;
    GetSearchFilterData getSearchFilterData;

    GetSetAccountData getSetAccountData;
    RequestUserData requestUserData;

    DisplayMarketUI displayMarketUI;
    DisplayDirectionAndPageNumberUI displayDirectionAndPageNumber;
    DisplayConfirmPanelEvent displayConfirmPanelEvent;
    DisplayMessageGameLobby displayMessage;

    private void Awake()
    {
        setSearchFilterData = new SetSearchFilterData();
        getSearchFilterData = new GetSearchFilterData();

        getSetAccountData = new GetSetAccountData();
        requestUserData = new RequestUserData();

        displayMarketUI = GetComponent<DisplayMarketUI>();
        displayDirectionAndPageNumber = GetComponent<DisplayDirectionAndPageNumberUI>();
        displayConfirmPanelEvent = GameObject.FindWithTag("GameManager").GetComponent<DisplayConfirmPanelEvent>();
        displayMessage = GameObject.FindWithTag("GameManager").GetComponent<DisplayMessageGameLobby>();
    }

    // �Ʒ� 2���� ���� Panel���� �߻��ϴ� Rank Filter�� ���õ� �Լ����̴�.
    public void InitializeLIst()
    {
        setSearchFilterData.SetFilterData(new string[] { "0", "", "" });

        string[] filter = getSearchFilterData.GetFilterData();

        JArray products = JArray.Parse(requestUserData.RequestProductsInfo(filter));

        if (products.Count == 0) return;

        displayMarketUI.DisplayProducts(products);

        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(System.Int32.Parse(filter[0]));
        displayDirectionAndPageNumber.DisplayPageNumberButton(System.Int32.Parse(filter[0]));
    }

    public void FilteringByRank(TextMeshProUGUI rank)
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        string rankText = rank.text;
        if (rank.text == "Total") rankText = "";

        Debug.Log("rank.text : " + rankText);

        setSearchFilterData.SetFilterData(new string[] { "0", preFilter[1], rankText });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (products.Count == 0) return;

        displayMarketUI.DisplayProducts(products);

        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(System.Int32.Parse(nextFilter[0]));
        displayDirectionAndPageNumber.DisplayPageNumberButton(System.Int32.Parse(nextFilter[0]));
    }

    public void FilteringByString(TextMeshProUGUI searchWord)
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        /*if(searchWord.text.Length < 3)  // ��� ��.
        {
            // �� �� �� ���� �Է��� �޶�� ��Ź.
            return;
        }*/

        setSearchFilterData.SetFilterData(new string[] { "0", searchWord.text, preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (products.Count == 0) return;

        displayMarketUI.DisplayProducts(products);

        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(System.Int32.Parse(nextFilter[0]));
        displayDirectionAndPageNumber.DisplayPageNumberButton(System.Int32.Parse(nextFilter[0]));
    }


    // �Ʒ� 5���� �ϴ��� PageNumber Filter�� ���� �Լ����̴�.
    public void NNextButtonEvent()
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        setSearchFilterData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) / 10 * 10 + 10), preFilter[1], preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (products.Count == 0) return;

        displayMarketUI.DisplayProducts(products);
        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
    }

    public void BBackButtonEvent()
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        setSearchFilterData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) / 10 * 10 - 10), preFilter[1], preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (products.Count == 0) return;

        displayMarketUI.DisplayProducts(products);
        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
    }

    public void NextButtonEvent()
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        setSearchFilterData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) + 1), preFilter[1], preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        if ((Int32.Parse(preFilter[0])) / 10 != (Int32.Parse(nextFilter[0])) / 10)
        {
            JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

            if (products.Count == 0) return;

            displayMarketUI.DisplayProducts(products);
            displayConfirmPanelEvent.BlockConfirmPanel();

            displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
        }
        else
        {
            displayDirectionAndPageNumber.DisplayAnotherPageNumber(preFilter[0], nextFilter[0]);

            displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
        }
    }

    public void BackButtonEvent()
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        setSearchFilterData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) - 1), preFilter[1], preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        if ((Int32.Parse(preFilter[0])) / 10 != (Int32.Parse(nextFilter[0])) / 10)
        {
            JArray products = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

            if (products.Count == 0) return;

            displayMarketUI.DisplayProducts(products);
            displayConfirmPanelEvent.BlockConfirmPanel();

            displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
        }
        else
        {
            displayDirectionAndPageNumber.DisplayAnotherPageNumber(preFilter[0], nextFilter[0]);

            displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
        }
    }

    public void ChangePageNumber(TextMeshProUGUI pageNumber)
    {
        string[] preFilter = getSearchFilterData.GetFilterData();

        setSearchFilterData.SetFilterData(new string[] { pageNumber.text, preFilter[1], preFilter[2] });

        string[] nextFilter = getSearchFilterData.GetFilterData();

        displayDirectionAndPageNumber.DisplayAnotherPageNumber(preFilter[0], nextFilter[0]);

        displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
    }



    // ���ϰ� ���� ������ �ʿ��� ����� ����� ��, ���� ���� ���θ� �Ǵ��Ѵ�.
    public bool DetermineAccessibility()
    {
        string userDataInJson = getSetAccountData.GetAccountDataInJsonType();
        JObject success = JObject.Parse(requestUserData.RequestMarketAccessibility_test(userDataInJson));

        Debug.Log(success["success"]);

        return (bool)success["success"];
    }


    // �Ʒ� ''���� BytButton�� Ŭ������ �� �߻��ϴ� �Լ����̴�.
    public void OnClickedBuyButton()
    {
        // true�� ����.
        if (DetermineAccessibility())
        {
            displayConfirmPanelEvent.DisplayProductConfirmPanel(true);
        }
        else
        {
            // �����ؾ� �ȴٰ� �޽��� ���.
            displayMessage.DisplayMessagePanel(21);
        }
    }

    public void OnClickedBuyYesButton()
    {
        string[] userData = getSetAccountData.GetAccountData();
        string selectedProductData = displayConfirmPanelEvent.GetSelectedProduct();

        string success = requestUserData.RequestProductBuy(new string[] { userData[0], userData[1], selectedProductData });

        if (success == "false")
        {
            Debug.Log("error");
        }

        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);

        InitializeLIst();
    }

    public void OnClickedBuyNoButton()
    {
        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);
    }
}
