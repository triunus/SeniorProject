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
        // ��ư�� Ŭ���� ����, ��� raycast�� ��Ȱ��ȭ �ǵ��� ����. (�߸��� �Է� ���� ���� ����)
        canvasGroup.blocksRaycasts = false;

        RectTransform prePanel = currentPanel;
        prePanel.gameObject.SetActive(false);
        stackPanels.Push(currentPanel);

        currentPanel = nextPanel;
        currentPanel.gameObject.SetActive(true);

        // �г� ���� �۾��� ��������, ���󺹱�
        canvasGroup.blocksRaycasts = true;

        SetTitlePanel(currentPanel.name);
    }

    public void Pop()
    {
        // MainPanel�� ���� ���� ���� ���� �гο� �ƹ��͵� ���� �� ��µȴ�.
        // ��, ���ʿ� ���ǰ�� ������ ���� �ʴ� �̻� ������ �ʾƾ� �ϴ� �����̴�.
        if(stackPanels.Count < 1)
        {
            return;
        }

        canvasGroup.blocksRaycasts = false;

        RectTransform prePanel = currentPanel;
        prePanel.gameObject.SetActive(false);

        currentPanel = stackPanels.Pop();
        currentPanel.gameObject.SetActive(true);

        // �ɼǿ��� ���� ������ ����� ���, ���濡 ���� ���� �κ� ����� ���ش�.
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
        // ���� �г��� �̸��� MainPanel�̸� ��Ȱ��ȭ
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
