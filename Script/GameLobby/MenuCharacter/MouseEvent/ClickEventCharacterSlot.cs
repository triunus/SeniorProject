using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickEventCharacterSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    DisplayCharacterMenuUI displayCharacterMenuUI;

    private RectTransform characterSlot;

    private bool checkable;

    private void Awake()
    {
        characterSlot = GetComponent<RectTransform>();

        displayCharacterMenuUI = GameObject.FindWithTag("MenuCharacter").GetComponent<DisplayCharacterMenuUI>();
    }

    public bool Checkable
    {
        get { return checkable; }
        set { checkable = value; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        characterSlot.gameObject.GetComponent<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        characterSlot.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        displayCharacterMenuUI.DisplayCheckedCharacterSlot(characterSlot);
        // 캐릭터 이미지의 번호를 넘김.
        displayCharacterMenuUI.DisplaySelectedCharacterUI(
            characterSlot.GetChild(0).gameObject.GetComponent<Image>().sprite.name.Replace("Character", System.String.Empty)
        );
    }
}
