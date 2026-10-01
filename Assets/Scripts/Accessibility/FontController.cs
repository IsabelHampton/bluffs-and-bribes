using UnityEngine;
using TMPro;

public class FontController : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset defaultFontAsset;
    [SerializeField] private TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        UpdateFont();
    }
    public void UpdateFont()
    {
        if (AccessibilityManager.Instance.accessibleFontActive)
            text.font = AccessibilityManager.Instance.accessibleFontAsset;
        else
            text.font = defaultFontAsset;
    }
}
