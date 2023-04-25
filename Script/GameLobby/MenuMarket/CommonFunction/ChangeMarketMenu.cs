using UnityEngine;

public class ChangeMarketMenu : MonoBehaviour
{
    RectTransform marketMenuPanel;

    private void Awake()
    {
        marketMenuPanel = GetComponent<RectTransform>();
    }

    // 아래는 main, enroll, mytransaction으로 이동하는 버튼 클릭 함수
    public void OnClickedChangeMenuButton(int childNumber)
    {
        marketMenuPanel.GetChild(0).gameObject.SetActive(true);
        marketMenuPanel.GetChild(1).gameObject.SetActive(false);
        marketMenuPanel.GetChild(2).gameObject.SetActive(false);
        marketMenuPanel.GetChild(3).gameObject.SetActive(false);

        marketMenuPanel.GetChild(childNumber).gameObject.SetActive(true);
    }
}
