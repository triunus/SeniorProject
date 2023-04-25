using System;

using UnityEngine;
using TMPro;

public class DisplayDirectionAndPageNumberUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentOfProductsPanel;
    [SerializeField]
    private RectTransform PageNumberButtonPanel;

    public void DisplayDirectionButton(int pageNumber)
    {
        PageNumberButtonPanel.GetChild(0).GetChild(0).gameObject.SetActive(false);
        PageNumberButtonPanel.GetChild(0).GetChild(1).gameObject.SetActive(false);
        PageNumberButtonPanel.GetChild(2).GetChild(0).gameObject.SetActive(false);
        PageNumberButtonPanel.GetChild(2).GetChild(1).gameObject.SetActive(false);

        if (pageNumber / 10 != 0)
        {
            PageNumberButtonPanel.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }

        if (pageNumber != 0)
        {
            PageNumberButtonPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }

        if (parentOfProductsPanel.childCount == 10 && parentOfProductsPanel.GetChild(9).childCount == 5)
        {
            PageNumberButtonPanel.GetChild(2).GetChild(0).gameObject.SetActive(true);
            PageNumberButtonPanel.GetChild(2).GetChild(1).gameObject.SetActive(true);
        }

        if (pageNumber % 10 + 1 < parentOfProductsPanel.childCount)
        {
            PageNumberButtonPanel.GetChild(2).GetChild(1).gameObject.SetActive(true);
        }
    }

    // 직전에 생성한 Products 패널들의 개수를 사용하여, 아래에 생성 할 버튼의 개수를 지정하는 함수.
    public void DisplayPageNumberButton(int filter)
    {
        // 부모 버튼 패널에서 [1]에 존재하는 숫자 버튼 패널을 전부 비활성화 한다.
        for (int i = 0; i < PageNumberButtonPanel.GetChild(1).childCount; i++)
        {
            PageNumberButtonPanel.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }

        // 이후, 상품의 개수를 기준으로 활성화 시킬 버튼을 정한다.
        // 버튼의 text도 수정해 준다.
        for (int i = 0; i < parentOfProductsPanel.childCount; i++)
        {
            PageNumberButtonPanel.GetChild(1).GetChild(i).gameObject.SetActive(true);
            PageNumberButtonPanel.GetChild(1).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(filter + i);
        }
    }

    // 버튼 패널 중, 숫자를 클릭하였을 때 일어나는 패널 변경을 정의한 함수.
    // 버튼을 클릭하였을 때, 버튼에 정의된 숫자 값을 어떻게 가져올지...button.text를 미리 가져온 후, pageNumber만 가져온다.
    public void DisplayAnotherPageNumber(string prePageNumber, string nextPageNumber)
    {
        parentOfProductsPanel.GetChild(Int32.Parse(prePageNumber) % 10).gameObject.SetActive(false);

        parentOfProductsPanel.GetChild(Int32.Parse(nextPageNumber) % 10).gameObject.SetActive(true);
    }
}
