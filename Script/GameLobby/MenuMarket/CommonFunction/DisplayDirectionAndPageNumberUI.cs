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

    // ������ ������ Products �гε��� ������ ����Ͽ�, �Ʒ��� ���� �� ��ư�� ������ �����ϴ� �Լ�.
    public void DisplayPageNumberButton(int filter)
    {
        // �θ� ��ư �гο��� [1]�� �����ϴ� ���� ��ư �г��� ���� ��Ȱ��ȭ �Ѵ�.
        for (int i = 0; i < PageNumberButtonPanel.GetChild(1).childCount; i++)
        {
            PageNumberButtonPanel.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }

        // ����, ��ǰ�� ������ �������� Ȱ��ȭ ��ų ��ư�� ���Ѵ�.
        // ��ư�� text�� ������ �ش�.
        for (int i = 0; i < parentOfProductsPanel.childCount; i++)
        {
            PageNumberButtonPanel.GetChild(1).GetChild(i).gameObject.SetActive(true);
            PageNumberButtonPanel.GetChild(1).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(filter + i);
        }
    }

    // ��ư �г� ��, ���ڸ� Ŭ���Ͽ��� �� �Ͼ�� �г� ������ ������ �Լ�.
    // ��ư�� Ŭ���Ͽ��� ��, ��ư�� ���ǵ� ���� ���� ��� ��������...button.text�� �̸� ������ ��, pageNumber�� �����´�.
    public void DisplayAnotherPageNumber(string prePageNumber, string nextPageNumber)
    {
        parentOfProductsPanel.GetChild(Int32.Parse(prePageNumber) % 10).gameObject.SetActive(false);

        parentOfProductsPanel.GetChild(Int32.Parse(nextPageNumber) % 10).gameObject.SetActive(true);
    }
}
