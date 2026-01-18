using UnityEngine;

public class WireNodePair : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform L;
    public Transform R;
    public Transform MidPoint;
    public WireModule module;
    public Color color;
    public Material glow;
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

        // Debug.Log(cylinder.name + " color set to " + newColor);
    }

    public void CutAttempt()
    {
        Debug.Log("TriedToCut the cable!!!");
        int answ = module.getAnswer();
        if (answ == this.index)
        {
            module.detonated = true;   
            Cut();
            Debug.Log("Correct");
            return;
        }
        Debug.Log("Incorrect");
        Debug.Log("Correct index was " + answ + "you clicked "+ this.index);
        

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
    public void HoverOn()
    {
        AddGlow(L);
        AddGlow(MidPoint);
        AddGlow(R);
    }

    public void HoverOff()
    {
        RemoveGlow(L);
        RemoveGlow(MidPoint);
        RemoveGlow(R);
    }
    private void AddGlow(Transform cylinder)
    {
        if (cylinder == null) return;

        MeshRenderer r = cylinder.GetComponent<MeshRenderer>();
        if (r == null) return;

        Material[] mats = r.materials;

        // už tam glow je → nič nerob
        if (mats.Length > 1)
            return;

        Material[] newMats = new Material[2];
        newMats[0] = mats[0];   // pôvodný material
        newMats[1] = glow;     // glow ako druhý

        r.materials = newMats;
    }
    private void RemoveGlow(Transform cylinder)
    {
        if (cylinder == null) return;

        MeshRenderer r = cylinder.GetComponent<MeshRenderer>();
        if (r == null) return;

        Material[] mats = r.materials;

        // už je len base material
        if (mats.Length == 1)
            return;

        Material[] newMats = new Material[1];
        newMats[0] = mats[0];   // ponechaj base

        r.materials = newMats;
    }



}
