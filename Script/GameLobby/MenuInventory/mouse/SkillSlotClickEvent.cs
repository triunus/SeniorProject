using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 해당 클래스는 인벤토리 내 스킬 아이콘과의 상호작용을 보여준다.
// 상호작용에 대한 상세한 명세는 관련 클래스에서 확인가능하다.
public class SkillSlotClickEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DisplayInventoryMenuUI displayInventoryMenuUI;

    private RectTransform skillSlotPanel;

    private bool checkable;


    private void Awake()
    {
        checkable = true;

        skillSlotPanel = GetComponent<RectTransform>();

        displayInventoryMenuUI = GameObject.FindWithTag("MenuInventory").GetComponent<DisplayInventoryMenuUI>();
    }

    public bool Checkable
    {
        get { return checkable; }
        set { checkable = value; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (checkable)  skillSlotPanel.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (checkable)  skillSlotPanel.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        displayInventoryMenuUI.ClickEventSkillSlot(skillSlotPanel);
    }
}


/*if (skillImagePanel.parent.parent.parent.CompareTag("SkillListPanel") && checkable)
{
    RectTransform selectedSkillListPanel = GameObject.FindWithTag("SelectedSkillListPanel").GetComponent<RectTransform>();

    // 선택한 스킬의 개수가 6개 이상이면 그냥 리턴.
    if (selectedSkillListPanel.childCount > 5) return;

    createPrefab.CreateSkillSlot02InInventory(selectedSkillListPanel, skillImagePanel.name.Split('_'));

    skillImagePanel.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
    Checkable = false;
}

if (skillImagePanel.parent.parent.CompareTag("SelectedSkillListPanel"))
{
    Destroy(skillImagePanel.parent.gameObject);
    createPrefab.DestroyPartPanel("SkillContent");

    // 우측에서 선택한 스킬의 이름과 동일한 스킬 이름을 좌측 패널에서 찾는다.
    RectTransform selectedskill = GameObject.Find(skillImagePanel.name).GetComponent<RectTransform>();

    selectedskill.GetComponent<Image>().color = Color.white;

    selectedskill.GetComponent<SkillClickEvent>().Checkable = true;
}*/