using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static int strikes;

    public Renderer strike1a;
    public Renderer strike1b;
    public Renderer strike2a;
    public Renderer strike2b;
    public Renderer strike3a;
    public Renderer strike3b;
    public List<GameObject> modulePrefabs;
    public List<Transform> moduleContainers;
 

    public void Update()
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

    public void Start()
    {
        strikes = 0;
        GenerateModules();
    }


    public void GenerateModules()
    {
        foreach (Transform t in moduleContainers)
        {
            int moduleIndex = Random.Range(0, 3);

            GameObject module = Instantiate(modulePrefabs[moduleIndex],t.position, t.rotation,t);     
        }
    }


}
