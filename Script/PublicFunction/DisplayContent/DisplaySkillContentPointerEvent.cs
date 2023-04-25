using UnityEngine;
using UnityEngine.EventSystems;

// �ش� Ŭ������ Image ������Ʈ�� ��ġ�ϴ� RectTransform�� �����ϴ� ������ ���콺�� �̺�Ʈ�� �ش� Ŭ������ ��ġ�� ��ü���� �۵��ϱ� �����̴�.
public class DisplaySkillContentPointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform currentPanel;
    DisplaySkillContent01 displaySkillContent;

    private void Awake()
    {
        currentPanel = GetComponent<RectTransform>();

        displaySkillContent = GameObject.FindWithTag("GameManager").GetComponent<DisplaySkillContent01>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        displaySkillContent.DestroyConstent(currentPanel.parent.GetComponent<RectTransform>());

        displaySkillContent.ClassificationAccordingToUsage(currentPanel.parent.GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displaySkillContent.DestroyConstent(currentPanel.parent.GetComponent<RectTransform>());
    }


}

