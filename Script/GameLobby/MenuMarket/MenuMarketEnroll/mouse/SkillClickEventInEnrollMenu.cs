using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using SkillInformaion;

// 스킬을 클릭했을 때, 우측에 출력되는 클래스
public class SkillClickEventInEnrollMenu : MonoBehaviour, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform skillImagePanel;

    private DisplayMarketEnrollUI displayMarketEnrollUI;

    private void Awake()
    {
        skillImagePanel = GetComponent<RectTransform>();

        displayMarketEnrollUI = GameObject.FindWithTag("MenuMarketEnroll").GetComponent<DisplayMarketEnrollUI>();
    }

/*    public void OnPointerEnter(PointerEventData eventData)
    {
        skillImagePanel.parent.GetComponent<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillImagePanel.parent.GetComponent<Image>().color = Color.white;
    }*/
    public void OnPointerClick(PointerEventData eventData)
    {
        displayMarketEnrollUI.DisplaySkillContent( new UserSkillDataInfo(
                skillImagePanel.name, skillImagePanel.GetChild(0).GetComponent<Image>().sprite.name.Replace("Skill", string.Empty),
                skillImagePanel.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text, skillImagePanel.GetChild(0).name
            ));
    }
}


