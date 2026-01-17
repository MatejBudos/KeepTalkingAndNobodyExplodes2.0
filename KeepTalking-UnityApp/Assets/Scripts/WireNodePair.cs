using UnityEngine;

public class WireNodePair : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform L;
    public Transform R;
    public Transform MidPoint;
    public WireModule module;
    public Color color;
    public int index;

    public void Start()
    {
       
        index = int.Parse(gameObject.name);
    }

    // Update is called once per frame
    public void Update()
    {
   
    }

    public void SetColor(Color color)
    {
        SetCylinderColor(L, color);
        SetCylinderColor(MidPoint, color);
        SetCylinderColor(R, color);
        this.color = color;

    }

  private void SetCylinderColor(Transform cylinder, Color newColor)
    {
        if (cylinder == null) return;

        MeshRenderer renderer = cylinder.GetComponent<MeshRenderer>();
        if (renderer == null) return;

        // Vytvor unikátny materiál pre každý drôt, aby sa farby neprepísali
        renderer.material = new Material(Shader.Find("Unlit/Color"));
        renderer.material.color = newColor;

        Debug.Log(cylinder.name + " color set to " + newColor);
    }

    public void CutAttempt()
    {
        Debug.Log("TriedToCut the cable!!!");
        module.CheckCutAttempt(this);

    }
    public void DisablePair()
    {
        L.gameObject.SetActive(false);
        MidPoint.gameObject.SetActive(false);
        R.gameObject.SetActive(false);
    }

    public void Cut()
    {
        MidPoint.gameObject.SetActive(false);
       
    }


}
