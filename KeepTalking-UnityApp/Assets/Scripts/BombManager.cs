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

    public GameObject emptyModulePrefab;


    public static BombManager Instance;
 

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
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void GenerateModules(int moduleCount)
    {
        strikes = 0;
        ClearModules();

        moduleCount = Mathf.Min(moduleCount, moduleContainers.Count);


        List<Transform> shuffledContainers = new List<Transform>(moduleContainers);
        ShuffleContainers(shuffledContainers);

        // Place modules
        for (int i = 0; i < moduleCount; i++)
        {
            Transform container = shuffledContainers[i];
            int moduleIndex = Random.Range(0, modulePrefabs.Count);

            Instantiate(
                modulePrefabs[moduleIndex],
                container.position,
                container.rotation,
                container
            );
        }

        // Fill remaining slots with empty modules
        for (int i = moduleCount; i < shuffledContainers.Count; i++)
        {
            Transform container = shuffledContainers[i];

            Instantiate(
                emptyModulePrefab,
                container.position,
                container.rotation,
                container
            );
        }
    }



    void ClearModules()
    {
        foreach (Transform container in moduleContainers)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    void ShuffleContainers(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }


}
