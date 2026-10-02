using UnityEngine;
using TMPro;
public class ActionDisplay : MonoBehaviour
{ 
    private Vector3 startPosition;

    private int lifeTime = 3;
    private float travelDistance = 20;
    private float creationTime;
    private float fadeOutDelay = 0.5f;
    private float timeToFadeOutOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startPosition = GetComponent<RectTransform>().position; // Get where text should start
        timeToFadeOutOver = lifeTime - fadeOutDelay; // Used for lerp
        gameObject.SetActive(false); // Start disabled
    }
    private void OnEnable()
    {
        creationTime = Time.time;
    }
    private void OnDisable()
    {
        GetComponent<RectTransform>().position = startPosition; // Reset position of text
        GetComponent<TMP_Text>().alpha = 255; // Reset to full opacity
    }

    // Update is called once per frame
    void Update()
    {
        float timeEnabled = Time.time - creationTime;

        // Check if lifetime exceeded needs disabling
        if (timeEnabled > lifeTime)
            gameObject.SetActive(false);

        if (timeEnabled > fadeOutDelay)
        {
            float timeFadingOut = timeEnabled - fadeOutDelay;
            float t = timeFadingOut / timeToFadeOutOver; // Fraction used for lerp

            // Move up
            GetComponent<RectTransform>().position = new Vector3(
                gameObject.transform.position.x,
                Mathf.Lerp(startPosition.y, startPosition.y + travelDistance, t),
                gameObject.transform.position.z);

            // Fade out text
            GetComponent<TMP_Text>().alpha = Mathf.Lerp(1, 0, t);
        }
    }
}
