using UnityEngine;

public class SimonTileClick : MonoBehaviour
{
    public int tileIndex;

    private SimonSequenceController module;

    void Awake()
    {
        // Find the module this tile belongs to
        module = GetComponentInParent<SimonSequenceController>();

        if (module == null)
        {
            Debug.LogError($"SimonTileClick on {name} could not find a parent SimonSequenceController");
        }
    }

    public void OnMouseDown()
    {
        if (module != null)
        {
            module.OnTileClicked(tileIndex);
        }
    }
}
