using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
public class StartMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TeleportationProvider teleportProvider;
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform xrOrigin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Teleport()
    {
        TeleportRequest request = new TeleportRequest
        {
            destinationPosition = teleportTarget.position,
            destinationRotation = teleportTarget.rotation
        };

        teleportProvider.QueueTeleportRequest(request);
    }

    public void ClickedButton()
    {
        Debug.Log("CLICKED");
        Teleport();
    }
}
