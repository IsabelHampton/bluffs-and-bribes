using UnityEngine;
using DG.Tweening;
public class DOTweenManager : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init();
    }
}
