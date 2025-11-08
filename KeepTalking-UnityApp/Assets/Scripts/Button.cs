using UnityEngine;

public class Button : Interactable
{
    public GameObject buttonPrefab;
    // Start is
    //   called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Interact()
    {
        Debug.Log("buttoooon");
    }
}
