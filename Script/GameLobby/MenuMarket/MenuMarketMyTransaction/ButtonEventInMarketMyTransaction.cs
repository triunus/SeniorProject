using GetSetRequestRecordInfo;
using GetSetAccountInfo;
using ConnectServer;

using UnityEngine;
using TMPro;

using System;
using Newtonsoft.Json.Linq;

public class ButtonEventInMarketMyTransaction : MonoBehaviour
{
    SetRequestRecordData setRequestRecordData;
    GetRequestRecordData getRequestRecordData;

    GetSetAccountData getSetAccountData;
    RequestUserData requestUserData;

    DisplayMarketTransactionRecordUI displayMarketTransactionRecordUI;
    DisplayDirectionAndPageNumberUI displayDirectionAndPageNumber;
    DisplayConfirmPanelEvent displayConfirmPanelEvent;
    DisplayMessageGameLobby displayMessage;

    private void Awake()
    {
        setRequestRecordData = new SetRequestRecordData();
        getRequestRecordData = new GetRequestRecordData();

        getSetAccountData = new GetSetAccountData();
        requestUserData = new RequestUserData();

        displayMarketTransactionRecordUI = GetComponent<DisplayMarketTransactionRecordUI>();
        displayDirectionAndPageNumber = GetComponent<DisplayDirectionAndPageNumberUI>();
        displayConfirmPanelEvent = GameObject.FindWithTag("GameManager").GetComponent<DisplayConfirmPanelEvent>();
        displayMessage = GameObject.FindWithTag("GameManager").GetComponent<DisplayMessageGameLobby>();
    }

    public void InitializeRecord()
    {
        setRequestRecordData.SetFilterData(new string[] { "0", "onSale" });

        string[] userData = getSetAccountData.GetAccountData();
        string[] recordData = getRequestRecordData.GetFilterData();

        JArray record = JArray.Parse(requestUserData.RequestTransactionHistorysInfo(new string[] { userData[0], userData[1], recordData[0], recordData[1] }));

        if (record.Count == 0) return;

        displayMarketTransactionRecordUI.DisplayProductsOnSales(record);

        displayConfirmPanelEvent.BlockConfirmPanel();

        displayDirectionAndPageNumber.DisplayDirectionButton(System.Int32.Parse(recordData[0]));
        displayDirectionAndPageNumber.DisplayPageNumberButton(System.Int32.Parse(recordData[0]));
    }

    public void DisplayAntherRecord(TextMeshProUGUI category)
    {
        Debug.Log("category.text : " + category.text.Replace(" ", System.String.Empty));
        setRequestRecordData.SetFilterData(new string[] { "0", category.text.Replace(" ", System.String.Empty) });

        string[] userData = getSetAccountData.GetAccountData();
        string[] recordData = getRequestRecordData.GetFilterData();

        JArray record = JArray.Parse(requestUserData.RequestTransactionHistorysInfo(new string[] { userData[0], userData[1], recordData[0], recordData[1] }));

        if (record.Count == 0) return;

        if (recordData[1] == "onSale") {
            displayMarketTransactionRecordUI.DisplayProductsOnSales(record);
            displayConfirmPanelEvent.BlockConfirmPanel();
        }
        else { displayMarketTransactionRecordUI.DisplayProductsTransactionRecord(record); }

        displayDirectionAndPageNumber.DisplayDirectionButton(System.Int32.Parse(recordData[0]));
        displayDirectionAndPageNumber.DisplayPageNumberButton(System.Int32.Parse(recordData[0]));
    }


    // 공통 함수들 나중에 묶어서 클래스로 빼자.

    public void NNextButtonEvent()
    {
        string[] preFilter = getRequestRecordData.GetFilterData();

        setRequestRecordData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) / 10 * 10 + 10), preFilter[1] });

        string[] nextFilter = getRequestRecordData.GetFilterData();

        JArray record = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (record.Count == 0) return;

        if (nextFilter[1].Equals("onSale")) { 
            displayMarketTransactionRecordUI.DisplayProductsOnSales(record);
            displayConfirmPanelEvent.BlockConfirmPanel();
        }
        else { displayMarketTransactionRecordUI.DisplayProductsTransactionRecord(record); }

        displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
    }

    public void BBackButtonEvent()
    {
        string[] preFilter = getRequestRecordData.GetFilterData();

        setRequestRecordData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) / 10 * 10 - 10), preFilter[1] });

        string[] nextFilter = getRequestRecordData.GetFilterData();

        JArray record = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

        if (record.Count == 0) return;

        if (nextFilter[1].Equals("onSale")) { 
            displayMarketTransactionRecordUI.DisplayProductsOnSales(record);
            displayConfirmPanelEvent.BlockConfirmPanel();
        }
        else { displayMarketTransactionRecordUI.DisplayProductsTransactionRecord(record); }

        displayDirectionAndPageNumber.DisplayDirectionButton(Int32.Parse(nextFilter[0]));
    }

    public void NextButtonEvent()
    {
        string[] preFilter = getRequestRecordData.GetFilterData();

        setRequestRecordData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) + 1), preFilter[1] });

        string[] nextFilter = getRequestRecordData.GetFilterData();

        if ((Int32.Parse(preFilter[0])) / 10 != (Int32.Parse(nextFilter[0])) / 10)
        {
            JArray record = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

            if (record.Count == 0) return;

            if (nextFilter[1].Equals("onSale")) { 
                displayMarketTransactionRecordUI.DisplayProductsOnSales(record);
                displayConfirmPanelEvent.BlockConfirmPanel();
            }
            else { displayMarketTransactionRecordUI.DisplayProductsTransactionRecord(record); }

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
        string[] preFilter = getRequestRecordData.GetFilterData();

        setRequestRecordData.SetFilterData(new string[] { Convert.ToString(Int32.Parse(preFilter[0]) - 1), preFilter[1] });

        string[] nextFilter = getRequestRecordData.GetFilterData();

        if ((Int32.Parse(preFilter[0])) / 10 != (Int32.Parse(nextFilter[0])) / 10)
        {
            JArray record = JArray.Parse(requestUserData.RequestProductsInfo(nextFilter));

            if (record.Count == 0) return;

            if (nextFilter[1].Equals("onSale")) { 
                displayMarketTransactionRecordUI.DisplayProductsOnSales(record);
                displayConfirmPanelEvent.BlockConfirmPanel();
            }
            else { displayMarketTransactionRecordUI.DisplayProductsTransactionRecord(record); }

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
        string[] preFilter = getRequestRecordData.GetFilterData();

        setRequestRecordData.SetFilterData(new string[] { pageNumber.text, preFilter[1] });

        string[] nextFilter = getRequestRecordData.GetFilterData();

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

    // 버튼 클릭
    public void OnClickedCancelButton()
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

    public void OnClickedCancelYesButton()
    {
        string[] userData = getSetAccountData.GetAccountData();
        string selectedProductData = displayConfirmPanelEvent.GetSelectedProduct();

        string success = requestUserData.RequestTransactionCancel(new string[] { userData[0], userData[1], selectedProductData });

        if (success == "false")
        {
            Debug.Log("error");
        }

        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);

        InitializeRecord();
    }

    public void OnClickedCancelNoButton()
    {
        displayConfirmPanelEvent.DisplayProductConfirmPanel(false);
    }
}
