using UnityEngine;
using TMPro;

public class ViewAccountLinkingPanel : MonoBehaviour
{

    [SerializeField]
    private RectTransform phoneNumberInputPanel;
    [SerializeField]
    private RectTransform verificationCodeInputPanel;
    [SerializeField]
    private RectTransform walletAddressInputPanel;
    [SerializeField]
    private TMP_InputField phoneNumberText;
    [SerializeField]
    private TMP_InputField verificationCodeText;
    [SerializeField]
    private TMP_InputField walletAddresstext;



    public void ViewPhoneNumberInputPanel()
    {
        phoneNumberInputPanel.parent.gameObject.SetActive(true); 
        phoneNumberInputPanel.gameObject.SetActive(true);
        phoneNumberText.text = null;
    }

    public void ClosePhoneNumberInputPanel()
    {
        phoneNumberInputPanel.parent.gameObject.SetActive(false);
        phoneNumberInputPanel.gameObject.SetActive(false);
    }

    public void ViewVerificationCodeInputPanel()
    {
        verificationCodeInputPanel.parent.gameObject.SetActive(true);
        verificationCodeInputPanel.gameObject.SetActive(true);
        verificationCodeText.text = null;
    }
    public void CloseVerificationCodeInputPanel()
    {
        verificationCodeInputPanel.parent.gameObject.SetActive(false);
        verificationCodeInputPanel.gameObject.SetActive(false);
    }

    public void ViewWalletAddressInputPanel()
    {
        walletAddressInputPanel.parent.gameObject.SetActive(true);
        walletAddressInputPanel.gameObject.SetActive(true);
        walletAddresstext.text = null;
    }

    public void CloseWalletAddressInputPanel()
    {
        walletAddressInputPanel.parent.gameObject.SetActive(false);
        walletAddressInputPanel.gameObject.SetActive(false);
    }
}
