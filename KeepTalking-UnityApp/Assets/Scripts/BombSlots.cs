using UnityEngine;

public class BombSlots : MonoBehaviour
{
    public GameObject slotPrefab;     
    public int rows = 3;               
    public int cols = 3;               
    public float padding = 0.1f;       

    private Vector3 bombSize;

    void Start()
    {
        // získame veľkosť bomby (cube)
        bombSize = GetComponent<Renderer>().bounds.size;

        CreateSlots();
    }
    void CreateSlots()
    {
 
    }
}

