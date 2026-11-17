using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card_DOTween : MonoBehaviour
{
    public Image cardBack; 

    public Tween cardPosTween;
    public Tween cardRotTween;
    public Tween cardRevealTween;

    Vector3 startRotation;
    Vector3 startPosition;
    float duration = .5f;

    private void Awake()
    {
        startRotation = new Vector3(transform.rotation.x, transform.rotation.y, 180);
        startPosition = new Vector3(Screen.width, Screen.height / 2);
    }

    public IEnumerator DealInCard()
    {
        cardPosTween = transform.DOBlendableMoveBy(startPosition, duration)
            .SetEase(Ease.OutQuad)
            .From();
            //.SetDelay(delay);

        cardRotTween = transform.DOBlendableRotateBy(startRotation, duration)
            .SetEase(Ease.OutQuint)
            .From();
            //.SetDelay(delay);

        yield return cardRotTween.WaitForCompletion();
    }
    public IEnumerator RevealCard()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(new Vector3(transform.rotation.x, 90, transform.rotation.z), duration)
            .SetEase(Ease.InSine));

        sequence.AppendCallback(() =>
        {
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<Card>().faceUpSprite; // Change sprite
        });
        sequence.Append(transform.DORotate(new Vector3(transform.rotation.x, 0, transform.rotation.z), duration)
            .SetEase(Ease.OutSine));

        yield return sequence.WaitForCompletion();
    }
}
