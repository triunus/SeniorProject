using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �ش� Ŭ������ �κ��丮 �� ��ų �����ܰ��� ��ȣ�ۿ��� �����ش�.
// ��ȣ�ۿ뿡 ���� ���� ���� ���� Ŭ�������� Ȯ�ΰ����ϴ�.
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

    // ������ ��ų�� ������ 6�� �̻��̸� �׳� ����.
    if (selectedSkillListPanel.childCount > 5) return;

    createPrefab.CreateSkillSlot02InInventory(selectedSkillListPanel, skillImagePanel.name.Split('_'));

    skillImagePanel.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
    Checkable = false;
}

if (skillImagePanel.parent.parent.CompareTag("SelectedSkillListPanel"))
{
    Destroy(skillImagePanel.parent.gameObject);
    createPrefab.DestroyPartPanel("SkillContent");

    // �������� ������ ��ų�� �̸��� ������ ��ų �̸��� ���� �гο��� ã�´�.
    RectTransform selectedskill = GameObject.Find(skillImagePanel.name).GetComponent<RectTransform>();

    selectedskill.GetComponent<Image>().color = Color.white;

    selectedskill.GetComponent<SkillClickEvent>().Checkable = true;
}*/