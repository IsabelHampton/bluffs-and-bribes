using UnityEngine;
using TMPro;

public class FontController : MonoBehaviour
{
    private TMP_Text text;
    private TMP_FontAsset defaultFontAsset;
    private void Awake()
    {
        //UpdateFont(SettingsManager.Instance.SimpleFontEnabled);
        //SettingsManager.OnSimpleFontToggleChanged += UpdateFont;
    }
    private void OnEnable()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();

        if (defaultFontAsset == null)
            defaultFontAsset = text.font;
        if (SettingsManager.Instance != null) 
            UpdateFont(SettingsManager.Instance.SimpleFontEnabled);
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
