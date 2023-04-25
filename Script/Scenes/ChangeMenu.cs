using UnityEngine;
using TMPro;

using System.Collections.Generic;

using GetSetCheckForChangesInfo;

public class ChangeMenu : MonoBehaviour
{
    GetSetCheckForChanges getSetCheckForChanges;

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Stack<RectTransform> stackPanels;

    [SerializeField]
    private RectTransform menuTitlePanel;
    [SerializeField]
    private RectTransform currentPanel;

    private void Awake()
    {
        stackPanels = new Stack<RectTransform>();

        getSetCheckForChanges = new GetSetCheckForChanges();

        menuTitlePanel.gameObject.SetActive(false);

        SetTitlePanel(currentPanel.name);
    }

    public void Push(RectTransform nextPanel)
    {
        // 버튼이 클릭된 이후, 모든 raycast가 비활성화 되도록 설정. (잘못된 입력 값이 들어가기 방지)
        canvasGroup.blocksRaycasts = false;

        RectTransform prePanel = currentPanel;
        prePanel.gameObject.SetActive(false);
        stackPanels.Push(currentPanel);

        currentPanel = nextPanel;
        currentPanel.gameObject.SetActive(true);

        // 패널 변경 작업이 끝났으니, 원상복귀
        canvasGroup.blocksRaycasts = true;

        SetTitlePanel(currentPanel.name);
    }

    public void Pop()
    {
        // MainPanel에 있을 떄와 같이 현재 패널에 아무것도 없을 떄 출력된다.
        // 즉, 최초에 사용되고는 에러가 나지 않는 이상 사용되지 않아야 하는 구문이다.
        if(stackPanels.Count < 1)
        {
            return;
        }

        canvasGroup.blocksRaycasts = false;

        RectTransform prePanel = currentPanel;
        prePanel.gameObject.SetActive(false);

        currentPanel = stackPanels.Pop();
        currentPanel.gameObject.SetActive(true);

        // 옵션에서 로컬 정보가 변경될 경우, 변경에 따라서 게임 로비를 재출력 해준다.
        if (prePanel.tag.Equals("MenuOption") && getSetCheckForChanges.GetCheckForChanges())
        {
            getSetCheckForChanges.SetCheckForChanges(false);
            currentPanel.GetComponent<ButtonClickInGameLobbyMenu>().Initialize();
        }

        canvasGroup.blocksRaycasts = true;

        SetTitlePanel(currentPanel.name);
    }

    public void SetTitlePanel(string currentTitle)
    {
        // 현재 패널의 이름의 MainPanel이면 비활성화
        if (currentTitle.Replace("Panel", System.String.Empty) == "Main")
        {
            menuTitlePanel.gameObject.SetActive(false);
        }
        else
        {
            menuTitlePanel.gameObject.SetActive(true);
            menuTitlePanel.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentTitle.Replace("Panel", System.String.Empty);
        }
    }
}
