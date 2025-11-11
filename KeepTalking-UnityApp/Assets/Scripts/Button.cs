using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Button : MonoBehaviour
{
    [Header("Hold Settings")]
    public float holdThreshold = 0.5f;

    [Header("Stripe Settings")]
    public Renderer stripeRenderer;

    [Header("Text Settings")]
    public TextMeshPro textLabel;

    private float mouseDownTime;
    private bool isHeld;
    private bool isMouseDown;

    // Možné farby a texty pre náhodnú generáciu
    private Color[] buttonColors = new Color[] { Color.red, Color.blue, Color.white, Color.yellow };
    private Color[] stripeColors = new Color[] { Color.red, Color.blue, Color.white, Color.yellow };
    private string[] buttonTexts = new string[] { "PRESS", "HOLD", "ABORT", "DETONATE" };

    private Color buttonColor;
    private Color stripeColor;
    private string buttontext;
    private bool firstPartSolved = false;
    private bool detonated = false;
    
    [Header("References")]
    public Renderer buttonRenderer; // odkaz na samotný button (cube)
    //PodmienkaAkcia hráča                   
    // | ------------------------------------------------------------------ | ----------------------------- |
    // | Ak je tlačidlo **modré** a má text **"ABORT"**                     | Podrž tlačidlo a sleduj pásik.|
    // | Ak je tlačidlo **biele** a text je PRESS alebo DETONATE**          | Podrž tlačidlo a sleduj pasik.|
    // | Ak je tlačidlo **žlté**                                            | Podrž tlačidlo a sleduj pasik.|
    // | Ak je tlačidlo **červené** a text **"HOLD"**                       | Stlač a **okamžite pusť**.    |
    // | Inak                                                               | Stlač a **okamžite pusť**.    |
    //| Farba pásika | Pusť tlačidlo, keď časovač má číslo ... |
    // | ------------ | --------------------------------------- |
    // | Modrý        | 4                                       |
    // | Biely        | 1                                       |
    // | Žltý         | 5                                       |
    // | Iný          | 1                                       |

    void Start()
    {

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
            //stripeRenderer.material.color = stripeColor;
        } 

        // Náhodný text
        if (textLabel != null)
        {
            buttontext = buttonTexts[Random.Range(0, buttonTexts.Length)];
            textLabel.text = buttontext;
        }
           
    }

    void Update()
    {
        if (isMouseDown && !isHeld)
        {
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
        if (!isHeld)
            OnClick();
    }

    void OnClick()
    {
        Debug.Log($"{name}: CLICK");
        if (buttonColor == Color.yellow)
        {
            detonated = true;
        }
        else if (buttonColor == Color.red && buttontext == "HOLD")
        {
            detonated = true;
        }
      
    }

    void OnHold()
    {
        Debug.Log($"{name}: HOLD");
        if (buttonColor == Color.blue && buttontext == "ABORT")
        {
            stripeRenderer.material.color = stripeColor;
            firstPartSolved = true;

        }
        else if (buttonColor == Color.white && (buttontext == "PRESS" || buttontext == "DETONATE"))
        {
            stripeRenderer.material.color = stripeColor;
            firstPartSolved = true;
        }
        else
        {
            Debug.Log("INCORRECT");
        }
        
        
    }
}
