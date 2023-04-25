using UnityEngine;
using TMPro;

public class ViewModifyNickname : MonoBehaviour
{
    [SerializeField]
    private RectTransform nicknamePanel;
    [SerializeField]
    private TMP_InputField nicknameText;

    public void ViewNicknameInputPanel()
    {
        nicknamePanel.parent.gameObject.SetActive(true);
        nicknamePanel.gameObject.SetActive(true);
        nicknameText.text = null;
    }

    public void CloseNicknameInputPanel()
    {
        nicknamePanel.parent.gameObject.SetActive(false);
        nicknamePanel.gameObject.SetActive(false);
    }
}
