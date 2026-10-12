using UnityEngine;
using TMPro;

public class AccessibilityManager : MonoBehaviour
{
    public bool accessibleFontActive; 

    public static AccessibilityManager Instance;

    public TMP_FontAsset accessibleFontAsset;
    public TMP_FontAsset defaultFontAsset;


    private void Awake()
    {
        Instance = this;
    }
}
