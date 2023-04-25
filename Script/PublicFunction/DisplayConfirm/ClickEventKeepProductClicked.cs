using UnityEngine.EventSystems;
using UnityEngine;

// Ŭ������ ��, Ȯ�� ��ο� ���ԵǴ� Ŭ����
public class ClickEventKeepProductClicked : MonoBehaviour, IPointerClickHandler
{
    private RectTransform productPanel;
    DisplayConfirmPanelEvent displayComfirmPanelEvent;

    private bool checkable;

    private void Awake()
    {
        productPanel = GetComponent<RectTransform>();

        displayComfirmPanelEvent = GameObject.FindWithTag("GameManager").GetComponent<DisplayConfirmPanelEvent>();
    }

    public bool Checkable
    {
        get { return checkable; }
        set { checkable = value; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        displayComfirmPanelEvent.DisplayCheckedProductPanel(productPanel);

        displayComfirmPanelEvent.ClassificationConfirmPanel(productPanel);
    }
}
