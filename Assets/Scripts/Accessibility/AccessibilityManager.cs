using UnityEngine;
using TMPro;

public class AccessibilityManager : MonoBehaviour
{
    public bool accessibleFontActive; 

    public static AccessibilityManager Instance;
    public TMP_FontAsset accessibleFontAsset;


    private void Awake()
    {
        Instance = this;
    }
}
