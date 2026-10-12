using UnityEngine;
using TMPro;

public class FontController : MonoBehaviour
{
    private TMP_Text text;
    private TMP_FontAsset defaultFontAsset;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        defaultFontAsset = text.font;
        //UpdateFont(SettingsManager.Instance.SimpleFontEnabled);
        //SettingsManager.OnSimpleFontToggleChanged += UpdateFont;
    }
    private void OnDestroy()
    {
        //SettingsManager.OnSimpleFontToggleChanged -= UpdateFont;
    }
    private void Update()
    {
        UpdateFont(SettingsManager.Instance.SimpleFontEnabled);
    }

    public void ChangeToAccessibleFont()
    {
        text.font = AccessibilityManager.Instance.accessibleFontAsset;
        text.fontSize *= 0.75f;
    }

    public void ChangeToDefaultFont()
    {
        text.font = AccessibilityManager.Instance.defaultFontAsset;
        text.fontSize /= 0.75f;
    }
    public void UpdateFont(bool _isSimpleFont)
    {
        if (_isSimpleFont && text.font != AccessibilityManager.Instance.accessibleFontAsset)
        {
            ChangeToAccessibleFont();
        }
        else if (!_isSimpleFont && text.font != defaultFontAsset)
        {
            ChangeToDefaultFont();
        }
    }
}
