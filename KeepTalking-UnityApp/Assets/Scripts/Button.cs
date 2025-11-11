using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Hold Settings")]
    public float holdThreshold = 0.5f; // čas, po ktorom sa klik považuje za držanie

    [Header("Stripe Settings")]
    public Renderer stripeRenderer; // odkaz na stripe (napr. plane)
    
    private float mouseDownTime;
    private bool isHeld;
    private bool isMouseDown;

    void Start()
    {
        // Ak stripeRenderer nie je nastavený, pokús sa ho nájsť automaticky
        if (stripeRenderer == null)
        {
            Transform stripe = transform.Find("stripe");
            if (stripe != null)
                stripeRenderer = stripe.GetComponent<Renderer>();
        }

        // Priradíme náhodnú farbu pásiku pri štarte
        if (stripeRenderer != null)
            stripeRenderer.material.color = Random.ColorHSV();
    }

    void Update()
    {
        if (isMouseDown && !isHeld)
        {
            // Ak sa drží dlhšie ako holdThreshold, spustí sa hold
            if (Time.time - mouseDownTime >= holdThreshold)
            {
                isHeld = true;
                OnHold();
            }
        }
    }

    void OnMouseDown()
    {
        isMouseDown = true;
        isHeld = false;
        mouseDownTime = Time.time;
    }

    void OnMouseUp()
    {
        isMouseDown = false;

        // Ak nebol držaný dosť dlho, je to klik
        if (!isHeld)
            OnClick();
    }

    void OnClick()
    {
        Debug.Log($"{name}: CLICK");
    }

    void OnHold()
    {
        Debug.Log($"{name}: HOLD");
    }
}
