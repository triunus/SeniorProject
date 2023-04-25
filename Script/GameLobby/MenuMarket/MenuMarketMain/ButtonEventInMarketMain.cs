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

    // 아래 2개는 좌측 Panel에서 발생하는 Rank Filter와 관련된 함수들이다.
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

        /*if(searchWord.text.Length < 3)  // 없어도 됨.
        {
            // 좀 더 긴 문자 입력해 달라고 부탁.
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


    // 아래 5개는 하단의 PageNumber Filter와 관련 함수들이다.
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



    // 마켓과 같이 인증이 필요한 기능을 사용할 때, 접근 가능 여부를 판단한다.
    public bool DetermineAccessibility()
    {
        string userDataInJson = getSetAccountData.GetAccountDataInJsonType();
        JObject success = JObject.Parse(requestUserData.RequestMarketAccessibility_test(userDataInJson));

        Debug.Log(success["success"]);

        return (bool)success["success"];
    }


    // 아래 ''개는 BytButton을 클릭했을 떄 발생하는 함수들이다.
    public void OnClickedBuyButton()
    {
        // true면 실행.
        if (DetermineAccessibility())
        {
            displayConfirmPanelEvent.DisplayProductConfirmPanel(true);
        }
        else
        {
            // 인증해야 된다고 메시지 출력.
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
