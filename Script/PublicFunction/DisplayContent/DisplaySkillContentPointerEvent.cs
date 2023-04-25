using UnityEngine;
using UnityEngine.EventSystems;

// 해당 클래스가 Image 컴포넌트가 위치하는 RectTransform에 존재하는 이유는 마우스의 이벤트가 해당 클래스가 위치한 객체에서 작동하기 때문이다.
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

