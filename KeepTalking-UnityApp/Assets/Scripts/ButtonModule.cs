using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonModule : MonoBehaviour
{
    [Header("Hold Settings")]
    public float holdThreshold = 2f;

    [Header("Stripe Settings")]
    public Renderer stripeRenderer;

    [Header("Text Settings")]
    public TextMeshPro textLabel;

    private float mouseDownTime;
    private bool isHeld;
    private bool isMouseDown;

    // Možné farby a texty pre náhodnú generáciu
    private Color[] buttonColors = new Color[] { Color.red, Color.blue, Color.white, Color.yellow };
    private Color[] stripeColors = new Color[] { Color.red, Color.blue, Color.white, Color.yellow, Color.green, Color.violet };
    private string[] buttonTexts = new string[] { "PRESS", "HOLD", "ABORT", "DETONATE" };

    private Color buttonColor;
    private Color stripeColor;
    private string buttontext;

    private bool firstPartSolved = false;
    private bool detonated = false;
    private Timer timer;

    public Renderer LED;

    [Header("References")]
    public Renderer buttonRenderer;

    // ------------------------------------------------------------

    public void Start()
    {
        if (timer == null)
        {
            timer = FindFirstObjectByType<Timer>();
            if (timer == null)
            {
                Debug.LogWarning("[Button] Timer not found in scene!");
            }
        }

        // Náhodná farba tlačidla
        if (buttonRenderer != null)
        {
            buttonColor = buttonColors[Random.Range(0, buttonColors.Length)];
            buttonRenderer.material.color = buttonColor;
        }

        // Náhodná farba pásiku
        if (stripeRenderer != null)
        {
            stripeColor = stripeColors[Random.Range(0, stripeColors.Length)];
        }

        // Náhodný text
        if (textLabel != null)
        {
            buttontext = buttonTexts[Random.Range(0, buttonTexts.Length)];
            textLabel.text = buttontext;
        }
    }

    // ------------------------------------------------------------

    public void Update()
    {
        if (isMouseDown && !isHeld)
        {
            if (Time.time - mouseDownTime >= holdThreshold)
            {
                isHeld = true;
                OnHold();
            }
        }

        if (detonated)
        {
            LED.material.color = Color.lightGreen;
        }
    }
    // | ------------------------------------------------------------------ | ----------------------------- | // 
    // | Ak je tlačidlo **modré** a má text **"ABORT" alebo PRESS**     | Podrž tlačidlo a sleduj pásik.| // 
    // | Ak je tlačidlo **biele** a text je PRESS alebo DETONATE**      | Podrž tlačidlo a sleduj pasik.| // 
    // | Ak je tlačidlo **žlté a text NIE je ABORT**                    | Podrž tlačidlo a sleduj pasik.| // 
    // | Ak je tlačidlo **červené** a text **"HOLD"**                   | Stlač a **okamžite pusť**. |
    // | Inak                                                           | Stlač a **okamžite pusť**. | 
    // 
    // 
    // //| Farba pásika | Pusť tlačidlo, keď časovač má číslo ... | // 
    // | ------------ | --------------------------------------- | // 
    // | Modrý | 4 | 
    // | Biely | 1 | // 
    // | Žltý | 5 | // 
    // | Iný | 3 |

    // ================= XR SIMPLE INTERACTABLE =================

    public void OnSelectEntered()
    {
        isMouseDown = true;
        isHeld = false;
        mouseDownTime = Time.time;
    }

    public void OnSelectExited()
    {
        stripeRenderer.material.color = Color.black;
        isMouseDown = false;

        if (!isHeld)
        {
            OnClick();
            return;
        }

        if (!firstPartSolved)
        {
            BombManager.strikes++;
            Debug.Log("INCORRECT");
        }

        string s = timer.currentSeconds.ToString();
        string m = timer.currentMinutes.ToString();

        if (stripeColor == Color.blue && (s.Contains('4') || m.Contains('4')))
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
        else if (stripeColor == Color.white && (s.Contains('1') || m.Contains('1')))
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
        else if (stripeColor == Color.yellow && (s.Contains('5') || m.Contains('5')))
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
        else if ((stripeColor == Color.yellow || stripeColor == Color.green ||
                  stripeColor == Color.magenta || stripeColor == Color.red) &&
                  (s.Contains('3') || m.Contains('3')))
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
        else
        {
            firstPartSolved = false;
            BombManager.strikes++;
            Debug.Log("INCORRECT");
        }
    }

    // ================= LOGIKA =================

    public void OnClick()
    {
        if (detonated)
        {
            return;
        }

        Debug.Log($"{name}: CLICK");

        if (buttonColor == Color.red && buttontext == "HOLD")
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
        else if ((buttonColor == Color.blue && (buttontext == "ABORT" || buttontext == "PRESS")) ||
                 (buttonColor == Color.white && (buttontext == "PRESS" || buttontext == "DETONATE")) ||
                 (buttonColor == Color.yellow && buttontext != "ABORT"))
        {
            BombManager.strikes++;
            Debug.Log("INCORRECT");
        }
        else
        {
            detonated = true;
            Debug.Log("DETONATED");
        }
    }

    public void OnHold()
    {
        if (detonated)
        {
            return;
        }

        Debug.Log($"{name}: HOLD");

        if (buttonColor == Color.blue && (buttontext == "ABORT" || buttontext == "PRESS"))
        {
            stripeRenderer.material.color = stripeColor;
            firstPartSolved = true;
            Debug.Log("CORRECT");
        }
        else if (buttonColor == Color.white && (buttontext == "PRESS" || buttontext == "DETONATE"))
        {
            stripeRenderer.material.color = stripeColor;
            firstPartSolved = true;
            Debug.Log("CORRECT");
        }
        else if (buttonColor == Color.yellow && buttontext != "ABORT")
        {
            stripeRenderer.material.color = stripeColor;
            firstPartSolved = true;
            Debug.Log("CORRECT");
        }
        else
        {
            BombManager.strikes++;
            Debug.Log("INCORRECT");
        }
    }
}
