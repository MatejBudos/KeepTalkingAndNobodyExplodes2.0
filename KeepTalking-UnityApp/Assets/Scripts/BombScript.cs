using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public static int strikes = 0;

    public Renderer strike1a;
    public Renderer strike1b;
    public Renderer strike2a;
    public Renderer strike2b;
    public Renderer strike3a;
    public Renderer strike3b;
 
    void OnMouseDrag()
    {
        float XaxisRotation = Input.GetAxis("Mouse Y") * -rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse X") * -rotationSpeed;

        transform.Rotate(Vector3.right, XaxisRotation);
        transform.Rotate(Vector3.up, YaxisRotation);
    }

    void Update()
    {
        if (strikes > 0)
        {
            if (strikes == 1)
            {
                strike1a.material.color = Color.red;
                strike1b.material.color = Color.red;
            }
            else if (strikes == 2)
            {
                strike2a.material.color = Color.red;
                strike2b.material.color = Color.red;
                
            }
            else if (strikes == 3)
            {
                strike3a.material.color = Color.red;
                strike3b.material.color = Color.red;
            }
        }
    }



}
