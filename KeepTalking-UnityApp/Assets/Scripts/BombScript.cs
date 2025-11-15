using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float rotationSpeed = 1f;


    void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.right, XaxisRotation);
        transform.Rotate(Vector3.up, YaxisRotation);
    }
    

    
}
